using System;
using System.Collections;
using Tweens;
using UnityEngine;

namespace BlueSnake.UI.Animation {
    [Serializable]
    public class FadeUIAnimation : UIAnimation {
        
        [Header("Reference")]
        [SerializeField]
        private CanvasGroup target;

        [Header("Properties")]
        [SerializeField]
        private bool handleRaycasts = true;

        [Header("Fade")]
        [SerializeField]
        private FadeType type = FadeType.FadeOut;
        [SerializeField]
        private float fadeDelay;
        [SerializeField]
        private float fadeDuration = 1f;
        
        private TweenInstance _tweenInstance;
        
        public override IEnumerator PlayAnimation() {
            CancelAnimation();
            if (handleRaycasts) {
                target.blocksRaycasts = type != FadeType.FadeOut;
            }

            bool running = true;
            FloatTween tween = new FloatTween {
                from = target.alpha,
                to = type == FadeType.FadeOut ? 0f : 1f,
                delay = fadeDelay,
                duration = fadeDuration,
                onUpdate = (_, value) => {
                    target.alpha = value;
                },
                onFinally = _ => {
                    if (handleRaycasts) {
                        target.blocksRaycasts = type == FadeType.FadeOut;
                    }

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
        
        public enum FadeType {
            FadeIn, FadeOut
        }
    }
}