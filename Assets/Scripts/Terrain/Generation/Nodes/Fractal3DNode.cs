using Rebirth.Terrain.Generation.Noise;
using UnityEngine;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("3D Noise/Fractal 3D")]
    public class Fractal3DNode : TerrainNode<float>
    {
        [Output, SerializeField] private float _value;
        
        [SerializeField] private Vector3 _frequency = Vector3.one;
        [SerializeField] private Vector3 _offset = Vector3.zero;
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
                    location.y * scale.y + _offset.y,
                    location.z * scale.z + _offset.z
                );
                amplitude *= _persistence;
                scale *= _lacunarity;
            }

            return result;
        }
    }
}
