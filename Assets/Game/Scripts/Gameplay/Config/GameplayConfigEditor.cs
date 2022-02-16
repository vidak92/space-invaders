#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SpaceInvaders.Gameplay.Config
{
    [CustomEditor(typeof(GameplayConfig))]
    public class GameplayConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Validate"))
            {
                var gameplayConfig = target as GameplayConfig;
                gameplayConfig.Validate();
            }

            base.OnInspectorGUI();
        }
    }
}
#endif