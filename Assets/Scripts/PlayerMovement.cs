using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Serialized for fine tuning purposes
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _sprintMultiplier = 1.5f;
    [SerializeField] private float _crouchMultiplier = 0.5f;
    [SerializeField] private float _jumpPower = 10f;
    [SerializeField] private float _acceleration = 2.5f;
    [SerializeField] private float _gravity = 0.1635f;
    [SerializeField] private float _terminalVelocity = -20f;
    [SerializeField] private float _crouchHeight = 0.5f;
    [SerializeField] private float _airControl = .1f;

    private PlayerControls _playerControls;
    private CharacterController _characterController;
    private Camera _camera;

    private float _verticalVelocity = 0f;
    private float _speedMultiplier = 1f;

    private bool _sprinting = false;
    private bool _crouching = false;


    private void Awake()
    {
        _playerControls = new PlayerControls();
        _characterController = GetComponent<CharacterController>();
        _camera = Transform.FindObjectOfType<Camera>();

        _playerControls.Default.Sprint.started += context => SprintStarted();
        _playerControls.Default.Sprint.canceled += context => SprintCanceled();
        _playerControls.Default.Crouch.started += context => CrouchStarted();
        _playerControls.Default.Crouch.canceled += context => CrouchCanceled();
        _playerControls.Default.Jump.started += context => Jump();
    }

    private void Update()
    {
        var movement = _playerControls.Default.Move.ReadValue<Vector2>() * _speedMultiplier * _movementSpeed;
        var acceleration = _characterController.isGrounded ? _acceleration : _acceleration * _airControl;

        _verticalVelocity = Mathf.MoveTowards(_verticalVelocity, _terminalVelocity, _gravity);

        // Gets unrotated velocity vector from last frame
        var adjustedVelocity = Quaternion.Inverse(_characterController.transform.rotation) * _characterController.velocity; 

        var adjustedHorizontalVelocity = new Vector2(adjustedVelocity.x, adjustedVelocity.z);
        adjustedHorizontalVelocity = Vector2.MoveTowards(adjustedHorizontalVelocity, movement, acceleration);

        var velocity = adjustedHorizontalVelocity.x * transform.right +
            _verticalVelocity * transform.up +
            adjustedHorizontalVelocity.y * transform.forward;

        _characterController.Move(velocity * Time.deltaTime);
    }

    private void SprintStarted()
    {
        if(!_sprinting)
        {
            _sprinting = !_sprinting;
            _speedMultiplier *= _sprintMultiplier;
        }
    }

    private void SprintCanceled()
    {
        if(_sprinting)
        {
            _sprinting = !_sprinting;
            _speedMultiplier /= _sprintMultiplier;
        }
    }

    private void CrouchStarted()
    {
        if (!_crouching)
        {
            _crouching = !_crouching;
            _speedMultiplier *= _crouchMultiplier;
            _camera.transform.position -= new Vector3(0 , _crouchHeight, 0);
        }
    }

    private void CrouchCanceled()
    {
        if (_crouching)
        {
            _crouching = !_crouching;
            _speedMultiplier /= _crouchMultiplier;
            _camera.transform.position += new Vector3(0, _crouchHeight, 0);
        }
    }

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            _verticalVelocity = _jumpPower;
        }
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
