using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BlueSnake.UI.Buttons {
    public class Button : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

        [SerializeField]
        private UnityEvent onClick;
        
        
        public void OnPointerClick(PointerEventData eventData) {
            Click();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            OnHoverEnter();
        }

        public void OnPointerExit(PointerEventData eventData) {
            OnHoverExit();
        }

        public void Click() {
            onClick?.Invoke();
            OnClick();
        }
        
        public virtual void OnClick() {}
        public virtual void OnHoverEnter() { }
        public virtual void OnHoverExit() { }
    }
}