using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Manipulation/Math")]
    public class MathNode : TerrainNode<float, float, float>
    {
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private float _input1;
        
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private float _input2;

        [Output, SerializeField] private float _output;

        [NodeEnum]
        [SerializeField] private MathOperation _operation;

        private enum MathOperation
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            Max,
            Min
        }
        protected override Generator<float> GetDelegate(NodePort port)
        {
            var input1 = GetInputValue<Generator<float>>(nameof(_input1), _ => _input1);
            var input2 = GetInputValue<Generator<float>>(nameof(_input2), _ => _input2);
            return CreateDelegate(input1, input2);
        }

        protected override float Generate(Vector3Int location, float input1, float input2)
        {
            switch (_operation)
            {
                case MathOperation.Add:
                    return input1 + input2;
                case MathOperation.Subtract:
                    return input1 - input2;
                case MathOperation.Multiply:
                    return input1 * input2;
                case MathOperation.Divide:
                    return input1 / input2;
                case MathOperation.Max:
                    return Mathf.Max(input1, input2);
                case MathOperation.Min:
                    return Mathf.Min(input1, input2);
                default: return 0;
            }
        }
    }
}
