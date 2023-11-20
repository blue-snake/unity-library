using BlueSnake.Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlueSnake.Player {
    public class FirstPersonMovementController : MonoBehaviour {
        [Header("Reference")]
        public Transform orientation;
        public new Camera camera;

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

        /** Private fields **/
        private Vector3 _gravityVelocity;
        private Vector3 _currentDirection;
        private Vector3 _currentMoveVelocity;
        private Vector2 _currentMoveInput;
        private float _jumpHeight;
        private bool _hasLanded;
        private bool _moved;

        /** Event **/
        private EventManager _eventManager;

        public void InitializeEventManager(EventManager eventManager) {
            _eventManager = eventManager;
        }

        public virtual void Update() {
            if (camera != null) {
                orientation.rotation = Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);
            }
            isGrounded = Physics.CheckSphere(groundCheckOrigin.position, groundCheckRadius, groundCheckLayer);

            HandleLanding();
            HandleGravity();

            float verticalInput = _currentMoveInput.y;
            float horizontalInput = _currentMoveInput.x;

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

            Vector3 position = transform.position;
            Vector3 move = direction * (moveSpeed * Time.deltaTime);

            Vector3.SmoothDamp(position, position + move, ref _currentMoveVelocity, smoothTime);
            controller.Move(_currentMoveVelocity);
        }

        /// <summary>
        /// Register under unity event in input actions
        ///
        /// Those setting should be applied to the input:
        /// Action type: Value
        /// Control type: Vector2
        /// Mode: Digital
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context) {
            _currentMoveInput = context.ReadValue<Vector2>();
        }
        
        /// <summary>
        /// Register under unity event in input actions
        ///
        /// Those setting should be applied to the input:
        /// Action type: Button
        /// Mode: Digital
        /// </summary>
        /// <param name="context"></param>
        public void OnSprint(InputAction.CallbackContext context) {
            isSprinting = context.started || !context.canceled;
        }

        /// <summary>
        /// Register under unity event in input actions
        ///
        /// Those settings should be applied to the input:
        /// Interactions: Tap
        /// </summary>
        /// <param name="context"></param>
        public void OnJump(InputAction.CallbackContext context) {
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