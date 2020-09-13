using System.Collections.Generic;
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
        [SerializeField] private float _snowHeight;
        [SerializeField] private float _rockHeight;
        [SerializeField] private Vector2 _textureScale;
        [SerializeField] private Vector2 _textureOffset;
        [SerializeField] private float _amplitude;
        [SerializeField] private Texture2D _texture;
        [SerializeField] private Material _material;
        [SerializeField] private bool _autoUpdateInEditor;
        
        [Header("Voxel Types")]
        // Voxel types
        [SerializeField] private VoxelType _snow;
        [SerializeField] private VoxelType _grass;
        [SerializeField] private VoxelType _rock;

        private bool _settingsUpdated;

        private IChunk _chunk;
        private readonly Dictionary<Vector3Int, IChunk> _chunkDictionary =
            new Dictionary<Vector3Int, IChunk>();
        private GameObject _meshHolder;
        private IVoxelProvider _voxelProvider;
        private IMeshGenerator _meshGenerator;

        private void Awake()
        {
            _meshGenerator = GetComponent<IMeshGenerator>();
        }
        private void Update()
        {
            if (!_settingsUpdated)
            {
                return;
            }

            // Only update in editor if auto update is enabled
            if (!Application.isPlaying && !_autoUpdateInEditor)
            {
                return;
            }
            Run();
            _settingsUpdated = false;
        }

        private void OnValidate()
        {
            _settingsUpdated = true;
        }

        private void Run()
        {
            if (_meshGenerator == null)
            {
                return;
            }
            // Uses a simple implementation of IVoxelProvider, to be replaced later
            _voxelProvider = new HeightMapVoxelProvider
            {
                HeightMap = _texture,
                Scale = _textureScale,
                Offset = _textureOffset,
                Amplitude = _amplitude,
                BaseHeight = _groundHeight,
                RockHeight = _rockHeight,
                SnowHeight = _snowHeight,
                Snow = _snow,
                Rock = _rock,
                Grass = _grass
            };
            // Create a new chunk and fill with voxels based on _voxelProvider
            var width = 1 << _chunkSubdivisions;
            _chunk = new ArrayChunk(width, width, width,
                _chunkOffset.x, _chunkOffset.y, _chunkOffset.z);
            _chunk.Load(_voxelProvider);
            _chunkDictionary[Vector3Int.zero] = _chunk;
            if (_meshHolder == null)
            {
                for (var i = 0; i < transform.childCount; i++)
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }
                // Create a GameObject to hold and render the mesh
                _meshHolder = new GameObject("Chunk Mesh");
                _meshHolder.transform.SetParent(transform);
                var meshFilter = _meshHolder.AddComponent<MeshFilter>();
                Mesh mesh = null;
                _meshGenerator.GenerateMesh(
                    Vector3Int.zero, _chunkDictionary,
                    ref mesh
                );
                meshFilter.sharedMesh = mesh;
                meshFilter.sharedMesh.RecalculateNormals();
                var meshRenderer = _meshHolder.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = _material;
            }
            else
            {
                var meshFilter = _meshHolder.GetComponent<MeshFilter>();
                var mesh = meshFilter.sharedMesh;
                _meshGenerator.GenerateMesh(
                    Vector3Int.zero, _chunkDictionary,
                    ref mesh
                );
                meshFilter.sharedMesh.RecalculateNormals();
            }
        }
    }
}
