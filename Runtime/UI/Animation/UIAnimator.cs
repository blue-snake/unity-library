using System;
using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

namespace BlueSnake.UI.Animation {
    
    public class UIAnimator : MonoBehaviour {

        [SerializeField, ReorderableList]
        private List<UIAnimationTransition> transitions;
        
        private Dictionary<string, UIAnimationTransition> internalDictionary = new();

        private UIAnimationTransition? _currentTransition;

        private void Awake() {
            ReloadTransitions();
        }

        /// <summary>
        /// This method is used to run a transition by name with a coroutine, Use StartCoroutine to run this method
        /// </summary>
        /// <param name="name">Name of the transition</param>
        /// <returns></returns>
        public IEnumerator RunTransition(string name) {
            if (_currentTransition.HasValue) {
                _currentTransition.Value.animation.CancelAnimation();
                _currentTransition = null;
            } 
            if (internalDictionary.TryGetValue(name, out UIAnimationTransition transition)) {
                _currentTransition = transition;
                yield return transition.animation.PlayAnimation();
            }
        }

        /// <summary>
        /// This executes RunTransition without needing to use StartCoroutine, it's basically the same as StartCoroutine(RunTransition(name))
        /// </summary>
        /// <param name="name">Name of the transition</param>
        public void PlayTransition(string name) {
            StartCoroutine(RunTransition(name));
        }

        /// <summary>
        /// This method allows one to create a transition without needing to use the inspector
        /// </summary>
        /// <param name="transition"></param>
        public void CreateTransition(UIAnimationTransition transition) {
            transitions.Add(transition);
            ReloadTransitions();
        }

        /// <summary>
        /// Mostly called when a transition is added or removed, this method reloads the internal dictionary
        /// </summary>
        public void ReloadTransitions() {
            internalDictionary.Clear();
            foreach (var transition in transitions) {
                internalDictionary.Add(transition.name, transition);
            }
        }
        

    }
    
    [Serializable]
    public struct UIAnimationTransition {

        public string name;
        [SerializeReference, SubclassSelector]
        public UIAnimation animation;

    }
    
    [Serializable]
    public abstract class UIAnimation {

        public abstract IEnumerator PlayAnimation();

        public abstract void CancelAnimation();

    }

    
    
  
}