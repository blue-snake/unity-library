using BlueSnake.UI.Animation;
using UnityEngine;

namespace BlueSnake.UI.Buttons {
    public class AnimatedButton : Button {

        [SerializeField]
        private UIAnimator animator;

        public override void OnHoverEnter() { 
            animator.PlayTransition("Hover Enter");
        }

        public override void OnHoverExit() {
            animator.PlayTransition("Hover Exit");
        }
    }
}