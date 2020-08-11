using Rebirth.Terrain.Voxel;

namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Represents a chunk of voxels in space.
    /// </summary>
    public interface IChunk
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
        /// Fills the data array with voxels from a provider.
        /// </summary>
        /// <param name="voxelProvider">The object which provides voxel info.</param>
        void Load(IVoxelProvider voxelProvider);

        /// <summary>
        /// Returns a 1-D array containing the data in the chunk's 3-D data array.
        /// Indexing is determined with the following mapping:
        ///     index(x, y, z) = z << (chunkWidthBits + chunkHeightBits) | y << chunkWidthBits | x,
        /// where (x, y, z) are local data array indices.
        /// </summary>
        VoxelInfo[] ToArray();
    }
}
