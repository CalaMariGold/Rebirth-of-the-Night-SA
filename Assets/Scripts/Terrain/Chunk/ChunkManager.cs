using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rebirth.Terrain.Chunk
{
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField] private Transform _chunkLoadingTarget;
        [SerializeField] private int _chunkSize;
        [SerializeField] private int _chunkLoadDistance;
        
        // TODO: change to an actual Dictionary
        private HashSet<Vector3Int> _loadedChunks;

        private void Awake()
        {
            _loadedChunks = new HashSet<Vector3Int>();
        }

        private void Update()
        {
            var chunksToRender = new HashSet<Vector3Int>();
            var currentChunk = Vector3Int.FloorToInt(_chunkLoadingTarget.position / _chunkSize);
            // Get the set of chunks we want loaded this frame
            for (var x = -_chunkLoadDistance; x <= _chunkLoadDistance; x++)
            {
                for (var y = -_chunkLoadDistance; y <= _chunkLoadDistance; y++)
                {
                    for (var z = -_chunkLoadDistance; z <= _chunkLoadDistance; z++)
                    {
                        var coord = new Vector3Int(x, y, z);
                        // Ignore chunks outside rendering sphere
                        if (coord.magnitude > _chunkLoadDistance)
                        {
                            continue;
                        }
                        chunksToRender.Add(coord + currentChunk);
                    }
                }
            }
            // Recycle old loaded chunk memory we no longer need
            foreach (var chunkPos in _loadedChunks.Except(chunksToRender).ToArray())
            {
                _loadedChunks.Remove(chunkPos);
                // _recyclableChunks.Add(oldChunk);
            }
            // Add chunks we still need to load to the load queue
            foreach (var chunkPos in chunksToRender.Except(_loadedChunks).ToArray())
            {
                // TODO: use a threaded loading queue
                _loadedChunks.Add(chunkPos);
            }
            
        }

        private void OnDrawGizmos()
        {
            if (_loadedChunks == null)
            {
                return;
            }
            Gizmos.color = Color.red;
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var chunkCoord in _loadedChunks)
            {
                var worldSpaceCoord = (chunkCoord + new Vector3(0.5f, 0.5f, 0.5f)) * _chunkSize;
                Gizmos.DrawWireCube(worldSpaceCoord, Vector3.one * _chunkSize);
            }
        }
    }
}
