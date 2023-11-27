using Tweens;
using UnityEngine;

namespace BlueSnake.UI.Animation {
    public class FadeUIAnimation : UIAnimation {
        
        [Header("Reference")]
        [SerializeField]
        private CanvasGroup target;

        [Header("Properties")]
        [SerializeField]
        private FadeType type = FadeType.FadeOut;
        [SerializeField]
        private bool handleRaycasts = true;

        [Header("Fade")]
        [SerializeField]
        private float fadeDelay;
        [SerializeField]
        private float fadeDuration = 1f;
        
        [Header("Fade Back")]
        [SerializeField]
        private float fadeBackDelay;
        [SerializeField]
        private float fadeBackDuration = 1f;
        
        private TweenInstance _tweenInstance;
        
        public override void StartAnimation(UIAnimationCallback callback) {
            _tweenInstance?.Cancel();
            if (handleRaycasts) {
                target.blocksRaycasts = type != FadeType.FadeOut;
            }
            FloatTween tween = new FloatTween {
                from = target.alpha,
                to = type == FadeType.FadeOut ? 0f : 1f,
                delay = fadeDelay,
                duration = fadeDuration,
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
                target.blocksRaycasts = type == FadeType.FadeOut;

            }
            if (force) {
                target.alpha = 1f;
            } else {
                FloatTween tween = new FloatTween {
                    from = target.alpha,
                    to = type == FadeType.FadeOut ? 1f : 0f,
                    delay = fadeBackDelay,
                    duration = fadeBackDuration,
                    onUpdate = (_, value) => { target.alpha = value; },
                    onEnd = _ => {
                        callback?.Invoke();
                    }
                };
                _tweenInstance = target.gameObject.AddTween(tween);
            }
        }
        
        public enum FadeType {
            FadeIn, FadeOut
        }
    }
}