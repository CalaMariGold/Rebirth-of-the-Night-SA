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
        /// Generates a mesh using an implementation of Marching Cubes with a compute shader.
        /// </summary>
        /// <param name="chunk">The chunk to generate a mesh from.</param>
        /// <param name="computeShader">The compute shader to use when generating the mesh.</param>
        /// <returns>A Mesh which can be added to a scene.</returns>
        /// <remarks>Based on Sebastian Lague's compute shader implementation.</remarks>
        public Mesh GenerateMesh(IChunk chunk, ComputeShader computeShader)
        {
            CreateBuffers(chunk);
            var mesh = CreateChunkMesh(chunk, computeShader);
            ReleaseBuffers();
            return mesh;
        }

        /// <summary>
        /// Helper method for generating a Mesh via Marching Cubes algorithm.
        /// Fills buffers, executes compute shader, and returns result.
        /// </summary>
        /// <param name="chunk">The chunk to generate a mesh from.</param>
        /// <param name="computeShader">The compute shader to use when generating the mesh.</param>
        /// <returns>A Mesh which can be added to a scene.</returns>
        private Mesh CreateChunkMesh(IChunk chunk, ComputeShader computeShader)
        {
            // Fill buffers
            computeShader.SetBuffer(0, "chunkPoints", _pointBuffer);
            computeShader.SetBuffer(0, "triangles", _triangleBuffer);
            _pointBuffer.SetData(chunk.CalcDistanceArray());

            // Update shader params
            computeShader.SetInt("chunkWidth", chunk.Width);
            computeShader.SetInt("chunkHeight", chunk.Height);
            computeShader.SetInt("chunkDepth", chunk.Depth);

            // Determine number of thread groups to use for each axis
            var numThreadGroupsX = Mathf.CeilToInt(chunk.Width / (float) _threadGroupSize);
            var numThreadGroupsY = Mathf.CeilToInt(chunk.Height / (float) _threadGroupSize);
            var numThreadGroupsZ = Mathf.CeilToInt(chunk.Depth / (float) _threadGroupSize);

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

            return new Mesh
            {
                vertices = vertices,
                colors = colours,
                triangles = Enumerable.Range(0, vertices.Length).ToArray()
            };
        }

        /// <summary>
        /// Creates compute buffers necessary for Marching Cubes algorithm.
        /// </summary>
        /// <param name="chunk">The chunk to generate a mesh from.</param>
        private void CreateBuffers(IChunk chunk)
        {
            var numVoxels = chunk.Width * chunk.Height * chunk.Depth;
            var maxTriangleCount = numVoxels * 5;
            
            ReleaseBuffers(); // Ensure previous buffers are released

            _pointBuffer = new ComputeBuffer(numVoxels, Marshal.SizeOf<VoxelComputeInfo>());
            _triangleBuffer = new ComputeBuffer(maxTriangleCount, Marshal.SizeOf<Triangle>(), ComputeBufferType.Append);
            _triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
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
