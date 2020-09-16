using System;
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

    [CreateNodeMenu("Control Flow/Float Comparison")]
    public class FloatComparisonNode : ComparisonNode<float>
    {
        protected override bool Generate(Vector3Int location, float input1, float input2)
        {
            switch (_operator)
            {
                case ComparisonOperator.EqualTo:
                    return Mathf.Approximately(input1, input2);
                case ComparisonOperator.NotEqualTo:
                    return !Mathf.Approximately(input1, input2);
                case ComparisonOperator.GreaterThan:
                    return input1 > input2;
                case ComparisonOperator.LessThan:
                    return input1 < input2;
                case ComparisonOperator.GreaterOrEqual:
                    return input1 >= input2;
                case ComparisonOperator.LessOrEqual:
                    return input1 <= input2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    [CreateNodeMenu("Control Flow/Int Comparison")]
    public class IntComparisonNode : ComparisonNode<int>
    {
        protected override bool Generate(Vector3Int location, int input1, int input2)
        {
            switch (_operator)
            {
                case ComparisonOperator.EqualTo:
                    return input1 == input2;
                case ComparisonOperator.NotEqualTo:
                    return input1 != input2;
                case ComparisonOperator.GreaterThan:
                    return input1 > input2;
                case ComparisonOperator.LessThan:
                    return input1 < input2;
                case ComparisonOperator.GreaterOrEqual:
                    return input1 >= input2;
                case ComparisonOperator.LessOrEqual:
                    return input1 <= input2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
