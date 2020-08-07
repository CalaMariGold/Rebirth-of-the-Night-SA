
using System.Collections;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    #region Member Variables
    [Header("Movement Speed")]
    [SerializeField] private float _baseSpeed = 6.75f;
    [SerializeField] private float _sprintMultiplier = 1.5f;
    [SerializeField] private float _crouchMultiplier = 0.5f;
    [SerializeField] private float _reverseMultiplier = 0.5f;
    [SerializeField] private float _minSpeed = 0.1f;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce = 4.5f;

    [Header("Physics")]
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _terminalVelocity = -50f;
    [SerializeField] private float _maxUpwardVelocity = 150f;
    [Space]
    [SerializeField] private float _airAcceleration = 10f;
    [SerializeField, Range(0f, 1f)] private float _airFriction = 0.95f;
    [SerializeField, Range(0f, 1f)] private float _groundFriction = 0.75f;
    [Space]
    [SerializeField, Range(0f, 90f)] private float _slopeLimit = 45f;

    private float _speedMultiplier = 1f;

    private bool _sprinting = false;
    private bool _crouching = false;

    [SerializeField, ReadOnly(true)] private bool _isGrounded = false;
    [SerializeField, ReadOnly(true)] private Vector3 _groundVector = Vector3.up;

    private PlayerControls _playerControls;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();

        _playerControls.Default.Jump.started += context => Jump();
        _playerControls.Default.Sprint.started += context => SprintStarted();
        _playerControls.Default.Sprint.canceled += context => SprintCanceled();
        _playerControls.Default.Crouch.started += context => CrouchStarted();
        _playerControls.Default.Crouch.canceled += context => CrouchCanceled();
    }

    private void FixedUpdate()
    {
        Gravity();
        Locomotion();
        Friction();
        _isGrounded = false;
        _groundVector = transform.up;
    }

    private void OnCollisionStay(Collision collision)
    {
        var contacts = new ContactPoint[collision.contactCount];
        var groundVector = Vector3.zero;

        collision.GetContacts(contacts);

        foreach (var contact in contacts)
        {
            var bottom = _collider.bounds.center - Vector3.up * _collider.bounds.extents.y;
            var curve = bottom + (Vector3.up * _collider.radius);
            var dir = curve - contact.point;
            var angle = Vector3.Angle(contact.normal, transform.up);

            Debug.DrawLine(curve, contact.point, Color.blue, 0.5f);
            Debug.Log(dir);

            if (dir.y > 0f && (Mathf.Abs(angle) <= _slopeLimit))
            {
                _isGrounded = true;
                groundVector += contact.normal;
            }
        }
 
        _groundVector = groundVector == Vector3.zero ? transform.up : groundVector.normalized;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
    #endregion

    #region Member Methods
    private void Locomotion()
    { 
        var movement = _playerControls.Default.Move.ReadValue<Vector2>() * _baseSpeed * _speedMultiplier;
        
        if(movement.magnitude < _minSpeed)
        {
            return;
        }

        if (movement.y <= 0)
        {
            movement *= _reverseMultiplier;
            SprintCanceled();
        }

        if (_isGrounded)
        {
            var hVel = new Vector3(movement.x, 0.0f, movement.y);
            hVel = transform.rotation * hVel;
            hVel += transform.up * _rigidbody.velocity.y;
            _rigidbody.velocity = hVel;
        } else
        {
            var airAcceleration = movement.normalized * _airAcceleration;

            _rigidbody.AddRelativeForce(new Vector3(airAcceleration.x, 0f, airAcceleration.y), ForceMode.Acceleration);
        }
    }

    private void Gravity()
    {
        _rigidbody.AddForce(_groundVector * -_gravity, ForceMode.Acceleration);

        var velocity = _rigidbody.velocity;
        velocity.y = Mathf.Clamp(_rigidbody.velocity.y, _terminalVelocity, _maxUpwardVelocity);

        _rigidbody.velocity = velocity;
    }

    private void Friction()
    {
        var friction = _isGrounded ? _groundFriction : _airFriction;
        var hVelocity = _rigidbody.velocity;
        hVelocity.y = 0f;

        if (hVelocity.magnitude <= _minSpeed)
        {
            hVelocity = Vector3.zero;
        } else
        {
            hVelocity *= friction;
        }

        _rigidbody.velocity = new Vector3(hVelocity.x, _rigidbody.velocity.y, hVelocity.z);
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode.VelocityChange);
        }
    }

    private void SprintStarted()
    {
        if (!_sprinting)
        {
            _sprinting = true;
            _speedMultiplier *= _sprintMultiplier;
        }
    }

    private void SprintCanceled()
    {
        if (_sprinting)
        {
            _sprinting = false;
            _speedMultiplier /= _sprintMultiplier;
        }
    }

    private void CrouchStarted()
    {
        if (!_crouching)
        {
            _crouching = true;
            _speedMultiplier *= _crouchMultiplier;
        }
    }

    private void CrouchCanceled()
    {
        if (_crouching)
        {
            _crouching = false;
            _speedMultiplier /= _crouchMultiplier;
        }
    }
    #endregion
}
