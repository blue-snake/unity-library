using UnityEngine;

namespace BlueSnake.Utils {
    public class Singleton<T> : MonoBehaviour where T : Component {
        protected static T Instance;

        public virtual void Awake() {
            Instance = this as T;
        }

        public static T GetInstance() {
            return Instance;
        }
    }
}