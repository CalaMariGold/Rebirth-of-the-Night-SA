using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Rebirth.Terrain.Chunk;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Meshing
{
    /// <summary>
    /// Provides Unity meshes for voxel chunks using the Marching Cubes algorithm.
    /// </summary>
    public class MarchingCubes : IMeshGenerator
    {
        private const int _threadGroupSize = 8;
        private ComputeBuffer _pointBuffer;
        private ComputeBuffer _triangleBuffer;
        private ComputeBuffer _triCountBuffer;

        /// <summary>
        /// Generates a mesh from the data in an <seealso cref="IChunk"/>.
        /// </summary>
        /// <param name="chunkLocation">The location of the chunk to mesh.</param>
        /// <param name="chunks">The loaded chunks to use in mesh generation.</param>
        /// <param name="computeShader">The compute shader to use when generating the mesh.</param>
        /// <returns>A Unity mesh which can be added to a scene.</returns>
        /// <remarks>Based on Sebastian Lague's compute shader implementation.</remarks>
        public void GenerateMesh(Vector3Int chunkLocation,
            IDictionary<Vector3Int, IChunk> chunks,
            ComputeShader computeShader,
            ref Mesh mesh)
        {
            // Do we need to check if chunkLocation is in chunks?
            var chunk = chunks[chunkLocation];
            CreateBuffers(chunk);
            CreateChunkMesh(chunkLocation, chunks, computeShader, ref mesh);
            ReleaseBuffers();
        }

        /// <summary>
        /// Helper method for generating a Mesh via Marching Cubes algorithm.
        /// Fills buffers, executes compute shader, and returns result.
        /// </summary>
        /// <param name="chunkLocation">The location of the chunk to mesh.</param>
        /// <param name="chunks">The loaded chunks to use in mesh generation.</param>
        /// <param name="computeShader">The compute shader to use when generating the mesh.</param>
        /// <returns>A Unity mesh which can be added to a scene.</returns>
        private void CreateChunkMesh(Vector3Int chunkLocation,
            IDictionary<Vector3Int, IChunk> chunks,
            ComputeShader computeShader,
            ref Mesh mesh)
        {
            var chunk = chunks[chunkLocation];
            // Fill buffers
            computeShader.SetBuffer(0, "chunkPoints", _pointBuffer);
            computeShader.SetBuffer(0, "triangles", _triangleBuffer);
            
            _pointBuffer.SetData(CalcDistanceArray(chunkLocation, chunks));

            // Update shader params
            computeShader.SetInt("chunkWidth", chunk.Width + 1);
            computeShader.SetInt("chunkHeight", chunk.Height + 1);
            computeShader.SetInt("chunkDepth", chunk.Depth + 1);

            // Determine number of thread groups to use for each axis
            var numThreadGroupsX = Mathf.CeilToInt(chunk.Width + 1 / (float) _threadGroupSize);
            var numThreadGroupsY = Mathf.CeilToInt(chunk.Height + 1 / (float) _threadGroupSize);
            var numThreadGroupsZ = Mathf.CeilToInt(chunk.Depth + 1 / (float) _threadGroupSize);

            // Dispatch
            computeShader.Dispatch(0, numThreadGroupsX, numThreadGroupsY, numThreadGroupsZ);

            // Get number of triangles in the triangle buffer
            ComputeBuffer.CopyCount(_triangleBuffer, _triCountBuffer, 0);
            int[] triCountArray = { 0 };
            _triCountBuffer.GetData(triCountArray);
            var numTris = triCountArray[0];

            // Get triangle data from shader
            var tris = new Triangle[numTris];
            _triangleBuffer.GetData(tris, 0, 0, numTris);

            var vertices = new Vector3[numTris * 3];
            var colours = new Color[numTris * 3];

            for (var i = 0; i < numTris; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    vertices[i * 3 + j] = tris[i][j];
                    colours[i * 3 + j] = tris[i].Color;
                }
            }

            if(mesh != null)
            {
                mesh.vertices = vertices;
                mesh.colors = colours;
                mesh.triangles = Enumerable.Range(0, vertices.Length).ToArray();
            } else
            {
                mesh = new Mesh()
                {
                    vertices = vertices,
                    colors = colours,
                    triangles = Enumerable.Range(0, vertices.Length).ToArray()
                };
            } 
        }

        /// <summary>
        /// Provides a 1-D array containing the voxel Distance data in the chunk's 3-D data array.
        /// Indexing is determined with the following mapping:
        /// <code><![CDATA[
        ///    index(x, y, z) = z * (width + height) + y * height + x
        /// ]]></code>
        /// where (x, y, z) are local data array indices.
        /// </summary>
        /// <param name="chunkLocation">The location of the chunk.</param>
        /// <param name="chunks">The loaded chunks.</param>
        /// <returns>The 1-D array holding the compute data.</returns>
        private static VoxelComputeInfo[] CalcDistanceArray(Vector3Int chunkLocation,
            IDictionary<Vector3Int, IChunk> chunks)
        {
            // Initial chunk
            var chunk = chunks[chunkLocation];
            var width = chunk.Width + 1;
            var height = chunk.Height + 1;
            var depth = chunk.Depth + 1;
            var computeInfo = new VoxelComputeInfo[width * height * depth];
            // NOTE: IEnumerable used because it should be cheaper than indexed access on an octree.
            foreach (var item in chunk)
            {
                var index = item.Key.z * width * height + item.Key.y * width + item.Key.x;
                computeInfo[index] = item.Value.ToCompute();
            }

            // Side faces, edges, corner
            // NOTE: This whole section is very janky and could do with a refactor
            for (var i = 1; i <= 7; i++)
            {
                var otherChunkVector = new Vector3Int(
                    i & 1,
                    (i >> 1) & 1,
                    (i >> 2) & 1);

                var otherLocation = chunkLocation + otherChunkVector;
                var found = chunks.TryGetValue(otherLocation, out var otherChunk);
                for (var x = 0; x <= (chunk.Width - 1) * (1 - otherChunkVector.x); x++)
                {
                    for (var y = 0; y <= (chunk.Height - 1) * (1 - otherChunkVector.y); y++)
                    {
                        for (var z = 0; z <= (chunk.Depth - 1) * (1 - otherChunkVector.z); z++)
                        {
                            var index = ((otherChunkVector.z * chunk.Width + z) * width * height) +
                                        ((otherChunkVector.y * chunk.Height + y) * width) +
                                        (otherChunkVector.x * chunk.Depth + x);
                            if (found)
                            {
                                computeInfo[index] = otherChunk[x, y, z].ToCompute();
                            }
                            else
                            {
                                // TODO: use a better default?
                                computeInfo[index] = new VoxelComputeInfo
                                {
                                    Distance = 1.0f
                                };
                            }
                        }
                    }
                }
            }

            return computeInfo;
        }

        /// <summary>
        /// Creates compute buffers necessary for Marching Cubes algorithm.
        /// </summary>
        /// <param name="chunk">The chunk to generate a mesh from.</param>
        private void CreateBuffers(IChunk chunk)
        {
            var numVoxels = (chunk.Width + 1) * (chunk.Height + 1) * (chunk.Depth + 1);
            var maxTriangleCount = numVoxels * 5;
            
            ReleaseBuffers(); // Ensure previous buffers are released

            _pointBuffer = new ComputeBuffer(numVoxels, Marshal.SizeOf<VoxelComputeInfo>());
            _triangleBuffer = new ComputeBuffer(maxTriangleCount, Marshal.SizeOf<Triangle>(), ComputeBufferType.Append);
            _triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
            // Ensures that correct values are read from the buffer
            _triangleBuffer.SetCounterValue(0);
        }

        // NOTE: not a MonoBehaviour, so this will need to be called by the owning behaviour
        public void OnDestroy()
        {
            if (Application.isPlaying)
            {
                ReleaseBuffers();
            }
        }

        /// <summary>
        /// Releases all compute buffers.
        /// </summary>
        private void ReleaseBuffers()
        {
            if (_pointBuffer == default)
            {
                return;
            }

            _pointBuffer.Release();
            _triangleBuffer.Release();
            _triCountBuffer.Release();
        }
    }
}
