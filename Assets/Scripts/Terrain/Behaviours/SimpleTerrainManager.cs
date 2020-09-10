using Rebirth.Terrain.Chunk;
using Rebirth.Terrain.Meshing;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Behaviours
{
    [RequireComponent(typeof(ChunkManager))]
    [RequireComponent(typeof(MeshManager))]
    public class SimpleTerrainManager : MonoBehaviour, IChunkLoader
    {
        [SerializeField] private int _chunkSize;
        [SerializeField] private Vector2 _offset;
        private ChunkManager _chunkManager;
        private IChunkLoader _chunkLoader;

        public void Awake()
        {
            _chunkManager = GetComponent<ChunkManager>();
            var voxelProvider = new UnityPerlinVoxelProvider(
                0, 5, Vector2.one * 0.1f, _offset
            );
            _chunkLoader = new VoxelChunkLoader(voxelProvider);
            // This should be improved using the component system,
            // to avoid circular dependencies
            _chunkManager.Setup(
                ChunkFactory
            );
        }

        private IChunk ChunkFactory(int x, int y, int z)
        {
            return new ArrayChunk(
                _chunkSize, _chunkSize, _chunkSize,
                x, y, z
            );
        }

        public void Load(IChunk chunk)
        {
            _chunkLoader.Load(chunk);
        }
    }
}
