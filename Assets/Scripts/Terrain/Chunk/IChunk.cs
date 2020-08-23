using System;
using System.Collections.Generic;
using System.IO;
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
        /// <remarks>
        /// I'm not totally happy with this current architecture for loading chunks.
        /// I think for it to be properly extensible, it will make sense to have another interface
        /// which manages all forms of chunk loading e.g. <c>IChunkLoader</c>. This way there will be a
        /// unified interface for loading from files, procedural generation etc. so that the
        /// <see cref="ChunkManager"/> can remain decoupled from the chunk loading implementation.
        /// TODO: Create a unified interface for chunk data loading.
        /// </remarks>
        void Load(IVoxelProvider voxelProvider);

        /// <summary>
        /// Serializes a chunk to a stream.
        /// </summary>
        /// <param name="writer">An object used to write to the stream.</param>
        void Serialize(BinaryWriter writer);

        /// <summary>
        /// Deserializes a chunk from a stream.
        /// </summary>
        /// <param name="reader">An object used to read from the stream.</param>
        /// <param name="voxelTypeProvider">A delegate to look up voxel types by ID.</param>
        /// <returns><c>true</c> if the chunk could be deserialized; otherwise, <c>false</c>.</returns>
        bool Deserialize(BinaryReader reader, Func<int, IVoxelType> voxelTypeProvider);
    }
}
