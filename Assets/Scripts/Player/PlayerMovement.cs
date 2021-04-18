using System.Collections;
using System.ComponentModel;
using UnityEngine;
// ReSharper disable BitwiseOperatorOnEnumWithoutFlags

namespace Rebirth.Player
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Member Fields

        // Exposed to editor
        [Header("Movement Speed")]
        [Tooltip("Base move speed of player")]
        [SerializeField] private float _baseSpeed = 5f;

        [Tooltip("Move speed multiplier while sprinting")]
        [SerializeField] private float _sprintMultiplier = 1.5f;

        [Tooltip("Move speed multiplier while crouching")]
        [SerializeField] private float _crouchMultiplier = 0.5f;

        [Tooltip("Move speed multiplier while moving backwards")]
        [SerializeField] private float _reverseMultiplier = 0.5f;

        [Tooltip("Minimum speed before player speed is set to zero")]
        [SerializeField] private float _minSpeed = 0.1f;

        [Header("Jumping")]
        [Tooltip("Force applied to player when jumping")]
        [SerializeField] private float _jumpForce = 4.5f;
        
        [Tooltip("Min number of seconds between jumps")]
        [SerializeField] private float _jumpCooldown = 0.5f;

        [Header("Physics")]
        [Tooltip("Static downward force on player (not applied when grounded)")]
        [SerializeField] private float _gravity = 9.81f;
        
        [Tooltip("Maximum downward velocity of player")]
        [SerializeField] private float _terminalVelocity = -50f;
        
        [Tooltip("Maximum upward velocity of player")]
        [SerializeField] private float _maxUpwardVelocity = 150f;
        
        [Space]
        [Tooltip("Directional force applied to the player when moving in a direction while not grounded")]
        [SerializeField] private float _airAcceleration = 10f;

        [Tooltip("Air speed scalar applied every FixedUpdate (Slows ungrounded player)")]
        [SerializeField, Range(0f, 1f)] private float _airFriction = 0.95f;

        [Tooltip("Ground speed scalar applied every FixedUpdate (Slows grounded player)")]
        [SerializeField, Range(0f, 1f)] private float _groundFriction = 0.75f;

        [Space]
        [Tooltip("Maximum slope that is considered ground")]
        [SerializeField, Range(0f, 90f)] private float _slopeLimit = 45f;

        [Tooltip("Height above the ground player floats (player is snapped this distance away from the ground while grounded)")]
        [SerializeField] private float _playerHeightAdjust = 0.125f;

        [Tooltip("Extra distance away from _playerHeightAdjust that is considered ground")]
        [SerializeField] private float _groundMagnetDistance = 0.125f;

        [Tooltip("Downward force applied to player when further above valid ground _playerHeightAdjust, but still grounded")]
        [SerializeField] private float _groundedDownforce = 1.5f;

        private float _speedMultiplier = 1f;

        private bool _sprinting;
        private bool _resumeSprint;
        private bool _crouching;
        private bool _canJump = true;

        [SerializeField, ReadOnly(true)] private bool _isGrounded;
        [SerializeField, ReadOnly(true)] private bool _wasGrounded;
        [SerializeField, ReadOnly(true)] private bool _jumping;
        private Vector3 _groundNormal = Vector3.up;

        // Components
        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;
        
        public Vector2 Movement { get; set; }
        #endregion

        #region Monobehaviour
        private void Awake()
        {
            _groundNormal = Vector3.up;
            
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
        }

        private void FixedUpdate()
        {
            _wasGrounded = _isGrounded;
            _isGrounded = CheckGround();
            Friction();
            Gravity();
            Locomotion();



            _jumping = (!_wasGrounded || _isGrounded) && _jumping;
        }
        #endregion

        #region Member Methods
        public void Jump()
        {
            if (_isGrounded && _canJump)
            {
                _jumping = true;
                _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
                var velocity = _rigidbody.velocity;
                velocity.y = _jumpForce;
                _rigidbody.velocity = velocity;

                _isGrounded = false;

                StartCoroutine(JumpCooldown());
            }
        }
        
        public void SprintStarted()
        {
            if (!_sprinting)
            {
                _sprinting = true;
                _speedMultiplier *= _sprintMultiplier;
            }
        }

        public void SprintCanceled()
        {
            // Allow cancel sprint while moving backwards
            _resumeSprint = false;
            // Cancel sprint
            if (_sprinting)
            {
                _sprinting = false;
                _speedMultiplier /= _sprintMultiplier;
            }
        }

        public void CrouchStarted()
        {
            if (!_crouching)
            {
                _crouching = true;
                _speedMultiplier *= _crouchMultiplier;
            }
        }

        public void CrouchCanceled()
        {
            if (_crouching)
            {
                _crouching = false;
                _speedMultiplier /= _crouchMultiplier;
            }
        }
        
        private void Locomotion()
        { 
            var movement = _baseSpeed * _speedMultiplier * Movement;

            if(movement.magnitude <= _minSpeed)
            {
                return;
            }

            // Reduces travel speed and prevents sprinting when traveling backwards
            if (movement.y <= 0)
            {
                movement *= _reverseMultiplier;
                if (_sprinting)
                {
                    SprintCanceled();
                    // Only do this once to allow canceling sprinting while moving backwards
                    _resumeSprint = true;
                }
            } else if (_resumeSprint)
            {
                SprintStarted();
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
                }

                _rigidbody.velocity = hVel + _rigidbody.velocity.y * Vector3.up;
            } else
            {
                var airAcceleration = movement.normalized * _airAcceleration;

                _rigidbody.AddRelativeForce(new Vector3(airAcceleration.x, 0f, airAcceleration.y), ForceMode.Acceleration);
            }
        }

        private void Gravity()
        {
            var velocity = _rigidbody.velocity;
            if (!_isGrounded)
            {
                _rigidbody.AddForce(transform.up * -_gravity, ForceMode.Acceleration);

                // Limits vertical speed within defined bounds
                velocity.y = Mathf.Clamp(_rigidbody.velocity.y, _terminalVelocity, _maxUpwardVelocity);

                _rigidbody.velocity = velocity;
            } else if(!_jumping){
                velocity.y = -_groundedDownforce;
            }

            _rigidbody.velocity = velocity;
        }

        private void Friction()
        {
            var friction = _isGrounded ? _groundFriction : _airFriction;
            var velocity = _rigidbody.velocity;
            var hVel = velocity;
            hVel.y = 0.0f;

            hVel *= hVel.magnitude <= _minSpeed ? 0.0f : friction;
            _rigidbody.velocity = hVel + velocity.y * transform.up;
        }


        private bool CheckGround()
        {
            var colliderBounds = _collider.bounds;
            var rayLen = _playerHeightAdjust + colliderBounds.extents.y;
            var up = transform.up;
            var rayHit = Physics.Raycast(colliderBounds.center, -up, out var hit, rayLen + _groundMagnetDistance);
            var slopeValid = Vector3.Angle(hit.normal, up) <= _slopeLimit;

            if (rayHit && slopeValid)
            {
                if (!_jumping)
                {
                    //Adjust players height to properly 
                    var transformValue = transform;
                    var adjPos = transformValue.position;
                    adjPos.y = adjPos.y + rayLen - hit.distance;
                    transformValue.position = adjPos;

                    //Ensures player doesn't jitter when rigid body isn't contacting ground
                    _rigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
                } 
                _groundNormal = hit.normal;

                return true;
            }

            _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
            _groundNormal = transform.up;
            return false;
        }

        private IEnumerator JumpCooldown()
        {
            _canJump = false;
            yield return new WaitForSeconds(_jumpCooldown);
            _canJump = true;
        }
        #endregion
    }
}
