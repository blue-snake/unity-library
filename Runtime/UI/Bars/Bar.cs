using UnityEngine;

namespace BlueSnake.UI.Bars {
    public class Bar : MonoBehaviour {

        [Header("Properties")]
        [SerializeField]
        protected float initialValue = 1f;
        [SerializeField]
        protected float maxValue = 1f;
        

        protected float currentValue;
        
        public virtual void Awake() {
            currentValue = initialValue;
        }

        public virtual void SetValue(float value) {
            currentValue = Mathf.Clamp(value, 0f, maxValue);
        }

        public float GetCurrentValue() {
            return currentValue;
        }
    }
}