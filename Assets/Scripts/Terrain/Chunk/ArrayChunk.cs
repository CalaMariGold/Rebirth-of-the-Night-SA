using System.Collections;
using System.Collections.Generic;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Represents a chunk of voxels in space using a 3D array.
    /// </summary>
    public class ArrayChunk : IChunk
    {
        private readonly VoxelInfo[,,] _voxelData;
        
        /// <summary>
        /// Initialises a new instance of the <see cref="ArrayChunk"/> class.
        /// </summary>
        /// <param name="width">The width of the chunk.</param>
        /// <param name="height">The height of the chunk.</param>
        /// <param name="depth">The depth of the chunk.</param>
        /// <param name="offsetX">The offset of the chunk in the x-axis.</param>
        /// <param name="offsetY">The offset of the chunk in the y-axis.</param>
        /// <param name="offsetZ">The offset of the chunk in the z-axis.</param>
        public ArrayChunk(int width, int height, int depth,
            int offsetX, int offsetY, int offsetZ)
        {
            // Set the bounding size of the chunk
            Width = width;
            Height = height;
            Depth = depth;
            // Set the offset in voxel space of the chunk
            OffsetX = offsetX;
            OffsetY = offsetY;
            OffsetZ = offsetZ;
            // Initialise the array holding voxel data
            _voxelData = new VoxelInfo[width,height,depth];
        }

        /// <summary>
        /// Gets the <see cref="VoxelInfo"/> for the voxel at a given location
        /// within the chunk.
        /// </summary>
        public VoxelInfo this[int x, int y, int z] => _voxelData[x, y, z];

        /// <summary>
        /// Gets the width of the chunk in the x-axis.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of the chunk in the y-axis.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the depth of the chunk in the z-axis.
        /// </summary>
        public int Depth { get; }
        
        /// <summary>
        /// Gets or sets the offset of the chunk in the x-axis.
        /// </summary>
        public int OffsetX { get; set; }
        
        /// <summary>
        /// Gets or sets the offset of the chunk in the y-axis.
        /// </summary>
        public int OffsetY { get; set; }
        
        /// <summary>
        /// Gets or sets the offset of the chunk in the z-axis.
        /// </summary>
        public int OffsetZ { get; set; }

        /// <summary>
        /// Fills the data array with voxels from a provider.
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
                        _voxelData[x, y, z] = voxelProvider.GetVoxelInfo(
                            x + OffsetX,
                            y + OffsetY,
                            z + OffsetZ
                            );
                    }
                }
            }
        }

        public IEnumerator<KeyValuePair<Vector3Int, VoxelInfo>> GetEnumerator()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    for (var z = 0; z < Depth; z++)
                    {
                        yield return new KeyValuePair<Vector3Int, VoxelInfo>(
                            new Vector3Int(x, y, z),
                            _voxelData[x, y, z]
                        );
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
