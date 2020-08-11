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
            _pointBuffer.SetData(chunk.ToArray());

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
            var triangles = new int[numTris * 3];

            for (int i = 0; i < numTris; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    triangles[i * 3 + j] = i * 3 + j;
                    vertices[i * 3 + j] = tris[i][j];
                }
            }

            return new Mesh
            {
                vertices = vertices,
                triangles = triangles
            };
        }

        /// <summary>
        /// Creates compute buffers necessary for Marching Cubes algorithm.
        /// </summary>
        /// <param name="chunk">The chunk to generate a mesh from.</param>
        private void CreateBuffers(IChunk chunk)
        {
            int maxTriangleCount = ((chunk.Width - 1) * (chunk.Height - 1) * (chunk.Depth - 1)) * 5;
            int numPoints = chunk.Width * chunk.Height * chunk.Depth;
            _pointBuffer = new ComputeBuffer(numPoints, sizeof(float));
            _triangleBuffer = new ComputeBuffer(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);
            _triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        }

        /// <summary>
        /// Releases all compute buffers.
        /// </summary>
        private void ReleaseBuffers()
        {
            if (_triangleBuffer != null)
            {
                _pointBuffer.Release();
                _triangleBuffer.Release();
                _triCountBuffer.Release();
            }
        }
    }
}
