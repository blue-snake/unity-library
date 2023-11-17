using UnityEngine;

namespace BlueSnake.Utils {
    public static class Bootstrapper {
        public static void Load(string prefabName) {
            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load(prefabName)));
        }
    }
}