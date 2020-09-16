using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    public abstract class ComparisonNode<T> : TerrainNode<T, T, bool>
    {
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private T _input1;

        [NodeEnum]
        [SerializeField] protected ComparisonOperator _operator;
        
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private T _input2;

        [Output, SerializeField] private bool _output;

        protected enum ComparisonOperator
        {
            EqualTo,
            NotEqualTo,
            GreaterThan,
            LessThan,
            GreaterOrEqual,
            LessOrEqual
        }
        protected override Generator<bool> GetDelegate(NodePort port)
        {
            var input1 = GetInputValue<Generator<T>>(nameof(_input1), _ => _input1);
            var input2 = GetInputValue<Generator<T>>(nameof(_input2), _ => _input2);
            return CreateDelegate(input1, input2);
        }
    }
}
