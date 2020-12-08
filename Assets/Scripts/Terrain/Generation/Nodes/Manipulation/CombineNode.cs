using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Manipulation/Combine")]
    public class CombineNode : TerrainNode<float, float, float>
    {
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private float _input1;
        
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private float _input2;

        [Output, SerializeField] private float _output;

        [NodeEnum]
        [SerializeField] private CombineOperation _operation;

        [NodeEnum]
        [SerializeField] private SmoothingMode _smoothing;

        [SerializeField] private float _amount;
        
        private enum CombineOperation
        {
            Union,
            Intersect,
            Difference
        }

        private enum SmoothingMode
        {
            None,
            Polynomial,
            Exponential
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
                case CombineOperation.Intersect:
                    return -SmoothMin(-input1, -input2, _amount, _smoothing);
                case CombineOperation.Union:
                    return SmoothMin(input1, input2, _amount, _smoothing);
                case CombineOperation.Difference:
                    return -SmoothMin(-input1, input2, _amount, _smoothing);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Calculates a smoothed minimum of two floats.
        /// </summary>
        /// <param name="a">The first input.</param>
        /// <param name="b">The second input.</param>
        /// <param name="k">The amount of smoothing.</param>
        /// <param name="mode">The smoothing mode.</param>
        /// <returns>The smoothed minimum value.</returns>
        private static float SmoothMin(float a, float b, float k, SmoothingMode mode)
        {
            switch (mode)
            {
                case SmoothingMode.None:
                    return Mathf.Min(a, b);
                case SmoothingMode.Polynomial:
                    // k approx 0.1
                    var h = Mathf.Clamp(0.5f + 0.5f * (b-a) / k, 0, 1);
                    return Mathf.Lerp(b, a, h) - k * h * (1.0f - h);
                case SmoothingMode.Exponential:
                    // k approx 32
                    var res = Mathf.Pow(2, -k * a) + Mathf.Pow(2, -k * b);
                    return -Mathf.Log(res, 2)/k;
                default:
                    return 0;
            }
        }
    }
}
