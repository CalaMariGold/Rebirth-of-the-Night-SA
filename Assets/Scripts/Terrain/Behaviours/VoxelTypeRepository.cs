using System.Collections.Generic;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Behaviours
{
    /// <summary>
    /// Provides a list of simple voxel types.
    /// </summary>
    public class VoxelTypeRepository : MonoBehaviour
    {
        [SerializeField] private List<SimpleVoxel> _voxelTypes;

        /// <summary>
        /// Gets an enumerable of voxel types.
        /// </summary>
        public IEnumerable<SimpleVoxel> VoxelTypes => _voxelTypes;
    }
}
