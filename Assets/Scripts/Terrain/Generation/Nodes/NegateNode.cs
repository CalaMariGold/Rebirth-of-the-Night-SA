using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Control Flow/Negate")]
    [NodeWidth(100)]
    public class NegateNode : TerrainNode<bool, bool>
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override)]
        [SerializeField] private bool _input;

        [Output, SerializeField] private bool _output;
        
        protected override Generator<bool> GetDelegate(NodePort port)
        {
            var input = GetInputValue<Generator<bool>>(nameof(_input), _ => _input);
            return CreateDelegate(input);
        }

        protected override bool Generate(Vector3Int location, bool input)
        {
            return !input;
        }
    }
}
