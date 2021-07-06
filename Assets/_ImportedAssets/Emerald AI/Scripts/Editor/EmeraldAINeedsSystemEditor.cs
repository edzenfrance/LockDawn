using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace EmeraldAI.Utility
{
    [CustomEditor(typeof(EmeraldAINeedsSystem))]
    [System.Serializable]
    public class EmeraldAINeedsSystemEditor : Editor
    {
        SerializedProperty SearchLayerMaskProp, GatherEventProp;

        private void OnEnable()
        {
            SearchLayerMaskProp = serializedObject.FindProperty("SearchLayerMask");
            GatherEventProp = serializedObject.FindProperty("GatherEvent");
        }

        public override void OnInspectorGUI()
        {
            EmeraldAINeedsSystem self = (EmeraldAINeedsSystem)target;
            serializedObject.Update();

            var TitleStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            var NonTitleStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter };

            EditorGUILayout.BeginVertical("Box");

            //EditorGUILayout.LabelField(new GUIContent(SettingsIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("AI Needs System", TitleStyle, GUILayout.ExpandWidth(true));
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The Needs System is an external tool that can be attached to an AI that allows them to procedurally generate waypoints to resources when needed. " +
                "An AI will look for resources when their Current Resources level reaches thier Resources Low Threshold. Resources can be anything found in the AI's world and is specified based " +
                "on the SearchLayerMask. An AI will dynamically wander when not looking for resources.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("AI's Current Resources: " + self.CurrentResourcesLevel.ToString(), NonTitleStyle, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();

            var HeaderStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("System Settings", HeaderStyle);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            var layersSelection = EditorGUILayout.MaskField("Searchable Layers", LayerMaskToField(SearchLayerMaskProp.intValue), InternalEditorUtility.layers);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(self, "Layers changed");
                SearchLayerMaskProp.intValue = FieldToLayerMask(layersSelection);
            }
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("The layers the AI will use when searching for resources. Any object with the above layer will be picked when generating waypoints, " +
                "given that the Max Resource Waypoints is not exceeded.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.SearchRadius = EditorGUILayout.IntSlider("Search Radius", self.SearchRadius, 10, 300);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the radius in which an AI will search for resources.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.WanderRadius = EditorGUILayout.IntSlider("Wander Radius", self.WanderRadius, 10, 300);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the radius in which an AI will dynmaically wander when not looking for resources.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.UpdateResourcesFrequency = EditorGUILayout.IntSlider("Update Resources Frequency", self.UpdateResourcesFrequency, 1, 30);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls how often the AI's resources are updated. When this happens, an AI's Current Resources are used based on their Resources Usage amount.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.MaxWaypoints = EditorGUILayout.IntSlider("Max Resource Waypoints", self.MaxWaypoints, 2, 15);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the max waypoints an AI will generate when searching for resources. An AI will place a waypoint for each resource found.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.IdleAnimationIndex = EditorGUILayout.IntSlider("Idle Animation Index", self.IdleAnimationIndex, 1, 3);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the idle animation that will be used when wandering (not looking for resources) according your AI's Idle Animation List index.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.GatherResourceAnimationIndex = EditorGUILayout.IntSlider("Gather Animation Index", self.GatherResourceAnimationIndex, 1, 6);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the animation that will be used when an AI at a resource location. This is based off of your AI's Idle Animation List index.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Resource Settings", HeaderStyle);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            self.CurrentResourcesLevel = EditorGUILayout.IntSlider("Starting Resources", self.CurrentResourcesLevel, 0, 100);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the amount of resources an AI will start with. If set to 0, an AI will immediately start looking for resources.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.ResourcesFullThreshold = EditorGUILayout.IntSlider("Resources Full Threshold", self.ResourcesFullThreshold, 0, 100);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the threshold of when an AI's resources are full. When this happens, an AI will no longe look for resources and will instead " +
                "dynamically wander around their current destination.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.ResourceUsage = EditorGUILayout.IntSlider("Resource Usage", self.ResourceUsage, 1, 30);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls how many resources are used when an AI's needs are updated which is based on the AI's Update Needs Frequency.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.ResourcesLowThreshold = EditorGUILayout.IntSlider("Resources Low Threshold", self.ResourcesLowThreshold, 0, 80);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the threshold of when an AI will generate new waypoints to resources. This is based off of the AI's Current Reource amount.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            self.ResourceRefillMultiplier = EditorGUILayout.Slider("Resource Refill Multiplier", self.ResourceRefillMultiplier, 0.1f, 5f);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the speed in which an AI's Current Resources are replenished (By default, this is 1 resource for each second an AI is grazing/idling at a resource destination).", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("An Event that is triggered every time a resource is gathered. This can be used to run external code to add items to inventories or " +
                "increase custom variables.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.PropertyField(GatherEventProp);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            self.DepletedResourcesKillsAI = (EmeraldAINeedsSystem.DepletedResourcesKillsAIEnum)EditorGUILayout.EnumPopup("Depleted Resources Kills AI", self.DepletedResourcesKillsAI);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls whether or not this AI will die if they run out of resources. Users can customize the amount of seconds before this happens.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;

            if (self.DepletedResourcesKillsAI == EmeraldAINeedsSystem.DepletedResourcesKillsAIEnum.Yes)
            {
                self.SecondsNeededForDeath = EditorGUILayout.IntSlider("Max Resource Waypoints", self.SecondsNeededForDeath, 5, 600);
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.LabelField("Controls the amount of time, in seconds, it will take for an AI to die after their Current Resources amount reached 0.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.Space();           

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            EmeraldAINeedsSystem self = (EmeraldAINeedsSystem)target;
            Handles.color = self.SearchRadiusColor;
            Handles.DrawWireDisc(self.transform.position, self.transform.up, (float)self.SearchRadius);
            Handles.color = Color.white;

            Handles.color = self.WanderRadiusColor;
            Handles.DrawWireDisc(self.transform.position, self.transform.up, (float)self.WanderRadius);
            Handles.color = Color.white;
        }

        // Converts the field value to a LayerMask
        private LayerMask FieldToLayerMask(int field)
        {
            LayerMask mask = 0;
            var layers = InternalEditorUtility.layers;
            for (int c = 0; c < layers.Length; c++)
            {
                if ((field & (1 << c)) != 0)
                {
                    mask |= 1 << LayerMask.NameToLayer(layers[c]);
                }
            }
            return mask;
        }
        // Converts a LayerMask to a field value
        private int LayerMaskToField(LayerMask mask)
        {
            int field = 0;
            var layers = InternalEditorUtility.layers;
            for (int c = 0; c < layers.Length; c++)
            {
                if ((mask & (1 << LayerMask.NameToLayer(layers[c]))) != 0)
                {
                    field |= 1 << c;
                }
            }
            return field;
        }
    }
}