using UnityEngine;

namespace Rebirth.Terrain.Voxel
{
    /// <summary>
    /// Represents a type of voxel.
    /// </summary>
    [CreateAssetMenu(menuName = "Voxels/Voxel Type")]
    public class VoxelType : ScriptableObject
    {
        [SerializeField] private int _id = 0;
        [SerializeField] private Color _colour = Color.black;

        /// <summary>
        /// Gets or sets a unique identifier for this type, used for serialization.
        /// </summary>
        public int Id => _id;

        /// <summary>
        /// Gets or sets the colour of this voxel type.
        /// </summary>
        /// <remarks>
        /// This will probably be changed later to support textures.
        /// </remarks>
        public Color Colour => _colour;
    }
}
