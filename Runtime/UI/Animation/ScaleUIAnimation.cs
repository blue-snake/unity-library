using System;
using System.Collections;
using PrimeTween;
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
        
        
        public override IEnumerator PlayAnimation() {
            CancelAnimation();
            yield return Tween.Scale(target.transform, scaleAmount, scaleDuration, Ease.Linear, startDelay: scaleDelay).ToYieldInstruction();
        }
        
        public override void CancelAnimation() {
            Tween.StopAll(target.transform);
        }
    }
}