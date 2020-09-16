using Rebirth.Terrain.Generation.Noise;
using UnityEngine;

namespace Rebirth.Terrain.Generation.Nodes
{
    [CreateNodeMenu("3D Noise/Simplex 3D")]
    public class Simplex3DNode : TerrainNode<float>
    {
        [Output, SerializeField] private float _value;
    
        [SerializeField] private Vector3 _frequency = Vector3.one;
        [SerializeField] private Vector3 _offset = Vector3.zero;
        [SerializeField] private float _amplitude = 1;
        protected override float Generate(Vector3Int location)
        {
            // TODO: Seeding
            return _amplitude * SimplexNoise.Generate(
                location.x * _frequency.x + _offset.x,
                location.y * _frequency.y + _offset.y,
                location.z * _frequency.z + _offset.z
            );
        }
    }
}
