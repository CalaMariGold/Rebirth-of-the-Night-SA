using System;
using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Manipulation/Unary Math")]
    public class UnaryMathNode : TerrainNode<float, float>
    {
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private float _input;

        [NodeEnum, SerializeField] private UnaryOperation _operation;

        [Output, SerializeField] private float _output;

        private enum UnaryOperation
        {
            Abs,
            Sqrt,
            Exp,
            Log,
            Sin,
            Cos,
            Tan,
            Asin,
            Acos,
            Atan
        }
        
        protected override Generator<float> GetDelegate(NodePort port)
        {
            var input = GetInputValue<Generator<float>>(nameof(_input), _ => _input);
            return CreateDelegate(input);
        }

        protected override float Generate(Vector3Int location, float input)
        {
            switch (_operation)
            {
                case UnaryOperation.Abs:
                    return Mathf.Abs(input);
                case UnaryOperation.Sqrt:
                    return Mathf.Sqrt(input);
                case UnaryOperation.Exp:
                    return Mathf.Exp(input);
                case UnaryOperation.Log:
                    return Mathf.Log(input);
                case UnaryOperation.Sin:
                    return Mathf.Sin(input);
                case UnaryOperation.Cos:
                    return Mathf.Cos(input);
                case UnaryOperation.Tan:
                    return Mathf.Tan(input);
                case UnaryOperation.Asin:
                    return Mathf.Asin(input);
                case UnaryOperation.Acos:
                    return Mathf.Acos(input);
                case UnaryOperation.Atan:
                    return Mathf.Atan(input);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
