using BlueSnake.UI.Animation;
using UnityEngine;

namespace BlueSnake.UI.Buttons {
    public class AnimatedButton : Button {

        [SerializeField]
        private new UIAnimation animation;

        public override void OnHoverEnter() {
            animation.StartAnimation();
        }

        public override void OnHoverExit() {
            animation.StopAnimation();
        }
    }
}