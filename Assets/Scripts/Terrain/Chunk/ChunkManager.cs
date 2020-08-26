using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Rebirth.Terrain.Chunk
{
    /// <summary>
    /// Asynchronously loads chunks around a target transform.
    /// </summary>
    public class ChunkManager : MonoBehaviour
    {
        public event Action<Vector3Int> ChunkLoaded;
        public event Action<Vector3Int> ChunkUnloaded;
        
        [SerializeField] private Transform _chunkLoadingTarget;
        [SerializeField] private int _chunkSize;
        [SerializeField] private int _chunkLoadDistance;
        
        // Which chunks are we waiting for load
        private HashSet<Vector3Int> _loadingChunks;
        // Freshly loaded chunks to be consumed by the main thread into _loadedChunks on update
        private ConcurrentQueue<(Vector3Int, IChunk)> _freshChunks;
        // Chunk load queue consumed by the load thread
        private BlockingCollection<Vector3Int> _chunksToLoad;
        // Recyclable chunks which can be reused to save memory
        private ConcurrentBag<IChunk> _recyclableChunks;
        // Chunk creation function for DI
        private Func<int, int, int, IChunk> _chunkFactory;
        // Chunk loader for DI
        private IChunkLoader _chunkLoader;
        // Provides a token used to cancel the asynchronous load task
        private CancellationTokenSource _tokenSource;
        
        /// <summary>
        /// Gets the currently loaded chunks.
        /// </summary>
        public Dictionary<Vector3Int, IChunk> LoadedChunks { get; private set; }

        public int ChunkSize => _chunkSize;

        /// <summary>
        /// Set up the Chunk Manager by dependency injection.
        /// </summary>
        /// <param name="chunkFactory">
        /// A function which produces an <see cref="IChunk"/> for a given location.
        /// </param>
        /// <param name="chunkLoader">
        /// An <see cref="IChunkLoader"/> which will be used to fill chunks.
        /// </param>
        public void Setup(Func<int, int, int, IChunk> chunkFactory, IChunkLoader chunkLoader)
        {
            _chunkFactory = chunkFactory;
            _chunkLoader = chunkLoader;
        }

        private void Awake()
        {
            // Create required objects and collections for Async chunk loads
            LoadedChunks = new Dictionary<Vector3Int, IChunk>();
            _loadingChunks = new HashSet<Vector3Int>();
            _freshChunks = new ConcurrentQueue<(Vector3Int, IChunk)>();
            _chunksToLoad = new BlockingCollection<Vector3Int>(new ConcurrentQueue<Vector3Int>());
            _recyclableChunks = new ConcurrentBag<IChunk>();
        }

        private void OnEnable()
        {
            // Consume a cancellation token and use it to start the chunk load task
            _tokenSource = new CancellationTokenSource();
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
            if (_chunkFactory == null || _chunkLoader == null)
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
                if (_recyclableChunks.TryTake(out var chunk))
                {
                    // Recycle an old chunk by updating its offset
                    chunk.OffsetX = offset.x;
                    chunk.OffsetY = offset.y;
                    chunk.OffsetZ = offset.z;
                }
                else
                {
                    chunk = _chunkFactory(offset.x, offset.y, offset.z);
                }
                _chunkLoader.Load(chunk);
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
                LoadedChunks.Add(data.Item1, data.Item2);
                _loadingChunks.Remove(data.Item1);
                ChunkLoaded?.Invoke(data.Item1);
            }
        }

        /// <summary>
        /// Decide which chunks to load and recycle chunks no longer needed.
        /// </summary>
        private void QueueNewChunkLoads()
        {
            var chunksToLoad = new HashSet<Vector3Int>();
            var currentChunk = Vector3Int.FloorToInt(_chunkLoadingTarget.position / _chunkSize);

            // Gets list of chunks to be loaded this frame from the center, out
            for (var n = 0; n < _chunkLoadDistance; n++)
            {
                // Loop through wall coordinates of cuboid with edge length 2n+1
                for (var i = 0; i <= n; i++)
                {
                    for (var j = 0; j <= n; j++)
                    {
                        //Verifies that chunk is within sphere
                        if (Mathf.Sqrt(i * i + j * j + n * n) > _chunkLoadDistance)
                        {
                            break;
                        }

                        // Loop through positive and negative combinations of i and j
                        for (var k = 0; k <= 3; k ++) {
                            var iMod = k & 2;
                            iMod -= 1;
                            iMod *= i;

                            var jMod = (k << 1) & 2;
                            jMod -= 1;
                            jMod *= j;

                            // Places (i,j) coordinates on each wall of cuboid -- There may be a better way to do this
                            var coords = new[]
                            {
                                new Vector3Int(-n, iMod, jMod),
                                new Vector3Int(iMod, -n, jMod),
                                new Vector3Int(iMod, jMod, -n),
                                new Vector3Int(n, iMod, jMod),
                                new Vector3Int(iMod, n, jMod),
                                new Vector3Int(iMod, jMod, n)
                            };

                            foreach (var coord in coords.Distinct())
                            {
                                chunksToLoad.Add(coord + currentChunk);
                            }
                        }
                    }
                }
            }
            
            // Recycle old loaded chunk memory we no longer need
            foreach (var chunkPos in LoadedChunks.Keys.Except(chunksToLoad).ToArray())
            {
                var chunk = LoadedChunks[chunkPos];
                LoadedChunks.Remove(chunkPos);
                // Only recycle if the chunk size hasn't changed
                if (chunk.Width == _chunkSize)
                {
                    _recyclableChunks.Add(chunk);
                }
                ChunkUnloaded?.Invoke(chunkPos);
            }
            
            // Add chunks we still need to load to the load queue
            foreach (var chunkPos in chunksToLoad.Except(LoadedChunks.Keys).Except(_loadingChunks).ToArray())
            {
                _loadingChunks.Add(chunkPos);
                _chunksToLoad.Add(chunkPos);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (LoadedChunks == null)
            {
                return;
            }
            Gizmos.color = Color.red;
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var chunkCoord in LoadedChunks)
            {
                var worldSpaceCoord = (chunkCoord.Key + new Vector3(0.5f, 0.5f, 0.5f)) * _chunkSize;
                Gizmos.DrawWireCube(worldSpaceCoord, Vector3.one * _chunkSize);
            }
        }
    }
}