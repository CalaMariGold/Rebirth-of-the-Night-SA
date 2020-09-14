using Rebirth.Terrain.Generation.Noise;
using UnityEngine;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("2D Noise/Simplex 2D")]
    public class Simplex2DNode : TerrainNode<float>
    {
        [Output, SerializeField] private float _value;
        
        [SerializeField] private Vector2 _frequency = Vector2.one;
        [SerializeField] private Vector2 _offset = Vector2.zero;
        [SerializeField] private float _amplitude = 1;
        protected override float Generate(Vector3Int location)
        {
            // TODO: Seeding
            return _amplitude * SimplexNoise.Generate(
                location.x * _frequency.x + _offset.x,
                location.z * _frequency.y + _offset.y
            );
        }
    }
}
