using Rebirth.Terrain.Generation.Nodes;
using Rebirth.Terrain.Voxel;
using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation
{
    [CreateAssetMenu(menuName = "Terrain Graph")]
    public class TerrainGraph : NodeGraph
    { 
        [SerializeField] private VoxelInfoNode _outputNode;
        public Vector3Int Position { get; set; }

        public VoxelInfo GetValue()
        {
            return _outputNode.GetValue();
        }
    }
}
