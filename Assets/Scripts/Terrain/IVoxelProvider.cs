namespace Rebirth.Terrain
{
    /// <summary>
    /// Provides location-based voxel information.
    /// </summary>
    public interface IVoxelProvider
    {
        /// <summary>
        /// Provides info for a specific voxel, given its location.
        /// </summary>
        /// <param name="x">The x-coordinate of the voxel.</param>
        /// <param name="y">The y-coordinate of the voxel.</param>
        /// <param name="z">The z-coordinate of the voxel.</param>
        /// <returns>
        /// A <see cref="VoxelInfo"/> object corresponding to the voxel at the specified location.
        /// </returns>
        VoxelInfo GetVoxelInfo(int x, int y, int z);
    }
}
