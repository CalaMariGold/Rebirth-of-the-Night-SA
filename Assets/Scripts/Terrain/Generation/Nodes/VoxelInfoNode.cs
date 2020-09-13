using System;
using Rebirth.Terrain.Voxel;
using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Output/Voxel Info")]
    public class VoxelInfoNode : Node
    {
        [Input, SerializeField] private float _distance = 0;
        [Input, SerializeField] private VoxelType _voxelType;

        public VoxelInfo GetValue()
        {
            return new VoxelInfo
            {
                Distance = GetInputValue("_distance", _distance),
                VoxelType = GetInputValue("_voxelType", _voxelType)
            };
        }
    }
}
