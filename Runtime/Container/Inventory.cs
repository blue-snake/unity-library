using System.Collections.Generic;
using System.Linq;
using BlueSnake.Container.Event;
using BlueSnake.Event;
using UnityEngine;

namespace BlueSnake.Container {
    public class Inventory : MonoBehaviour {
        
        public List<ItemStack> items = new();

        [Header("Properties")]
        public int maxSize = 10;

        private EventManager _eventManager;

        public void InitializeEventManager(EventManager eventManager) {
            _eventManager = eventManager;
        }

        public bool AddItem(ItemStack stack) {
            foreach (ItemStack current in items) {
                if (!current.Compare(stack)) continue;
                if (current.HasReachedMaxStackSize()) {
                    continue;
                }
                int next = current.amount + stack.amount;
                if (next <= current.type.maxStackSize) {
                    current.amount = next;
                    _eventManager?.Publish(new InventoryUpdateItemEvent() {
                        Inventory = this,
                        ItemStack = current
                    });
                    return true;
                }
                int difference = current.type.maxStackSize - current.amount;
                current.amount = current.type.maxStackSize;
                stack.amount -= difference;
                    
                _eventManager?.Publish(new InventoryUpdateItemEvent() {
                    Inventory = this,
                    ItemStack = current
                });
                if (stack.amount <= 0) {
                    return true;
                }
            }

            if (!IsInventoryFull()) {
                items.Add(stack);
                _eventManager?.Publish(new InventoryAddItemEvent {
                    Inventory = this,
                    ItemStack = stack
                });
                return true;
            }
            return false;
        }

        public ItemStack GetItem(int index) {
            return items[index];
        }

        public bool HasItems(string itemTypeId, int amount) {
            int sum = items.Where(current => current.type.id.Equals(itemTypeId)).Sum(current => current.amount);
            return sum >= amount;
        }

        public bool RemoveItem(string itemTypeId, int amount) {
            int remaining = amount;
            for (int i = 0; i < items.Count; i++) {
                ItemStack current = items[i];
                if (!current.type.id.Equals(itemTypeId)) continue;
                
                int next = current.amount - remaining;
                if (next > 0) {
                    current.amount = next;
                    _eventManager?.Publish(new InventoryUpdateItemEvent() {
                        Inventory = this,
                        ItemStack = current
                    });
                    remaining = 0;
                    break;
                }
                RemoveItem(i);
                remaining -= current.amount;
            }
            return remaining <= 0;
        }

        public bool RemoveItem(int index) {
            if (items.Count - 1 < index) return false;
            
            ItemStack current = items[index];
            items.RemoveAt(index);
            _eventManager?.Publish(new InventoryRemoveItemEvent() {
                Inventory = this,
                Index = index,
                ItemStack = current
            });
            return true;
        }

        public bool IsInventoryFull() {
            return items.Count >= maxSize;
        }
    }
}