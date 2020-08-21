using System.Collections.Generic;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Represents a chunk of voxels in space.
    /// </summary>
    public interface IChunk : IEnumerable<KeyValuePair<Vector3Int, VoxelInfo>>
    {
        /// <summary>
        /// Gets the <see cref="VoxelInfo"/> for the voxel at a given location
        /// within the chunk.
        /// </summary>
        VoxelInfo this[int x, int y, int z] { get; }

        /// <summary>
        /// Gets the width of the chunk in the x-axis.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the height of the chunk in the y-axis.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets the depth of the chunk in the z-axis.
        /// </summary>
        int Depth { get; }
        
        /// <summary>
        /// Gets or sets the offset of the chunk in the x-axis.
        /// </summary>
        int OffsetX { get; set; }
        
        /// <summary>
        /// Gets or sets the offset of the chunk in the y-axis.
        /// </summary>
        int OffsetY { get; set; }
        
        /// <summary>
        /// Gets or sets the offset of the chunk in the z-axis.
        /// </summary>
        int OffsetZ { get; set; }

        /// <summary>
        /// Fills the chunk with voxels from a provider.
        /// </summary>
        /// <param name="voxelProvider">The object which provides voxel info.</param>
        void Load(IVoxelProvider voxelProvider);
    }
}
