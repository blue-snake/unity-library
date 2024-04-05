using System;
using System.Collections;
using Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace BlueSnake.UI.Animation {
    [Serializable]
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
        
        private TweenInstance _tweenInstance;

        public override IEnumerator PlayAnimation() {
            CancelAnimation();
            bool running = true;
            Debug.Log("ColorUIAnimation PlayAnimation");
            ColorTween tween = new ColorTween {
                from = target.color,
                to = color,
                delay = colorDelay,
                duration = colorDuration,
                easeType = EaseType.Linear,
                onFinally = _ => {
                    running = false;
                },
                onUpdate = (_, clr) => {
                    Debug.Log(clr);
                    target.color = clr;
                }
            };
            _tweenInstance = target.gameObject.AddTween(tween);
            while (running) {
                yield return null;
            }
            Debug.Log("ColorUIAnimation PlayAnimation Done");

            _tweenInstance = null;
        }

        public override void CancelAnimation() {
            _tweenInstance?.Cancel();
            _tweenInstance = null;
        }
    }
}