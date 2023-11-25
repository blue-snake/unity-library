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

        /// <summary>
        /// Initialize event bus.
        /// It is optional
        /// </summary>
        /// <param name="eventManager"></param>
        public void InitializeEventManager(EventManager eventManager) {
            _eventManager = eventManager;
        }

        /// <summary>
        /// Add a item to the inventory.
        /// It searches for same-type items and increases it's amount, if nothing is found
        /// it just adds it to the inventory.
        /// </summary>
        /// <param name="stack"></param>
        /// <returns>The only time it can be false if the inventory is full</returns>
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

        /// <summary>
        /// Get item by index
        /// </summary>
        /// <param name="index">It is mostly used for slots</param>
        /// <returns></returns>
        public ItemStack GetItem(int index) {
            return items[index];
        }

        /// <summary>
        /// Check if inventory contains specific item type by x amount
        /// </summary>
        /// <param name="itemTypeId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool HasItems(string itemTypeId, int amount) {
            int sum = items.Where(current => current.type.id.Equals(itemTypeId)).Sum(current => current.amount);
            return sum >= amount;
        }

        /// <summary>
        /// Remove specific item type from inventory (Taking items from inventory by x amount)
        /// </summary>
        /// <param name="itemTypeId"></param>
        /// <param name="amount"></param>
        /// <returns>If nothing was found it is false otherwise true</returns>
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

        /// <summary>
        /// Remove item by index
        /// </summary>
        /// <param name="index">Mostly used for slots</param>
        /// <returns></returns>
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

        /// <summary>
        /// Check if inventory is full
        /// </summary>
        /// <returns></returns>
        public bool IsInventoryFull() {
            return items.Count >= maxSize;
        }
    }
}