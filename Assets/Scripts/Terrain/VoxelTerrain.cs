using Rebirth.Terrain.Voxel;

namespace Rebirth.Terrain
{
    /// <summary>
    /// Represents a voxel-based terrain.
    /// </summary>
    public class VoxelTerrain
    {
        private IVoxelProvider _voxelProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="VoxelTerrain"/> class.
        /// </summary>
        /// <param name="voxelProvider">
        /// An <seealso cref="IVoxelProvider"/> to load or generate terrain.
        /// </param>
        public VoxelTerrain(IVoxelProvider voxelProvider)
        {
            _voxelProvider = voxelProvider;
        }
    }
}
