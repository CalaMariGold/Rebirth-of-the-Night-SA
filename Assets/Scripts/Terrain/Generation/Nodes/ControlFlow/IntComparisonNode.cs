using System;
using UnityEngine;

namespace Rebirth.Terrain.Generation.Nodes
{
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
