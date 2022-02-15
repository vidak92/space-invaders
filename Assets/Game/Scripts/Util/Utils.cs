using System;

namespace SpaceInvaders.Util
{
    public static class Utils
    {
        public static bool IsCurrentPlatformEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsCurrentPlatformStandalone
        {
            get
            {
#if UNITY_STANDALONE
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsCurrentPlatformMobile
        {
            get
            {
#if UNITY_ANDROID || UNITY_IOS
                return true;
#else
                return false;
#endif
            }
        }

        public static string GetAssertGreaterThanMessage(Type objectType, string nameGreater, string nameLesser)
        {
            return $"{objectType}: {nameGreater} must be greater than {nameLesser}.";
        }

        public static string GetAssertGreaterThanMessage(Type objectType, string name, int value)
        {
            return $"{objectType}: {name} must be greater than {value}.";
        }

        public static string GetAssertGreaterThanMessage(Type objectType, string name, float value)
        {
            return $"{objectType}: {name} must be greater than {value}.";
        }

        public static string GetAssertGreaterThanOrEqualMessage(Type objectType, string name, float value)
        {
            return $"{objectType}: {name} must be greater than or equal to {value}.";
        }
    }
}