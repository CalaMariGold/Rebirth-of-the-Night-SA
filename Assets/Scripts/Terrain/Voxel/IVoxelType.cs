using UnityEngine;

namespace Rebirth.Terrain.Voxel
{
    /// <summary>
    /// Represents a type of voxel.
    /// </summary>
    public interface IVoxelType
    {
        /// <summary>
        /// Gets a unique identifier for this type, used for serialization.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Gets the colour of this voxel type.
        /// </summary>
        /// <remarks>
        /// This will probably be changed later to support textures.
        /// </remarks>
        Color Colour { get; }
    }
}
