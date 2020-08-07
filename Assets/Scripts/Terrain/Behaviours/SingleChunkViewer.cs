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

        private Chunk _chunk;
        private GameObject _meshHolder;
        private IVoxelProvider _voxelProvider;
        
        // TODO: Write an implementation for IMeshGenerator
        private IMeshGenerator _meshGenerator;

        private void Awake()
        {
            // Uses a simple implementation of IVoxelProvider, to be replaced later
            _voxelProvider = new FlatLandVoxelProvider(_groundHeight);
            // Create a new chunk and fill with voxels based on _voxelProvider
            _chunk = new Chunk(_chunkSize.x, _chunkSize.y, _chunkSize.z,
                _chunkOffset.x, _chunkOffset.y, _chunkOffset.z);
            _chunk.Load(_voxelProvider);
            // Create a GameObject to hold and render the mesh
            _meshHolder = new GameObject("Chunk Mesh");
            var meshFilter = _meshHolder.AddComponent<MeshFilter>();
            if (_meshGenerator == null)
            {
                return;
            }
            meshFilter.sharedMesh = _meshGenerator.GenerateMesh(_chunk);
            _meshHolder.AddComponent<MeshRenderer>();
        }
    }
}
