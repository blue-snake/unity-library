using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlueSnake.Camera {
    public class CameraController : MonoBehaviour {


        [Header("Reference")]
        public new UnityEngine.Camera camera;
        
        [SerializeField]
        private List<Transform> transformsToRotate;

        [SerializeField]
        private Transform followTarget;

        [Header("Inputs")]
        [SerializeField]
        private InputActionReference lookInput;

        [Header("Properties")]
        [SerializeField]
        private Vector2 clampInDegress = new(360, 180);

        [SerializeField]
        private Vector2 sensitivity = new(0.3f, 0.3f);

        [SerializeField]
        private Vector2 smoothing = new(10, 10);
        
        [SerializeField]
        private Vector2 targetDirection;

        [SerializeField]
        private float fovLerpTime = 1f;
        
        
        private Vector2 _mouseAbsolute;
        private Vector2 _smoothMouse;

        private float _nextFov;

        private Transform _transform;
        

        private void Awake() {
            _transform = transform;
            _nextFov = camera.fieldOfView;
        }

        private void Start() {
            targetDirection = transform.rotation.eulerAngles;
        }

        public void Update() {
            if (followTarget != null) {
                _transform.position = followTarget.position;
            }
            Quaternion targetOrientation = Quaternion.Euler(targetDirection);
            
            Vector2 mouseDelta = lookInput.action.ReadValue<Vector2>();
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

            _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
            _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

            _mouseAbsolute += _smoothMouse;

            if (clampInDegress.x < 360) {
                _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegress.x * 0.5f, clampInDegress.x * 0.5f);
            }
            Quaternion xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
            _transform.localRotation = xRotation;

            if (clampInDegress.y < 360) {
                _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegress.y * 0.5f, clampInDegress.y * 0.5f);
            }
            
            Quaternion yRotation =
                Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            
            foreach (Transform to in transformsToRotate) {
                to.localRotation = xRotation;
                to.localRotation *= yRotation;
            }
            
            _transform.localRotation *= yRotation;
            _transform.rotation *= targetOrientation;
    
           
            
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, _nextFov, Time.deltaTime * fovLerpTime);
        }

        public void SetNextFov(float fov) {
            _nextFov = fov;
        }
      
    }
}
