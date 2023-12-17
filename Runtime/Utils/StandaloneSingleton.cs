using System;

namespace BlueSnake.Utils {
    public class StandaloneSingleton<T> where T : StandaloneSingleton<T> {
        protected static T instance;

        public virtual void OnCreate() { }

        public static T GetInstance() {
            return instance ??= Activator.CreateInstance<T>();
        }
    }
}