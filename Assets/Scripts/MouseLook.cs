using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float MouseSensitivity = 50f;
    
    private PlayerControls _playerControls;
    
    private Vector2 _look;

    private float _xRotation = 0f;
    private const float _xRotationClamp = 75f;
    private Transform _playerBody;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        Cursor.lockState = CursorLockMode.Locked;

        _playerBody = transform.parent;
    }

    private void Update()
    {
        _look = _playerControls.Default.Look.ReadValue<Vector2>() * MouseSensitivity * Time.deltaTime;

        _xRotation -= _look.y;
        _xRotation = Mathf.Clamp(_xRotation, -1 * _xRotationClamp, _xRotationClamp);

        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _playerBody.Rotate(Vector3.up * _look.x);
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
