using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlueSnake.UI.Menu {
    public class MenuProxy : MonoBehaviour {
        
        public void OpenMenu(string id) {
            MenuManager.GetInstance().Open(id);
        }

        public void CloseAllMenus() {
            MenuManager.GetInstance().Close();
        }

        public void CloseMenu(string id) {
            MenuManager.GetInstance().Close(id);
        }

        public void CloseChildrenMenus(string id) {
            MenuManager.GetInstance().CloseChildren(id);
        }

        public void ChangeScene(string scene) {
            SceneManager.LoadScene(scene);
        }

        public void ExitGame() {
            Application.Quit();
        }
        
    }
}