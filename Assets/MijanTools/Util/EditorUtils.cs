#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace MijanTools.Util
{
    public static class EditorUtils
    {
#if UNITY_2019
        public static void DrawEditableList(SerializedProperty list)
        {
            EditorGUILayout.LabelField(list.displayName);
            if (list.isExpanded)
            {
                for (int i = 0; i < list.arraySize; i++)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
                    EditorGUILayout.Separator();
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button(new GUIContent("\u2191", "move up"), EditorStyles.miniButtonLeft))
                    {
                        list.MoveArrayElement(i, i - 1);
                    }
                    if (GUILayout.Button(new GUIContent("\u2193", "move down"), EditorStyles.miniButtonRight))
                    {
                        list.MoveArrayElement(i, i + 1);
                    }
                    if (GUILayout.Button(new GUIContent("+", "add"), EditorStyles.miniButtonLeft))
                    {
                        list.InsertArrayElementAtIndex(i);
                    }
                    if (GUILayout.Button(new GUIContent("-", "remove"), EditorStyles.miniButtonRight))
                    {
                        list.DeleteArrayElementAtIndex(i);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Separator();
                }
            }
            EditorGUILayout.Separator();
        }
#endif
    }
}
#endif