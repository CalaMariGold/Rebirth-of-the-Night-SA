namespace Rebirth.Terrain.Voxel
{
    /// <summary>
    /// Represents a single voxel.
    /// </summary>
    // Still not sure if it's wise to use a struct and not a class here.
    // Worth looking into.
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

        /// <summary>
        /// Gets or sets the type of voxel.
        /// </summary>
        public IVoxelType VoxelType;
    }
}
