using System;
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
        private readonly Dictionary<Vector3Int, GameObject> _meshHolders =
            new Dictionary<Vector3Int, GameObject>();
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
            _chunkManager.ChunkLoaded += LoadChunkMesh;
            _chunkManager.ChunkUnloaded += UnloadChunkMesh;
        }

        public void OnDisable()
        {
            // Remove event handlers
            _chunkManager.ChunkLoaded -= LoadChunkMesh;
            _chunkManager.ChunkUnloaded -= UnloadChunkMesh;
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
            if (!_chunkManager.LoadedChunks.TryGetValue(chunkLocation, out var chunk))
            {
                // Chunk could not be found
                return;
            }

            // TODO: mesh chunk borders
            var mesh = _meshGenerator.GenerateMesh(chunk, _computeShader);
            mesh.RecalculateNormals();
            var meshHolder = new GameObject(
                $"Chunk {chunkLocation.x}, {chunkLocation.y}, {chunkLocation.z}"
            );
            meshHolder.transform.position = chunkLocation * _chunkManager.ChunkSize;
            meshHolder.transform.parent = transform;
            var meshFilter = meshHolder.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
            var meshRenderer = meshHolder.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = _material;
            _meshHolders.Add(chunkLocation, meshHolder);
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
            // TODO: add unloaded GameObject to recyclable queue
            Destroy(chunkHolder);
        }
    }
}
