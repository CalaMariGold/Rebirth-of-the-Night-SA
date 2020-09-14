using System.Collections.Generic;
using Rebirth.Terrain.Chunk;
using UnityEngine;

namespace Rebirth.Terrain.Meshing
{
    [RequireComponent(typeof(ChunkManager))]
    [RequireComponent(typeof(IMeshGenerator))]
    public class MeshManager : MonoBehaviour
    {
        [SerializeField] private Material _material;
        
        // Mesh Generator for DI
        private IMeshGenerator _meshGenerator;

        // The Chunk Manager component
        private ChunkManager _chunkManager;

        // Currently loaded chunk GameObjects
        private readonly Dictionary<Vector3Int, ChunkHolder> _meshHolders =
            new Dictionary<Vector3Int, ChunkHolder>();

        // GameObjects which we can reuse for new chunks
        private readonly Queue<ChunkHolder> _recyclableChunks = new Queue<ChunkHolder>();

        // Chunks which still need to be meshed
        private readonly HashSet<Vector3Int> _chunksToMesh = new HashSet<Vector3Int>();
        private readonly Queue<Vector3Int> _meshingQueue = new Queue<Vector3Int>();

        // Offsets for a chunk's neighbors, required for meshing edges
        private List<Vector3Int> _chunkNeighborOffsets = new List<Vector3Int>();
        
        /// <summary>
        /// Represents a <seealso cref="GameObject"/> holding a chunk mesh.
        /// </summary>
        private struct ChunkHolder
        {
            public GameObject GameObject { get; set; }
            public MeshFilter MeshFilter { get; set; }
        }

        public void Awake()
        {
            _chunkManager = GetComponent<ChunkManager>();
            _meshGenerator = GetComponent<IMeshGenerator>();
            for (var i = 0; i < 8; i++)
            {
                _chunkNeighborOffsets.Add(new Vector3Int((i & 1), ((i >> 1) & 1), ((i >> 2) & 1)));
            }
        }

        public void Update()
        {
            var meshesThisFrame = Mathf.CeilToInt(_meshingQueue.Count * Time.deltaTime / 2);
            MeshChunks(meshesThisFrame);
        }

        public void OnEnable()
        {
            // Handle chunk load & unload events from _chunkManager
            _chunkManager.ChunkLoaded += OnChunkLoaded;
            _chunkManager.ChunkUnloaded += UnloadChunkMesh;
        }

        public void OnDisable()
        {
            // Remove event handlers
            _chunkManager.ChunkLoaded -= OnChunkLoaded;
            _chunkManager.ChunkUnloaded -= UnloadChunkMesh;
        }

        /// <summary>
        /// Meshes and loads queued chunks.
        /// </summary>
        /// <param name="chunkCount">Number of chunks to mesh.</param>
        private void MeshChunks(int chunkCount)
        {
            // TODO: Profile and consider capping number of chunks meshed per frame
            chunkCount = System.Math.Min(chunkCount, _meshingQueue.Count);

            for (var i = 0; i < chunkCount; i++)
            {
                var chunkToMesh = _meshingQueue.Dequeue();
                var hasEdgeChunks = true;

                // Disregard if this chunk has been unloaded
                if (!_chunksToMesh.Contains(chunkToMesh)){
                    continue;
                }

                // Ensures that chunks necessary for meshing are present 
                for (var j = 0; j < 8; j++)
                {
                    if (!_chunkManager.LoadedChunks.ContainsKey(chunkToMesh + _chunkNeighborOffsets[j]))
                    {
                        hasEdgeChunks = false;
                        break;
                    }
                }

                if (!hasEdgeChunks)
                {
                    _meshingQueue.Enqueue(chunkToMesh);
                    continue;
                }

                _chunksToMesh.Remove(chunkToMesh);

                Mesh mesh;
                if (_recyclableChunks.Count > 0)
                {
                    mesh = _recyclableChunks.Peek().MeshFilter.sharedMesh;
                }
                else
                {
                    mesh = null;
                }

                if (LoadChunkMesh(chunkToMesh, ref mesh))
                {
                    UnloadChunkMesh(chunkToMesh);
                    CreateMeshedObject(chunkToMesh, mesh);
                }
            }
        }
    
