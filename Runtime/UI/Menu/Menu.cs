using System.Collections.Generic;
using BlueSnake.UI.Animation;
using UnityEngine;
using UnityEngine.Events;

namespace BlueSnake.UI.Menu {
    public class Menu : MonoBehaviour {

        [Header("Properties")]
        public string id;
        
        [Header("Animation")]
        [SerializeField]
        private new UIAnimation animation;
        [SerializeField]
        private bool animationForceStop;
        
        [Header("Fallback")]
        [Tooltip("Not needed when using animation")]
        [SerializeField]
        private GameObject content;
        
        [Header("Children")]
        public List<Menu> children;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onOpen;

        [SerializeField]
        private UnityEvent onClose;
        
        [HideInInspector]
        public Menu parent;
        

        private void Awake() {
            MenuManager.GetInstance().Register(this);
        }

        public void Open() {
            if (animation != null) {
                animation.StartAnimation(() => {
                    onOpen?.Invoke();
                });
            } else {
                content.SetActive(true);
                onOpen?.Invoke();
            }
        }

        public void Close() {
            if (animation != null) {
                animation.StopAnimation(animationForceStop, () => {
                    onClose?.Invoke();
                });
            } else {
                content.SetActive(false);
                onClose?.Invoke();
            }
        }

        public bool HasParent() {
            return parent != null;
        }

    }
}