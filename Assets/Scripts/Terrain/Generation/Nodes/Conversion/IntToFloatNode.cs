using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Convert/Int to Float")]
    [NodeWidth(100)]
    public class IntToFloatNode : TerrainNode<int, float>
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)]
        [SerializeField] private int _input;

        [Output, SerializeField] private float _output;
        protected override Generator<float> GetDelegate(NodePort port)
        {
            var input = GetInputValue<Generator<int>>(nameof(_input), _ => _input);
            return CreateDelegate(input);
        }

        protected override float Generate(Vector3Int location, int input)
        {
            return input;
        }
    }
}
