using UnityEngine;

namespace Rebirth.Terrain.Voxel
{
    public class HeightMapVoxelProvider : IVoxelProvider
    {
        public Texture2D HeightMap { get; set; }
        public Vector2 Scale { get; set; } = Vector2.one;
        public Vector2 Offset { get; set; } = Vector2.zero;
        public float Amplitude { get; set; } = 1.0f;
        public float BaseHeight { get; set; }
        public float RockHeight { get; set; }
        public float SnowHeight { get; set; }
        public VoxelType Snow { get; set; }
        public VoxelType Rock { get; set; }
        public VoxelType Grass { get; set; }
        
        public VoxelInfo GetVoxelInfo(int x, int y, int z)
        {
            var rawHeight = HeightMap.GetPixelBilinear(
                x / Scale.x + Offset.x,
                z / Scale.y + Offset.y
            ).grayscale;
            return new VoxelInfo
            {
                Distance = y - BaseHeight - rawHeight * Amplitude,
                VoxelType = GetVoxelType(y)
            };
        }

        private VoxelType GetVoxelType(int height)
        {
            if (height > SnowHeight)
            {
                return Snow;
            }

            return height > RockHeight ? Rock : Grass;
        }
    }
}
