using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlueSnake.Camera {
    public class CameraController : MonoBehaviour {


        [Header("Reference")]
        [SerializeField]
        private Transform cameraHolder;
        
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
        private float sensitivityMultiplier = 2f;

        [SerializeField]
        private float minRotationX = -80f;

        [SerializeField]
        private float maxRotationX = 80f;

        [SerializeField]
        private float fovLerpTime = 1f;
        
        private float _sensitivityValue = 1;
        private float _currentRotationY;
        private float _currentRotationX;

        private float _nextFov;
        

        private void Awake() {
            _nextFov = camera.fieldOfView;
        }

        public void Update() {
            if (followTarget != null) {
                cameraHolder.position = followTarget.position;
            }

            float sens = _sensitivityValue * sensitivityMultiplier;
            Vector2 input = lookInput.action.ReadValue<Vector2>() * sens;
            _currentRotationY += input.x;
            _currentRotationX -= input.y;
            _currentRotationX = Mathf.Clamp(_currentRotationX, minRotationX, maxRotationX);
            
            cameraHolder.rotation = Quaternion.Euler(_currentRotationX, _currentRotationY, 0);
            
            Quaternion euler = Quaternion.Euler(0, _currentRotationY, 0);
            foreach (Transform rotate in transformsToRotate) {
                rotate.rotation = euler;
            }
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, _nextFov, Time.deltaTime * fovLerpTime);
        }

        public void SetNextFov(float fov) {
            _nextFov = fov;
        }
        
        public void SetSensitivity(float sens) {
            _sensitivityValue = sens;
        }
      
    }
}
