using UnityEngine;

namespace Rebirth.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(DeformationController))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerControls _playerControls;
        private PlayerMovement _playerMovement;
        private DeformationController _deformation;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _playerMovement = GetComponent<PlayerMovement>();
            _deformation = GetComponent<DeformationController>();
            
            _playerControls.Default.Jump.started += context => _playerMovement.Jump();
            _playerControls.Default.Sprint.started += context => _playerMovement.SprintStarted();
            _playerControls.Default.Sprint.canceled += context => _playerMovement.SprintCanceled();
            _playerControls.Default.Crouch.started += context => _playerMovement.CrouchStarted();
            _playerControls.Default.Crouch.canceled += context => _playerMovement.CrouchCanceled();
            
            // NOTE: this should be changed later to a generic item action
            _playerControls.Default.RaiseTerrain.started += context => _deformation.OnRaiseStarted();
            _playerControls.Default.RaiseTerrain.canceled += context => _deformation.OnRaiseCanceled();
            _playerControls.Default.DigTerrain.started += context => _deformation.OnDigStarted();
            _playerControls.Default.DigTerrain.canceled += context => _deformation.OnDigCanceled();
            _playerControls.Default.SmoothTerrain.started += context => _deformation.OnSmoothStarted();
            _playerControls.Default.SmoothTerrain.canceled += context => _deformation.OnSmoothCanceled();
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void Update()
        {
            _playerMovement.Movement = _playerControls.Default.Move.ReadValue<Vector2>();
        }
    }
}
