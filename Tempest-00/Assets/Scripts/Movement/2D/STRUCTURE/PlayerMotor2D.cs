//-------------------------------------------------------------------//
//--------------------------TEMPEST ARCHIVE--------------------------//
//-------------------------------------------------------------------//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tempest.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMotor2D : MonoBehaviour
    {
        private Rigidbody2D _rb;

        // Public read-only access
        public Vector2 Velocity => _velocity;
        public float HorizontalVelocity => _velocity.x;
        public float VerticalVelocity => _velocity.y;
        public int FacingDirection { get; private set; } = 1;

        // Internal velocity cache (staged)
        private Vector2 _velocity;

        // Horizontal movement intent
        private float _inputX;
        private float _maxSpeed;
        private float _acceleration;
        private float _deceleration;

        // Jump request
        private bool _jumpRequested;
        private float _jumpForce;

        // Gravity
        private bool _useGravity;
        private float _gravity;
        private float _maxFallSpeed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _velocity = _rb.velocity;
        }

        //----- PUBLIC API -----//
        public void SetHorizontalMovement(float inputX, float maxSpeed, float acceleration, float deceleration)
        {
            _inputX = inputX;
            _maxSpeed = maxSpeed;
            _acceleration = acceleration;
            _deceleration = deceleration;
        }

        public void ApplyGravity(float gravity, float maxFallSpeed)
        {
            _useGravity = true;
            _gravity = gravity;
            _maxFallSpeed = maxFallSpeed;
        }

        public void Jump(float jumpForce)
        {
            _jumpRequested = true;
            _jumpForce = jumpForce;
        }

        public void FaceInput(float inputX)
        {
            if (inputX > 0.01f) FacingDirection = 1;
            else if (inputX < -0.01f) FacingDirection = -1;
        }

        public void CommitVelocity()
        {
            float deltaTime = Time.fixedDeltaTime;

            // Start from current velocity
            _velocity = _rb.velocity;

            //----- HORIZONTAL MOVEMENT -----
            float targetSpeed = _inputX * _maxSpeed;

            if (Mathf.Abs(_inputX) > 0.01f)
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, targetSpeed, _acceleration * deltaTime);
            }
            else
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0f, _deceleration * deltaTime);
            }

            //----- JUMP -----
            if (_jumpRequested)
            {
                _velocity.y = _jumpForce;
                _jumpRequested = false;
            }

            //----- GRAVITY -----
            if (_useGravity)
            {
                _velocity.y += _gravity * deltaTime;

                if (_velocity.y < _maxFallSpeed) _velocity.y = _maxFallSpeed;
            }

            //Apply final velocity
            _rb.velocity = _velocity;

            //Reset per-frame flags
            _useGravity = false;
        }
    }
}