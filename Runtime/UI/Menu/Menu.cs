using System.Collections;
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
        private UIAnimator animator;
        
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
            if (animator != null) {
                animator.PlayTransition("Open");
                onOpen?.Invoke();
            } else {
                OpenFallback();
            }
        }

        public IEnumerator OpenAwaitable() {
            if (animator != null) {
                yield return animator.RunTransition("Open");
                onOpen?.Invoke();
            } else {
                OpenFallback();
            }
        }
        
        public void Close() {
            if (animator != null) {
                animator.PlayTransition("Closed");
                onClose?.Invoke();
            } else {
                CloseFallback();
            }
        }

        public IEnumerator CloseAwaitable() {
            if (animator != null) {
                yield return animator.RunTransition("Closed");
                onClose?.Invoke();
            } else {
                CloseFallback();
            }
        }

        private void OpenFallback() {
            content.SetActive(true);
            onOpen?.Invoke();
        }

        private void CloseFallback() {
            content.SetActive(false);
            onClose?.Invoke();
        }
        

  

        public bool HasParent() {
            return parent != null;
        }
    }
}