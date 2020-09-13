using Rebirth.Terrain.Chunk;
using Rebirth.Terrain.Meshing;
using Rebirth.Terrain.Generation;
using UnityEngine;

namespace Rebirth.Terrain.Behaviours
{
    [RequireComponent(typeof(ChunkManager))]
    [RequireComponent(typeof(MeshManager))]
    public class GraphTerrainManager : MonoBehaviour, IChunkLoader
    {
        [SerializeField] private int _chunkSize;
        [SerializeField] private TerrainGraph _graph;
        private ChunkManager _chunkManager;

        public void Awake()
        {
            _chunkManager = GetComponent<ChunkManager>();
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
            for (var x = 0; x < chunk.Width; x++)
            {
                for (var y = 0; y < chunk.Height; y++)
                {
                    for (var z = 0; z < chunk.Depth; z++)
                    {
                        _graph.Position = new Vector3Int(
                            x + chunk.OffsetX,
                            y + chunk.OffsetY,
                            z + chunk.OffsetZ
                        );
                        chunk[x, y, z] = _graph.GetValue();
                    }
                }
            }
        }
    }
}
