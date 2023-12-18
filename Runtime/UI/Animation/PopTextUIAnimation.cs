using Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace BlueSnake.UI.Animation {
    

    public class PopTextUIAnimation : UIAnimation {

        [Header("Properties")]
        [SerializeField]
        private Vector3 scaleAmount = new(1.1f, 1.1f, 1.1f);
        [SerializeField]
        private float scaleDuration = 0.2f;
        [SerializeField]
        private float scaleDelay;

        [Space]
        [SerializeField]
        private Color color;
        [SerializeField]
        private float colorDuration = 0.1f;
        [SerializeField]
        private float colorDelay = 0.1f;
    
        [Header("Reference")]
        [SerializeField]
        private Graphic text;

        private TweenInstance[] _instances;
    
        public override void StartAnimation(UIAnimationCallback callback) {
            StopAnimation(true, null);
            LocalScaleTween scaleTween = new LocalScaleTween {
                from = text.transform.localScale,
                to = scaleAmount,
                usePingPong = true,
                duration = scaleDuration,
                delay = scaleDelay
            };
            ColorTween colorTween = new ColorTween {
                from = text.color,
                to = color,
                usePingPong = true,
                duration = colorDuration,
                delay = colorDelay,
                onUpdate = (_, clr) => {
                    text.color = clr;
                },
                onEnd = _ => {
                    callback?.Invoke();
                }
            };
            TweenInstance scaleInstance = text.gameObject.AddTween(scaleTween);
            TweenInstance colorInstance = text.gameObject.AddTween(colorTween);
            _instances = new[] { scaleInstance, colorInstance };
        }

        public override void StopAnimation(bool force, UIAnimationCallback callback) {
            if (_instances == null) return;
            foreach (TweenInstance instance in _instances) {
                instance.Cancel();
            }
        }
    }
}