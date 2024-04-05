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
        

        /// <summary>
        /// Pick up item
        /// Can be cancelled with events
        /// </summary>
        /// <param name="pickable">Can only be false if event is cancelled</param>
        public bool PickUpItem(PickableItem pickable) {
            InventoryPickUpEvent ev = new InventoryPickUpEvent {
                Inventory = this,
                PickableItem = pickable
            };
            EventManager.GetInstance().Publish(ev);
            if (ev.IsCancelled()) {
                return false;
            }
            Destroy(pickable.gameObject);
            AddItem(pickable.item);
            return true;
        }

        /// <summary>
        /// Drop item by index
        /// Throw rigidbody with force in specific direction
        /// </summary>
        /// <param name="index"></param>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <param name="force"></param>
        /// <returns>Can only be false if item not found</returns>
        public bool DropItem(int index, Vector3 position, Vector3 direction, float force) {
            if (!HasItem(index)) {
                return false;
            }
            ItemStack stack = GetItem(index);
            GameObject dropItemObject = Instantiate(stack.type.worldPrefab, position, Quaternion.identity);
            Rigidbody rigidbody = dropItemObject.GetComponent<Rigidbody>();
            if (rigidbody != null) {
                rigidbody.AddForce(direction * force, ForceMode.Impulse);
            }
            PickableItem pickable = dropItemObject.GetComponent<PickableItem>();
            if (pickable != null) {
                pickable.item = stack;
            }
            RemoveItem(index);
            return true;
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
                    EventManager.GetInstance().Publish(new InventoryUpdateItemEvent() {
                        Inventory = this,
                        ItemStack = current
                    });
                    return true;
                }
                int difference = current.type.maxStackSize - current.amount;
                current.amount = current.type.maxStackSize;
                stack.amount -= difference;
                    
                EventManager.GetInstance().Publish(new InventoryUpdateItemEvent() {
                    Inventory = this,
                    ItemStack = current
                });
                if (stack.amount <= 0) {
                    return true;
                }
            }

            if (!IsInventoryFull()) {
                items.Add(stack);
                EventManager.GetInstance().Publish(new InventoryAddItemEvent {
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
        /// Check if item exists by index
        /// </summary>
        /// <param name="index">Mostly used for slots</param>
        /// <returns></returns>
        public bool HasItem(int index) {
            return items.Count - 1 >= index;
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
                    EventManager.GetInstance().Publish(new InventoryUpdateItemEvent() {
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
            EventManager.GetInstance().Publish(new InventoryRemoveItemEvent() {
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