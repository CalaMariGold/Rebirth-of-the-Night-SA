using System.Collections.Generic;
using System.Linq;
using Rebirth.Terrain.Chunk;
using UnityEngine;

namespace Rebirth.Terrain.Meshing
{
    public class MarchingCubes : IMeshGenerator
    {
        public Mesh GenerateMesh(IChunk chunk)
        {
            var vertices = new List<Vector3>();
            // Iterate through the cubes
            for (var x = 0; x < chunk.Width - 1; x++)
            {
                for (var y = 0; y < chunk.Height - 1; y++)
                {
                    for (var z = 0; z < chunk.Depth - 1; z++)
                    {
                        vertices.AddRange(GenerateVertices(chunk, x, y, z));
                    }
                }
            }

            var mesh = new Mesh
            {
                vertices = vertices.ToArray(),
                triangles = Enumerable.Range(0, vertices.Count).ToArray()
            };
            // Set vertices & triangles in mesh
            return mesh;
        }

        /// <summary>
        /// Generates a list of vertices to generate mesh triangles for a given cube.
        /// </summary>
        /// <param name="chunk">The chunk to get voxel data from/</param>
        /// <param name="x">The cube's x-index.</param>
        /// <param name="y">The cube's y-index.</param>
        /// <param name="z">The cube's z-index.</param>
        /// <returns>An enumerable representing the vertices to be triangulated.</returns>
        private static IEnumerable<Vector3> GenerateVertices(IChunk chunk, int x, int y, int z)
        {
            // 8 corners of the current cube
            VoxelInfo[] cubeCorners =
            {
                chunk[x, y, z],
                chunk[x + 1, y, z],
                chunk[x + 1, y, z + 1],
                chunk[x, y, z + 1],
                chunk[x, y + 1, z],
                chunk[x + 1, y + 1, z],
                chunk[x + 1, y + 1, z + 1],
                chunk[x, y + 1, z + 1]
            };
            // This is kind of messy, but probably more efficient than a loop
            Vector3Int[] cornerLocations =
            {
                new Vector3Int(x, y, z),
                new Vector3Int(x + 1, y, z),
                new Vector3Int(x + 1, y, z + 1),
                new Vector3Int(x, y, z + 1),
                new Vector3Int(x, y + 1, z),
                new Vector3Int(x + 1, y + 1, z),
                new Vector3Int(x + 1, y + 1, z + 1),
                new Vector3Int(x, y + 1, z + 1)
            };

            // Generate a unique index for the cube configuration.
            // 0 means entirely within the surface, 255 entirely outside.
            // This is used to lookup the edge table.
            var cubeIndex = 0;
            for (var i = 0; i < 8; i++)
            {
                if (cubeCorners[i].Distance > 0)
                {
                    cubeIndex |= 1 << i;
                }
            }

            // Create triangles for the current config
            for (var t = 0; MarchingTables.Triangulation[cubeIndex, t] != -1; t += 3)
            {
                // Get indices of corners A and B for each edge that needs to be joined
                var a0 = MarchingTables.CornerIndexAFromEdge[
                    MarchingTables.Triangulation[cubeIndex, t]
                ];
                var b0 = MarchingTables.CornerIndexBFromEdge[
                    MarchingTables.Triangulation[cubeIndex, t]
                ];
                
                var a1 = MarchingTables.CornerIndexAFromEdge[
                    MarchingTables.Triangulation[cubeIndex, t + 1]
                ];
                var b1 = MarchingTables.CornerIndexBFromEdge[
                    MarchingTables.Triangulation[cubeIndex, t + 1]
                ];
                
                var a2 = MarchingTables.CornerIndexAFromEdge[
                    MarchingTables.Triangulation[cubeIndex, t + 2]
                ];
                var b2 = MarchingTables.CornerIndexBFromEdge[
                    MarchingTables.Triangulation[cubeIndex, t + 2]
                ];

                // Interpolate for smooth terrain
                yield return InterpolateVertices(
                    cubeCorners[a0], cubeCorners[b0],
                    cornerLocations[a0], cornerLocations[b0]
                    );
                yield return InterpolateVertices(
                    cubeCorners[a2], cubeCorners[b2],
                    cornerLocations[a2], cornerLocations[b2]
                );
                yield return InterpolateVertices(
                    cubeCorners[a1], cubeCorners[b1],
                    cornerLocations[a1], cornerLocations[b1]
                );
            }
        }

        /// <summary>
        /// Interpolates between two voxels to create smooth terrain.
        /// </summary>
        /// <param name="cornerA"></param>
        /// <param name="cornerB"></param>
        /// <param name="coordinateA"></param>
        /// <param name="coordinateB"></param>
        /// <returns></returns>
        private static Vector3 InterpolateVertices(VoxelInfo cornerA, VoxelInfo cornerB,
            Vector3Int coordinateA, Vector3Int coordinateB)
        {
            var t = cornerA.Distance / (cornerA.Distance - cornerB.Distance);
            return coordinateA + t * ((Vector3) coordinateB - coordinateA);
        }
    }
}
