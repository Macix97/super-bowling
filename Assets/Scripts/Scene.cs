using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct Scene
{
    [SerializeField] private string _value;

    public string Value => _value;

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Scene))]
    private class PropertyDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            string[] scenes = GetScenes();
            SerializedProperty valueProperty = property.FindPropertyRelative(nameof(_value));
            int index = Mathf.Max(Array.IndexOf(scenes, valueProperty.stringValue), 0);
            index = EditorGUI.Popup(position, property.displayName, index, scenes);
            if (EditorGUI.EndChangeCheck()) valueProperty.stringValue = scenes[index];
        }

        private string[] GetScenes()
        {
            string[] guids = AssetDatabase.FindAssets("t:Scene");
            string[] scenes = new string[guids.Length];
            for (int i = 0; i < scenes.Length; i++)
                scenes[i] = Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guids[i]));
            return scenes;
        }
    }
#endif
}