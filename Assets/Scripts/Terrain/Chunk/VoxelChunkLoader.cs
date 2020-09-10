using Rebirth.Terrain.Voxel;

namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Provides data to a chunk using an <see cref="IVoxelProvider"/>.
    /// </summary>
    public class VoxelChunkLoader : IChunkLoader
    {
        private readonly IVoxelProvider _voxelProvider;
        
        /// <summary>
        /// Initialises a new instance of the <see cref="VoxelChunkLoader"/> class.
        /// </summary>
        /// <param name="voxelProvider">The voxel provider to use when loading the chunk.</param>
        public VoxelChunkLoader(IVoxelProvider voxelProvider)
        {
            _voxelProvider = voxelProvider;
        }
        
        /// <summary>
        /// Load a chunk with data.
        /// </summary>
        /// <param name="chunk">The chunk to fill.</param>
        public void Load(IChunk chunk)
        {
            chunk.Load(_voxelProvider);
        }
    }
}
