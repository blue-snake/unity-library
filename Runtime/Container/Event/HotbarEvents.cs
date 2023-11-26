using BlueSnake.Event;

namespace BlueSnake.Container.Event {
    
    public class HotbarEquipEvent : IEvent {

        public Hotbar Hotbar;
        public EquippedItem EquippedItem;
    }
    
    public class HotbarUnEquipEvent : IEvent {

        public Hotbar Hotbar;
        public EquippedItem EquippedItem;
    }
    
    public class HotbarPrimaryUseEvent : IEvent {

        public Hotbar Hotbar;
        public EquippedItem EquippedItem;
    }
    
    public class HotbarSecondaryUseEvent : IEvent {

        public Hotbar Hotbar;
        public EquippedItem EquippedItem;
    }
}