using Rebirth.Terrain.Generation.Noise;
using UnityEngine;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("2D Noise/Fractal 2D")]
    public class Fractal2DNode : TerrainNode<float>
    {
        [Output, SerializeField] private float _value;
        
        [SerializeField] private Vector2 _frequency = Vector2.one;
        [SerializeField] private Vector2 _offset = Vector2.zero;
        [SerializeField] private float _amplitude = 1;
        [SerializeField] private float _lacunarity = 2.0f;
        [SerializeField] private int _octaves = 3;
        [SerializeField] private float _persistence = 0.75f;
        protected override float Generate(Vector3Int location)
        {
            var result = 0.0f;
            var amplitude = _amplitude;
            var scale = _frequency;
            for (var i = 0; i < _octaves; i++)
            {
                result += amplitude * SimplexNoise.Generate(
                    location.x * scale.x + _offset.x,
                    location.z * scale.y + _offset.y
                );
                amplitude *= _persistence;
                scale *= _lacunarity;
            }

            return result;
        }
    }
}
