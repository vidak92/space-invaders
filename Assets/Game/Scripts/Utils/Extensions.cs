using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Utils
{
    public static class Extensions
    {
        // List
        public static bool ContainsIndex<T>(this List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        // Dictionary
        public static bool HasKey<T, U>(this Dictionary<T, U> dictionary, T key)
        {
            if (dictionary == null)
            {
                return false;
            }
            return dictionary.ContainsKey(key);
        }

        // Transform
        public static void AddPositionX(this Transform transform, float x)
        {
            var tempPosition = transform.position;
            tempPosition.x += x;
            transform.position = tempPosition;
        }

        public static void ClampPositionX(this Transform transform, float minX, float maxX)
        {
            var tempPosition = transform.position;
            tempPosition.x = Mathf.Clamp(tempPosition.x, minX, maxX);
            transform.position = tempPosition;
        }

        // LayerMask
        public static int ToIndex(this LayerMask layerMask)
        {
            var layer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            return layer;
        }
    }
}