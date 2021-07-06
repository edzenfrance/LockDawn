using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.IMGUI.Controls;
using System.Linq;

namespace EmeraldAI.Utility
{
    public class EmeraldAISetupManager : EditorWindow
    {
        public GameObject ObjectToSetup;
        public LayerMask AILayer = 4;
        public string AITag = "Untagged";
        public List<GameObject> ObjectsToSetup = new List<GameObject>();

        public enum AIBehavior { Passive = 0, Cautious = 1, Companion = 2, Aggressive = 3, Pet = 4 };
        public AIBehavior AIBehaviorRef = AIBehavior.Aggressive;

        public enum ConfidenceType { Coward = 0, Brave = 1, Foolhardy = 2 };
        public ConfidenceType ConfidenceRef = ConfidenceType.Foolhardy;

        public enum WanderType { Dynamic = 0, Waypoints = 1, Stationary = 2, Destination = 3 };
        public WanderType WanderTypeRef = WanderType.Dynamic;

        public enum SetupOptimizationSettings { Yes = 0, No = 1 };
        public SetupOptimizationSettings SetupOptimizationSettingsRef = SetupOptimizationSettings.Yes;

        public enum WeaponType { Melee = 0, Ranged = 1, Both = 2 };
        public WeaponType WeaponTypeRef = WeaponType.Melee;

        public enum DeathType { Animation = 0, Ragdoll };
        public DeathType DeathTypeRef = DeathType.Animation;

        public AnimatorTypeState AnimatorType = AnimatorTypeState.NavMeshDriven;
        public enum AnimatorTypeState { RootMotion, NavMeshDriven }

        BoxBoundsHandle m_BoundsHandle;
        Vector3 TotalBondsSize;
        Vector3 TotalBondsExtends;
        Vector2 scrollPos;
        string FilePath;
        Bounds RendererBounds;
        Texture SettingsIcon;
        static float secs = 0;
        static double startVal = 0;
        static double progress = 0;
        Vector3 _T;
        bool DisplayConfirmation = false;
        static bool DontShowDisplayConfirmation = false;

        void OnInspectorUpdate()
        {
            Repaint();
        }


        [MenuItem("Window/Emerald AI/Setup Manager #%e", false, 200)]
        public static void ShowWindow()
        {
            EditorWindow APS = EditorWindow.GetWindow(typeof(EmeraldAISetupManager), false, "Setup Manager");
            APS.minSize = new Vector2(300f, 250f);
        }

        void OnEnable()
        {
            if (SettingsIcon == null) SettingsIcon = Resources.Load("SettingsIcon") as Texture;
        }

