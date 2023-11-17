using UnityEngine;
using UnityEngine.Audio;

namespace BlueSnake.Audio {
    [CreateAssetMenu(fileName = "AudioGroup", menuName = "Audio/Group", order = 1)]
    public class AudioGroup : ScriptableObject {
        public AudioMixerGroup mixer;
        public AudioEntry[] entries;
    }
}