using System;
using System.Collections;
using Tweens;
using UnityEngine;

namespace BlueSnake.UI.Animation {
    [Serializable]
    public class ScaleUIAnimation : UIAnimation {

        [SerializeField]
        private GameObject target;

        [SerializeField]
        private Vector3 scaleAmount = new(1.1f, 1.1f, 1.1f);
        
        [SerializeField]
        private float scaleDelay;

        [SerializeField]
        private float scaleDuration = 0.2f;

        private TweenInstance _instance;
        
        public override IEnumerator PlayAnimation() {
            CancelAnimation();
            bool running = true;
            LocalScaleTween tween = new LocalScaleTween {
                from = target.transform.localScale,
                to = scaleAmount,
                delay = scaleDelay,
                duration = scaleDuration,
                onFinally = _ => {
                    running = false;
                }
            };
            _instance = target.AddTween(tween);
            while (running) {
                yield return null;
            }
            _instance = null;
        }
        
        public override void CancelAnimation() {
            _instance?.Cancel();
        }
    }
}