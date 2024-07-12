using System;
using System.Collections;
using PrimeTween;
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
        

        public override IEnumerator PlayAnimation() {
            CancelAnimation();
            yield return Tween.LocalPosition(target, slidePosition, slideDuration, startDelay: slideDelay).ToYieldInstruction();
        }

        public override void CancelAnimation() {
            Tween.StopAll(target);
        }
    }
} 