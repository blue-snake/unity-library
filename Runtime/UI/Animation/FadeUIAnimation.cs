using System;
using System.Collections;
using PrimeTween;
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
        
        
        public override IEnumerator PlayAnimation() {
            CancelAnimation();
            if (handleRaycasts) {
                target.blocksRaycasts = type != FadeType.FadeOut;
            }
            yield return Tween.Alpha(target, type == FadeType.FadeOut ? 0f : 1f, fadeDuration, startDelay: fadeDelay).ToYieldInstruction();
        }

        public override void CancelAnimation() {
            Tween.StopAll(target);
        }
        
        public enum FadeType {
            FadeIn, FadeOut
        }
    }
}