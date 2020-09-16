using System;
using Rebirth.Terrain.Voxel;
using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    public class ConditionalNode<T> : Node
    {
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private bool _condition;

        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] protected T _trueValue;
        
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] protected T _falseValue;

        [Output, SerializeField] protected T _output;
        
        public sealed override object GetValue(NodePort port)
        {
            var condition = GetInputValue<Generator<bool>>(nameof(_condition), _ => _condition);
            var trueValue = GetInputValue<Generator<T>>(nameof(_trueValue), _ => _trueValue);
            var falseValue = GetInputValue<Generator<T>>(nameof(_falseValue), _ => _falseValue);
            return new Generator<T>(i => condition(i) ? trueValue(i) : falseValue(i));
        }
    }

    [CreateNodeMenu("Control Flow/Conditional (Float)")]
    public class ConditionalFloatNode : ConditionalNode<float>
    {
    }

    [CreateNodeMenu("Control Flow/Conditional (Voxel Type)")]
    public class ConditionalVoxelTypeNode : ConditionalNode<VoxelType>
    {
    }
}
