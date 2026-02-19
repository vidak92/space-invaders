#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MijanTools.Util
{
    public static class MenuItems
    {
        [MenuItem("Mijan Tools/Player Prefs/Delete All")]
        public static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Mijan Tools/Application Data Path/Print to Console")]
        public static void LogApplicationDataPath()
        {
            Debug.Log(Application.persistentDataPath);
        }

        [MenuItem("Mijan Tools/Application Data Path/Open Folder")]
        public static void OpenApplicationDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("Mijan Tools/Take Screenshot")]
        public static void TakeScreenshot()
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss");
            var directoryPath = $"{Application.persistentDataPath}/screenshots";
            if (!System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.CreateDirectory(directoryPath);
            }
            var filePath = $"{directoryPath}/{timestamp}.png";
            ScreenCapture.CaptureScreenshot(filePath);
            Debug.Log($"Screenshot saved at: {filePath}");
        }
    }
}
#endif