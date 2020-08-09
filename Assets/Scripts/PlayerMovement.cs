
using System;
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
    [SerializeField] private float _jumpCooldown = 1f;

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
    [SerializeField] private float _slopeMagnetDistance = 0.5f;

    private float _speedMultiplier = 1f;

    private bool _sprinting = false;
    private bool _crouching = false;
    private bool _canJump = true;

    [SerializeField, ReadOnly(true)] private bool _isGrounded = false;
    [SerializeField, ReadOnly(true)] private bool _wasGrounded = false;
    [SerializeField, ReadOnly(true)] private Vector3 _groundNormal = Vector3.up;
    [SerializeField, ReadOnly(true)] private Vector3 _lastNormal = Vector3.up;


    private PlayerControls _playerControls;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        _groundNormal = Vector3.up;
        _lastNormal = Vector3.up;

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
        Locomotion();
        Gravity();
        Friction();
        CheckSlopeChange();

        _wasGrounded = _isGrounded;
        _lastNormal = _groundNormal;

        _isGrounded = false;
        _groundNormal = transform.up;
    }

    private void OnCollisionStay(Collision collision)
    {
        var contacts = new ContactPoint[collision.contactCount];
        var groundNormal = Vector3.zero;

        collision.GetContacts(contacts);

        foreach (var contact in contacts)
        {
            var bottom = _collider.bounds.center - Vector3.up * _collider.bounds.extents.y;
            var curve = bottom + (Vector3.up * _collider.radius);
            var dir = curve - contact.point;
            var angle = Vector3.Angle(contact.normal, transform.up);

            //Checks if contact is valid ground
            if (dir.y > 0f && (Mathf.Abs(angle) <= _slopeLimit))
            {
                _isGrounded = true;
                groundNormal += contact.normal;
            }
        }
        // Sets normalized average of valid ground normals if grounded
        _groundNormal = groundNormal == Vector3.zero ? transform.up : groundNormal.normalized;
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

        // Reduces travel speed and prevents sprinting when traveling backwards
        if (movement.y <= 0)
        {
            movement *= _reverseMultiplier;
            SprintCanceled();
        }

        if (_isGrounded)
        {
            var hVel = new Vector3(movement.x, 0.0f, movement.y);
            hVel = transform.rotation * hVel;

            var incline = Vector3.Angle(hVel, _groundNormal) - 90.0f;

            if(incline > 0.0f)
            {
                // Reduces velocity when climbing a slope
                var speedScale = 1 - incline / 90.0f;
                hVel *= speedScale;
                _rigidbody.velocity = hVel + _rigidbody.velocity.y * transform.up;
            } else
            {
                // Rotates velocity to align with slope when descending a slope
                _rigidbody.velocity = AlignVectorWithGround(hVel) + GetGravityVelocity();
            }
        } else
        {
            var airAcceleration = movement.normalized * _airAcceleration;

            _rigidbody.AddRelativeForce(new Vector3(airAcceleration.x, 0f, airAcceleration.y), ForceMode.Acceleration);
        }
    }

    private void Gravity()
    {
        _rigidbody.AddForce(_groundNormal * -_gravity, ForceMode.Acceleration);

        // Limits vertical speed within defined bounds
        var velocity = _rigidbody.velocity;
        velocity.y = Mathf.Clamp(_rigidbody.velocity.y, _terminalVelocity, _maxUpwardVelocity);

        _rigidbody.velocity = velocity;
    }

    private void Friction()
    {
        var friction = _isGrounded ? _groundFriction : _airFriction;
        var hVel = _rigidbody.velocity;
        hVel.y = 0.0f;

        hVel *= hVel.magnitude <= _minSpeed ? 0.0f : friction;

        _rigidbody.velocity = hVel + _rigidbody.velocity.y * transform.up;
    }

    private void CheckSlopeChange()
    {
        if (_wasGrounded && !_isGrounded)
        {
            var rayHit = Physics.Raycast(transform.position, -transform.up, out var hit, _slopeMagnetDistance);

            if (rayHit && Vector3.Angle(hit.normal, transform.up) < _slopeLimit)
            {
                var rotation = Quaternion.FromToRotation(_lastNormal, hit.normal);
                _rigidbody.velocity = rotation * _rigidbody.velocity;
            }
        }

    }

    private Vector3 GetGravityVelocity()
    {
        var gravSpeed = AlignVectorWithGround(_rigidbody.velocity).y;
        return _groundNormal * gravSpeed;
    }

    private Vector3 AlignVectorWithGround(Vector3 v)
    {
        var rotation = Quaternion.FromToRotation(transform.up, _groundNormal);
        return rotation * v;
    }

    private void Jump()
    {
        if (_isGrounded && _canJump)
        {
            _wasGrounded = false;
            _isGrounded = false;
            _rigidbody.AddForce(_groundNormal * _jumpForce, ForceMode.VelocityChange);
            StartCoroutine(JumpCooldown());
        }
    }

    private IEnumerator JumpCooldown()
    {
        _canJump = false;
        yield return new WaitForSeconds(_jumpCooldown);
        _canJump = true;
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
