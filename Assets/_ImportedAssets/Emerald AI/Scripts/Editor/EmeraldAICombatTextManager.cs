using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace EmeraldAI.Utility
{
    public class EmeraldAICombatTextManager : EditorWindow
    {
        EmeraldAICombatTextData m_EmeraldAICombatTextData;
        Texture CombatTextIcon;
        Vector2 scrollPos;
        float MessageTimer;
        int CurrentTab = 0;
        bool MessageDisplay;

        SerializedObject serializedObject;
        SerializedProperty TextFont;
        SerializedProperty CombatTextState;
        SerializedProperty PlayerTextColor;
        SerializedProperty PlayerCritTextColor;
        SerializedProperty PlayerTakeDamageTextColor;
        SerializedProperty AITextColor;
        SerializedProperty AICritTextColor;
        SerializedProperty HealingTextColor;
        SerializedProperty FontSize;
        SerializedProperty MaxFontSize;
        SerializedProperty AnimationType;
        SerializedProperty CombatTextTargets;
        SerializedProperty OutlineEffect;
        SerializedProperty UseAnimateFontSize;
        SerializedProperty TextHeight;

        [MenuItem("Window/Emerald AI/Combat Text Manager #%c", false, 200)]
        public static void ShowWindow()
        {
            EditorWindow APS = EditorWindow.GetWindow(typeof(EmeraldAICombatTextManager), true, "Combat Text Manager");
            APS.minSize = new Vector2(600f, 650);
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        protected virtual void OnEnable()
        {
            m_EmeraldAICombatTextData = (EmeraldAICombatTextData)Resources.Load("Combat Text Data") as EmeraldAICombatTextData;
            if (CombatTextIcon == null) CombatTextIcon = Resources.Load("CombatTextIcon") as Texture;

            serializedObject = new SerializedObject(m_EmeraldAICombatTextData);
            TextFont = serializedObject.FindProperty("TextFont");
            CombatTextState = serializedObject.FindProperty("CombatTextState");
            PlayerTextColor = serializedObject.FindProperty("PlayerTextColor");
            PlayerCritTextColor = serializedObject.FindProperty("PlayerCritTextColor");
            PlayerTakeDamageTextColor = serializedObject.FindProperty("PlayerTakeDamageTextColor");
            AITextColor = serializedObject.FindProperty("AITextColor");
            AICritTextColor = serializedObject.FindProperty("AICritTextColor");
            HealingTextColor = serializedObject.FindProperty("HealingTextColor");
            FontSize = serializedObject.FindProperty("FontSize");
            MaxFontSize = serializedObject.FindProperty("MaxFontSize");
            AnimationType = serializedObject.FindProperty("AnimationType");
            CombatTextTargets = serializedObject.FindProperty("CombatTextTargets");
            OutlineEffect = serializedObject.FindProperty("OutlineEffect");
            UseAnimateFontSize = serializedObject.FindProperty("UseAnimateFontSize");
            TextHeight = serializedObject.FindProperty("DefaultHeight");
        }

        void OnGUI()
        {
            serializedObject.Update();
            GUILayout.Space(15);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("Box");
            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            EditorGUILayout.LabelField(new GUIContent(CombatTextIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(65));
            EditorGUILayout.LabelField("Combat Text Manager", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox("With the Combat Text Manager, you can globally handle all combat text settings such as text size, text font, text color, text animation, and more.", MessageType.None, true);
            GUILayout.Space(4);

            GUIContent[] CombatTextManagerButtons = new GUIContent[2] { new GUIContent("Combat Text Settings"), new GUIContent("Combat Text Colors") };
            CurrentTab = GUILayout.Toolbar(CurrentTab, CombatTextManagerButtons, EditorStyles.miniButton, GUILayout.Height(25));

            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("Box");

            if (CurrentTab == 0)
            {
                GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField("Combat Text Settings", EditorStyles.boldLabel);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndVertical();
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(CombatTextState, new GUIContent("Combat Text State"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("Controls the whether the Combat Text System is enabled or disabled.", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(TextHeight, new GUIContent("Combat Text Height"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("Controls the overall height the Combat Text will spawn above targets.", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(CombatTextTargets, new GUIContent("Combat Text Targets"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.LabelField("Controls the target types that will be able to have the Combat Text displayed.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(OutlineEffect, new GUIContent("Use Outline Effect"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.LabelField("Controls the whether the Combat Text System will use a text outline effect.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(AnimationType, new GUIContent("Animation Type"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.LabelField("Controls the Combat Text's Animation Type.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), TextFont, "Text Font", typeof(Font), true);
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("The Combat Text's font.", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), FontSize, "Font Size", 10, 50);
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("Controls the size of the Combat Text's font.", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(UseAnimateFontSize, new GUIContent("Use Animate Font"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.LabelField("Controls whether or not the font size is animated.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                if (m_EmeraldAICombatTextData.UseAnimateFontSize == EmeraldAICombatTextData.UseAnimateFontSizeEnum.Enabled)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    EditorGUILayout.BeginVertical();

                    CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaxFontSize, "Animated Font Size", 1, 30);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.HelpBox("Controls the additional size of the Combat Text's font when Animate Size is enabled. After the text has been animated, it will return to the Font Size.", MessageType.None, true);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }
            }

            if (CurrentTab == 1)
            {
                GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField("Combat Text Colors", EditorStyles.boldLabel);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndVertical();
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(PlayerTextColor, new GUIContent("Player Color"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("The Combat Text's color when used by the player.", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(PlayerTakeDamageTextColor, new GUIContent("Take Damage Color"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("The Combat Text's color when a player takes damage.", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(PlayerCritTextColor, new GUIContent("Player Crit Color"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("The Combat Text's critical hit color when used by the player.", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(AITextColor, new GUIContent("AI Color"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("The Combat Text's AI color when used between other AI.", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(AICritTextColor, new GUIContent("AI Crit Color"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("The Combat Text's critical hit color when used between other AI.", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(HealingTextColor, new GUIContent("Healing Color"));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.HelpBox("The Combat Text's color when used for healing.", MessageType.None, true);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();

            if (!Application.isPlaying && GUI.changed)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
}
