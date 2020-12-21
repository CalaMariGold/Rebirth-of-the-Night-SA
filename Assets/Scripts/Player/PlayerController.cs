using UnityEngine;

namespace Rebirth.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {
        public PlayerControls _playerControls;
        private PlayerMovement _playerMovement;
        private PlayerUI _playerUI;
        private bool _uiOpened;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerUI = GetComponent<PlayerUI>();

            _playerControls.Movement.Jump.started += context => _playerMovement.Jump();
            _playerControls.Movement.Sprint.started += context => _playerMovement.SprintStarted();
            _playerControls.Movement.Sprint.canceled += context => _playerMovement.SprintCanceled();
            _playerControls.Movement.Crouch.started += context => _playerMovement.CrouchStarted();
            _playerControls.Movement.Crouch.canceled += context => _playerMovement.CrouchCanceled();
            _playerControls.UI.Inventory.performed += context => _playerUI.InventoryToggle(ref _playerControls);
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
            _playerMovement.Movement = _playerControls.Movement.Move.ReadValue<Vector2>();
        }
    }
}
