using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("2D Noise/Unity Perlin")]
    public class UnityPerlinNode : Node
    {
        [Output, SerializeField] private float _value;

        [SerializeField] private Vector2 _frequency = Vector2.one;
        [SerializeField] private Vector2 _offset = Vector2.zero;
        [SerializeField] private float _amplitude = 1;

        public override object GetValue(NodePort port)
        {
            if (graph is TerrainGraph terrainGraph)
            {
                return _amplitude * Mathf.PerlinNoise(
                    terrainGraph.Position.x * _frequency.x + _offset.x,
                    terrainGraph.Position.z * _frequency.y + _offset.y
                );
            }

            return null;
        }
    }
}
