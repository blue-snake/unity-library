using UnityEngine;

namespace BlueSnake.Container {
    
    [CreateAssetMenu(fileName = "Item", menuName = "Item/Default", order = 0)]
    public class Item : ScriptableObject {

        public string id;
        public string displayName;
        public int maxStackSize = 10;

    }
    
}