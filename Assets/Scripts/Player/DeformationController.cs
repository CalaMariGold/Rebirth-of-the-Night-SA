using System.Collections.Generic;
using Rebirth.Terrain.Chunk;
using UnityEngine;

namespace Rebirth.Player
{
    [RequireComponent(typeof(ChunkManager))]
    public class DeformationController : MonoBehaviour
    {
        [SerializeField] private Transform _handle;
        [SerializeField] private Transform _camera;
        [SerializeField] private ChunkManager _chunkManager;
        [SerializeField] private float _range = 10;
        [SerializeField] private float _handleHeight = 0.1f;
        [SerializeField] private float _radius = 2;
        [SerializeField] private float _delta = 0.1f;
        [SerializeField] private float _raycastRadius = 0.5f;
        
        private bool _isRaising;
        private bool _isDigging;

        #region Control Events

        public void OnDigCanceled()
        {
            _isDigging = false;
        }

        public void OnDigStarted()
        {
            _isDigging = true;
        }

        public void OnRaiseCanceled()
        {
            _isRaising = false;
        }

        public void OnRaiseStarted()
        {
            _isRaising = true;
        }

        #endregion

        private void Update()
        {
            var position = _camera.position;
            var forward = _camera.forward;
            var hasResult = Physics.SphereCast(
                position, _raycastRadius, forward,
                out var result, _range
            );
            if (!hasResult)
            {
                // Maybe use previous result moved by delta?
                _handle.gameObject.SetActive(false);
                return;
            }

            UpdateHandle(result);
            DeformTerrain(result);
        }

        /// <summary>
        /// Update the terrain at the targeted location.
        /// </summary>
        /// <param name="hitInfo">The raycast result from the camera's viewpoint.</param>
        private void DeformTerrain(RaycastHit hitInfo)
        {
            if (_isDigging && _isRaising)
            {
                return;
            }

            var spherePoints = GenerateSphere(_radius, hitInfo.point);
            
            if (_isDigging)
            {
                // Dig terrain
                foreach (var point in spherePoints)
                {
                    var voxelInfo = _chunkManager[point.x, point.y, point.z];
                    voxelInfo.Distance = Mathf.Clamp(voxelInfo.Distance + _delta, -1, 1);
                    _chunkManager[point.x, point.y, point.z] = voxelInfo;
                }

                return;
            }

            if (_isRaising)
            {
                foreach (var point in spherePoints)
                {
                    var voxelInfo = _chunkManager[point.x, point.y, point.z];
                    voxelInfo.Distance = Mathf.Clamp(voxelInfo.Distance - _delta, -1, 1);
                    _chunkManager[point.x, point.y, point.z] = voxelInfo;
                }
            }
        }

        /// <summary>
        /// Determine the locations of points within a sphere of specified parameters.
        /// </summary>
        /// <param name="radius">The sphere's radius.</param>
        /// <param name="point">The location of the centre of the sphere.</param>
        /// <returns>An <seealso cref="IEnumerable{T}"/> of point locations in the sphere.</returns>
        private static IEnumerable<Vector3Int> GenerateSphere(float radius, Vector3 point)
        {
            var rounded = Vector3Int.RoundToInt(point);
            var roundedRadius = Mathf.CeilToInt(radius);
            for (var x = rounded.x - (roundedRadius + 1); x <= rounded.x + (roundedRadius + 1); x++)
            {
                for (var y = rounded.y - (roundedRadius + 1); y <= rounded.y + (roundedRadius + 1); y++)
                {
                    for (var z = rounded.z - (roundedRadius + 1); z <= rounded.z + (roundedRadius + 1); z++)
                    {
                        if (Vector3.Distance(new Vector3(x, y, z), point) <= radius)
                        {
                            yield return new Vector3Int(x, y, z);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Show the deformation handle at the targeted location.
        /// </summary>
        /// <param name="result">The raycast result from the camera's viewpoint.</param>
        private void UpdateHandle(RaycastHit result)
        {
            _handle.position = result.point + result.normal * _handleHeight;
            _handle.up = result.normal;
            _handle.gameObject.SetActive(true);
        }
    }
}
