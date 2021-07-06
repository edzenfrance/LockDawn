using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor.IMGUI.Controls;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditorInternal;

namespace EmeraldAI.Utility
{
    public class EmeraldAIFactionManager : EditorWindow
    {
        Texture FactionIcon;
        Texture FactionListIcon;
        Vector2 scrollPos;
        float MessageTimer;
        bool MessageDisplay;
        SerializedObject serializedObject;
        ReorderableList FactionListProp;
        EmeraldAIFactionData FactionData;

        public List<string> FactionList = new List<string>();
        public string Faction = "New Faction";

        [MenuItem("Window/Emerald AI/Faction Manager #%r", false, 200)]
        public static void ShowWindow()
        {
            EditorWindow APS = EditorWindow.GetWindow(typeof(EmeraldAIFactionManager));
            APS.minSize = new Vector2(300f, 300f);
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        protected virtual void OnEnable()
        {
            if (FactionIcon == null) FactionIcon = Resources.Load("FactionIcon") as Texture;
            if (FactionListIcon == null) FactionListIcon = Resources.Load("FactionListIcon") as Texture;
            LoadFactionData();
        }

        protected virtual void OnDisable()
        {
            MessageTimer = 0;
            MessageDisplay = false;
        }

        void Update()
        {
            if (MessageDisplay)
            {
                MessageTimer += 0.005f;

                if (MessageTimer > 2.55f)
                {
                    MessageTimer = 0;
                    MessageDisplay = false;
                }
            }
        }

        void CheckFaction()
        {
            if (FactionList.Contains(Faction))
            {
                MessageDisplay = true;
            }
            if (!FactionList.Contains(Faction))
            {
                FactionList.Add(Faction);
                AddFactionData(Faction);
            }
        }

        void OnGUI()
        {

            GUILayout.Space(15);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box");
            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            EditorGUILayout.LabelField(new GUIContent(FactionIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(50));
            EditorGUILayout.LabelField("Emerald AI Faction Manager - v2.0", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox("With the Emerald AI Faction Manager, you can create factions that your AI will use to to determine who is an enemy and who is an ally. " +
                "Factions created here will be globally available for all Emerald AI agents to use. These can be found under your AI's Detection & Tags>Faction Options tab.", MessageType.None, true);
            GUILayout.Space(4);
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(25);

            //Faction Creator
            EditorGUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Space(25);

            EditorGUILayout.BeginVertical("Box");

            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Faction Creator", EditorStyles.boldLabel);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = Color.white;

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("Type a faction name below and press the 'Add Faction' button to create a new faction. You can define what faction an AI is, as well as its opposing factions, via the Emerald AI Editor under the Tag Options.", MessageType.None, true);
            GUI.backgroundColor = Color.white;

            GUILayout.Space(15);

            Faction = EditorGUILayout.TextField("Faction Name", Faction);

            GUILayout.Space(5);

            if (GUILayout.Button("Add Faction"))
            {
                CheckFaction();
            }

            if (MessageDisplay)
            {
                GUI.backgroundColor = new Color(1f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.LabelField("The '" + Faction + "' faction is already defined. Please choose another faction to create.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
            }

            GUILayout.Space(5);

            EditorGUILayout.EndVertical();
            GUILayout.Space(25);
            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            //Faction Creator

            GUILayout.Space(25);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical("Box");
            var style6 = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            EditorGUILayout.LabelField(new GUIContent(FactionListIcon), style6, GUILayout.ExpandWidth(true), GUILayout.Height(32));
            EditorGUILayout.LabelField("Current Factions", style6, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox("Below are all of the globally available factions. Pressing the 'Remove' button will remove the above faction from the global faction list.", MessageType.None, true);
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            //Current Faction
            EditorGUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            GUI.backgroundColor = new Color(0.6f, 0.6f, 0.6f, 0.9f);
            EditorGUILayout.BeginVertical("Box");
            GUI.backgroundColor = Color.white;

            EditorGUILayout.BeginVertical("Box");
            GUI.backgroundColor = Color.white;

            if (FactionList.Count == 0)
            {
                GUI.contentColor = new Color(0.75f, 0.0f, 0.0f, 0.75f);
                EditorGUILayout.LabelField("There are currently no created factions.", EditorStyles.boldLabel);
                GUI.contentColor = Color.white;
            }


            GUILayout.Space(5);

            //FactionListProp.DoLayoutList();
            
            foreach (string s in FactionList.ToArray())
            {
                GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);

                GUILayout.Space(5);
                var style5 = new GUIStyle(GUI.skin.button);
                style5.fontStyle = FontStyle.Bold;

                GUI.backgroundColor = new Color(0.85f, 0.85f, 0.85f, 0.85f);
                GUILayout.Box(s, style5);
                GUI.backgroundColor = Color.white;

                var style4 = new GUIStyle(GUI.skin.button);
                style4.normal.textColor = Color.white;
                style4.fontStyle = FontStyle.Bold;

                GUI.backgroundColor = new Color(0.9f, 0.0f, 0.0f, 0.75f);
                if (GUILayout.Button("Remove", style4))
                {
                    RemoveFactionData(s);
                }

                style4.normal.textColor = Color.black;
                GUI.backgroundColor = Color.white;
                GUILayout.Space(10);
            }

            EditorGUILayout.EndVertical();
            GUI.contentColor = Color.white;

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            GUILayout.Space(25);
            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUILayout.Space(25);
            //Current Faction
        }

        void LoadFactionData()
        {
            string path = AssetDatabase.GetAssetPath(Resources.Load("Faction Data"));
            EmeraldAIFactionData FactionData = (EmeraldAIFactionData)AssetDatabase.LoadAssetAtPath(path, typeof(EmeraldAIFactionData));
            serializedObject = new SerializedObject(FactionData);

            foreach (string s in FactionData.FactionNameList)
            {
                if (!FactionData.FactionNameList.Contains(s) && s != "")
                {
                    FactionData.FactionNameList.Add(s);
                }
            }

            FactionList = new List<string>(FactionData.FactionNameList);

            FactionListProp = new ReorderableList(serializedObject, serializedObject.FindProperty("FactionNameList"), true, true, true, true);
            FactionListProp.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Faction List", EditorStyles.boldLabel);
            };
            FactionListProp.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = FactionListProp.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };
        }

        void AddFactionData(string FactionToAdd)
        {
            string path = AssetDatabase.GetAssetPath(Resources.Load("Faction Data"));
            EmeraldAIFactionData FactionData = (EmeraldAIFactionData)AssetDatabase.LoadAssetAtPath(path, typeof(EmeraldAIFactionData));

            if (!FactionData.FactionNameList.Contains(FactionToAdd) && FactionToAdd != "")
            {
                FactionData.FactionNameList.Add(FactionToAdd);
            }

            FactionList = new List<string>(FactionData.FactionNameList);
            EmeraldAISystem.StringFactionList = FactionList;

            EditorUtility.SetDirty(FactionData);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        void RemoveFactionData(string FactionToRemove)
        {
            string path = AssetDatabase.GetAssetPath(Resources.Load("Faction Data"));
            EmeraldAIFactionData FactionData = (EmeraldAIFactionData)AssetDatabase.LoadAssetAtPath(path, typeof(EmeraldAIFactionData));
            FactionData.FactionNameList.Remove(FactionToRemove);
            FactionList = new List<string>(FactionData.FactionNameList);
            EmeraldAISystem.StringFactionList = FactionList;

            EditorUtility.SetDirty(FactionData);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}
