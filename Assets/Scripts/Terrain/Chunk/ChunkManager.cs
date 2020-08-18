using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Asynchronously loads chunks around a target transform.
    /// </summary>
    public class ChunkManager : MonoBehaviour
    {
        [SerializeField] private Transform _chunkLoadingTarget;
        [SerializeField] private int _chunkSize;
        [SerializeField] private int _chunkLoadDistance;
        
        // Currently loaded chunks
        private Dictionary<Vector3Int, IChunk> _loadedChunks;
        // Which chunks are we waiting for load
        private HashSet<Vector3Int> _loadingChunks;
        // Freshly loaded chunks to be consumed by the main thread into _loadedChunks on update
        private ConcurrentQueue<(Vector3Int, IChunk)> _freshChunks;
        // Chunk load queue consumed by the load thread
        private BlockingCollection<Vector3Int> _chunksToLoad;
        // Chunk creation function for DI
        private Func<int, int, int, IChunk> _chunkFactory;
        // Voxel provider for DI
        private IVoxelProvider _voxelProvider;
        // Provides a token used to cancel the asynchronous load task
        private CancellationTokenSource _tokenSource;

        /// <summary>
        /// Set up the Chunk Manager by dependency injection.
        /// </summary>
        /// <param name="chunkFactory">
        /// A function which produces an <see cref="IChunk"/> for a given location.
        /// </param>
        /// <param name="voxelProvider">
        /// An <see cref="IVoxelProvider"/> which will be used to fill chunks.
        /// </param>
        public void Setup(Func<int, int, int, IChunk> chunkFactory, IVoxelProvider voxelProvider)
        {
            _chunkFactory = chunkFactory;
            _voxelProvider = voxelProvider;
        }

        private void Awake()
        {
            // Create required objects and collections for Async chunk loads
            _tokenSource = new CancellationTokenSource();
            _loadedChunks = new Dictionary<Vector3Int, IChunk>();
            _loadingChunks = new HashSet<Vector3Int>();
            _freshChunks = new ConcurrentQueue<(Vector3Int, IChunk)>();
            _chunksToLoad = new BlockingCollection<Vector3Int>(new ConcurrentQueue<Vector3Int>());
        }

        private void OnEnable()
        {
            // Consume the cancellation token and use it to start the chunk load task
            var token = _tokenSource.Token;
            Task.Run(() => LoadChunks(token), token);
        }

        private void OnDisable()
        {
            // Cancel the chunk load task
            _tokenSource.Cancel();
        }

        /// <summary>
        /// Load chunks from the load queue.
        /// </summary>
        /// <param name="ct">A token used to cancel the asynchronous task.</param>
        /// <remarks>This action will be run asynchronously.</remarks>
        private void LoadChunks(CancellationToken ct)
        {
            if (_chunkFactory == null || _voxelProvider == null)
            {
                return;
            }
            while (!ct.IsCancellationRequested)
            {
                Vector3Int chunkToLoad;
                try
                {
                    chunkToLoad = _chunksToLoad.Take(ct);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                // Load chunk by location
                var offset = chunkToLoad * _chunkSize;
                // TODO: use recyclable chunks where possible
                var chunk = _chunkFactory(offset.x, offset.y, offset.z);
                chunk.Load(_voxelProvider);
                _freshChunks.Enqueue((chunkToLoad, chunk));
            }
        }

        private void Update()
        {
            QueueNewChunkLoads();
            ApplyFreshChunks();
        }

        /// <summary>
        /// Consume freshly loaded chunks and add them to the loaded chunk collection.
        /// </summary>
        private void ApplyFreshChunks()
        {
            while (_freshChunks.TryDequeue(out var data))
            {
                _loadedChunks.Add(data.Item1, data.Item2);
                _loadingChunks.Remove(data.Item1);
            }
        }

        /// <summary>
        /// Decide which chunks to load and recycle chunks no longer needed.
        /// </summary>
        private void QueueNewChunkLoads()
        {
            var chunksToLoad = new HashSet<Vector3Int>();
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

                        chunksToLoad.Add(coord + currentChunk);
                    }
                }
            }
            
            // Recycle old loaded chunk memory we no longer need
            foreach (var chunkPos in _loadedChunks.Keys.Except(chunksToLoad).ToArray())
            {
                _loadedChunks.Remove(chunkPos);
                // TODO: _recyclableChunks.Add(oldChunk);
            }
            
            // Add chunks we still need to load to the load queue
            foreach (var chunkPos in chunksToLoad.Except(_loadedChunks.Keys).Except(_loadingChunks).ToArray())
            {
                _loadingChunks.Add(chunkPos);
                _chunksToLoad.Add(chunkPos);
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
                var worldSpaceCoord = (chunkCoord.Key + new Vector3(0.5f, 0.5f, 0.5f)) * _chunkSize;
                Gizmos.DrawWireCube(worldSpaceCoord, Vector3.one * _chunkSize);
            }
        }
    }
}
