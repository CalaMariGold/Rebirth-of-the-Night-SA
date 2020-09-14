using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Manipulation/Remap")]
    public class RemapNode : TerrainNode<float, float>
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)]
        [SerializeField] private float _input;
        
        [SerializeField] private float _inputLow;
        [SerializeField] private float _inputHigh;
        
        [Output, SerializeField] private float _output;
        
        [SerializeField] private float _outputLow;
        [SerializeField] private float _outputHigh;
        protected override Generator<float> GetDelegate(NodePort port)
        {
            var input = GetInputValue<Generator<float>>(nameof(_input), _ => _input);
            return CreateDelegate(input);
        }

        protected override float Generate(Vector3Int location, float input)
        {
            var remapped01 = (input - _inputLow) / (_inputHigh - _inputLow);
            return remapped01 * (_outputHigh - _outputLow) + _inputLow;
        }
    }
}
