﻿using System;
using BlueSnake.Container.Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlueSnake.Container {
    public class ItemPicker : MonoBehaviour {

        [Header("Reference")]
        public Inventory inventory;
        [SerializeField]
        private new UnityEngine.Camera camera;

        [Header("Properties")]
        [SerializeField]
        private float range = 5f;
        [SerializeField]
        private LayerMask itemLayer;

        [Header("Input")]
        [SerializeField]
        private InputActionReference pickUpInput;

        [Header("Ignore Collisions")]
        [SerializeField]
        private int playerCollisionLayer = -1;
        [SerializeField]
        private int itemCollisionLayer = -1;
        
        [HideInInspector]
        public PickableItem currentSelectedItem;

        private void Awake() {
            if (playerCollisionLayer != -1 && itemCollisionLayer != -1) {
                Physics.IgnoreLayerCollision(playerCollisionLayer, itemCollisionLayer);
            }
            pickUpInput.action.performed += _ => {
                if (currentSelectedItem != null) {
                    inventory.PickUpItem(currentSelectedItem);
                    inventory.eventManager?.Publish(new InventoryPickUpHoverEndEvent {
                        Inventory = inventory,
                        PickableItem = currentSelectedItem
                    });
                    currentSelectedItem = null;
                }
            };
        }

        private void Update() {
            if (currentSelectedItem != null) {
                inventory.eventManager?.Publish(new InventoryPickUpHoverEndEvent {
                    Inventory = inventory,
                    PickableItem = currentSelectedItem
                });
                currentSelectedItem = null;
            }
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, range, itemLayer)) {
                if (hit.transform.gameObject.TryGetComponent(out PickableItem pickableItem)) {
                    inventory.eventManager?.Publish(new InventoryPickUpHoverEvent {
                        Inventory = inventory,
                        PickableItem = pickableItem
                    });
                    currentSelectedItem = pickableItem;
                }
            } 
        }
        
        
    }
}