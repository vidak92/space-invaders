using UnityEngine;
using MijanTools.Common;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MijanTools.Components
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ObjectShaker))]
    public class ObjectShakerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var screenShake = target as ObjectShaker;
            if (GUILayout.Button("Start Shake"))
            {
                screenShake.StartShake(screenShake.ShakeIntensity);
            }
            if (GUILayout.Button("Stop Shake"))
            {
                screenShake.StopShake();
            }
            EditorGUILayout.Separator();
            DrawDefaultInspector();
        }
    }
#endif

    public class ObjectShaker : MonoBehaviour
    {
        [Space]
        [Tooltip("If checked, shakes Camera.main and TargetObject is ignored.")]
        public bool TargetIsMainCamera;
        public GameObject TargetObject;

        [Space]
        [Range(0f, 1f)]
        [Tooltip("If 0 - uses MinXXX params.\nIf 1 - uses MaxXXX params.\nOtherwise, interpolates between MinXXX/MaxXXX.")]
        public float ShakeIntensity;
        
        [Space]
        public float MinDuration;
        public float MinSpeed;
        public float MinDistance;

        [Space]
        public float MaxDuration;
        public float MaxSpeed;
        public float MaxDistance;

        private float _shakeTimer;
        private bool _shake;
        private bool _targetReached;
        private bool _returnToDefaultPosition;
        private Vector3 _targetPosition;
        private Vector3 _initialPosition;
        private Vector3 _defaultPosition;
        private float _t;
        private float _shakeSpeed;
        private float _shakeDuration;
        private float _shakeDistance;

        private Transform TargetTransform => TargetIsMainCamera ? CameraUtils.MainCamera.transform : TargetObject.transform;

        private void Update()
        {
            if (_shake)
            {
                var targetTransform = TargetTransform;

                if (_targetReached)
                {
                    // New target.
                    _initialPosition = targetTransform.position;
                    
                    _targetPosition = (Vector3)Random.insideUnitCircle.normalized * _shakeDistance + _defaultPosition;
                    _targetPosition.z = _defaultPosition.z;

                    _targetReached = false;
                    _t = 0f;
                }
                else
                {
                    // Move to target.
                    _t += Time.deltaTime * _shakeSpeed;
                    targetTransform.position = Vector3.Lerp(_initialPosition, _targetPosition, _t);
                    _targetReached = _t > 0.99f;

                    if (_returnToDefaultPosition && _targetReached)
                    {
                        _shake = false;
                        _targetReached = false;
                        _shakeTimer = 0f;
                        targetTransform.position = _defaultPosition;
                    }
                }

                if (!_returnToDefaultPosition)
                {
                    _shakeTimer += Time.deltaTime;
                    if (_shakeTimer >= _shakeDuration && _shakeDuration > 0f && _targetReached)
                    {
                        _returnToDefaultPosition = true;
                        _initialPosition = targetTransform.position;
                        _targetPosition = _defaultPosition;
                        _targetReached = false;
                        _t = 0f;
                    }
                }
            }
        }

        public void StartShake(float intensity = 1f)
        {
            intensity = Mathf.Clamp01(intensity);
            if (_shake && intensity < ShakeIntensity)
            {
                // Do not start shake if a more intense one is already running.
                return;
            }

            ShakeIntensity = intensity;
            _shakeTimer = 0f;
            _shake = true;
            _targetReached = true;
            _returnToDefaultPosition = false;
            _shakeSpeed = Mathf.Lerp(MinSpeed, MaxSpeed, intensity);
            _shakeDuration = Mathf.Lerp(MinDuration, MaxDuration, intensity);
            _shakeDistance = Mathf.Lerp(MinDistance, MaxDistance, intensity);
            _defaultPosition = TargetTransform.position;
        }

        public void StopShake()
        {
            _returnToDefaultPosition = true;
            _initialPosition = TargetTransform.position;
            _targetPosition = _defaultPosition;
            _targetReached = false;
            _t = 0f;
        }
    }
}