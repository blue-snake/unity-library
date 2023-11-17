using System;
using Tweens;
using UnityEngine;

namespace BlueSnake.UI.Animation {
    public class ScaleUIAnimation : UIAnimation {
        
        [Header("Reference")]
        [SerializeField]
        private Transform target;

        [Header("Properties")]
        [Header("Scale")]
        [SerializeField]
        private float scaleDelay;
        [SerializeField]
        private float scaleDuration = 1f;
        [SerializeField]
        private Vector3 scaleAmount = new(1.2f, 1.2f, 1.2f);

        [Header("Scale Back")]
        [SerializeField]
        private float scaleBackDelay;
        [SerializeField]
        private float scaleBackDuration = 1f;
        
        private TweenInstance _tweenInstance;
        private Vector3 _initialScaleAmount;

        private void Awake() {
            _initialScaleAmount = target.localScale;
        }

        public override void StartAnimation(UIAnimationCallback callback) {
            _tweenInstance?.Cancel();

            LocalScaleTween tween = new LocalScaleTween {
                from = target.localScale,
                to = scaleAmount,
                delay = scaleDelay,
                duration = scaleDuration,
                onEnd = _ => {
                    callback?.Invoke();
                }
            };
            _tweenInstance = target.gameObject.AddTween(tween);
        }

        public override void StopAnimation(bool force, UIAnimationCallback callback) {
            _tweenInstance?.Cancel();
            if (force) {
                target.localScale = _initialScaleAmount;
            } else {
                LocalScaleTween tween = new LocalScaleTween {
                    from = target.localScale,
                    to = _initialScaleAmount,
                    delay = scaleBackDelay,
                    duration = scaleBackDuration,
                    onEnd = _ => {
                        callback?.Invoke();
                    }
                };
                _tweenInstance = target.gameObject.AddTween(tween);
            }

        }
    }
}