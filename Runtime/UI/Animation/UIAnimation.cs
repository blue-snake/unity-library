using UnityEngine;

namespace BlueSnake.UI.Animation {
    public abstract class UIAnimation : MonoBehaviour {

        public virtual void StartAnimation() {
            StartAnimation(null);
        }

        public virtual void StopAnimation(bool force) {
            StopAnimation(force, null);
        }

        public virtual void StopAnimation() {
            StopAnimation(false);
        }

        public abstract void StartAnimation(UIAnimationCallback callback);

        public abstract void StopAnimation(bool force, UIAnimationCallback callback);
        
        public delegate void UIAnimationCallback();
    }
}