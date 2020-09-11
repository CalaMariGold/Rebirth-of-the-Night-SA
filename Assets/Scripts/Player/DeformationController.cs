using UnityEngine;

namespace Rebirth.Player
{
    public class DeformationController : MonoBehaviour
    {
        [SerializeField] private Transform _handle;
        [SerializeField] private Transform _camera;
        [SerializeField] private float _range = 10;
        [SerializeField] private float _handleHeight = 0.1f;

        private bool _isRaising;
        private bool _isDigging;
        
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

        private void Update()
        {
            if (!Physics.Raycast(
                _camera.position, _camera.forward,
                out var result, _range))
            {
                _handle.gameObject.SetActive(false);
                return;
            }

            UpdateHandle(result);
            DeformTerrain(result);
        }

        private void DeformTerrain(RaycastHit hitInfo)
        {
            if (_isDigging && _isRaising)
            {
                return;
            }
            
            
            
            if (_isDigging)
            {
                // Dig terrain
                
            }

            if (_isRaising)
            {
                // Raise terrain
            }
        }

        private void UpdateHandle(RaycastHit result)
        {
            _handle.position = result.point + result.normal * _handleHeight;
            _handle.up = result.normal;
            _handle.gameObject.SetActive(true);
        }
    }
}
