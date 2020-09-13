using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("Manipulation/Height")]
    public class HeightNode : Node
    {
        [Input, SerializeField] private float _height;
        [Output, SerializeField] private float _value;

        [SerializeField] private float _baseHeight;

        public override object GetValue(NodePort port)
        {
            if (graph is TerrainGraph terrainGraph)
            {
                return terrainGraph.Position.y - _baseHeight - GetInputValue("_height", _height);
            }

            return null;
        }
    }
}
