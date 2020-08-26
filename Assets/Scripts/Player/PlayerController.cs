using UnityEngine;

namespace Rebirth.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerControls _playerControls;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _playerMovement = GetComponent<PlayerMovement>();
            
            _playerControls.Default.Jump.started += context => _playerMovement.Jump();
            _playerControls.Default.Sprint.started += context => _playerMovement.SprintStarted();
            _playerControls.Default.Sprint.canceled += context => _playerMovement.SprintCanceled();
            _playerControls.Default.Crouch.started += context => _playerMovement.CrouchStarted();
            _playerControls.Default.Crouch.canceled += context => _playerMovement.CrouchCanceled();
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
