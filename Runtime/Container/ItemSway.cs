using UnityEngine;
using UnityEngine.InputSystem;

namespace BlueSnake.Container
{
    
    /// <summary>
    /// Create a parent over the item container that holds the item and apply the script to the parent.
    /// </summary>
    public class ItemSway : MonoBehaviour
    {
        [Header("Sway")]
        public float step = 0.01f;
        public float maxStepDistance = 0.06f;
        
        [Header("Sway Rotation")]
        public float rotationStep = 4f;
        public float maxRotationStep = 5f;
        
        [Header("Smooth")]
        public float smooth = 10f;
        public float smoothRot = 12f;

        [Header("Inputs")]
        [SerializeField]
        private InputActionReference lookInput;
        
        private Vector3 _swayEulerRot; 
        private Vector3 _swayPos;

        private void Update() {
            Sway();
            SwayRotation();
            CompositePositionRotation();
        }
        
        void Sway(){
            Vector3 invertLook = lookInput.action.ReadValue<Vector2>() *-step;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

            _swayPos = invertLook;
        }

        private void SwayRotation(){
            Vector2 invertLook = lookInput.action.ReadValue<Vector2>() * -rotationStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);
            _swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
        }

        private void CompositePositionRotation(){
            transform.localPosition = Vector3.Lerp(transform.localPosition, _swayPos, Time.deltaTime * smooth);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(_swayEulerRot), Time.deltaTime * smoothRot);
        }
    }
}
