using System;
using System.Collections;
using UnityEngine;

namespace MijanTools.Util
{
    public class CoroutineUtils : MonoBehaviour
    {
        private static CoroutineUtils _instance;

        // Initialization
        private static void Init()
        {
            if (_instance == null)
            {
                _instance = new GameObject("CoroutineUtils").AddComponent<CoroutineUtils>();
                _instance.InitState();
                DontDestroyOnLoad(_instance.gameObject);
            }
        }

        private void InitState()
        {
            // TODO: Setup internal state here.
        }

        // Public Methods
        public static void StopCoroutines()
        {
            Init();

            _instance.StopAllCoroutines();
        }

        public static Coroutine CallAfterOneFrame(Action action)
        {
            Init();

            return _instance.StartCoroutine(_instance.CallAfterXFramesCoroutine(1, action));
        }

        public static Coroutine CallAfterXFrames(int frameCount, Action action)
        {
            Init();

            return _instance.StartCoroutine(_instance.CallAfterXFramesCoroutine(frameCount, action));
        }

        public static Coroutine CallAfterXSeconds(float seconds, Action action)
        {
            Init();

            return _instance.StartCoroutine(_instance.CallAfterXSecondsCoroutine(seconds, ignoreTimeScale: false, action));
        }

        public static Coroutine CallAfterXSecondsIgnoreTimeScale(float seconds, Action action)
        {
            Init();

            return _instance.StartCoroutine(_instance.CallAfterXSecondsCoroutine(seconds, ignoreTimeScale: true, action));
        }

        // Private Methods
        private IEnumerator CallAfterXFramesCoroutine(int frameCount, Action action)
        {
            for (int i = 0; i < frameCount; i++)
            {
                yield return null;
            }
            action?.Invoke();
        }

        private IEnumerator CallAfterXSecondsCoroutine(float seconds, bool ignoreTimeScale, Action action)
        {
            var totalTime = 0f;
            while (totalTime < seconds)
            {
                totalTime += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                yield return null;
            }
            action?.Invoke();
        }
    }
}