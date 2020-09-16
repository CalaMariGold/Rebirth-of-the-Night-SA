using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Manipulation/Height Conversion")]
    public class HeightNode : TerrainNode<float, float>
    {
        [Input(connectionType: ConnectionType.Override)]
        [SerializeField] private float _height;
        
        [Output, SerializeField] private float _value;

        [SerializeField] private float _baseHeight;

        protected override Generator<float> GetDelegate(NodePort port)
        {
            var height = GetInputValue<Generator<float>>("_height", _ => _height);
            return CreateDelegate(height);
        }

        protected override float Generate(Vector3Int location, float input)
        {
            return location.y - _baseHeight - input;
        }
    }
}
