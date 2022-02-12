using System;

namespace SpaceInvaders.Utils
{
    public static class Util
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
    }
}