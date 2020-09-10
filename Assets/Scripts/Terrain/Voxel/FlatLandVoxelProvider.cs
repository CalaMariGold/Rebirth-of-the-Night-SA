namespace Rebirth.Terrain.Voxel
{
    /// <summary>
    /// Provides a completely flat terrain.
    /// </summary>
    public class FlatLandVoxelProvider : IVoxelProvider
    {
        private readonly float _groundHeight;

        /// <summary>
        /// Initialises a new instance of the <see cref="FlatLandVoxelProvider"/> class.
        /// </summary>
        /// <param name="groundHeight">The y-coordinate of the ground.</param>
        public FlatLandVoxelProvider(float groundHeight)
        {
            _groundHeight = groundHeight;
        }

        public VoxelInfo GetVoxelInfo(int x, int y, int z)
        {
            return new VoxelInfo
            {
                Distance = y - _groundHeight
            };
        }
    }
}
