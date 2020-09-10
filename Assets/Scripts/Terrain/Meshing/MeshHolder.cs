using UnityEngine;

namespace Rebirth.Terrain.Meshing
{
    public class MeshHolder : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;
        private Mesh _mesh;

        public Mesh Mesh
        {
            get => _mesh;
            set
            {
                _mesh = value;
                if (_meshFilter != null)
                {
                    _meshFilter.sharedMesh = _mesh;
                }

                if (_meshCollider != null)
                {
                    _meshCollider.sharedMesh = _mesh;
                }
            }
        }

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();
        }
    }
}
