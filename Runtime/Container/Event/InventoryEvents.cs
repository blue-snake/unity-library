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
}