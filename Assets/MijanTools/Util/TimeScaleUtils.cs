using UnityEngine;

namespace MijanTools.Util
{
    public static class TimeScaleUtils
    {
        public static float BaseTimeScale { get; private set; } = 1f;
        public static float TimeScaleMultiplier { get; private set; } = 1f;

        public static void AddTimeScaleMultiplier(float timeScaleMultiplierIncrement)
        {
            TimeScaleMultiplier = Mathf.Clamp01(TimeScaleMultiplier + timeScaleMultiplierIncrement);
            Debug.Log($"Time scale multiplier: {TimeScaleMultiplier}");
            UpdateTimeScale();
        }

        public static void SetBaseTimeScale(float timeScale)
        {
            BaseTimeScale = Mathf.Clamp01(timeScale);
            UpdateTimeScale();
        }

        private static void UpdateTimeScale()
        {
            Time.timeScale = BaseTimeScale * TimeScaleMultiplier;
        }

        public static void Reset()
        {
            BaseTimeScale = 1f;
            TimeScaleMultiplier = 1f;
            UpdateTimeScale();
        }
    }
}