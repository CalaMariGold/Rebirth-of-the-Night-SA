namespace Rebirth.Terrain.Voxel
{
    /// <summary>
    /// Represents a single voxel.
    /// </summary>
    public struct VoxelInfo
    {
        /// <summary>
        /// Gets or sets the distance from the surface of the terrain.
        /// </summary>
        /// <remarks>
        /// Should be positive if the voxel is outside the terrain,
        /// and negative if the voxel is within the terrain.
        /// </remarks>
        public float Distance;
    }
}
