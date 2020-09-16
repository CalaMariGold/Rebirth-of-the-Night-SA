using System;
using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    public abstract class ConditionalNode<T> : Node
    {
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private bool _condition;

        [Input(connectionType: ConnectionType.Override)]
        
#pragma warning disable 649
        [SerializeField] private T _trueValue;

        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private T _falseValue;
#pragma warning restore 649
        
        [Output, SerializeField] private T _output;
        
        public sealed override object GetValue(NodePort port)
        {
            var condition = GetInputValue<Generator<bool>>(nameof(_condition), _ => _condition);
            var trueValue = GetInputValue<Generator<T>>(nameof(_trueValue), _ => _trueValue);
            var falseValue = GetInputValue<Generator<T>>(nameof(_falseValue), _ => _falseValue);
            return new Generator<T>(i => condition(i) ? trueValue(i) : falseValue(i));
        }
    }
}
