using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace BlueSnake.UI.Bars {
    public class FillAmountBar : Bar {

        [Header("Reference")]
        [SerializeField]
        private Image[] targets;

        [Header("Animation")]
        [SerializeField]
        private float duration = 1f;

        private Tween? _tween;
        
        public override void SetValue(float value) {
            float current = currentValue;
            base.SetValue(value);
            _tween?.Stop();
            _tween = Tween.Custom(current, value, duration, updatedVal => {
                foreach (Image target in targets) {
                    target.fillAmount = updatedVal;
                }
            });
        }
    }
}