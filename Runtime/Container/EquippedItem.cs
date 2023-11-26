using UnityEngine;

namespace BlueSnake.Container {
    public class EquippedItem : MonoBehaviour {

        [HideInInspector]
        public ItemStack item;

        [HideInInspector]
        public int inventoryIndex;

        /// <summary>
        /// Will be called when player presses primary key (GetKeyDown)
        /// </summary>
        /// <param name="hotbar"></param>
        public virtual void OnPrimaryUse(Hotbar hotbar) { }

        /// <summary>
        /// Will be called when player holds primary key (GetKey)
        /// </summary>
        /// <param name="hotbar"></param>
        public virtual void OnPrimaryHoldUse(Hotbar hotbar) { }

        /// <summary>
        /// Will be called when player presses primary key (GetKeyDown)
        /// </summary>
        /// <param name="hotbar"></param>
        public virtual void OnSecondaryUse(Hotbar hotbar) { }

        /// <summary>
        /// Will be called when player holds secondary key (GetKey)
        /// </summary>
        /// <param name="hotbar"></param>
        public virtual void OnSecondaryHoldUse(Hotbar hotbar) { }

        /// <summary>
        /// Called when player equips this item
        /// </summary>
        /// <param name="hotbar"></param>
        public virtual void OnEquip(Hotbar hotbar) { }

        /// <summary>
        /// Called when player un equips this item
        /// </summary>
        /// <param name="hotbar"></param>
        public virtual void OnUnEquip(Hotbar hotbar) { }

    }
}