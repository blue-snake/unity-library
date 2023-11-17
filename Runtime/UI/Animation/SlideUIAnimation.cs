using Tweens;
using UnityEngine;

namespace BlueSnake.UI.Animation {
    public class SlideUIAnimation : UIAnimation {
        
        [Header("Reference")]
        [SerializeField]
        private Transform target;

        [Header("Properties")]
        [Header("Slide")]
        [SerializeField]
        private float slideDelay;
        [SerializeField]
        private float slideDuration = 1f;

        [SerializeField]
        private Vector3 slidePosition;

        [Header("Slide Back")]
        [SerializeField]
        private float slideBackDelay;
        [SerializeField]
        private float slideBackDuration = 1f;
        
        private TweenInstance _tweenInstance;
        private Vector3 _initialSlidePosition;

        private void Awake() {
            _initialSlidePosition = target.localPosition;
        }

        public override void StartAnimation(UIAnimationCallback callback) {
            _tweenInstance?.Cancel();

            LocalPositionTween tween = new LocalPositionTween() {
                from = target.localPosition,
                to = slidePosition,
                delay = slideDelay,
                duration = slideDuration,
                onEnd = _ => {
                    callback?.Invoke();
                }
            };
            _tweenInstance = target.gameObject.AddTween(tween);
        }

        public override void StopAnimation(bool force, UIAnimationCallback callback) {
            _tweenInstance?.Cancel();
            if (force) {
                target.localPosition = _initialSlidePosition;
            } else {
                LocalPositionTween tween = new LocalPositionTween {
                    from = target.localPosition,
                    to = _initialSlidePosition,
                    delay = slideBackDelay,
                    duration = slideBackDuration,
                    onEnd = _ => {
                        callback?.Invoke();
                    }
                };
                _tweenInstance = target.gameObject.AddTween(tween);
            }

        }
    }
}