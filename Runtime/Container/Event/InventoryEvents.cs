using BlueSnake.Event;

namespace BlueSnake.Container.Event {
    
    public class InventoryAddItemEvent : IEvent {

        public Inventory Inventory;
        public ItemStack ItemStack;
    }
    
    public class InventoryUpdateItemEvent : IEvent {

        public Inventory Inventory;
        public ItemStack ItemStack;
    }
    
    public class InventoryRemoveItemEvent : IEvent {

        public Inventory Inventory;
        public int Index;
        public ItemStack ItemStack;
    }
    
    public class InventoryPickUpEvent : ICancelableEvent {

        public Inventory Inventory;
        public PickableItem PickableItem;
        private bool _cancelled;
        
        public bool IsCancelled() {
            return _cancelled;
        }

        public void SetCancelled(bool value) {
            _cancelled = value;
        }
    }
    
    public class InventoryPickUpHoverEvent : IEvent {

        public Inventory Inventory;
        public PickableItem PickableItem;
    }
}