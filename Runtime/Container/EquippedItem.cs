using UnityEngine;

namespace BlueSnake.Container {
    public class EquippedItem : MonoBehaviour {

        [HideInInspector]
        public ItemStack item;

        [HideInInspector]
        public int inventoryIndex;

        public virtual void OnPrimaryUse(Hotbar hotbar) { }

        public virtual void OnSecondaryUse(Hotbar hotbar) { }

        public virtual void OnEquip(Hotbar hotbar) { }

        public virtual void OnUnEquip(Hotbar hotbar) { }

    }
}