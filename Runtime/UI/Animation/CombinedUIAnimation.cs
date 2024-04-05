using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueSnake.UI.Animation {
    [Serializable]
    public class CombinedUIAnimation : UIAnimation {
        [SerializeField]
        private CombinedUIAnimationPlayType playType;

        [SerializeField, SerializeReference, SubclassSelector, ReorderableList]
        private List<UIAnimation> animations;

        public override IEnumerator PlayAnimation() {
            switch (playType) {
                case CombinedUIAnimationPlayType.Parallel: {
                    foreach (UIAnimation animation in animations) {
                        animation.PlayAnimation().MoveNext();
                    }
                    break;
                }
                case CombinedUIAnimationPlayType.Sequence: {
                    foreach (UIAnimation animation in animations) {
                        yield return animation.PlayAnimation();
                    }
                    break;
                }
            }
        }

        public override void CancelAnimation() {
            foreach(UIAnimation animation in animations) {
                animation.CancelAnimation();
            }
        }
    }

    public enum CombinedUIAnimationPlayType {
        Parallel,
        Sequence
    }
}