using System.Threading;
using UnityEngine;

namespace BlueSnake.UI.Animation {
    public class CombinedUIAnimation : UIAnimation {

        [Header("Reference")]
        [SerializeField]
        private UIAnimation[] animations;
        
        public override void StartAnimation(UIAnimationCallback callback) {
            if (callback == null) {
                foreach (UIAnimation animation in animations) {
                    animation.StartAnimation();
                }
                return;
            }
            CountdownEvent latch = new CountdownEvent(animations.Length);
            ThreadPool.QueueUserWorkItem(_ => {
                latch.Wait();
                callback.Invoke();
            });
            foreach (UIAnimation animation in animations) {
                animation.StartAnimation(() => {
                    latch.Signal();
                });
            }
        }

        public override void StopAnimation(bool force, UIAnimationCallback callback) {
            if (callback == null) {
                foreach(UIAnimation animation in animations)
                {
                    animation.StopAnimation(force);
                }
                return;
            }
            CountdownEvent latch = new CountdownEvent(animations.Length);
            ThreadPool.QueueUserWorkItem(_ => {
                latch.Wait();
                callback();
            });
            foreach (UIAnimation animation in animations) {
                animation.StopAnimation(force, () => {
                    latch.Signal();
                });
            }
        }
        
    }
}