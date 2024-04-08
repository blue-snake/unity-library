using System.Collections;
using BlueSnake.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlueSnake {
    /// <summary>
    /// This class is responsible for shaking the camera in the game.
    /// </summary>
    public class CameraShaker : Singleton<CameraShaker> {
        // The influence of the shake effect on the camera.
        [Range(0f, 1f)]
        [SerializeField]
        private float shakeInfluence = 0.5f;

        // The influence of the rotation effect on the camera.
        [Range(0f, 10f)]
        [SerializeField]
        public float rotationInfluence;

        // The original position of the camera before the shake effect.
        private Vector3 _originalPosition;
        // The original rotation of the camera before the shake effect.
        private Quaternion _originalRotation;
        // A flag to indicate if the shake effect is currently running.
        private bool _isRunning;

        // The transform component of the camera.
        private Transform _transform;

        public override void Awake() {
            base.Awake();
            _transform = transform;
        }

        /// <summary>
        /// Shakes the camera with a given intensity and duration.
        /// </summary>
        /// <param name="minIntensity">The minimum intensity of the shake.</param>
        /// <param name="maxIntensity">The maximum intensity of the shake.</param>
        /// <param name="duration">The duration of the shake.</param>
        public void Shake(float minIntensity, float maxIntensity, float duration) {
            if (_isRunning) {
                return;
            }
            _originalPosition = _transform.position;
            _originalRotation = _transform.rotation;
            float shake = Random.Range(minIntensity, maxIntensity) * shakeInfluence;
            duration *= shakeInfluence;
            StartCoroutine(ProcessShake(shake, duration));
        }

        /// <summary>
        /// Coroutine that processes the shake effect.
        /// </summary>
        /// <param name="shake">The intensity of the shake.</param>
        /// <param name="duration">The duration of the shake.</param>
        /// <returns>An IEnumerator to be used in a coroutine.</returns>
        IEnumerator ProcessShake(float shake, float duration)
        {
            _isRunning = true;
            float countdown = duration;
            float initialShake = shake;

            while (countdown > 0)
            {
                countdown -= Time.deltaTime;

                float lerpIntensity = countdown / duration;
                shake = Mathf.Lerp(0f, initialShake, lerpIntensity);
                _transform.position = _originalPosition + Random.insideUnitSphere * shake;
                _transform.rotation = Quaternion.Euler(_originalRotation.eulerAngles + Random.insideUnitSphere * shake * rotationInfluence);
                yield return null;
            }
            _transform.position = _originalPosition;
            _transform.rotation = _originalRotation;
            _isRunning = false;
        }
    }
}