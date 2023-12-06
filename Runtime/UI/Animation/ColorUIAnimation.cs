using Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace BlueSnake.UI.Animation {
    public class ColorUIAnimation : UIAnimation {
        
        [Header("Reference")]
        [SerializeField]
        private Graphic target;

        [Header("Properties")]
        [Header("Color")]
        [SerializeField]
        private float colorDelay;
        [SerializeField]
        private float colorDuration = 1f;
        [SerializeField]
        private Color color = Color.blue;

        [Header("Color Back")]
        [SerializeField]
        private float colorBackDelay;
        [SerializeField]
        private float colorBackDuration = 1f;
        
        private TweenInstance _tweenInstance;
        private Color _initialColor;

        private void Awake() {
            _initialColor = target.color;
        }

        public override void StartAnimation(UIAnimationCallback callback) {
            _tweenInstance?.Cancel();

            ColorTween tween = new ColorTween {
                from = target.color,
                to = color,
                delay = colorDelay,
                duration = colorDuration,
                onEnd = _ => {
                    callback?.Invoke();
                },
                onUpdate = (_, clr) => {
                    target.color = clr;
                }
            };
            _tweenInstance = target.gameObject.AddTween(tween);
        }

        public override void StopAnimation(bool force, UIAnimationCallback callback) {
            _tweenInstance?.Cancel();
            if (force) {
                target.color = _initialColor;
            } else {
                ColorTween tween = new ColorTween {
                    from = target.color,
                    to = _initialColor,
                    delay = colorBackDelay,
                    duration = colorBackDuration,
                    onEnd = _ => {
                        callback?.Invoke();
                    },
                    onUpdate = (_, clr) => {
                        target.color = clr;
                    }
                };
                _tweenInstance = target.gameObject.AddTween(tween);
            }

        }
    }
}