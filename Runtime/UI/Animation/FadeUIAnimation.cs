using Tweens;
using UnityEngine;

namespace BlueSnake.UI.Animation {
    public class FadeUIAnimation : UIAnimation {
        
        [Header("Reference")]
        [SerializeField]
        private CanvasGroup target;

        [Header("Properties")]
        [SerializeField]
        private bool handleRaycasts = true;

        [SerializeField]
        private float fadeOutDelay = 0f;
        [SerializeField]
        private float fadeOutDuration = 1f;
        [SerializeField]
        private float fadeInDelay = 0f;
        [SerializeField]
        private float fadeInDuration = 1f;
        
        private TweenInstance _tweenInstance;
        
        public override void StartAnimation(UIAnimationCallback callback) {
            _tweenInstance?.Cancel();
            if (handleRaycasts) {
                target.blocksRaycasts = false;
            }
            FloatTween tween = new FloatTween {
                from = target.alpha,
                to = 0f,
                delay = fadeOutDelay,
                duration = fadeOutDuration,
                onUpdate = (_, value) => {
                    target.alpha = value;
                },
                onEnd = _ => {
                    callback?.Invoke();
                }
            };
            _tweenInstance = target.gameObject.AddTween(tween);
        }

        public override void StopAnimation(bool force, UIAnimationCallback callback) {
            _tweenInstance?.Cancel();
            if (handleRaycasts) {
                target.blocksRaycasts = true;
            }
            if (force) {
                target.alpha = 1f;
            } else {
                FloatTween tween = new FloatTween {
                    from = target.alpha,
                    to = 1f,
                    delay = fadeInDelay,
                    duration = fadeInDuration,
                    onUpdate = (_, value) => { target.alpha = value; },
                    onEnd = _ => {
                        callback?.Invoke();
                    }
                };
                _tweenInstance = target.gameObject.AddTween(tween);
            }
        }
    }
}