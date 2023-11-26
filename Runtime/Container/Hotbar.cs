using System;
using System.Collections.Generic;
using BlueSnake.Container.Event;
using BlueSnake.Event;
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
                        inventory.eventManager?.Publish(new HotbarPrimaryUseEvent {
                            Hotbar = this,
                            EquippedItem = _currentEquippedItem
                        });
                    }
                };
            }
            if (secondaryUse != null) {
                secondaryUse.action.performed += _ => {
                    if (HasEquippedItem()) {
                        _currentEquippedItem.OnSecondaryUse(this);
                        inventory.eventManager?.Publish(new HotbarSecondaryUseEvent {
                            Hotbar = this,
                            EquippedItem = _currentEquippedItem
                        });
                    }
                };
            }
            inventory.eventManager?.Subscribe<InventoryRemoveItemEvent>(ev => {
                if (!HasEquippedItem()) {
                    return;
                }
                if (_currentEquippedItem.inventoryIndex == ev.Index) {
                    UnEquip();
                }
            });
        }

        private void Update() {
            if (!HasEquippedItem()) {
                return;
            }
            if (primaryUse != null) {
                if (primaryUse.action.IsPressed()) {
                    _currentEquippedItem.OnPrimaryHoldUse(this);
                }
            }

            if (secondaryUse != null) {
                if (secondaryUse.action.IsPressed()) {
                    _currentEquippedItem.OnSecondaryHoldUse(this);
                }
            }
        }

        /// <summary>
        /// Equip specific item by inventory index
        /// </summary>
        /// <param name="index">Mostly used for slots</param>
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
            inventory.eventManager?.Publish(new HotbarEquipEvent {
                Hotbar = this,
                EquippedItem = _currentEquippedItem
            });
        }

        /// <summary>
        /// Unequip current equipped item
        /// </summary>
        public void UnEquip() {
            if (HasEquippedItem()) {
                foreach (Transform child in itemContainer) {
                    Destroy(child.gameObject);
                }
                _currentEquippedItem.OnUnEquip(this);
                inventory.eventManager?.Publish(new HotbarUnEquipEvent {
                    Hotbar = this,
                    EquippedItem = _currentEquippedItem
                });
                _currentEquippedItem = null;
            }
        }

        public bool HasEquippedItem() {
            return _currentEquippedItem != null;
        }
    }
}