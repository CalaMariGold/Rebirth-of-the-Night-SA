using Rebirth.Terrain.Chunk;
using Rebirth.Terrain.Meshing;
using Rebirth.Terrain.Voxel;
using UnityEngine;

namespace Rebirth.Terrain.Behaviours
{
    /// <summary>
    /// Provides a GameObject holding the mesh for a single chunk.
    /// </summary>
    public class SingleChunkViewer : MonoBehaviour
    {
        [SerializeField] private Vector3Int _chunkSize = new Vector3Int(16, 16, 16);
        [SerializeField] private Vector3Int _chunkOffset = Vector3Int.zero;
        [SerializeField] private float _groundHeight;
        [SerializeField] private Vector2 _textureScale;
        [SerializeField] private Vector2 _textureOffset;
        [SerializeField] private float _amplitude;
        [SerializeField] private Texture2D _texture;
        [SerializeField] private Material _material = default;

        private IChunk _chunk;
        private GameObject _meshHolder;
        private IVoxelProvider _voxelProvider;
        private IMeshGenerator _meshGenerator;

        private void Awake()
        {
            // Uses a simple implementation of IVoxelProvider, to be replaced later
            _voxelProvider = new HeightMapVoxelProvider(
                _texture, _amplitude, _textureScale,
                _groundHeight, _textureOffset
            );
            // Create a new chunk and fill with voxels based on _voxelProvider
            _chunk = new ArrayChunk(_chunkSize.x, _chunkSize.y, _chunkSize.z,
                _chunkOffset.x, _chunkOffset.y, _chunkOffset.z);
            _chunk.Load(_voxelProvider);
            // Create a GameObject to hold and render the mesh
            _meshHolder = new GameObject("Chunk Mesh");
            _meshHolder.transform.position = transform.position;
            var meshFilter = _meshHolder.AddComponent<MeshFilter>();
            _meshGenerator = new MarchingCubes();
            meshFilter.sharedMesh = _meshGenerator.GenerateMesh(_chunk);
            meshFilter.sharedMesh.RecalculateNormals();
            var meshRenderer = _meshHolder.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = _material;
        }
    }
}
