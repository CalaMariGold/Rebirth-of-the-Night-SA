using System.Linq;
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

        public Generator<VoxelInfo> GetValue()
        {
            if (_outputNode == null)
            {
                _outputNode = nodes.OfType<VoxelOutputNode>().First();
            }
            return _outputNode.GetValue();
        }
    }
}
