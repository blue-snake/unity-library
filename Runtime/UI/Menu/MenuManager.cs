using System.Collections.Generic;
using BlueSnake.Event;
using UnityEngine.SceneManagement;

namespace BlueSnake.UI.Menu {
    public class MenuManager {

        protected static MenuManager Instance;

        public Dictionary<string, Menu> Menus = new();
        private List<Menu> _openMenus = new();
        

        public MenuManager() {
            SceneManager.sceneUnloaded += scene => {
                _openMenus.Clear();
            };
        }
        

        public void Register(Menu menu) {
            Menus[menu.id] = menu;
            foreach(Menu child in menu.children) {
                child.parent = menu;
                Register(child);
            }
        }

        public void Unregister(string id) {
            Menus.Remove(id);
        }
        
        public void OpenOrClose(string id) {
            if (IsMenuOpen(id)) {
                Close(id);
            } else {
                Open(id);
            }
        }

        public Menu Open(string id) {
            if (!Exists(id) || IsMenuOpen(id)) {
                return null;
            }
            Menu menu = Menus[id];
            if (menu.HasParent()) {
                if (!IsMenuOpen(menu.parent.id)) {
                    return null;
                }
            } else {
                Close();
            }
            _openMenus.Add(menu);
            menu.Open();
            EventManager.GetInstance().Publish(new MenuOpenEvent {
                menu = menu
            });
            return menu;
        }
        
        
        public Menu CloseChildren(string id) {
            if (Menus.TryGetValue(id, out Menu menu)) {
                foreach (Menu child in menu.children) {
                    CloseChildrenRecursive(child);
                }
                return menu;
            }
            return null;
        }
        
        public Menu Close(string id) {
            if (Exists(id) && IsMenuOpen(id)) {
                Menu menu = Menus[id];
                if (menu.HasParent()) {
                    CloseChildrenRecursive(menu);
                } else {
                    Close();
                }
                return menu;
            }
            return null;
        }
        
        public void Close() {
            foreach (Menu menu in _openMenus) {
                menu.Close();
                EventManager.GetInstance().Publish(new MenuCloseEvent {
                    menu = menu
                });
            }
            _openMenus.Clear();
            EventManager.GetInstance().Publish(new MenuCloseAllEvent());
        }
        
        private void CloseChildrenRecursive(Menu menu) {
            menu.Close();
            EventManager.GetInstance().Publish(new MenuCloseEvent {
                menu = menu
            });
            _openMenus.Remove(menu);
            foreach (Menu child in menu.children) {
                if (IsMenuOpen(child.id)) {
                    CloseChildrenRecursive(child);
                }
            }
        }
        
        
        public bool Exists(string id) {
            return Menus.ContainsKey(id);
        }

        public bool IsMenuOpen(string id) {
            foreach (Menu menu in _openMenus) {
                if (menu.id.Equals(id)) {
                    return true;
                }
            }
            return false;
        }

        public bool IsAnyMenuOpen() {
            return _openMenus.Count >= 1;
        }

        public int GetCount() {
            return _openMenus.Count;
        }
        
        public static MenuManager GetInstance() {
            if (Instance == null) {
                Instance = new MenuManager();
            }
            return Instance;
        }
    }

    public class MenuOpenEvent : IEvent {
        
        public Menu menu;
    }
    
    public class MenuCloseEvent : IEvent {
        
        public Menu menu;
    }
    
    public class MenuCloseAllEvent : IEvent {
        
    }
}