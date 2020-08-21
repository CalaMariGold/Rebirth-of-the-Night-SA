using System.Collections;
using System.Collections.Generic;
using Rebirth.Terrain.Octree;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Represents a chunk of voxels in space using a Dense Octree.
    /// </summary>
    public class OctreeChunk : IChunk
    {
        private readonly DenseOctree<VoxelInfo> _voxelData;

        /// <summary>
        /// Initialises a new instance of the <see cref="OctreeChunk"/> class.
        /// </summary>
        /// <param name="subdivisions">The number of subdivisions in the octree.</param>
        /// <param name="offsetX">The offset of the chunk in the x-axis.</param>
        /// <param name="offsetY">The offset of the chunk in the y-axis.</param>
        /// <param name="offsetZ">The offset of the chunk in the z-axis.</param>
        public OctreeChunk(int subdivisions, int offsetX, int offsetY, int offsetZ)
        {
            // Set the offset in voxel space of the chunk
            OffsetX = offsetX;
            OffsetY = offsetY;
            OffsetZ = offsetZ;
            // Initialise the octree holding voxel data
            _voxelData = new DenseOctree<VoxelInfo>(subdivisions);
        }
        
        /// <summary>
        /// Gets the <see cref="VoxelInfo"/> for the voxel at a given location
        /// within the chunk.
        /// </summary>
        public VoxelInfo this[int x, int y, int z] => _voxelData[x, y, z];
        /// <summary>
        /// Gets the width of the chunk in the x-axis.
        /// </summary>
        public int Width => 1 << _voxelData.Subdivisions;
        /// <summary>
        /// Gets the height of the chunk in the y-axis.
        /// </summary>
        public int Height => 1 << _voxelData.Subdivisions;
        /// <summary>
        /// Gets the depth of the chunk in the z-axis.
        /// </summary>
        public int Depth => 1 << _voxelData.Subdivisions;
        
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int OffsetZ { get; set; }
        
        /// <summary>
        /// Fills the data octree with voxels from a provider.
        /// </summary>
        /// <param name="voxelProvider">The object which provides voxel info.</param>
        public void Load(IVoxelProvider voxelProvider)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    for (var z = 0; z < Depth; z++)
                    {
                        _voxelData.SetByIndex(
                            x, y, z,
                            voxelProvider.GetVoxelInfo(
                                x + OffsetX,
                                y + OffsetY,
                                z + OffsetZ
                            ),
                            true
                        );
                    }
                }
            }
        }

        public IEnumerator<KeyValuePair<Vector3Int, VoxelInfo>> GetEnumerator()
        {
            return _voxelData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
