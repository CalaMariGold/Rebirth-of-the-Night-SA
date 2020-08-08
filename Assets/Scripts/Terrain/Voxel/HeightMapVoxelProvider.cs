using UnityEngine;

namespace Rebirth.Terrain.Voxel
{
    public class HeightMapVoxelProvider : IVoxelProvider
    {
        private readonly Texture2D _heightMap;
        private readonly Vector2 _scale;
        private readonly Vector2 _offset;
        private readonly float _amplitude;
        private readonly float _baseHeight;

        public HeightMapVoxelProvider(Texture2D heightMap, float amplitude,
            float baseHeight = 0, Vector2 offset = default)
            : this(heightMap, amplitude, Vector2.one, baseHeight, offset) { }

        public HeightMapVoxelProvider(Texture2D heightMap, float amplitude,
            Vector2 scale, float baseHeight = 0, Vector2 offset = default)
        {
            _heightMap = heightMap;
            _scale = scale;
            _offset = offset;
            _amplitude = amplitude;
            _baseHeight = baseHeight;
        }
        
        public VoxelInfo GetVoxelInfo(int x, int y, int z)
        {
            var rawHeight = _heightMap.GetPixelBilinear(
                x / _scale.x + _offset.x,
                z / _scale.y + _offset.y
            ).grayscale;
            return new VoxelInfo
            {
                Distance = y - _baseHeight - rawHeight * _amplitude
            };
        }
    }
}
