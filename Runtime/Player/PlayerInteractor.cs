using System;
using BlueSnake.Event;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlueSnake.Player {
    public class PlayerInteractor : MonoBehaviour {
        
        [Header("Properties")]
        [SerializeField]
        private LayerMask layer;
        [SerializeField]
        private float range = 5;

        [Header("Inputs")]
        [SerializeField]
        private InputActionReference interactInput;

        [Header("References")]
        [SerializeField]
        private new Transform camera;
        
        [CanBeNull]
        protected IPlayerInteractable currentInteractable;
        [CanBeNull]
        protected GameObject currentGameObject;
        
        private void Awake() {
            interactInput.action.performed += _ => {
                if (currentInteractable != null && currentGameObject != null) {
                    currentInteractable.OnInteract(this);
                    EventManager.GetInstance().Publish(new PlayerInteractEvent {
                        interactor = this,
                        target = currentGameObject
                    });
                }
            };
        }

        private void Update() {
            if(Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, range, layer)) {
                if(hit.collider.TryGetComponent(out IPlayerInteractable interactable) && currentInteractable == null && currentGameObject == null) {
                    currentInteractable = interactable;
                    currentGameObject = hit.collider.gameObject;
                    OnHoverEnter();
                }
            } else {
                if(currentInteractable != null && currentGameObject != null) {
                    OnHoverExit();
                    currentInteractable = null;
                    currentGameObject = null;
                }
            }
        }
        
        protected virtual void OnHoverEnter() { }
        protected virtual void OnHoverExit() { }
        
    }

    public class PlayerInteractEvent : IEvent {

        public PlayerInteractor interactor;
        public GameObject target;

    }
    
    public interface IPlayerInteractable {
        
        void OnInteract(PlayerInteractor interactor);
        
    }
}