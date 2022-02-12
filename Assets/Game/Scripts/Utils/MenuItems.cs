#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SpaceInvaders.Utils
{
    public static class MenuItems
    {
        [MenuItem("Tools/Player Prefs/Delete All")]
        public static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}
#endif