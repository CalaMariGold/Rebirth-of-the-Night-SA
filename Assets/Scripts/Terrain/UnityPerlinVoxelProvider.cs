using UnityEngine;

namespace Rebirth.Terrain
{
    public class UnityPerlinVoxelProvider : IVoxelProvider
    {
        private readonly Vector2 _frequency;
        private readonly float _amplitude;
        private readonly float _baseHeight;

        public UnityPerlinVoxelProvider(float baseHeight, float amplitude, Vector2 frequency)
        {
            _baseHeight = baseHeight;
            _amplitude = amplitude;
            _frequency = frequency;
        }
        public VoxelInfo GetVoxelInfo(int x, int y, int z)
        {
            return new VoxelInfo
            {
                Distance = y - _baseHeight - (
                    _amplitude * Mathf.PerlinNoise(x * _frequency.x, z * _frequency.y)
                    )
            };
        }
    }
}
