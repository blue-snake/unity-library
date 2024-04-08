using BlueSnake.Utils;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

namespace BlueSnake.Camera {
    public class CursorManager : StandaloneSingleton<CursorManager> {

        public void ShowCursor() {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void HideCursor() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}