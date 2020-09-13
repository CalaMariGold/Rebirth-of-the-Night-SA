using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Manipulation/Height Conversion")]
    public class HeightNode : TerrainNode
    {
        [Input, SerializeField] private float _height;
        [Output, SerializeField] private float _value;

        [SerializeField] private float _baseHeight;

        public override object GetValue(NodePort port)
        {
            var height = GetInputValue<Generator<float>>("_height", _ => _height);
            return new Generator<float>(i => i.y - _baseHeight - height(i));
        }
    }
}
