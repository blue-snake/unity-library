using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlueSnake.Container {
    public class Hotbar : MonoBehaviour {
        
        [Header("Properties")]
        public List<InputActionReference> belt;

        [Header("Reference")]
        public Inventory inventory;
        [SerializeField]
        private Transform itemContainer;

        [Header("Inputs")]
        public InputActionReference primaryUse;
        public InputActionReference secondaryUse;

        private EquippedItem _currentEquippedItem;

        private void Awake() {
            for (int i = 0; i < belt.Count; i++) {
                InputActionReference reference = belt[i];
                int index = i;
                reference.action.performed += ev => {
                    if (HasEquippedItem()) {
                        if (_currentEquippedItem.inventoryIndex == index) {
                            UnEquip();
                            return;
                        }
                    }
                    Equip(index);
                };
            }

            if (primaryUse != null) {
                primaryUse.action.performed += _ => {
                    if (HasEquippedItem()) {
                        _currentEquippedItem.OnPrimaryUse(this);
                    }
                };
            }
            if (secondaryUse != null) {
                secondaryUse.action.performed += _ => {
                    if (HasEquippedItem()) {
                        _currentEquippedItem.OnSecondaryUse(this);
                    }
                };
            }
        }

        public void Equip(int index) {
            UnEquip();
            if (!inventory.HasItem(index)) {
                return;
            }

            ItemStack item = inventory.GetItem(index);
            if (item.type.equippedPrefab == null) {
                return;
            }
            _currentEquippedItem = Instantiate(item.type.equippedPrefab, itemContainer);
            _currentEquippedItem.inventoryIndex = index;
            _currentEquippedItem.OnEquip(this);
        }

        public void UnEquip() {
            if (HasEquippedItem()) {
                foreach (Transform child in itemContainer) {
                    Destroy(child.gameObject);
                }
                _currentEquippedItem.OnUnEquip(this);
                _currentEquippedItem = null;
            }
        }

        public bool HasEquippedItem() {
            return _currentEquippedItem != null;
        }
    }
}