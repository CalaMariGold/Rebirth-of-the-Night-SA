using System;
using Rebirth.Terrain.Chunk;
using Rebirth.Terrain.Meshing;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Behaviours
{
    /// <summary>
    /// Provides a GameObject holding the mesh for a single chunk.
    /// </summary>
    [ExecuteInEditMode]
    public class SingleChunkViewer : MonoBehaviour
    {
        [SerializeField] private int _chunkSubdivisions = 5;
        [SerializeField] private Vector3Int _chunkOffset = Vector3Int.zero;
        [SerializeField] private float _groundHeight;
        [SerializeField] private Vector2 _textureScale;
        [SerializeField] private Vector2 _textureOffset;
        [SerializeField] private float _amplitude;
        [SerializeField] private Texture2D _texture;
        [SerializeField] private Material _material;

        private bool _settingsUpdated;

        private IChunk _chunk;
        private GameObject _meshHolder;
        private IVoxelProvider _voxelProvider;
        private IMeshGenerator _meshGenerator;

        private void Awake()
        {
            Run();
        }

        private void Update()
        {
            if (_settingsUpdated)
            {
                Run();
                _settingsUpdated = false;
            }
        }

        private void OnValidate()
        {
            _settingsUpdated = true;
        }

        private void Run()
        {
            // Uses a simple implementation of IVoxelProvider, to be replaced later
            _voxelProvider = new HeightMapVoxelProvider(
                _texture, _amplitude, _textureScale,
                _groundHeight, _textureOffset
            );
            // Create a new chunk and fill with voxels based on _voxelProvider
            _chunk = new OctreeChunk(_chunkSubdivisions,
                _chunkOffset.x, _chunkOffset.y, _chunkOffset.z);
            _chunk.Load(_voxelProvider);
            if (_meshHolder == null)
            {
                // Create a GameObject to hold and render the mesh
                _meshHolder = new GameObject("Chunk Mesh");
                _meshHolder.transform.SetParent(transform);
                var meshFilter = _meshHolder.AddComponent<MeshFilter>();
                _meshGenerator = new MarchingCubes();
                meshFilter.sharedMesh = _meshGenerator.GenerateMesh(_chunk);
                meshFilter.sharedMesh.RecalculateNormals();
                var meshRenderer = _meshHolder.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = _material;
            }
            else
            {
                var meshFilter = _meshHolder.GetComponent<MeshFilter>();
                meshFilter.sharedMesh = _meshGenerator.GenerateMesh(_chunk);
                meshFilter.sharedMesh.RecalculateNormals();
            }
        }
    }
}
