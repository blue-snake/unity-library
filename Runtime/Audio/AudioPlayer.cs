using System;
using UnityEngine;

namespace BlueSnake.Audio {
    public class AudioPlayer : MonoBehaviour {
        [SerializeField]
        private new string audio;

        private void Start() {
            AudioManager.GetInstance().Play(audio);
        }
    }
}