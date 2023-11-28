using Tweens;
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
        
        private TweenInstance _tweenInstance;
        
        public override void SetValue(float value) {
            float current = currentValue;
            base.SetValue(value);
            _tweenInstance?.Cancel();
            FloatTween tween = new FloatTween {
                from = current,
                to = value,
                duration = duration,
                onUpdate = (_, updatedValue) => {
                    foreach (Image target in targets) {
                        target.fillAmount = updatedValue;
                    }
                }
            };
            _tweenInstance = gameObject.AddTween(tween);
        }
    }
}