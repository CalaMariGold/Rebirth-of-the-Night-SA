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
        private IMeshGenerator _meshGenerator;
        private ChunkManager _chunkManager;
        private readonly Dictionary<Vector3Int, ChunkHolder> _meshHolders =
            new Dictionary<Vector3Int, ChunkHolder>();
        private readonly Queue<ChunkHolder> _recyclableChunks = new Queue<ChunkHolder>();

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

        public void OnEnable()
        {
            // Handle chunk load & unload events from _chunkManager
            // TODO: maybe run mesh generation async to avoid lag?
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
        /// Handle the ChunkLoaded event by loading chunk meshes.
        /// </summary>
        /// <param name="chunkLocation">The location of the loaded chunk.</param>
        private void OnChunkLoaded(Vector3Int chunkLocation)
        {
            // This is really slow, and should be sped up with a queue
            // to prevent duplicate reloading
            for (var i = 0; i < 8; i++)
            {
                var offset = new Vector3Int(i & 1, (i >> 1) & 1, (i >> 2) & 1);
                UnloadChunkMesh(chunkLocation - offset);
                LoadChunkMesh(chunkLocation - offset);
            }
        }

        /// <summary>
        /// Load the mesh for the chunk at a given location.
        /// </summary>
        /// <param name="chunkLocation">The location of the chunk.</param>
        private void LoadChunkMesh(Vector3Int chunkLocation)
        {
            if (_meshGenerator == null || _computeShader == null)
            {
                // No valid mesh generator
                return;
            }
            if (!_chunkManager.LoadedChunks.ContainsKey(chunkLocation))
            {
                // Chunk could not be found
                return;
            }
            
            var mesh = _meshGenerator.GenerateMesh(
                chunkLocation,
                _chunkManager.LoadedChunks,
                _computeShader
            );
            mesh.RecalculateNormals();
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
