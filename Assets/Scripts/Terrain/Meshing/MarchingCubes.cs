using System.Collections.Generic;
using System.Linq;
using Rebirth.Terrain.Chunk;
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
            Mesh mesh = CreateChunkMesh(chunk, computeShader);
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
            int numThreadGroupsX = Mathf.CeilToInt (chunk.Width / (float) _threadGroupSize);
            int numThreadGroupsY = Mathf.CeilToInt (chunk.Height / (float) _threadGroupSize);
            int numThreadGroupsZ = Mathf.CeilToInt (chunk.Depth / (float) _threadGroupSize);

            // Dispatch
            computeShader.Dispatch(0, numThreadGroupsX, numThreadGroupsY, numThreadGroupsZ);

            // Get number of triangles in the triangle buffer
            ComputeBuffer.CopyCount(_triangleBuffer, _triCountBuffer, 0);
            int[] triCountArray = { 0 };
            _triCountBuffer.GetData(triCountArray);
            int numTris = triCountArray[0];

            // Get triangle data from shader
            Triangle[] tris = new Triangle[numTris];
            _triangleBuffer.GetData(tris, 0, 0, numTris);

            var vertices = new Vector3[numTris * 3];

            for (int i = 0; i < numTris; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    vertices[i * 3 + j] = tris[i][j];
                }
            }

            // TODO: properly add colors, probably in compute shader
            var colours = new List<Color>();
            for (var x = 0; x < chunk.Width - 1; x++)
            {
                for (var y = 0; y < chunk.Height - 1; y++)
                {
                    for (var z = 0; z < chunk.Depth - 1; z++)
                    {
                        var colour = chunk[x, y, z].VoxelType?.Colour ?? Color.white;
                    }
                }
            }

            return new Mesh
            {
                vertices = vertices,
                // colors = colours.ToArray(),
                triangles = Enumerable.Range(0, vertices.Length).ToArray()
            };
        }

        /// <summary>
        /// Creates compute buffers necessary for Marching Cubes algorithm.
        /// </summary>
        /// <param name="chunk">The chunk to generate a mesh from.</param>
        private void CreateBuffers(IChunk chunk)
        {
            int numVoxels = chunk.Width * chunk.Height * chunk.Depth;
            int maxTriangleCount = numVoxels * 5;
            
            ReleaseBuffers(); // Ensure previous buffers are released
            _pointBuffer = new ComputeBuffer(numVoxels, sizeof(float));
            _triangleBuffer = new ComputeBuffer(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);
            _triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        }

        void OnDestroy()
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
            if (_pointBuffer != default(ComputeBuffer))
            {
                _pointBuffer.Release();
                _triangleBuffer.Release();
                _triCountBuffer.Release();
            }
        }
    }
}
