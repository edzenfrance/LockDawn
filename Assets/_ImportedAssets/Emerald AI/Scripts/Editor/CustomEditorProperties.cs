using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace EmeraldAI.Utility
{
    public class CustomEditorProperties
    {
        public static void CustomIntField(Rect position, GUIContent label, SerializedProperty property, string Name)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUILayout.IntField(Name, property.intValue);
            if (EditorGUI.EndChangeCheck())
                property.intValue = newValue;

            EditorGUI.EndProperty();
        }

        public static void CustomIntSlider(Rect position, GUIContent label, SerializedProperty property, string Name, int MinValue, int MaxValue)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUILayout.IntSlider(Name, property.intValue, MinValue, MaxValue);
            if (EditorGUI.EndChangeCheck())
                property.intValue = newValue;

            EditorGUI.EndProperty();
        }

        public static void CustomFloatSlider(Rect position, GUIContent label, SerializedProperty property, string Name, float MinValue, float MaxValue)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUILayout.Slider(Name, property.floatValue, MinValue, MaxValue);
            if (EditorGUI.EndChangeCheck())
                property.floatValue = newValue;

            EditorGUI.EndProperty();
        }

        public static void CustomFloatField(Rect position, GUIContent label, SerializedProperty property, string Name)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUILayout.FloatField(Name, property.floatValue);
            if (EditorGUI.EndChangeCheck())
                property.floatValue = newValue;

            EditorGUI.EndProperty();
        }

        public static void CustomPopupColor(Rect position, GUIContent label, SerializedProperty property, string nameOfLabel, Type typeOfEnum, Color TextColor)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            string[] enumNamesList = System.Enum.GetNames(typeOfEnum);

            var Style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            Style.normal.textColor = TextColor;

            EditorGUILayout.LabelField(new GUIContent(nameOfLabel), Style, GUILayout.Width(75));
            var newValue = EditorGUILayout.Popup("", property.intValue, enumNamesList, GUILayout.Width(65));

            if (EditorGUI.EndChangeCheck())
                property.intValue = newValue;

            EditorGUI.EndProperty();
        }

        public static void DrawString(string text, Vector3 worldPos, Color? colour = null)
        {
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;

            UnityEditor.Handles.BeginGUI();

            var restoreColor = GUI.color;

            if (colour.HasValue) GUI.color = colour.Value;
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
                GUI.color = restoreColor;
                UnityEditor.Handles.EndGUI();
                return;
            }

            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text, style);
            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();
        }

        public static bool Foldout(bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style)
        {
            Rect position = GUILayoutUtility.GetRect(40f, 40f, 16f, 16f, style);
            return EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, style);
        }
        public static bool Foldout(bool foldout, string content, bool toggleOnLabelClick, GUIStyle style)
        {
            return Foldout(foldout, new GUIContent(content), toggleOnLabelClick, style);
        }

        public static void CustomTagField(Rect position, GUIContent label, SerializedProperty property, string Name)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUILayout.TagField(Name, property.stringValue);
            if (EditorGUI.EndChangeCheck())
                property.stringValue = newValue;

            EditorGUI.EndProperty();
        }

        public static void CustomEnumColor(Rect position, GUIContent label, SerializedProperty property, string Name, Color TextColor)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            var Style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            Style.normal.textColor = TextColor;

            EditorGUILayout.LabelField(new GUIContent(Name), Style, GUILayout.Width(50));
            var newValue = EditorGUILayout.Popup("", property.intValue, EmeraldAISystem.StringFactionList.ToArray(), GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true));

            if (property.intValue == EmeraldAISystem.StringFactionList.Count)
            {
                property.intValue -= 1;
            }
            if (EditorGUI.EndChangeCheck())
                property.intValue = newValue;

            EditorGUI.EndProperty();
        }

        public static void CustomEnum(Rect position, GUIContent label, SerializedProperty property, string Name)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUILayout.Popup(Name, property.intValue, EmeraldAISystem.StringFactionList.ToArray());
            if (property.intValue == EmeraldAISystem.StringFactionList.Count)
            {
                property.intValue -= 1;
            }
            if (EditorGUI.EndChangeCheck())
                property.intValue = newValue;

            EditorGUI.EndProperty();
        }

        public static void CustomObjectField(Rect position, GUIContent label, SerializedProperty property, string Name, Type typeOfObject, bool IsEssential)
        {
            if (IsEssential && property.objectReferenceValue == null)
            {
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.LabelField("This field cannot be left blank", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUILayout.ObjectField(Name, property.objectReferenceValue, typeOfObject, true);

            if (EditorGUI.EndChangeCheck())
                property.objectReferenceValue = newValue;

            EditorGUI.EndProperty();
        }
    }
}
