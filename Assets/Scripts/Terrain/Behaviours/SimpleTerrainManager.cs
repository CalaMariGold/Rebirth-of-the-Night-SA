using Rebirth.Terrain.Chunk;
using Rebirth.Terrain.Meshing;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Behaviours
{
    [RequireComponent(typeof(ChunkManager))]
    [RequireComponent(typeof(MeshManager))]
    public class SimpleTerrainManager : MonoBehaviour
    {
        [SerializeField] private int _chunkSize;
        private ChunkManager _chunkManager;
        private MeshManager _meshManager;

        public void Awake()
        {
            _chunkManager = GetComponent<ChunkManager>();
            _chunkManager.Setup(ChunkFactory, new UnityPerlinVoxelProvider(0, 5, Vector2.one * 0.1f));
            _meshManager = GetComponent<MeshManager>();
            _meshManager.Setup(new MarchingCubes());
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
