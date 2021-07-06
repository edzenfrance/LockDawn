using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace EmeraldAI.Utility
{
    [System.Serializable]
    [CustomEditor(typeof(LocationBasedDamage))]
    [CanEditMultipleObjects]
    public class LocationBasedDamageEditor : Editor
    {
        ReorderableList ColliderList;
        string ColliderListState;

        private void OnEnable()
        {
            //Label Style
            var LabelStyle = new GUIStyle();
            LabelStyle.fontStyle = FontStyle.Bold;

            ColliderList = new ReorderableList(serializedObject, serializedObject.FindProperty("ColliderList"), false, true, false, false);
            ColliderList.drawHeaderCallback = rect =>
            {

            };
            ColliderList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = ColliderList.serializedProperty.GetArrayElementAtIndex(index);
                    ColliderList.elementHeight = EditorGUIUtility.singleLineHeight * 2.5f;

                    //Label
                    EditorGUI.PrefixLabel(new Rect(rect.x + 120, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight),
                        new GUIContent(element.FindPropertyRelative("ColliderObject").objectReferenceValue.name), LabelStyle);

                    //Button
                    if (GUI.Button(new Rect(rect.x, rect.y, 110, EditorGUIUtility.singleLineHeight), "Select Collider"))
                    {
                        Selection.activeObject = element.FindPropertyRelative("ColliderObject").objectReferenceValue;
                    }

                    //Multiplier
                    element.FindPropertyRelative("DamageMultiplier").floatValue = EditorGUI.Slider(new Rect(rect.x , rect.y + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("ColliderObject").objectReferenceValue.name + " Multiplier", element.FindPropertyRelative("DamageMultiplier").floatValue, 0, 25);
                };
        }

        public override void OnInspectorGUI()
        {
            LocationBasedDamage self = (LocationBasedDamage)target;
            serializedObject.Update();

            GUIStyle FoldoutStyle = new GUIStyle(EditorStyles.foldout);
            FoldoutStyle.fontStyle = FontStyle.Bold;
            FoldoutStyle.fontSize = 12;
            FoldoutStyle.active.textColor = Color.black;
            FoldoutStyle.focused.textColor = Color.black;
            FoldoutStyle.onHover.textColor = Color.black;
            FoldoutStyle.normal.textColor = Color.black;
            FoldoutStyle.onNormal.textColor = Color.black;
            FoldoutStyle.onActive.textColor = Color.black;
            FoldoutStyle.onFocused.textColor = Color.black;
            Color myStyleColor = Color.black;
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            //EditorGUILayout.LabelField("Location Based Damage Manager", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("The Location Based Damage component allows each collider to detect damage and apply a customizable damage multiplier based on the damage receieved. The hit effect that will play upon impact, " +
                "and at the position the hit is detected, is based off of the AI's Hit Effects List (Located under AI's Settings>Combat>Hit Effect)", EditorStyles.helpBox);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Gets all colliders within the AI and applies Location Based Damage Area components to them.", EditorStyles.helpBox);
            if (GUILayout.Button("Get Colliders"))
            {
                var m_Colliders = self.GetComponentsInChildren<Collider>();

                foreach (Collider C in m_Colliders)
                {
                    if (C != null)
                    {
                        LocationBasedDamage.LocationBasedDamageClass lbdc = new LocationBasedDamage.LocationBasedDamageClass(C, 1);
                        if (!LocationBasedDamage.LocationBasedDamageClass.Contains(self.ColliderList, lbdc) && C.gameObject != self.gameObject)
                        {
                            self.ColliderList.Add(lbdc);
                        }
                    }
                }

                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();

                if (self.ColliderList.Count == 0)
                {
                    Debug.Log("There are no colliders within this AI. Please ensure that you have setup the AI with Unity's Ragdoll Wizard or a 3rd party ragdoll tool.");
                }
            }

            if (GUILayout.Button("Clear Colliders") && EditorUtility.DisplayDialog("Clear Collider List?", "Are you sure you want to clear the AI's Collider List? This process cannot be undone.", "Clear", "Do Not Clear"))
            {
                self.ColliderList.Clear();
                serializedObject.Update();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (self.ColliderListFoldout)
                ColliderListState = "(Visible)";
            else if(!self.ColliderListFoldout)
                ColliderListState = "(Hidden)";

            self.ColliderListFoldout = CustomEditorProperties.Foldout(self.ColliderListFoldout, "Collider List " + ColliderListState, true, FoldoutStyle);

            if (self.ColliderListFoldout)
                ColliderList.DoLayoutList();

            GUILayout.Space(20);

            serializedObject.ApplyModifiedProperties();
        }
    }
}