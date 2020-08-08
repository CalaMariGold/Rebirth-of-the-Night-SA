using Rebirth.Terrain.Chunk;
using Rebirth.Terrain.Meshing;
using UnityEngine;

namespace Rebirth.Terrain.Behaviours
{
    /// <summary>
    /// Provides a GameObject holding the mesh for a single chunk.
    /// </summary>
    public class SingleChunkViewer : MonoBehaviour
    {
        [SerializeField] private Vector3Int _chunkSize;
        [SerializeField] private Vector3Int _chunkOffset;
        [SerializeField] private float _groundHeight;
        [SerializeField] private float _noiseAmplitude;
        [SerializeField] private Vector2 _noiseFrequency;
        [SerializeField] private Material _material;

        private IChunk _chunk;
        private GameObject _meshHolder;
        private IVoxelProvider _voxelProvider;
        private IMeshGenerator _meshGenerator;

        private void Awake()
        {
            // Uses a simple implementation of IVoxelProvider, to be replaced later
            _voxelProvider = new UnityPerlinVoxelProvider(_groundHeight, _noiseAmplitude, _noiseFrequency);
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