        void OnGUI()
        {
            GUILayout.Space(15);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("Box");
            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            EditorGUILayout.LabelField(new GUIContent(SettingsIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
            EditorGUILayout.LabelField("Emerald AI Setup Manager", style, GUILayout.ExpandWidth(true));
            EditorGUILayout.HelpBox("The Emerald AI Setup Manager applies all needed settings and components to automatically create an AI on the applied object. Be aware that closing the Emerald Setup Manager will lose all references you've entered below. Make sure you select 'Setup AI' before closing, if you'd like your changes to be applied.", MessageType.None, true);
            GUILayout.Space(4);

            var HelpButtonStyle = new GUIStyle(GUI.skin.button);
            HelpButtonStyle.normal.textColor = Color.white;
            HelpButtonStyle.fontStyle = FontStyle.Bold;

            GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
            EditorGUILayout.LabelField("For a detailed tutorial on setting up an AI from start to finish, please see the Getting Started Tutorial below.", EditorStyles.helpBox);
            GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
            if (GUILayout.Button("See the Getting Started Tutorial", HelpButtonStyle, GUILayout.Height(20)))
            {
                Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Creating-a-New-AI");
            }
            GUI.backgroundColor = Color.white;
            GUILayout.Space(10);

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(15);

            EditorGUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.BeginVertical("Box");

            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Setup Settings", EditorStyles.boldLabel);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = Color.white;

            GUILayout.Space(15);

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The object that the Emerald AI system will be added to.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            if (ObjectToSetup == null)
            {
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.LabelField("This field cannot be left blank.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
            }
            ObjectToSetup = (GameObject)EditorGUILayout.ObjectField("AI Object", ObjectToSetup, typeof(GameObject), true);
            GUILayout.Space(10);

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The Unity Tag that will be applied to your AI. Note: Untagged cannot be used.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            AITag = EditorGUILayout.TagField("Tag for AI", AITag);
            GUILayout.Space(10);

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The Unity Layer that will be applied to your AI. Note: Default cannot be used.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            AILayer = EditorGUILayout.LayerField("Layer for AI", AILayer);
            GUILayout.Space(10);

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The Behavior that will be applied to this AI.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            AIBehaviorRef = (AIBehavior)EditorGUILayout.EnumPopup("AI's Behavior", AIBehaviorRef);
            GUILayout.Space(10);

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The Confidence that will be applied to this AI.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            ConfidenceRef = (ConfidenceType)EditorGUILayout.EnumPopup("AI's Confidence", ConfidenceRef);
            GUILayout.Space(10);

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The Wander Type  that will be applied to this AI.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            WanderTypeRef = (WanderType)EditorGUILayout.EnumPopup("AI's Wander Type", WanderTypeRef);
            GUILayout.Space(10);

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("Would you like the Setup Manager to automatically setup Emerald's optimization settings? This allows Emerald to be deactivated when an AI is culled or not visible which will help improve performance.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            SetupOptimizationSettingsRef = (SetupOptimizationSettings)EditorGUILayout.EnumPopup("Auto Optimize", SetupOptimizationSettingsRef);
            GUILayout.Space(10);

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("The Weapon Type this AI will use.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            WeaponTypeRef = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", WeaponTypeRef);
            GUILayout.Space(10);

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("Controls whether this AI will play an animation on death or transition to a ragdoll state.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            DeathTypeRef = (DeathType)EditorGUILayout.EnumPopup("Death Type", DeathTypeRef);
            GUILayout.Space(10);

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.HelpBox("Controls whether this AI will use Root Motion or NavMesh for its movement and speed.", MessageType.None, true);
            GUI.backgroundColor = Color.white;
            AnimatorType = (AnimatorTypeState)EditorGUILayout.EnumPopup("Animator Type", AnimatorType);
            GUILayout.Space(30);

            if (ObjectToSetup == null)
            {
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.LabelField("You must have an object applied to the AI Object slot before you can complete the setup process.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
            }

            EditorGUI.BeginDisabledGroup(ObjectToSetup == null);
            if (GUILayout.Button("Setup AI"))
            {
                if (EditorUtility.DisplayDialog("Emerald AI Setup Manager", "Are you sure you'd like to setup an AI on this object?", "Setup", "Cancel"))
                {
                    #if UNITY_2018_3_OR_NEWER
                    
                    PrefabAssetType m_AssetType = PrefabUtility.GetPrefabAssetType(ObjectToSetup);

                    //Only unpack prefab if the ObjectToSetup is a prefab.
                    if (m_AssetType != PrefabAssetType.NotAPrefab)
                    {
                        PrefabUtility.UnpackPrefabInstance(ObjectToSetup, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                    }
                    #else
                    PrefabUtility.DisconnectPrefabInstance(ObjectToSetup);
                    #endif
                    AssignEmeraldAIComponents();
                    startVal = EditorApplication.timeSinceStartup;
                }
            }
            GUILayout.Space(25);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            GUILayout.Space(25);
            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();


            GUILayout.Space(30);

            if (secs > 0)
            {
                progress = EditorApplication.timeSinceStartup - startVal;

                if (progress < secs)
                {
                    EditorUtility.DisplayProgressBar("Emerald AI Setup Manager", "Setting up AI...", (float)(progress / secs));
                }
                else
                {
                    EditorUtility.ClearProgressBar();

                    if (DisplayConfirmation && !DontShowDisplayConfirmation)
                    {
                        if (EditorUtility.DisplayDialog("Emerald AI Setup Manager - Success", "Your AI has been successfully created! You will still need to create an Animator Controller, " +
                            "apply your AI's Animations, and assign the AI's Head Transform from within the Emerald AI Editor. You may also need to adjust the generated Box Collider's " +
                            "position and size to properly fit your AI's model.", "Okay", "Okay, Don't Show Again"))
                        {
                            DisplayConfirmation = false;
                        }
                        else
                        {
                            DisplayConfirmation = false;
                            DontShowDisplayConfirmation = true;
                        }
                    }
                }
            }
        }

        void AssignEmeraldAIComponents()
        {
            if (ObjectToSetup != null && ObjectToSetup.GetComponent<EmeraldAISystem>() == null && ObjectToSetup.GetComponent<Animator>() != null)
            {
                secs = 1.5f;

                ObjectToSetup.AddComponent<EmeraldAISystem>();
                ObjectToSetup.GetComponent<AudioSource>().spatialBlend = 1;
                ObjectToSetup.GetComponent<AudioSource>().dopplerLevel = 0;
                ObjectToSetup.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
                ObjectToSetup.GetComponent<AudioSource>().minDistance = 1;
                ObjectToSetup.GetComponent<AudioSource>().maxDistance = 50;

                EmeraldAISystem Emerald = ObjectToSetup.GetComponent<EmeraldAISystem>();

                Emerald.BehaviorRef = (EmeraldAISystem.CurrentBehavior)AIBehaviorRef;
                Emerald.ConfidenceRef = (EmeraldAISystem.ConfidenceType)ConfidenceRef;
                Emerald.WanderTypeRef = (EmeraldAISystem.WanderType)WanderTypeRef;
                Emerald.WeaponTypeRef = (EmeraldAISystem.WeaponType)WeaponTypeRef;
                Emerald.DeathTypeRef = (EmeraldAISystem.DeathType)DeathTypeRef;
                Emerald.AnimatorType = (EmeraldAISystem.AnimatorTypeState)AnimatorType;

                if (SetupOptimizationSettingsRef == SetupOptimizationSettings.Yes)
                {
                    Emerald.DisableAIWhenNotInViewRef = EmeraldAISystem.YesOrNo.Yes;
                    if (ObjectToSetup.GetComponent<LODGroup>() != null)
                    {
                        LODGroup _LODGroup = ObjectToSetup.GetComponentInChildren<LODGroup>();

                        if (_LODGroup != null)
                        {
                            LOD[] AllLODs = _LODGroup.GetLODs();                            

                            if (_LODGroup.lodCount <= 4)
                            {
                                Emerald.TotalLODsRef = (EmeraldAISystem.TotalLODsEnum)(_LODGroup.lodCount - 2);
                                Emerald.HasMultipleLODsRef = EmeraldAISystem.YesOrNo.Yes;
                            }

                            if (_LODGroup.lodCount > 1)
                            {
                                for (int i = 0; i < _LODGroup.lodCount; i++)
                                {
                                    if (i == 0)
                                    {
                                        Emerald.Renderer1 = AllLODs[0].renderers[0];
                                    }
                                    if (i == 1)
                                    {
                                        Emerald.Renderer2 = AllLODs[1].renderers[0];
                                    }
                                    if (i == 2)
                                    {
                                        Emerald.Renderer3 = AllLODs[2].renderers[0];
                                    }
                                    if (i == 3)
                                    {
                                        Emerald.Renderer4 = AllLODs[3].renderers[0];
                                    }
                                }
                            }
                            else if (_LODGroup.lodCount == 1)
                            {
                                Emerald.HasMultipleLODsRef = EmeraldAISystem.YesOrNo.No;
                            }
                        }
                        else if (_LODGroup == null)
                        {
                            Emerald.HasMultipleLODsRef = EmeraldAISystem.YesOrNo.No;
                        }

                    }

                    List<SkinnedMeshRenderer> TempSkinnedMeshes = new List<SkinnedMeshRenderer>();
                    List<float> TempSkinnedMeshBounds = new List<float>();

                    foreach (SkinnedMeshRenderer SMR in Emerald.GetComponentsInChildren<SkinnedMeshRenderer>())
                    {
                        if (!TempSkinnedMeshes.Contains(SMR))
                        {                           
                            TempSkinnedMeshes.Add(SMR);
                            TempSkinnedMeshBounds.Add(SMR.bounds.size.sqrMagnitude);
                        }
                    }

                    float m_LargestBounds = TempSkinnedMeshBounds.Max();
                    Emerald.AIRenderer = TempSkinnedMeshes[TempSkinnedMeshBounds.IndexOf(m_LargestBounds)];
                }

                Emerald.gameObject.tag = AITag;
                Emerald.gameObject.layer = AILayer;
                Emerald.DetectionLayerMask = (1 << LayerMask.NameToLayer("Water"));

                Emerald.IdleAnimationList.Add(null);
                if (Emerald.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
                {
                    if (Emerald.BehaviorRef != EmeraldAISystem.CurrentBehavior.Passive || Emerald.BehaviorRef != EmeraldAISystem.CurrentBehavior.Cautious && Emerald.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward)
                    {
                        Emerald.AttackAnimationList.Add(null);
                        Emerald.RunAttackAnimationList.Add(null);
                    }
                }
                Emerald.HitAnimationList.Add(null);
                Emerald.CombatHitAnimationList.Add(null);
                Emerald.DeathAnimationList.Add(null);

                Component[] AllComponents = ObjectToSetup.GetComponents<Component>();

                for (int i = 0; i < AllComponents.Length; i++)
                {
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(Emerald);
                }

                ObjectToSetup.GetComponent<BoxCollider>().size = new Vector3(Emerald.AIRenderer.bounds.size.x / 3 / ObjectToSetup.transform.localScale.y, Emerald.AIRenderer.bounds.size.y / ObjectToSetup.transform.localScale.y, Emerald.AIRenderer.bounds.size.z / 3 / ObjectToSetup.transform.localScale.y);
                ObjectToSetup.GetComponent<BoxCollider>().center = new Vector3(ObjectToSetup.GetComponent<BoxCollider>().center.x, ObjectToSetup.GetComponent<BoxCollider>().size.y / 2, ObjectToSetup.GetComponent<BoxCollider>().center.z);

                if (!DontShowDisplayConfirmation)
                {
                    DisplayConfirmation = true;
                }

                ObjectToSetup = null;
            }
            else if (ObjectToSetup == null)
            {
                EditorUtility.DisplayDialog("Emerald AI Setup Manager - Oops!", "There is no object assigned to the AI Object slot. Please assign one and try again.", "Okay");
            }
            else if (ObjectToSetup.GetComponent<EmeraldAISystem>() != null)
            {
                EditorUtility.DisplayDialog("Emerald AI Setup Manager - Oops!", "There is already an Emerald AI component on this object. Please choose another object to apply an Emerald AI component to.", "Okay");
            }
            else if (ObjectToSetup.GetComponent<Animator>() == null)
            {
                EditorUtility.DisplayDialog("Emerald AI Setup Manager - Oops!", "There is no Animator component attached to your AI. Please assign one and try again.", "Okay");
            }
        }
    }
}
