using System;
using BlueSnake.Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlueSnake.Player {
    public class FirstPersonMovementController : MonoBehaviour {
        [Header("Reference")]
        public Transform orientation;

        public CharacterController controller;

        [Header("Properties")]
        public float gravity = -25f;

        public float speed = 2f;
        public float sprintSpeed = 3f;
        
        public float jumpHeight = 2f;

        public float smoothTime = 0.3f;

        [Header("Ground")]
        public Transform groundCheckOrigin;

        public LayerMask groundCheckLayer;
        public float groundCheckRadius = 0.2f;

        [HideInInspector]
        public bool isGrounded;

        [HideInInspector]
        public bool isSprinting;

        [Header("Inputs")]
        [SerializeField]
        private InputActionReference moveInput;
        [SerializeField]
        private InputActionReference sprintInput;
        [SerializeField]
        private InputActionReference jumpInput;
        

        /** Private fields **/
        private Vector3 _gravityVelocity;
        private Vector3 _currentDirection;
        private Vector3 _currentMoveVelocity;
        private Vector3 _currentMoveDampVelocity;
        private float _jumpHeight;
        private bool _hasLanded;
        private bool _moved;

        /** Event **/
        private EventManager _eventManager;

        private void Start() {
            jumpInput.action.performed += OnPlayerJump;
        }

        public void InitializeEventManager(EventManager eventManager) {
            _eventManager = eventManager;
        }

        public virtual void Update() {
            isSprinting = sprintInput.action.IsPressed();
            Vector2 input = moveInput.action.ReadValue<Vector2>();
            isGrounded = Physics.CheckSphere(groundCheckOrigin.position, groundCheckRadius, groundCheckLayer);

            HandleLanding();
            

            float verticalInput = input.y;
            float horizontalInput = input.x;

            Vector3 direction = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

            float moveSpeed = isSprinting ? sprintSpeed : speed;
            if (horizontalInput != 0 || verticalInput != 0) {
                PlayerMoveEvent moveEvent = new PlayerMoveEvent {
                    Speed = moveSpeed,
                    Cancelled = false,
                    Direction = direction
                };
                _eventManager.Publish(moveEvent);
                if (moveEvent.Cancelled) return;
                moveSpeed = moveEvent.Speed;
                direction = moveEvent.Direction;
                _moved = true;
            } else if (horizontalInput == 0 && verticalInput == 0 && _moved) {
                _moved = false;
                _eventManager.Publish(new PlayerMoveStopEvent());
            }
            
            _currentMoveVelocity =Vector3.SmoothDamp(_currentMoveVelocity, direction * (moveSpeed * 2f), ref _currentMoveDampVelocity, smoothTime);
            controller.Move(_currentMoveVelocity * Time.deltaTime);
            
            HandleGravity();
        }
        
        
        public void OnPlayerJump(InputAction.CallbackContext context) {
            if (jumpHeight == 0) return;

            if (!isGrounded) return;

            PlayerJumpEvent jumpEvent = new PlayerJumpEvent {
                Height = jumpHeight
            };
            _eventManager.Publish(jumpEvent);
            if (jumpEvent.Cancelled) return;
            _jumpHeight = jumpEvent.Height;
        }

        /// <summary>
        /// Call <see cref="PlayerLandEvent"/> when player has landed
        /// </summary>
        private void HandleLanding() {
            if (!isGrounded) {
                _hasLanded = false;
            } else {
                if (!_hasLanded) {
                    _hasLanded = true;
                    _eventManager.Publish(new PlayerLandEvent());
                }
            }
        }

        /// <summary>
        /// Handle custom gravity
        /// </summary>
        private void HandleGravity() {
            if (isGrounded && _gravityVelocity.y < 0) {
                _gravityVelocity.y = -2f;
            }

            if (_jumpHeight != 0) {
                _gravityVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * gravity);
                _jumpHeight = 0;
            }

            _gravityVelocity.y += gravity * Time.deltaTime;

            controller.Move(_gravityVelocity * Time.deltaTime);
        }
    }


    public class PlayerMoveEvent : IEvent {
        public Vector3 Direction { get; set; }
        public float Speed { get; set; }

        public bool Cancelled { get; set; }
    }

    public class PlayerMoveStopEvent : IEvent { }

    public class PlayerJumpEvent : IEvent {
        public float Height { get; set; }

        public bool Cancelled { get; set; }
    }

    public class PlayerLandEvent : IEvent { }
}