using System;
using Rebirth.Terrain.Chunk;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Behaviours
{
    [RequireComponent(typeof(ChunkManager))]
    public class SimpleTerrainManager : MonoBehaviour
    {
        [SerializeField] private int _chunkSize;
        private ChunkManager _chunkManager;

        public void Awake()
        {
            _chunkManager = GetComponent<ChunkManager>();
            _chunkManager.Setup(ChunkFactory, new FlatLandVoxelProvider(5.0f));
        }

        private IChunk ChunkFactory(int x, int y, int z)
        {
            return new ArrayChunk(
                _chunkSize, _chunkSize, _chunkSize,
                x, y, z
            );
        }
    }
}
