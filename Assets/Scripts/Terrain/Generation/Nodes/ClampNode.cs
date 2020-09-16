using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Manipulation/Clamp")]
    public class ClampNode : TerrainNode<float, float>
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)]
        [SerializeField] private float _input;

        [SerializeField] private float _clampLow = -1;
        [SerializeField] private float _clampHigh = 1;
        
        [Output, SerializeField] private float _output;
        protected override Generator<float> GetDelegate(NodePort port)
        {
            var input = GetInputValue<Generator<float>>(nameof(_input), _ => _input);
            return CreateDelegate(input);
        }

        protected override float Generate(Vector3Int location, float input)
        {
            return Mathf.Clamp(input, _clampLow, _clampHigh);
        }
    }
}
