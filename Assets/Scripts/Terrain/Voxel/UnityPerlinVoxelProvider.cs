using UnityEngine;

namespace Rebirth.Terrain.Voxel
{
    /// <summary>
    /// Provides a simple configurable Perlin Noise terrain.
    /// </summary>
    public class UnityPerlinVoxelProvider : IVoxelProvider
    {
        private readonly Vector2 _frequency;
        private readonly float _amplitude;
        private readonly float _baseHeight;

        /// <summary>
        /// Initialises a new instance of the <see cref="UnityPerlinVoxelProvider"/> class.
        /// </summary>
        /// <param name="baseHeight">The base ground height of the terrain.</param>
        /// <param name="amplitude">The amplitude of the Perlin Noise.</param>
        /// <param name="frequency">The 2D frequency of the Perlin Noise.</param>
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
