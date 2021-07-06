using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace EmeraldAI.Utility
{
    [CustomEditor(typeof(EmeraldAIAbility))]
    [System.Serializable]
    public class EmeraldAIAbilityEditor : Editor
    {
        public Texture DamageAbilityIcon;
        public Texture SupportAbilityIcon;
        public Texture SummonAbilityIcon;
        public Texture InfoIcon;
        public Texture SettingsIcon;
        public Texture EffectsSoundsIcon;

        void OnEnable()
        {
            if (DamageAbilityIcon == null) DamageAbilityIcon = Resources.Load("DamageAbilityIcon") as Texture;
            if (SupportAbilityIcon == null) SupportAbilityIcon = Resources.Load("SupportAbilityIcon") as Texture;
            if (SummonAbilityIcon == null) SummonAbilityIcon = Resources.Load("SummonAbilityIcon") as Texture;
            if (InfoIcon == null) InfoIcon = Resources.Load("InfoIcon") as Texture;
            if (SettingsIcon == null) SettingsIcon = Resources.Load("SettingsIcon") as Texture;
            if (EffectsSoundsIcon == null) EffectsSoundsIcon = Resources.Load("EffectsSoundsIcon") as Texture;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EmeraldAIAbility self = (EmeraldAIAbility)target;

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(GUILayout.Width(EditorGUIUtility.currentViewWidth - 45));
            if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Damage)
            {
                var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(self.AbilityName, style, GUILayout.ExpandWidth(true));
                GUILayout.Space(2);
                if (self.UseCustomAbilityIcon == EmeraldAIAbility.Yes_No.No)
                {
                    EditorGUILayout.LabelField(new GUIContent(DamageAbilityIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(50));
                }
                else
                {
                    EditorGUILayout.LabelField(new GUIContent(self.AbilityIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(50));
                }
            }
            else if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Support)
            {
                var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(self.AbilityName, style, GUILayout.ExpandWidth(true));
                GUILayout.Space(2);
                EditorGUILayout.LabelField(new GUIContent(SupportAbilityIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(50));
            }
            else if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Summon)
            {
                var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(self.AbilityName, style, GUILayout.ExpandWidth(true));
                GUILayout.Space(2);
                EditorGUILayout.LabelField(new GUIContent(SummonAbilityIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(50));
            }

            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField(self.AbilityDescription, EditorStyles.helpBox);
            EditorStyles.textField.wordWrap = true;
            GUI.backgroundColor = Color.white;

            GUILayout.Space(4);

            GUIContent[] TabButtons = new GUIContent[3] { new GUIContent(" Ability Info"), new GUIContent("Ability Settings"), new GUIContent(" Ability Effects \n & Sounds") };

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            GUILayout.Space(2);


            if (self.AbilityEditorTabs == 0)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical(GUILayout.Width(EditorGUIUtility.currentViewWidth - 45));
                var style2 = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                GUILayout.Space(2);

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                self.AbilityEditorTabs = GUILayout.SelectionGrid(self.AbilityEditorTabs, TabButtons, 3, GUILayout.Width(EditorGUIUtility.currentViewWidth - 45));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(4);

                EditorGUILayout.LabelField("Ability Info", style2, GUILayout.ExpandWidth(true));
                GUILayout.Space(6);
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                self.AbilityType = (EmeraldAIAbility.AbilityTypeEnum)EditorGUILayout.EnumPopup("Ability Type", self.AbilityType);
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.LabelField("Controls whether the Ability Type is Damage, Support, or Summon.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;

                EditorGUILayout.Space();
                self.AbilityName = EditorGUILayout.TextField("Ability Name", self.AbilityName);
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.LabelField("The name of this Ability.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;

                EditorGUILayout.Space();
                self.AbilityDescription = EditorGUILayout.TextField("Ability Description", self.AbilityDescription, GUILayout.MinHeight(50));
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.LabelField("The description of this Ability.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;

                EditorGUILayout.Space();
                self.UseCustomAbilityIcon = (EmeraldAIAbility.Yes_No)EditorGUILayout.EnumPopup("Use Custom Ability Icon?", self.UseCustomAbilityIcon);

                if (self.UseCustomAbilityIcon == EmeraldAIAbility.Yes_No.Yes)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    self.AbilityIcon = (Texture2D)EditorGUILayout.ObjectField("Ability Icon", self.AbilityIcon, typeof(Texture2D), false);
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("The icon that this Ability uses.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }

            if (self.AbilityEditorTabs == 1)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical(GUILayout.Width(EditorGUIUtility.currentViewWidth - 45));
                var style2 = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                GUILayout.Space(2);

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                self.AbilityEditorTabs = GUILayout.SelectionGrid(self.AbilityEditorTabs, TabButtons, 3, GUILayout.Width(EditorGUIUtility.currentViewWidth - 45));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(4);

                EditorGUILayout.LabelField("Ability Settings", style2, GUILayout.ExpandWidth(true));
                GUILayout.Space(6);
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                //Support
                if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Support)
                {
                    self.SupportType = (EmeraldAIAbility.SupportTypeEnum)EditorGUILayout.EnumPopup("Support Type", self.SupportType);

                    if (self.SupportType == EmeraldAIAbility.SupportTypeEnum.Instant)
                    {
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("When using Instant, a Ability will instantly support its target after it has collided with it.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();
                        self.AbilitySupportAmount = EditorGUILayout.IntField("Ability Support Amount", self.AbilitySupportAmount);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }
                    else if (self.SupportType == EmeraldAIAbility.SupportTypeEnum.OverTime)
                    {
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("When using Over Time, a Ability will apply support to its target for each second after it has collided with its target. " +
                            "It will continue to do so until the Ability Seconds Length has been reahced.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();
                        self.AbilityLength = EditorGUILayout.IntField("Ability Seconds Length", self.AbilityLength);
                        self.AbilitySupportAmount = EditorGUILayout.IntField("Ability Support Amount Per Second", self.AbilitySupportAmount);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.Space();
                    self.AbilityCooldown = EditorGUILayout.IntField("Ability Cooldown Length", self.AbilityCooldown);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("Controls, in seconds, how long it will take for an AI to use another support ability after using this Ability. This is important " +
                        "for balancing support Abilitys so AI aren't using them too often allowing them to be over powered.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }

                //Damage
                if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Damage)
                {
                    self.DamageType = (EmeraldAIAbility.DamageTypeEnum)EditorGUILayout.EnumPopup("Damage Type", self.DamageType);

                    if (self.DamageType == EmeraldAIAbility.DamageTypeEnum.Instant)
                    {
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("When using Instant, a Ability will instantly damage its target after it has collided with it.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();
                        self.AbilityDamage = EditorGUILayout.IntField("Ability Damage Amount", self.AbilityDamage);

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }
                    else if (self.DamageType == EmeraldAIAbility.DamageTypeEnum.OverTime)
                    {
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("When using Over Time, a Ability will do damage to its target for each second after it has collided with its target. " +
                            "It will continue to do so until the Ability Seconds Length has been reahced.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();

                        self.AbilityStacksRef = (EmeraldAIAbility.Yes_No)EditorGUILayout.EnumPopup("Ability Stacks?", self.AbilityStacksRef);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("Controls whether or not this Ability's damage over time will stack multiple times.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;

                        self.AbilityImpactDamage = EditorGUILayout.IntField("Initial Ability Damage", self.AbilityImpactDamage);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("The amount of damage that is initially applied when the ability hits its target.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        self.AbilityDamageIncrement = EditorGUILayout.FloatField("Increment Seconds", self.AbilityDamageIncrement);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("The amount, in seconds, that the damage will be applied. For example, a value of 1 will damage the target 1 time per second where a value of 0.5 would damage the target twice per second.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        self.AbilityDamagePerIncrement = EditorGUILayout.IntField("Damage Per Increment", self.AbilityDamagePerIncrement);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("The amount of damage that is applied per Increment Seconds.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        self.AbilityLength = EditorGUILayout.IntField("Ability Seconds Length", self.AbilityLength);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("The length, in seconds, in which the ability will last.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }
                }

                if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Damage)
                {
                    if (self.DamageType == EmeraldAIAbility.DamageTypeEnum.Instant)
                    {
                        EditorGUILayout.Space();
                        self.UseCriticalHits = (EmeraldAIAbility.Yes_No)EditorGUILayout.EnumPopup("Use Critical Hits?", self.UseCriticalHits);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("Controls whether or not this Ability will use Critical Hits. When a critical hit happens, the base Ability Damage will be multiplied by the Ability's " +
                            "Critical Hit Multiplier. The multiplier is randomized between a minimum and a maximum.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        if (self.UseCriticalHits == EmeraldAIAbility.Yes_No.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(15);
                            EditorGUILayout.BeginVertical();
                            self.CriticalHitOdds = EditorGUILayout.Slider("Critcal Hit Odds", self.CriticalHitOdds, 0.1f, 100f);
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("Controls the odds (in percentage) of a Critical Hit.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;

                            self.CriticalHitMultiplierMin = EditorGUILayout.Slider("Crit Min Multiplier", self.CriticalHitMultiplierMin, 1.1f, 3.0f);
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("The minimum critical hit multiplier.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;

                            self.CriticalHitMultiplierMax = EditorGUILayout.Slider("Crit Max Multiplier", self.CriticalHitMultiplierMax, 1.1f, 3.0f);
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("The minimum critical hit multiplier.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;

                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.EndVertical();
                        }
                    }

                    EditorGUILayout.Space();
                    self.ColliderRadius = EditorGUILayout.FloatField("Collider Radius", self.ColliderRadius);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("Controls the size of the damage ability's radius.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.Space();
                    self.ProjectileSpeed = EditorGUILayout.IntField("Projectile Speed", self.ProjectileSpeed);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("Controls how fast the Ability will move.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.Space();
                    self.CollisionTime = EditorGUILayout.FloatField("Collision Time", self.CollisionTime);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("Controls how fast the Ability will deactivate after it has collided with an object. A delay is useful if your Ability has a trail or effect. " +
                        "A value of 0 can be used if you'd like to deactivate instantly.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }

                //Summon
                if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Summon)
                {
                    EditorGUILayout.Space();
                    self.SummonRadius = EditorGUILayout.IntField("Summon Radius", self.SummonRadius);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("The radius that the summon object will spawn in around the summoner.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.Space();
                    self.AbilityLength = EditorGUILayout.IntField("Summon Length", self.AbilityLength);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("The amount, in seconds, that the summoned object will be alive. After the Summon Length has been met, the summoned object will die.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }

                if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Damage)
                {
                    EditorGUILayout.Space();
                    self.HeatSeekingRef = (EmeraldAIAbility.HeatSeeking)EditorGUILayout.EnumPopup("Use Heat Seeking", self.HeatSeekingRef);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("Controls whether or not this Ability will follow their target. A Heat Seeking Ability makes it easier for an AI to hit their target and makes it more challanging for them to avoid.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                    if (self.HeatSeekingRef == EmeraldAIAbility.HeatSeeking.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();
                        self.HeatSeekingSeconds = EditorGUILayout.FloatField("Heat Seeking Second", self.HeatSeekingSeconds);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("Controls how many seconds the Ability will heat seek for until the heat seeking feature is disabled. " +
                            "After this happens, the Ability will continue to fly in the direction it was going after the heat seeking feature was disabled.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.Space();
                    self.ArrowProjectileRef = (EmeraldAIAbility.ArrowObject)EditorGUILayout.EnumPopup("Is Arrow Projectile", self.ArrowProjectileRef);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("Controls whether or not this projectile is an arrow object and will briefly stay stuck into a non-target surface instead of disappearing.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;

                }

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }

            if (self.AbilityEditorTabs == 2)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical(GUILayout.Width(EditorGUIUtility.currentViewWidth - 45));
                var style2 = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                GUILayout.Space(2);

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                self.AbilityEditorTabs = GUILayout.SelectionGrid(self.AbilityEditorTabs, TabButtons, 3, GUILayout.Width(EditorGUIUtility.currentViewWidth - 45));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(4);

                EditorGUILayout.LabelField("Ability Effects & Sounds", style2, GUILayout.ExpandWidth(true));
                GUILayout.Space(6);
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                if (self.AbilityType != EmeraldAIAbility.AbilityTypeEnum.Summon)
                {
                    EditorGUILayout.Space();
                    self.AbilityEffect = (GameObject)EditorGUILayout.ObjectField("Ability Effect", self.AbilityEffect, typeof(GameObject), false);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("The prefab gameobject effect your AI will use for this Ability.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;

                    self.AbilityEffectTimeoutSeconds = EditorGUILayout.Slider("Ability Effect Timeout", self.AbilityEffectTimeoutSeconds, 1, 60);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("Controls the seconds it takes for the Ability Effect to despawn. When using Damage Type Abilitys, this controls how long it will take for " +
                        "the projectile to despawn, given that it hasn't collided with an object.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }

                if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Summon)
                {
                    EditorGUILayout.Space();
                    self.SummonEffect = (GameObject)EditorGUILayout.ObjectField("Summon Effect", self.SummonEffect, typeof(GameObject), false);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("The effect that will happen when the object is summoned.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;

                    self.AbilityEffectTimeoutSeconds = EditorGUILayout.Slider("Summon Effect Timeout", self.AbilityEffectTimeoutSeconds, 1, 10);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("Controls the seconds it takes for the Summon Effect to despawn.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }

                if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Damage)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    self.UseCastEffect = (EmeraldAIAbility.Yes_No)EditorGUILayout.EnumPopup("Use Cast Effect", self.UseCastEffect);
                    if (self.UseCastEffect == EmeraldAIAbility.Yes_No.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();
                        self.CastEffect = (GameObject)EditorGUILayout.ObjectField("Cast Effect", self.CastEffect, typeof(GameObject), false);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("The effect that happens when the Ability is being cast. The position of this effect is based off of your AI's Ranged Attack Transform.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;

                        self.CastEffectTimeoutSeconds = EditorGUILayout.Slider("Cast Effect Timeout", self.CastEffectTimeoutSeconds, 0.0025f, 4);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("Controls the seconds it takes for the Cast Effect to despawn.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.Space();
                    self.EffectOnCollisionRef = (EmeraldAIAbility.EffectOnCollision)EditorGUILayout.EnumPopup("Use Collision Effect", self.EffectOnCollisionRef);
                    if (self.EffectOnCollisionRef == EmeraldAIAbility.EffectOnCollision.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();
                        self.CollisionEffect = (GameObject)EditorGUILayout.ObjectField("Collision Effect", self.CollisionEffect, typeof(GameObject), false);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("The effect that happens after your Ability has collided with an object.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;

                        self.CollisionTimeout = EditorGUILayout.Slider("Collision Effect Timeout", self.CollisionTimeout, 0.5f, 5);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("Controls the seconds it takes for the Collision Effect to despawn.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }

                    if (self.DamageType == EmeraldAIAbility.DamageTypeEnum.OverTime)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        self.UseDamageOverTimeEffectRef = (EmeraldAIAbility.Yes_No)EditorGUILayout.EnumPopup("Use Damage Over Time Effect", self.UseDamageOverTimeEffectRef);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("Controls whether or not this Ability will use a damage over time effect.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;

                        if (self.UseDamageOverTimeEffectRef == EmeraldAIAbility.Yes_No.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(15);
                            EditorGUILayout.BeginVertical();
                            self.DamageOverTimeEffect = (GameObject)EditorGUILayout.ObjectField("Damage Over Time Effect", self.DamageOverTimeEffect, typeof(GameObject), false);
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("The gameobject effect that will be created on the Ability's target each time it is damaged over time.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;

                            self.DamageOvertimeTimeout = EditorGUILayout.Slider("Damage Over Time Effect Timeout", self.DamageOvertimeTimeout, 0.5f, 5);
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("Controls the seconds it takes for the Damage Overtime Effect to despawn.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;

                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.Space();
                        }
                    }
                }

                if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Support)
                {
                    if (self.SupportType == EmeraldAIAbility.SupportTypeEnum.OverTime)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        self.UseDamageOverTimeEffectRef = (EmeraldAIAbility.Yes_No)EditorGUILayout.EnumPopup("Use Support Over Time Effect", self.UseDamageOverTimeEffectRef);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("Controls whether or not this Ability will use a support over time effect.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;

                        if (self.UseDamageOverTimeEffectRef == EmeraldAIAbility.Yes_No.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(15);
                            EditorGUILayout.BeginVertical();
                            self.DamageOverTimeEffect = (GameObject)EditorGUILayout.ObjectField("Support Over Time Effect", self.DamageOverTimeEffect, typeof(GameObject), false);
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("The gameobject effect that will be created on the Ability's target each time it is healed over time.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;

                            self.DamageOvertimeTimeout = EditorGUILayout.Slider("Support Over Time Effect Timeout", self.DamageOvertimeTimeout, 0.5f, 5);
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("Controls the seconds it takes for the Support Overtime Effect to despawn.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;

                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.Space();
                        }
                    }
                }

                if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Summon)
                {
                    EditorGUILayout.Space();
                    self.SummonObject = (GameObject)EditorGUILayout.ObjectField("Summon Object", self.SummonObject, typeof(GameObject), false);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("The AI that will be summoned from this Ability. Note: This must be an Emerald AI object.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }

                EditorGUILayout.Space();
                self.UseCastSound = (EmeraldAIAbility.Yes_No)EditorGUILayout.EnumPopup("Use Cast Sound", self.UseCastSound);
                if (self.UseCastSound == EmeraldAIAbility.Yes_No.Yes)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(15);
                    EditorGUILayout.BeginVertical();
                    self.CastSound = (AudioClip)EditorGUILayout.ObjectField("Cast Sound", self.CastSound, typeof(AudioClip), false);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("The sound effect that happens when the Ability is being cast.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }

                if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Support)
                {
                    if (self.SupportType == EmeraldAIAbility.SupportTypeEnum.OverTime)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        self.UseDamageOverTimeSoundRef = (EmeraldAIAbility.Yes_No)EditorGUILayout.EnumPopup("Use Support Over Time Sound", self.UseDamageOverTimeSoundRef);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("Controls whether or not this Ability will use a support over time Sound.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;

                        if (self.UseDamageOverTimeSoundRef == EmeraldAIAbility.Yes_No.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(15);
                            EditorGUILayout.BeginVertical();
                            self.DamageOverTimeSound = (AudioClip)EditorGUILayout.ObjectField("Support Over Time Sound", self.DamageOverTimeSound, typeof(AudioClip), false);
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("The sound effect that will play each time is healed over time.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.Space();
                        }
                    }
                }

                if (self.AbilityType == EmeraldAIAbility.AbilityTypeEnum.Damage)
                {
                    if (self.DamageType == EmeraldAIAbility.DamageTypeEnum.OverTime)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        self.UseDamageOverTimeSoundRef = (EmeraldAIAbility.Yes_No)EditorGUILayout.EnumPopup("Use Damage Over Time Sound", self.UseDamageOverTimeSoundRef);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("Controls whether or not this Ability will use a damage over time Sound.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;

                        if (self.UseDamageOverTimeSoundRef == EmeraldAIAbility.Yes_No.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(15);
                            EditorGUILayout.BeginVertical();
                            self.DamageOverTimeSound = (AudioClip)EditorGUILayout.ObjectField("Damage Over Time Sound", self.DamageOverTimeSound, typeof(AudioClip), false);
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("The sound effect that will play each time is damaged over time.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.Space();
                        }
                    }

                    EditorGUILayout.Space();
                    self.SoundOnCollisionRef = (EmeraldAIAbility.EffectOnCollision)EditorGUILayout.EnumPopup("Use Collision Sound", self.SoundOnCollisionRef);
                    if (self.SoundOnCollisionRef == EmeraldAIAbility.EffectOnCollision.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();
                        self.ImpactSound = (AudioClip)EditorGUILayout.ObjectField("Collision Sound", self.ImpactSound, typeof(AudioClip), false);
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        EditorGUILayout.LabelField("The sound effect that happens after your Ability has collided with an object.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Undo.RecordObject(self, "Undo");

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                }
            }
#endif

            serializedObject.ApplyModifiedProperties();
        }
    }
}