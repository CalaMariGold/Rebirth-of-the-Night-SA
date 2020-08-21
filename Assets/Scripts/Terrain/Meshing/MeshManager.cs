using System.Collections.Generic;
using Rebirth.Terrain.Chunk;
using UnityEngine;

namespace Rebirth.Terrain.Meshing
{
    [RequireComponent(typeof(ChunkManager))]
    public class MeshManager : MonoBehaviour
    {
        [SerializeField] private ComputeShader _computeShader;
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
        
        /// <summary>
        /// Represents a <seealso cref="GameObject"/> holding a chunk mesh.
        /// </summary>
        private struct ChunkHolder
        {
            public GameObject GameObject { get; set; }
            public MeshFilter MeshFilter { get; set; }
        }
        
        /// <summary>
        /// Inject dependencies for the class.
        /// </summary>
        /// <param name="meshGenerator">An object to generate meshes for chunks.</param>
        public void Setup(IMeshGenerator meshGenerator)
        {
            _meshGenerator = meshGenerator;
        }
        
        public void Awake()
        {
            _chunkManager = GetComponent<ChunkManager>();
        }

        public void Update()
        {
            // HACK: Mesh at most one chunk loaded this frame
            if (_meshingQueue.Count == 0)
            {
                return;
            }

            var chunkToMesh = _meshingQueue.Dequeue();
            _chunksToMesh.Remove(chunkToMesh);
            if (!LoadChunkMesh(chunkToMesh, out var mesh))
            {
                return;
            }

            UnloadChunkMesh(chunkToMesh);
            CreateMeshedObject(chunkToMesh, mesh);
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
        /// Handle the ChunkLoaded event.
        /// </summary>
        /// <param name="chunkLocation">The location of the loaded chunk.</param>
        private void OnChunkLoaded(Vector3Int chunkLocation)
        {
            // Mesh chunk by location
            for (var i = 0; i < 8; i++)
            {
                var offset = new Vector3Int(i & 1, (i >> 1) & 1, (i >> 2) & 1);
                // So apparently, we can only call compute shaders from the main thread :'(
                if (_chunksToMesh.Add(chunkLocation - offset))
                {
                    _meshingQueue.Enqueue(chunkLocation - offset);
                }
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
            }
            chunkHolder.GameObject.transform.position = chunkLocation * _chunkManager.ChunkSize;
            chunkHolder.MeshFilter.sharedMesh = mesh;
            _meshHolders.Add(chunkLocation, chunkHolder);
        }

        /// <summary>
        /// Load the mesh for the chunk at a given location.
        /// </summary>
        /// <param name="chunkLocation">The location of the chunk.</param>
        /// <param name="mesh">The mesh to load.</param>
        /// <returns><c>true</c> if the mesh could be loaded; otherwise, <c>false</c>.</returns>
        private bool LoadChunkMesh(Vector3Int chunkLocation, out Mesh mesh)
        {
            if (_meshGenerator == null || _computeShader == null)
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

            mesh = _meshGenerator.GenerateMesh(
                chunkLocation,
                _chunkManager.LoadedChunks,
                _computeShader
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
            if (!_meshHolders.TryGetValue(chunkLocation, out var chunkHolder))
            {
                // No valid loaded chunk
                return;
            }
            _meshHolders.Remove(chunkLocation);
            // Recycle the chunk
            // TODO: Provide method to cleanup recycled chunks to save memory
            chunkHolder.MeshFilter.mesh = null;
            chunkHolder.GameObject.name = "recyclable_chunk";
            _recyclableChunks.Enqueue(chunkHolder);
        }
    }
}
