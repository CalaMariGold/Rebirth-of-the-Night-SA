using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls _playerControls;
    private CharacterController _characterController;
    private Camera _camera;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _characterController = GetComponent<CharacterController>();
        _camera = Transform.FindObjectOfType<Camera>();
    }

    private void Update()
    {
        Locomotion();
    }

    private void Locomotion()
    {

    }

    private void SprintStarted()
    {
    }

    private void SprintCanceled()
    {
    }

    private void CrouchStarted()
    {
    }

    private void CrouchCanceled()
    {
    }

    private void Jump()
    {
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
