using UnityEngine;
using XNode;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("2D Noise/Unity Perlin")]
    public class UnityPerlinNode : TerrainNode<float>
    {
        [Output, SerializeField] private float _value;

        [SerializeField] private Vector2 _frequency = Vector2.one;
        [SerializeField] private Vector2 _offset = Vector2.zero;
        [SerializeField] private float _amplitude = 1;

        protected override float Generate(Vector3Int location)
        {
            return _amplitude * Mathf.PerlinNoise(
                location.x * _frequency.x + _offset.x,
                location.z * _frequency.y + _offset.y
            );
        }
    }
}
