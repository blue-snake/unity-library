using System;

namespace BlueSnake.Container {
    
    [Serializable]
    public class ItemStack {

        public Item type;
        public int amount;

        public bool Compare(ItemStack stack) {
            return type.id.Equals(stack.type.id);
        }

        public bool HasReachedMaxStackSize() {
            return amount >= type.maxStackSize;
        }
        

        public ItemStack Clone() {
            ItemStack stack = new ItemStack {
                type = type,
                amount = amount
            };
            return stack;
        }
    }
}