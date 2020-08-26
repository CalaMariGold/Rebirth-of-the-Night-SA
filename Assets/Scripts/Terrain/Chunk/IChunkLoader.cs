namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Provides voxel data for a chunk.
    /// </summary>
    public interface IChunkLoader
    {
        /// <summary>
        /// Load a chunk with data.
        /// </summary>
        /// <param name="chunk">The chunk to fill.</param>
        void Load(IChunk chunk);
    }
}
