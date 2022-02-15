using System;

namespace SpaceInvaders.Util
{
    public static class Utils
    {
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