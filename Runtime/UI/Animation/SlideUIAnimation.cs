using System;
using System.Collections;
using Tweens;
using UnityEngine;

namespace BlueSnake.UI.Animation {
    
    [Serializable]
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
        
        private TweenInstance _tweenInstance;

        public override IEnumerator PlayAnimation() {
            CancelAnimation();
            bool running = true;
            LocalPositionTween tween = new LocalPositionTween() {
                from = target.localPosition,
                to = slidePosition,
                delay = slideDelay,
                duration = slideDuration,
                onFinally = _ => {
                    running = false;
                }
            };
            _tweenInstance = target.gameObject.AddTween(tween);

            while (running) {
                yield return null;
            }
            _tweenInstance = null;
        }

        public override void CancelAnimation() {
            _tweenInstance?.Cancel();
            _tweenInstance = null;
        }
    }
}