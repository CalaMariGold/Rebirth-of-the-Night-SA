using Rebirth.Terrain.Generation.Nodes;
using Rebirth.Terrain.Voxel;
using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation
{
    [CreateAssetMenu(menuName = "Terrain Graph")]
    public class TerrainGraph : NodeGraph
    { 
        [SerializeField] private VoxelOutputNode _outputNode;

        public TerrainNode.Generator<VoxelInfo> GetValue()
        {
            return _outputNode.GetValue();
        }
    }
}