        /// <summary>
        /// Handle the ChunkLoaded event.
        /// </summary>
        /// <param name="chunkLocation">The location of the loaded chunk.</param>
        private void OnChunkLoaded(Vector3Int chunkLocation)
        {
            // Mesh chunk by location
            if (_chunksToMesh.Add(chunkLocation))
            {
                _meshingQueue.Enqueue(chunkLocation);
            }
        }

        /// <summary>
        /// Create a GameObject for a loaded chunk.
        /// </summary>
        /// <param name="chunkLocation">The location of the chunk.</param>
        /// <param name="mesh">The mesh to load.</param>
        private void CreateMeshedObject(Vector3Int chunkLocation, Mesh mesh)
        {

            var holderName = $"Chunk {chunkLocation.x}, {chunkLocation.y}, {chunkLocation.z}";
            ChunkHolder chunkHolder;
            if (_recyclableChunks.Count > 0)
            {
                // Recycle a GameObject
                chunkHolder = _recyclableChunks.Dequeue();
                chunkHolder.GameObject.name = holderName;
            }
            else
            {
                // Create new GameObject to hold the chunk
                var chunkHolderGameObject = new GameObject(holderName);
                var meshFilter = chunkHolderGameObject.AddComponent<MeshFilter>();
                var meshRenderer = chunkHolderGameObject.AddComponent<MeshRenderer>();
                chunkHolderGameObject.transform.parent = transform;
                meshRenderer.sharedMaterial = _material;
                chunkHolder = new ChunkHolder
                {
                    GameObject = chunkHolderGameObject,
                    MeshFilter = meshFilter
                };
                chunkHolder.MeshFilter.sharedMesh = mesh;
            }
            chunkHolder.GameObject.transform.position = chunkLocation * _chunkManager.ChunkSize;
            _meshHolders.Add(chunkLocation, chunkHolder);
        }

        /// <summary>
        /// Load the mesh for the chunk at a given location.
        /// </summary>
        /// <param name="chunkLocation">The location of the chunk.</param>
        /// <param name="mesh">The mesh to load.</param>
        /// <returns><c>true</c> if the mesh could be loaded; otherwise, <c>false</c>.</returns>
        private bool LoadChunkMesh(Vector3Int chunkLocation, ref Mesh mesh)
        {
            if (_meshGenerator == null)
            {
                // No valid mesh generator
                mesh = default;
                return false;
            }

            if (!_chunkManager.LoadedChunks.ContainsKey(chunkLocation))
            {
                // Chunk could not be found
                mesh = default;
                return false;
            }

            _meshGenerator.GenerateMesh(
                chunkLocation,
                _chunkManager.LoadedChunks,
                ref mesh
            );
            mesh.RecalculateNormals();
            return true;
        }

        /// <summary>
        /// Unload the mesh for the chunk at a given location.
        /// </summary>
        /// <param name="chunkLocation">The location of the chunk.</param>
        private void UnloadChunkMesh(Vector3Int chunkLocation)
        {
            _chunksToMesh.Remove(chunkLocation);
            if (!_meshHolders.TryGetValue(chunkLocation, out var chunkHolder))
            {
                // No valid loaded chunk
                return;
            }
            _meshHolders.Remove(chunkLocation);
            // Recycle the chunk
            // TODO: Provide method to cleanup recycled chunks to save memory
            chunkHolder.MeshFilter.sharedMesh.Clear();
            chunkHolder.GameObject.name = "recyclable_chunk";
            _recyclableChunks.Enqueue(chunkHolder);
        }
    }
}
