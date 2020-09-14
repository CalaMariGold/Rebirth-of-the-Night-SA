using Rebirth.Terrain.Voxel;
using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Output/Voxel Output")]
    public class VoxelOutputNode : Node
    {
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private float _distance;
        
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private VoxelType _voxelType;

        public Generator<VoxelInfo> GetValue()
        {
            var distance = GetInputValue<Generator<float>>("_distance", _ => _distance);
            var voxelType = GetInputValue<Generator<VoxelType>>("_voxelType", _ => _voxelType);
            return i => new VoxelInfo
            {
                Distance = distance(i),
                VoxelType = voxelType(i)
            };
        }
    }
}
