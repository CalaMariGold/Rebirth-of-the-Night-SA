using System;
using UnityEngine;

namespace Rebirth.Terrain.Generation.Nodes
{
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
}
