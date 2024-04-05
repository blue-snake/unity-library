using System;
using System.Collections;
using System.Collections.Generic;
using BlueSnake.Utils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace BlueSnake.Audio {
    public class AudioManager : Singleton<AudioManager> {
        public AudioMixer mixer;
        public List<AudioGroup> groups;
        public readonly Dictionary<string, AudioContainer> containers = new();

        [Space]
        [Header("Loading")]
        [SerializeField]
        private bool loadFromResources;

        [SerializeField]
        private string resourcePath;

        public override void Awake() {
            if (Instance != null) {
                DestroyImmediate(gameObject);
            } else {
                DontDestroyOnLoad(this);
                SceneManager.sceneUnloaded += OnScene;
                base.Awake();

                if (loadFromResources) {
                    AudioGroup[] groups = Resources.LoadAll<AudioGroup>(resourcePath);
                    this.groups.AddRange(groups);
                }
            }
        }

        public void ChangeMixerVolume(string key, float value) {
            if (value <= 0) {
                mixer.SetFloat(key, -80f);
                return;
            }

            mixer.SetFloat(key, Mathf.Log10(value) * 20);
        }

        private void OnScene(Scene scene) {
            ClearAll();
        }

        private void OnDisable() {
            ClearAll();
        }

        public void ClearAll() {
            foreach (AudioSource component in transform.GetComponents<AudioSource>()) {
                if(component == null) continue;
                component.Stop();
                Destroy(component);
            }

            containers.Clear();
        }

        public void Stop(string name) {
            Stop(name, transform);
        }

        public AudioContainer Get(string name) {
            return Get(name, transform);
        }

        public AudioContainer Get(string name, Transform transform) {
            string id = name + transform.GetInstanceID();
            return containers.GetValueOrDefault(id);
        }

        public void Stop(string name, Transform transform) {
            string id = name + transform.GetInstanceID();
            if (!containers.TryGetValue(id, out AudioContainer container)) return;
            AudioSource source = container.source;
            source.Stop();
        }

        public AudioContainer Play(string name) {
            return Play(name, transform);
        }

        public void PlayDelayed(string name, float delay) {
            PlayDelayed(name, delay, transform);
        }

        public void PlayDelayed(string name, float delay, Transform transform) {
            StartCoroutine(PlayDelayedCoroutine(name, delay, transform));
        }

        public IEnumerator PlayDelayedCoroutine(string name, float delay, Transform transform) {
            yield return new WaitForSeconds(delay);
            Play(name, transform);
        }

        public AudioContainer Play(string name, Transform transform) {
            string id = name + transform.GetInstanceID();
            if (containers.TryGetValue(id, out AudioContainer container)) {
                AudioEntry audioEntry = container.entry;
                if (audioEntry.distanceBetween > 0) {
                    Vector3 position = transform.position;
                    position.y = 0;
                    Vector3 lastPlayedPosition = container.lastPlayedPosition;
                    lastPlayedPosition.y = 0;
                    float distance = Vector3.Distance(position, lastPlayedPosition);
                    if (distance < audioEntry.distanceBetween) {
                        return container;
                    }
                }

                if (audioEntry.cooldown > 0) {
                    if (Time.time < container.lastPlayedTimestamp + audioEntry.cooldown) {
                        return container;
                    }
                }

                AudioClip clip = audioEntry.clips.Length == 1
                    ? audioEntry.clips[0]
                    : audioEntry.clips[Random.Range(0, audioEntry.clips.Length)];
                container.source.clip = clip;

                container.lastPlayedPosition = transform.position;
                container.lastPlayedTimestamp = Time.time;
                container.source.Play();
                return container;
            }

            foreach (AudioGroup group in groups) {
                foreach (AudioEntry audio in group.entries) {
                    if (!name.Equals(audio.name)) continue;
                    AudioSource source = transform.gameObject.AddComponent<AudioSource>();
                    if (group.mixer != null) {
                        source.outputAudioMixerGroup = group.mixer;
                    }

                    source.clip = audio.clips[0];
                    source.volume = audio.volume;
                    source.pitch = audio.pitch;
                    source.loop = audio.loop;
                    source.rolloffMode = AudioRolloffMode.Linear;

                    if (audio.enabled) {
                        source.spatialBlend = 1f;
                        source.spatialize = audio.enabled;
                        source.minDistance = audio.minDistance;
                        source.maxDistance = audio.maxDistance;
                    }

                    AudioContainer cont = new AudioContainer(audio, source) {
                        lastPlayedPosition = transform.position,
                        lastPlayedTimestamp = Time.time
                    };
                    containers[id] = cont;
                    source.Play();
                    return containers[id];
                }
            }

            return null;
        }
    }

    public class AudioContainer {
        public readonly AudioEntry entry;
        public readonly AudioSource source;

        public Vector3 lastPlayedPosition;
        public float lastPlayedTimestamp;

        public AudioContainer(AudioEntry entry, AudioSource source) {
            this.entry = entry;
            this.source = source;
        }
    }

    [Serializable]
    public class AudioEntry {
        public string name;
        public AudioClip[] clips;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(-2f, 2f)]
        public float pitch = 1f;

        public bool loop;

        [Header("3D")]
        public bool enabled;

        public float minDistance = 0.1f;
        public float maxDistance = 15f;


        [Header("Time")]
        public float distanceBetween;

        public float cooldown;
    }
}