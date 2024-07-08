using System;
using System.Collections;
using PrimeTween;
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
        
        public override IEnumerator PlayAnimation() {
            CancelAnimation();
            yield return Tween.Color(target, color, colorDuration, startDelay: colorDelay).ToYieldInstruction();
        }

        public override void CancelAnimation() {
            Tween.StopAll(target);
        }
    }
}