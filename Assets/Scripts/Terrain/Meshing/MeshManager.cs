using System.Collections.Generic;
using System.Linq;
using Rebirth.Terrain.Chunk;
using UnityEngine;

namespace Rebirth.Terrain.Meshing
{
    [RequireComponent(typeof(ChunkManager))]
    public class MeshManager : MonoBehaviour
    {
        [SerializeField] private ComputeShader _computeShader;
        [SerializeField] private GameObject _chunkPrefab;
        
        // Mesh Generator for DI
        private IMeshGenerator _meshGenerator;
        // The Chunk Manager component
        private ChunkManager _chunkManager;
        // Currently loaded chunk GameObjects
        private readonly Dictionary<Vector3Int, MeshHolder> _meshHolders =
            new Dictionary<Vector3Int, MeshHolder>();
        // GameObjects which we can reuse for new chunks
        private readonly Queue<MeshHolder> _recyclableChunks = new Queue<MeshHolder>();
        // Chunks which still need to be meshed
        private readonly HashSet<Vector3Int> _chunksToMesh = new HashSet<Vector3Int>();
        private readonly Queue<Vector3Int> _meshingQueue = new Queue<Vector3Int>();
        // Chunks which should be meshed this frame
        private readonly HashSet<Vector3Int> _urgentChunks = new HashSet<Vector3Int>();

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
            LoadChunk(chunkToMesh);
        }

        public void LateUpdate()
        {
            foreach (var urgentChunk in _urgentChunks)
            {
                LoadChunk(urgentChunk);
            }
            _urgentChunks.Clear();
        }

        public void OnEnable()
        {
            // Handle chunk load & unload events from _chunkManager
            _chunkManager.ChunkLoaded += OnChunkLoaded;
            _chunkManager.ChunkUnloaded += UnloadChunkMesh;
            _chunkManager.ChunkModified += OnChunkModified;
        }

        public void OnDisable()
        {
            // Remove event handlers
            _chunkManager.ChunkLoaded -= OnChunkLoaded;
            _chunkManager.ChunkUnloaded -= UnloadChunkMesh;
            _chunkManager.ChunkModified -= OnChunkModified;
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
        /// Handle the ChunkModified event.
        /// </summary>
        /// <param name="sender">The object which invoked the event.</param>
        /// <param name="args">Info about the modified chunk.</param>
        private void OnChunkModified(object sender, ChunkModifiedEventArgs args)
        {
            _urgentChunks.Add(args.ChunkLocation);
            
            // Deal with modified borders; this can cause multiple reloads per frame
            var borders = new List<byte>();
            foreach (var modifiedVoxel in args.ModifiedVoxels)
            {
                byte border = 0;
                if (modifiedVoxel.x == 0)
                {
                    border |= 4;
                }
                if (modifiedVoxel.y == 0)
                {
                    border |= 2;
                }
                if (modifiedVoxel.z == 0)
                {
                    border |= 1;
                }

                for (byte i = 1; i < 8; i++)
                {
                    if ((i & border) == i)
                    {
                        borders.Add(i);
                    }
                }
            }

            foreach (var border in borders.Distinct())
            {
                var vec = new Vector3Int(
                    border >> 2,
                    (border >> 1) & 1,
                    border & 1
                );
                _urgentChunks.Add(args.ChunkLocation - vec);
            }
        }

        /// <summary>
        /// Load the mesh for a chunk and create its GameObject.
        /// </summary>
        /// <param name="chunkLocation">The chunk's location.</param>
        private void LoadChunk(Vector3Int chunkLocation)
        {
            if (!LoadChunkMesh(chunkLocation, out var mesh))
            {
                return;
            }

            UnloadChunkMesh(chunkLocation);
            CreateMeshedObject(chunkLocation, mesh);
        }

        /// <summary>
        /// Create a GameObject for a loaded chunk.
        /// </summary>
        /// <param name="chunkLocation">The location of the chunk.</param>
        /// <param name="mesh">The mesh to load.</param>
        private void CreateMeshedObject(Vector3Int chunkLocation, Mesh mesh)
        {

            var holderName = $"Chunk {chunkLocation.x}, {chunkLocation.y}, {chunkLocation.z}";
            MeshHolder meshHolder;
            var position = chunkLocation * _chunkManager.ChunkSize;
            if (_recyclableChunks.Count > 0)
            {
                // Recycle a GameObject
                meshHolder = _recyclableChunks.Dequeue();
                meshHolder.gameObject.name = holderName;
                meshHolder.transform.position = position;
            }
            else
            {
                // Create new GameObject to hold the chunk
                // var chunkHolderGameObject = new GameObject(holderName);
                // var meshFilter = chunkHolderGameObject.AddComponent<MeshFilter>();
                // var meshRenderer = chunkHolderGameObject.AddComponent<MeshRenderer>();
                var chunkHolderGameObject = Instantiate(
                    _chunkPrefab, position,
                    Quaternion.identity, transform
                );
                chunkHolderGameObject.name = holderName;
                meshHolder = chunkHolderGameObject.GetComponent<MeshHolder>();
            }

            meshHolder.Mesh = mesh;
            _meshHolders.Add(chunkLocation, meshHolder);
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
            chunkHolder.Mesh.Clear();
            chunkHolder.gameObject.name = "recyclable_chunk";
            _recyclableChunks.Enqueue(chunkHolder);
        }
    }
}
