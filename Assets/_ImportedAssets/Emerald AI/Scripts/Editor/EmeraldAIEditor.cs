using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEditorInternal;

namespace EmeraldAI.Utility
{
    [System.Serializable]
    [CustomEditor(typeof(EmeraldAISystem))]
    [CanEditMultipleObjects]
    public class EmeraldAIEditor : Editor
    {
        string[] RangedEnumAnimations;
        string[] MeleeEnumAnimations;
        string[] RunMeleeEnumAnimations;
        string[] RangedBlankOptions = { "No Ranged Attack Animations" };
        string[] MeleeBlankOptions = { "No Attack Animations" };
        string[] RunMeleeBlankOptions = { "No Run Attack Animations" };

        #region Serialized Properties
        //Head Look
        SerializedProperty HeadLookWeightCombatProp, BodyLookWeightCombatProp, HeadLookWeightNonCombatProp, BodyLookWeightNonCombatProp, MaxLookAtDistanceProp, HeadLookYOffsetProp,
            UseHeadLookProp, LookSmootherProp, NonCombatLookAtLimitProp, CombatLookAtLimitProp, HeadTransformProp;

        //System
        SerializedProperty AnimationListsChangedProp;

        //ints
        SerializedProperty TabNumberProp, TemperamentTabNumberProp, DetectionTagsTabNumberProp, AnimationTabNumberProp, SoundTabNumberProp, MovementTabNumberProp, fieldOfViewAngleProp, BackUpOddsProp,
        DetectionRadiusProp, WanderRadiusProp, MinimumWaitTimeProp, MaximumWaitTimeProp, WalkSpeedProp, RunSpeedProp, StartingHealthProp, ExpandedChaseDistanceProp, WeaponTypeControlTabNumberProp,
        TotalAggroHitsProp, SecondsToDisableProp, StationaryTurningSpeedCombatProp, BackupTurningSpeedProp, CritChanceProp, CritMultiplierProp,
        MinimumFollowWaitTimeProp, MaximumFollowWaitTimeProp, MaxChaseDistanceProp, HealthPercentageToFleeProp, DeactivateDelayProp, HealthRegRateProp, RegenerateAmountProp, MaxSlopeLimitProp,
        DeathDelayMinProp, DeathDelayMaxProp, CurrentFactionProp, ExpandedFieldOfViewAngleProp, ExpandedDetectionRadiusProp, StationaryIdleSecondsMinProp, StationaryIdleSecondsMaxProp, CautiousSecondsProp,
        IdleSoundsSecondsMinProp, IdleSoundsSecondsMaxProp, SentRagdollForceProp, ObstructionSecondsProp, EventTabNumberProp, WeaponTypeCombatTabNumberProp,
        CombatTabNumberProp, WeaponTypeTabNumberProp, BackupSecondsMinProp, FactionsAndTagTabNumberProp, MaxUIScaleSizeProp,
            MaxAllowedSummonedAIProp, HealthPercentageToHealProp, WalkBackwardsSpeedProp, PlayerDetectionEventCooldownProp, SwitchWeaponTypesDistanceProp, SwitchWeaponTypesCooldownProp, BackupSecondsMaxProp;

        //floats
        SerializedProperty MinimumAttackSpeedProp, MaximumAttackSpeedProp, MinimumRunAttackSpeedProp, MaximumRunAttackSpeedProp, StoppingDistanceProp, FollowingStoppingDistanceProp,
            RunAttackDistanceProp, AgentRadiusProp, AgentBaseOffsetProp, AgentAccelerationProp, MaxNormalAngleProp, NonCombatAlignSpeedProp, CombatAlignSpeedProp, AgentTurnSpeedProp,
            NonCombatRunAnimationSpeedProp, CombatRunAnimationSpeedProp, IdleWarningAnimationSpeedProp, IdleNonCombatAnimationSpeedProp, AINameLineSpacingProp, CriticalHitVolumeProp,
            IdleCombatAnimationSpeedProp, NonCombatWalkAnimationSpeedProp, CombatWalkAnimationSpeedProp, MaxDamageAngleProp, WalkFootstepVolumeProp, RunFootstepVolumeProp, BlockVolumeProp,
            TurnLeftAnimationSpeedProp, TurnRightAnimationSpeedProp, CombatTurnLeftAnimationSpeedProp, CombatTurnRightAnimationSpeedProp, AttackVolumeProp, DeathVolumeProp, IdleVolumeProp,
            NonCombatAngleToTurnProp, CombatAngleToTurnProp, AttackDistanceProp, InjuredVolumeProp, ImpactVolumeProp, EquipVolumeProp, UnequipVolumeProp, BloodEffectTimeoutSecondsProp,
            TooCloseDistanceProp, PlayerYOffsetProp, MitigationAmountProp, WarningVolumeProp, ProjectileCollisionPointYProp, RangedIdleCombatAnimationSpeedProp, DetectionFrequencyProp,
            RangedAttackDistanceProp, RangedTooCloseDistanceProp, RangedIdleWarningAnimationSpeedProp, RangedCombatTurnLeftAnimationSpeedProp, RangedCombatTurnRightAnimationSpeedProp,
            RangedCombatWalkAnimationSpeedProp, RangedCombatRunAnimationSpeedProp, RangedEquipVolumeProp, RangedUnequipVolumeProp, YAimOffsetProp, MaxFiringAngleProp, AttackHeightProp;

        //enums
        SerializedProperty BehaviorProp, ConfidenceProp, RandomizeDamageProp, DetectionTypeProp, MaxChaseDistanceTypeProp, CombatTypeProp, CreateHealthBarsProp, UseCustomFontAINameProp, UseCustomFontAILevelProp,
            CustomizeHealthBarProp, DisplayAINameProp, DisplayAITitleProp, DisplayAILevelProp, RefillHealthTypeProp, AttackOnArrivalProp, UseAINameUIOutlineEffectProp, UseAILevelUIOutlineEffectProp,
            WanderTypeProp, WaypointTypeProp, AlignAIWithGroundProp, CurrentMovementStateProp, UseBloodEffectProp, UseWarningAnimationProp, TotalLODsProp, HasMultipleLODsProp, TurnAnimationTypeProp,
            AlignAIOnStartProp, AlignmentQualityProp, PickTargetMethodProp, AIAttacksPlayerProp, UseNonAITagProp, BloodEffectPositionTypeProp, AggroActionProp, MaxBlockingAngleProp, DebugLogProjectileCollisionsEnabledProp,
            WeaponTypeProp, UseRunAttacksProp, ObstructionDetectionQualityProp, AvoidanceQualityProp, UseBlockingProp, BlockingOddsProp, UseAggroProp, SpawnedWithCruxProp, SummonsMultipleAIProp,
            UseEquipAnimationProp, AnimatorTypeProp, UseWeaponObjectProp, UseHitAnimationsProp, TargetObstructedActionProp, EnableDebuggingPop, DrawRaycastsEnabledProp, UseAIAvoidanceProp, AnimatorCullingModeProp,
            DebugLogTargetsEnabledProp, DebugLogObstructionsEnabledProp, UseRandomRotationOnStartProp, UseDeactivateDelayProp, DisableAIWhenNotInViewProp, SupportAbilityPickTypeProp, SummoningAbilityPickTypeProp,
            DeathTypeRefProp, UseDroppableWeaponProp, BackupTypeProp, AutoEnableAnimatePhysicsProp, DebugLogMissingAnimationsProp, UseCriticalHitsProp, OffensiveAbilityPickTypeProp, MeleeAttackPickTypeProp, MeleeRunAttackPickTypeProp;

        //strings
        SerializedProperty AINameProp, AITitleProp, AILevelProp, PlayerTagProp, FollowTagProp, UITagProp, NonAITagProp, EmeraldTagProp, RagdollTagProp, CameraTagProp;

        //objects
        SerializedProperty HealthBarImageProp, HealthBarBackgroundImageProp, Renderer1Prop, Renderer2Prop, Renderer3Prop,
            Renderer4Prop, RangedAttackTransformProp, SheatheWeaponProp, UnsheatheWeaponProp, RangedSheatheWeaponProp, RangedUnsheatheWeaponProp,
            WeaponObjectProp, AIRendererProp, DroppableWeaponObjectProp, BlockIdleAnimationProp, BlockHitAnimationProp, RangedWeaponObjectProp, RangedDroppableWeaponObjectProp;

        //vectors & Layer Masks
        SerializedProperty HealthBarPosProp, NameTextFontSizeProp, HealthBarScaleProp, BloodPosOffsetProp, AINamePosProp, AILevelPosProp, DetectionLayerMaskProp, ObstructionDetectionLayerMaskProp,
            AlignmentLayerMaskProp, AIAvoidanceLayerMaskProp, UILayerMaskProp, AINameUIOutlineSizeProp, AILevelUIOutlineSizeProp;

        //color
        SerializedProperty HealthBarColorProp, HealthBarBackgroundColorProp, NameTextColorProp, LevelTextColorProp, AINameUIOutlineColorProp, AILevelUIOutlineColorProp, AINameFontProp, AILevelFontProp;

        //bools
        SerializedProperty HealthBarsFoldoutProp, CombatTextFoldoutProp, NameTextFoldoutProp, AnimationsUpdatedProp, BehaviorFoldout, ConfidenceFoldout, WanderFoldout, CombatStyleFoldout,
            WaypointsFoldout, WalkFoldout, RunFoldout, TurnFoldout, CombatWalkFoldout, CombatRunFoldout, CombatTurnFoldout;

        //Animations
        SerializedProperty WalkLeftProp, WalkStraightProp, WalkRightProp, CombatWalkLeftProp, CombatWalkStraightProp, CombatWalkRightProp, WalkBackProp,
            RunLeftProp, RunStraightProp, RunRightProp, CombatRunLeftProp, CombatRunStraightProp, CombatRunRightProp, IdleWarningProp, IdleCombatProp, RangedIdleCombatProp, IdleNonCombatProp,
            TurnLeftProp, TurnRightProp, CombatTurnLeftProp, CombatTurnRightProp, RunAttackProp, PutAwayWeaponAnimationProp, PullOutWeaponAnimationProp, AnimationProfileProp;

        SerializedProperty RangedCombatWalkLeftProp, RangedCombatWalkStraightProp, RangedCombatWalkRightProp, RangedWalkBackProp, RangedCombatRunLeftProp, RangedCombatRunStraightProp, RangedCombatRunRightProp,
            RangedCombatTurnLeftProp, RangedCombatTurnRightProp, RangedRunAttackProp, RangedPutAwayWeaponAnimationProp, RangedPullOutWeaponAnimationProp, RangedIdleWarningProp;

        //Mirror Bools
        SerializedProperty MirrorWalkLeftProp, MirrorWalkRightProp, MirrorRunLeftProp, MirrorRunRightProp, MirrorCombatWalkLeftProp, MirrorCombatWalkRightProp, MirrorCombatRunLeftProp,
            MirrorCombatRunRightProp, MirrorCombatTurnLeftProp, MirrorCombatTurnRightProp, MirrorTurnLeftProp, MirrorTurnRightProp, ReverseWalkAnimationProp;

        //Ranged Mirror Bools
        SerializedProperty MirrorRangedCombatWalkLeftProp, MirrorRangedCombatWalkRightProp, MirrorRangedCombatRunLeftProp,
            MirrorRangedCombatRunRightProp, MirrorRangedCombatTurnLeftProp, MirrorRangedCombatTurnRightProp, ReverseRangedWalkAnimationProp;

        //Events
        SerializedProperty DeathEventProp, DamageEventProp, ReachedDestinationEventProp, OnStartEventProp, OnAttackEventProp, OnFleeEventProp, OnStartCombatEventProp, OnEnabledEventProp,
            OnPlayerDetectionEventProp, OnKillTargetEventProp, OnDoDamageEventProp, OnHealEventProp, OnCriticalHitEventProp;

        //Spell lists
        ReorderableList OffensiveAbilities, SupportAbilities, SummoningAbilities, BloodEffectsList;

        //Sound lists
        ReorderableList AttackSoundsList, InjuredSoundsList, WarningSoundsList, DeathSoundsList, FootStepSoundsList, IdleSoundsList, ImpactSoundsList, CriticalHitSoundsList, BlockSoundsList;

        //Animation lists
        ReorderableList HitAnimationList, CombatHitAnimationList, IdleAnimationList, AttackAnimationList, RunAttackAnimationList, DeathAnimationList, EmoteAnimationList, InteractSoundsList,
            ItemList, RangedCombatHitAnimationList, RangedAttackAnimationList, RangedRunAttackAnimationList, RangedDeathAnimationList;
        #endregion

        ReorderableList FactionsList, PlayerFaction, MeleeAttacks, MeleeRunAttacks;

        Texture TemperamentIcon, SettingsIcon, DetectTagsIcon, UIIcon, SoundIcon, WaypointEditorIcon, AnimationIcon, DocumentationIcon;
        string MeleeWeaponString, RangedWeaponString;

        void UpdateAbilityAnimationEnums()
        {
            EmeraldAISystem Ref = (EmeraldAISystem)target;

            if (Ref.WeaponTypeRef == EmeraldAISystem.WeaponType.Both || Ref.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
            {
                if (Ref.RangedAttackAnimationList.Count == 1 && Ref.RangedAttackAnimationList.Count == 1 && Ref.RangedAttackAnimationList[0].AnimationClip != null)
                {
                    RangedEnumAnimations = new string[] { Ref.RangedAttackAnimationList[0].AnimationClip.name };
                }
                else if (Ref.RangedAttackAnimationList.Count == 2 && Ref.RangedAttackAnimationList.Count == 2 && Ref.RangedAttackAnimationList[0].AnimationClip != null && Ref.RangedAttackAnimationList[1].AnimationClip != null)
                {
                    RangedEnumAnimations = new string[] { Ref.RangedAttackAnimationList[0].AnimationClip.name, Ref.RangedAttackAnimationList[1].AnimationClip.name };
                }
                else if (Ref.RangedAttackAnimationList.Count == 3 && Ref.RangedAttackAnimationList.Count == 3 && Ref.RangedAttackAnimationList[0].AnimationClip != null &&
                    Ref.RangedAttackAnimationList[1].AnimationClip != null && Ref.RangedAttackAnimationList[2].AnimationClip != null)
                {
                    RangedEnumAnimations = new string[] { Ref.RangedAttackAnimationList[0].AnimationClip.name, Ref.RangedAttackAnimationList[1].AnimationClip.name, Ref.RangedAttackAnimationList[2].AnimationClip.name };
                }
                else if (Ref.RangedAttackAnimationList.Count == 4 && Ref.RangedAttackAnimationList.Count == 4 && Ref.RangedAttackAnimationList[0].AnimationClip != null &&
                    Ref.RangedAttackAnimationList[1].AnimationClip != null && Ref.RangedAttackAnimationList[2].AnimationClip != null && Ref.RangedAttackAnimationList[3].AnimationClip != null)
                {
                    RangedEnumAnimations = new string[] { Ref.RangedAttackAnimationList[0].AnimationClip.name, Ref.RangedAttackAnimationList[1].AnimationClip.name, Ref.RangedAttackAnimationList[2].AnimationClip.name, Ref.RangedAttackAnimationList[3].AnimationClip.name };
                }
                else if (Ref.RangedAttackAnimationList.Count == 5 && Ref.RangedAttackAnimationList.Count == 5 && Ref.RangedAttackAnimationList[0].AnimationClip != null &&
                    Ref.RangedAttackAnimationList[1].AnimationClip != null && Ref.RangedAttackAnimationList[2].AnimationClip != null && Ref.RangedAttackAnimationList[3].AnimationClip != null && Ref.RangedAttackAnimationList[4].AnimationClip != null)
                {
                    RangedEnumAnimations = new string[] { Ref.RangedAttackAnimationList[0].AnimationClip.name, Ref.RangedAttackAnimationList[1].AnimationClip.name, Ref.RangedAttackAnimationList[2].AnimationClip.name, Ref.RangedAttackAnimationList[3].AnimationClip.name, Ref.RangedAttackAnimationList[4].AnimationClip.name };
                }
                else if (Ref.RangedAttackAnimationList.Count == 6 && Ref.RangedAttackAnimationList.Count == 6 && Ref.RangedAttackAnimationList[0].AnimationClip != null &&
                    Ref.RangedAttackAnimationList[1].AnimationClip != null && Ref.RangedAttackAnimationList[2].AnimationClip != null && Ref.RangedAttackAnimationList[3].AnimationClip != null && Ref.RangedAttackAnimationList[4].AnimationClip != null && Ref.RangedAttackAnimationList[5].AnimationClip != null)
                {
                    RangedEnumAnimations = new string[] { Ref.RangedAttackAnimationList[0].AnimationClip.name, Ref.RangedAttackAnimationList[1].AnimationClip.name, Ref.RangedAttackAnimationList[2].AnimationClip.name,
                        Ref.RangedAttackAnimationList[3].AnimationClip.name, Ref.RangedAttackAnimationList[4].AnimationClip.name, Ref.RangedAttackAnimationList[5].AnimationClip.name };
                }
                else
                {
                    RangedEnumAnimations = null;
                }
            }

            if (Ref.WeaponTypeRef == EmeraldAISystem.WeaponType.Both || Ref.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
            {
                if (Ref.AttackAnimationList.Count == 1 && Ref.AttackAnimationList.Count == 1 && Ref.AttackAnimationList[0].AnimationClip != null)
                {
                    MeleeEnumAnimations = new string[] { Ref.AttackAnimationList[0].AnimationClip.name };
                }
                else if (Ref.AttackAnimationList.Count == 2 && Ref.AttackAnimationList.Count == 2 && Ref.AttackAnimationList[0].AnimationClip != null && Ref.AttackAnimationList[1].AnimationClip != null)
                {
                    MeleeEnumAnimations = new string[] { Ref.AttackAnimationList[0].AnimationClip.name, Ref.AttackAnimationList[1].AnimationClip.name };
                }
                else if (Ref.AttackAnimationList.Count == 3 && Ref.AttackAnimationList.Count == 3 && Ref.AttackAnimationList[0].AnimationClip != null &&
                    Ref.AttackAnimationList[1].AnimationClip != null && Ref.AttackAnimationList[2].AnimationClip != null)
                {
                    MeleeEnumAnimations = new string[] { Ref.AttackAnimationList[0].AnimationClip.name, Ref.AttackAnimationList[1].AnimationClip.name, Ref.AttackAnimationList[2].AnimationClip.name };
                }
                else if (Ref.AttackAnimationList.Count == 4 && Ref.AttackAnimationList.Count == 4 && Ref.AttackAnimationList[0].AnimationClip != null &&
                    Ref.AttackAnimationList[1].AnimationClip != null && Ref.AttackAnimationList[2].AnimationClip != null && Ref.AttackAnimationList[3].AnimationClip != null)
                {
                    MeleeEnumAnimations = new string[] { Ref.AttackAnimationList[0].AnimationClip.name, Ref.AttackAnimationList[1].AnimationClip.name, Ref.AttackAnimationList[2].AnimationClip.name, Ref.AttackAnimationList[3].AnimationClip.name };
                }
                else if (Ref.AttackAnimationList.Count == 5 && Ref.AttackAnimationList.Count == 5 && Ref.AttackAnimationList[0].AnimationClip != null &&
                    Ref.AttackAnimationList[1].AnimationClip != null && Ref.AttackAnimationList[2].AnimationClip != null && Ref.AttackAnimationList[3].AnimationClip != null && Ref.AttackAnimationList[4].AnimationClip != null)
                {
                    MeleeEnumAnimations = new string[] { Ref.AttackAnimationList[0].AnimationClip.name, Ref.AttackAnimationList[1].AnimationClip.name, Ref.AttackAnimationList[2].AnimationClip.name, Ref.AttackAnimationList[3].AnimationClip.name, Ref.AttackAnimationList[4].AnimationClip.name };
                }
                else if (Ref.AttackAnimationList.Count == 6 && Ref.AttackAnimationList.Count == 6 && Ref.AttackAnimationList[0].AnimationClip != null &&
                    Ref.AttackAnimationList[1].AnimationClip != null && Ref.AttackAnimationList[2].AnimationClip != null && Ref.AttackAnimationList[3].AnimationClip != null && Ref.AttackAnimationList[4].AnimationClip != null && Ref.AttackAnimationList[5].AnimationClip != null)
                {
                    MeleeEnumAnimations = new string[] { Ref.AttackAnimationList[0].AnimationClip.name, Ref.AttackAnimationList[1].AnimationClip.name, Ref.AttackAnimationList[2].AnimationClip.name,
                        Ref.AttackAnimationList[3].AnimationClip.name, Ref.AttackAnimationList[4].AnimationClip.name, Ref.AttackAnimationList[5].AnimationClip.name };
                }
                else
                {
                    MeleeEnumAnimations = null;
                }
            }

            if (Ref.WeaponTypeRef == EmeraldAISystem.WeaponType.Both && Ref.UseRunAttacksRef == EmeraldAISystem.UseRunAttacks.Yes ||
                Ref.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && Ref.UseRunAttacksRef == EmeraldAISystem.UseRunAttacks.Yes)
            {
                if (Ref.RunAttackAnimationList.Count == 1 && Ref.RunAttackAnimationList.Count == 1 && Ref.RunAttackAnimationList[0].AnimationClip != null)
                {
                    RunMeleeEnumAnimations = new string[] { Ref.RunAttackAnimationList[0].AnimationClip.name };
                }
                else if (Ref.RunAttackAnimationList.Count == 2 && Ref.RunAttackAnimationList.Count == 2 && Ref.RunAttackAnimationList[0].AnimationClip != null && Ref.RunAttackAnimationList[1].AnimationClip != null)
                {
                    RunMeleeEnumAnimations = new string[] { Ref.RunAttackAnimationList[0].AnimationClip.name, Ref.RunAttackAnimationList[1].AnimationClip.name };
                }
                else if (Ref.RunAttackAnimationList.Count == 3 && Ref.RunAttackAnimationList.Count == 3 && Ref.RunAttackAnimationList[0].AnimationClip != null &&
                    Ref.RunAttackAnimationList[1].AnimationClip != null && Ref.RunAttackAnimationList[2].AnimationClip != null)
                {
                    RunMeleeEnumAnimations = new string[] { Ref.RunAttackAnimationList[0].AnimationClip.name, Ref.RunAttackAnimationList[1].AnimationClip.name, Ref.RunAttackAnimationList[2].AnimationClip.name };
                }
                else if (Ref.RunAttackAnimationList.Count == 4 && Ref.RunAttackAnimationList.Count == 4 && Ref.RunAttackAnimationList[0].AnimationClip != null &&
                    Ref.RunAttackAnimationList[1].AnimationClip != null && Ref.RunAttackAnimationList[2].AnimationClip != null && Ref.RunAttackAnimationList[3].AnimationClip != null)
                {
                    RunMeleeEnumAnimations = new string[] { Ref.RunAttackAnimationList[0].AnimationClip.name, Ref.RunAttackAnimationList[1].AnimationClip.name, Ref.RunAttackAnimationList[2].AnimationClip.name, Ref.RunAttackAnimationList[3].AnimationClip.name };
                }
                else if (Ref.RunAttackAnimationList.Count == 5 && Ref.RunAttackAnimationList.Count == 5 && Ref.RunAttackAnimationList[0].AnimationClip != null &&
                    Ref.RunAttackAnimationList[1].AnimationClip != null && Ref.RunAttackAnimationList[2].AnimationClip != null && Ref.RunAttackAnimationList[3].AnimationClip != null && Ref.RunAttackAnimationList[4].AnimationClip != null)
                {
                    RunMeleeEnumAnimations = new string[] { Ref.RunAttackAnimationList[0].AnimationClip.name, Ref.RunAttackAnimationList[1].AnimationClip.name, Ref.RunAttackAnimationList[2].AnimationClip.name, Ref.RunAttackAnimationList[3].AnimationClip.name, Ref.RunAttackAnimationList[4].AnimationClip.name };
                }
                else if (Ref.RunAttackAnimationList.Count == 6 && Ref.RunAttackAnimationList.Count == 6 && Ref.RunAttackAnimationList[0].AnimationClip != null &&
                    Ref.RunAttackAnimationList[1].AnimationClip != null && Ref.RunAttackAnimationList[2].AnimationClip != null && Ref.RunAttackAnimationList[3].AnimationClip != null && Ref.RunAttackAnimationList[4].AnimationClip != null && Ref.RunAttackAnimationList[5].AnimationClip != null)
                {
                    RunMeleeEnumAnimations = new string[] { Ref.RunAttackAnimationList[0].AnimationClip.name, Ref.RunAttackAnimationList[1].AnimationClip.name, Ref.RunAttackAnimationList[2].AnimationClip.name,
                        Ref.RunAttackAnimationList[3].AnimationClip.name, Ref.RunAttackAnimationList[4].AnimationClip.name, Ref.RunAttackAnimationList[5].AnimationClip.name };
                }
                else
                {
                    RunMeleeEnumAnimations = null;
                }
            }
        }

        void OnEnable()
        {
            EmeraldAISystem self = (EmeraldAISystem)target;
            LoadFactionData();

            if (self.m_ProjectileCollisionPoint == Vector3.zero)
            {
                self.m_ProjectileCollisionPoint = self.transform.position;
            }

            UpdateAbilityAnimationEnums();

            if (self.PlayerFaction.Count == 0)
            {
                self.PlayerFaction.Add(new EmeraldAISystem.PlayerFactionClass(self.PlayerTag, 0));
            }

            #region Serialized Properties
            //Head Look
            HeadLookWeightCombatProp = serializedObject.FindProperty("HeadLookWeightCombat");
            BodyLookWeightCombatProp = serializedObject.FindProperty("BodyLookWeightCombat");
            HeadLookWeightNonCombatProp = serializedObject.FindProperty("HeadLookWeightNonCombat");
            BodyLookWeightNonCombatProp = serializedObject.FindProperty("BodyLookWeightNonCombat");
            MaxLookAtDistanceProp = serializedObject.FindProperty("MaxLookAtDistance");
            HeadLookYOffsetProp = serializedObject.FindProperty("HeadLookYOffset");
            UseHeadLookProp = serializedObject.FindProperty("UseHeadLookRef");
            LookSmootherProp = serializedObject.FindProperty("LookSmoother");
            NonCombatLookAtLimitProp = serializedObject.FindProperty("NonCombatLookAtLimit");
            CombatLookAtLimitProp = serializedObject.FindProperty("CombatLookAtLimit");
            HeadTransformProp = serializedObject.FindProperty("HeadTransform");

            //System
            AnimationListsChangedProp = serializedObject.FindProperty("AnimationListsChanged");

            //ints
            TabNumberProp = serializedObject.FindProperty("TabNumber");
            TemperamentTabNumberProp = serializedObject.FindProperty("TemperamentTabNumber");
            DetectionTagsTabNumberProp = serializedObject.FindProperty("DetectionTagsTabNumber");
            AnimationTabNumberProp = serializedObject.FindProperty("AnimationTabNumber");
            SoundTabNumberProp = serializedObject.FindProperty("SoundTabNumber");
            MovementTabNumberProp = serializedObject.FindProperty("MovementTabNumber");
            WeaponTypeTabNumberProp = serializedObject.FindProperty("WeaponTypeTabNumber");
            WeaponTypeCombatTabNumberProp = serializedObject.FindProperty("WeaponTypeCombatTabNumber");
            WeaponTypeControlTabNumberProp = serializedObject.FindProperty("WeaponTypeControlTabNumber");
            CombatTabNumberProp = serializedObject.FindProperty("CombatTabNumber");
            FactionsAndTagTabNumberProp = serializedObject.FindProperty("FactionsAndTagTabNumber");
            EventTabNumberProp = serializedObject.FindProperty("EventTabNumber");
            fieldOfViewAngleProp = serializedObject.FindProperty("fieldOfViewAngle");
            DetectionRadiusProp = serializedObject.FindProperty("DetectionRadius");
            WanderRadiusProp = serializedObject.FindProperty("WanderRadius");
            MinimumWaitTimeProp = serializedObject.FindProperty("MinimumWaitTime");
            MaximumWaitTimeProp = serializedObject.FindProperty("MaximumWaitTime");
            WalkSpeedProp = serializedObject.FindProperty("WalkSpeed");
            WalkBackwardsSpeedProp = serializedObject.FindProperty("WalkBackwardsSpeed");
            RunSpeedProp = serializedObject.FindProperty("RunSpeed");
            StartingHealthProp = serializedObject.FindProperty("StartingHealth");
            StationaryTurningSpeedCombatProp = serializedObject.FindProperty("StationaryTurningSpeedCombat");
            BackupTurningSpeedProp = serializedObject.FindProperty("BackupTurningSpeed");
            CritChanceProp = serializedObject.FindProperty("CritChance");
            CritMultiplierProp = serializedObject.FindProperty("CritMultiplier");
            MinimumFollowWaitTimeProp = serializedObject.FindProperty("MinimumFollowWaitTime");
            MaximumFollowWaitTimeProp = serializedObject.FindProperty("MaximumFollowWaitTime");
            MaxChaseDistanceProp = serializedObject.FindProperty("MaxChaseDistance");
            HealthPercentageToFleeProp = serializedObject.FindProperty("HealthPercentageToFlee");
            DeactivateDelayProp = serializedObject.FindProperty("DeactivateDelay");
            RegenerateAmountProp = serializedObject.FindProperty("RegenerateAmount");
            DeathDelayMinProp = serializedObject.FindProperty("DeathDelayMin");
            DeathDelayMaxProp = serializedObject.FindProperty("DeathDelayMax");
            CurrentFactionProp = serializedObject.FindProperty("CurrentFaction");
            StationaryIdleSecondsMinProp = serializedObject.FindProperty("StationaryIdleSecondsMin");
            StationaryIdleSecondsMaxProp = serializedObject.FindProperty("StationaryIdleSecondsMax");
            IdleSoundsSecondsMinProp = serializedObject.FindProperty("IdleSoundsSecondsMin");
            IdleSoundsSecondsMaxProp = serializedObject.FindProperty("IdleSoundsSecondsMax");
            SentRagdollForceProp = serializedObject.FindProperty("SentRagdollForceAmount");
            ObstructionSecondsProp = serializedObject.FindProperty("ObstructionSeconds");
            TotalAggroHitsProp = serializedObject.FindProperty("TotalAggroHits");
            MaxSlopeLimitProp = serializedObject.FindProperty("MaxSlopeLimit");
            SecondsToDisableProp = serializedObject.FindProperty("SecondsToDisable");
            BackUpOddsProp = serializedObject.FindProperty("BackupOdds");
            BackupSecondsMinProp = serializedObject.FindProperty("BackingUpSecondsMin");
            BackupSecondsMaxProp = serializedObject.FindProperty("BackingUpSecondsMax");
            CautiousSecondsProp = serializedObject.FindProperty("CautiousSeconds");
            MaxAllowedSummonedAIProp = serializedObject.FindProperty("MaxAllowedSummonedAI");
            HealthPercentageToHealProp = serializedObject.FindProperty("HealthPercentageToHeal");
            PlayerDetectionEventCooldownProp = serializedObject.FindProperty("PlayerDetectionEventCooldown");
            SwitchWeaponTypesDistanceProp = serializedObject.FindProperty("SwitchWeaponTypesDistance");
            SwitchWeaponTypesCooldownProp = serializedObject.FindProperty("SwitchWeaponTypesCooldown");
            MaxUIScaleSizeProp = serializedObject.FindProperty("MaxUIScaleSize");

            //floats
            MinimumAttackSpeedProp = serializedObject.FindProperty("MinimumAttackSpeed");
            MaximumAttackSpeedProp = serializedObject.FindProperty("MaximumAttackSpeed");
            MinimumRunAttackSpeedProp = serializedObject.FindProperty("MinimumRunAttackSpeed");
            MaximumRunAttackSpeedProp = serializedObject.FindProperty("MaximumRunAttackSpeed");
            StoppingDistanceProp = serializedObject.FindProperty("StoppingDistance");
            FollowingStoppingDistanceProp = serializedObject.FindProperty("FollowingStoppingDistance");
            RunAttackDistanceProp = serializedObject.FindProperty("RunAttackDistance");
            AgentRadiusProp = serializedObject.FindProperty("AgentRadius");
            AgentBaseOffsetProp = serializedObject.FindProperty("AgentBaseOffset");
            AgentTurnSpeedProp = serializedObject.FindProperty("AgentTurnSpeed");
            AgentAccelerationProp = serializedObject.FindProperty("AgentAcceleration");
            MaxNormalAngleProp = serializedObject.FindProperty("MaxNormalAngleEditor");
            NonCombatAlignSpeedProp = serializedObject.FindProperty("NonCombatAlignmentSpeed");
            CombatAlignSpeedProp = serializedObject.FindProperty("CombatAlignmentSpeed");
            IdleWarningAnimationSpeedProp = serializedObject.FindProperty("IdleWarningAnimationSpeed");
            RangedIdleWarningAnimationSpeedProp = serializedObject.FindProperty("RangedIdleWarningAnimationSpeed");
            RangedCombatTurnLeftAnimationSpeedProp = serializedObject.FindProperty("RangedCombatTurnLeftAnimationSpeed");
            RangedCombatTurnRightAnimationSpeedProp = serializedObject.FindProperty("RangedCombatTurnRightAnimationSpeed");
            RangedCombatWalkAnimationSpeedProp = serializedObject.FindProperty("RangedCombatWalkAnimationSpeed");
            RangedCombatRunAnimationSpeedProp = serializedObject.FindProperty("RangedCombatRunAnimationSpeed");
            IdleCombatAnimationSpeedProp = serializedObject.FindProperty("IdleCombatAnimationSpeed");
            RangedIdleCombatAnimationSpeedProp = serializedObject.FindProperty("RangedIdleCombatAnimationSpeed");
            IdleNonCombatAnimationSpeedProp = serializedObject.FindProperty("IdleNonCombatAnimationSpeed");
            NonCombatRunAnimationSpeedProp = serializedObject.FindProperty("NonCombatRunAnimationSpeed");
            CombatRunAnimationSpeedProp = serializedObject.FindProperty("CombatRunAnimationSpeed");
            NonCombatWalkAnimationSpeedProp = serializedObject.FindProperty("NonCombatWalkAnimationSpeed");
            CombatWalkAnimationSpeedProp = serializedObject.FindProperty("CombatWalkAnimationSpeed");
            TurnLeftAnimationSpeedProp = serializedObject.FindProperty("TurnLeftAnimationSpeed");
            TurnRightAnimationSpeedProp = serializedObject.FindProperty("TurnRightAnimationSpeed");
            CombatTurnLeftAnimationSpeedProp = serializedObject.FindProperty("CombatTurnLeftAnimationSpeed");
            CombatTurnRightAnimationSpeedProp = serializedObject.FindProperty("CombatTurnRightAnimationSpeed");
            CombatAngleToTurnProp = serializedObject.FindProperty("CombatAngleToTurn");
            NonCombatAngleToTurnProp = serializedObject.FindProperty("NonCombatAngleToTurn");
            AttackDistanceProp = serializedObject.FindProperty("AttackDistance");
            RangedAttackDistanceProp = serializedObject.FindProperty("RangedAttackDistance");
            RangedTooCloseDistanceProp = serializedObject.FindProperty("RangedTooCloseDistance");
            HealthRegRateProp = serializedObject.FindProperty("HealthRegRate");
            BloodEffectTimeoutSecondsProp = serializedObject.FindProperty("BloodEffectTimeoutSeconds");
            TooCloseDistanceProp = serializedObject.FindProperty("TooCloseDistance");
            PlayerYOffsetProp = serializedObject.FindProperty("PlayerYOffset");
            ExpandedFieldOfViewAngleProp = serializedObject.FindProperty("ExpandedFieldOfViewAngle");
            ExpandedDetectionRadiusProp = serializedObject.FindProperty("ExpandedDetectionRadius");
            ExpandedChaseDistanceProp = serializedObject.FindProperty("ExpandedChaseDistance");
            MitigationAmountProp = serializedObject.FindProperty("MitigationAmount");
            ProjectileCollisionPointYProp = serializedObject.FindProperty("ProjectileCollisionPointY");
            MaxDamageAngleProp = serializedObject.FindProperty("MaxDamageAngle");
            MaxFiringAngleProp = serializedObject.FindProperty("MaxFiringAngle");
            WalkFootstepVolumeProp = serializedObject.FindProperty("WalkFootstepVolume");
            RunFootstepVolumeProp = serializedObject.FindProperty("RunFootstepVolume");
            BlockVolumeProp = serializedObject.FindProperty("BlockVolume");
            InjuredVolumeProp = serializedObject.FindProperty("InjuredVolume");
            CriticalHitVolumeProp = serializedObject.FindProperty("CriticalHitVolume");
            ImpactVolumeProp = serializedObject.FindProperty("ImpactVolume");
            AttackVolumeProp = serializedObject.FindProperty("AttackVolume");
            DeathVolumeProp = serializedObject.FindProperty("DeathVolume");
            EquipVolumeProp = serializedObject.FindProperty("EquipVolume");
            UnequipVolumeProp = serializedObject.FindProperty("UnequipVolume");
            RangedEquipVolumeProp = serializedObject.FindProperty("RangedEquipVolume");
            RangedUnequipVolumeProp = serializedObject.FindProperty("RangedUnequipVolume");
            IdleVolumeProp = serializedObject.FindProperty("IdleVolume");
            WarningVolumeProp = serializedObject.FindProperty("WarningVolume");
            DetectionFrequencyProp = serializedObject.FindProperty("DetectionFrequency");
            YAimOffsetProp = serializedObject.FindProperty("YAimOffset");
            AttackHeightProp = serializedObject.FindProperty("AttackHeight");
            AINameLineSpacingProp = serializedObject.FindProperty("AINameLineSpacing");

            //enums
            BehaviorProp = serializedObject.FindProperty("BehaviorRef");
            ConfidenceProp = serializedObject.FindProperty("ConfidenceRef");
            RandomizeDamageProp = serializedObject.FindProperty("RandomizeDamageRef");
            DetectionTypeProp = serializedObject.FindProperty("DetectionTypeRef");
            MaxChaseDistanceTypeProp = serializedObject.FindProperty("MaxChaseDistanceTypeRef");
            CombatTypeProp = serializedObject.FindProperty("CombatTypeRef");
            CreateHealthBarsProp = serializedObject.FindProperty("CreateHealthBarsRef");
            CustomizeHealthBarProp = serializedObject.FindProperty("CustomizeHealthBarRef");
            DisplayAINameProp = serializedObject.FindProperty("DisplayAINameRef");
            DisplayAITitleProp = serializedObject.FindProperty("DisplayAITitleRef");
            DisplayAILevelProp = serializedObject.FindProperty("DisplayAILevelRef");
            RefillHealthTypeProp = serializedObject.FindProperty("RefillHealthType");
            WanderTypeProp = serializedObject.FindProperty("WanderTypeRef");
            WaypointTypeProp = serializedObject.FindProperty("WaypointTypeRef");
            AlignAIWithGroundProp = serializedObject.FindProperty("AlignAIWithGroundRef");
            CurrentMovementStateProp = serializedObject.FindProperty("CurrentMovementState");
            UseBloodEffectProp = serializedObject.FindProperty("UseBloodEffectRef");
            UseWarningAnimationProp = serializedObject.FindProperty("UseWarningAnimationRef");
            TotalLODsProp = serializedObject.FindProperty("TotalLODsRef");
            HasMultipleLODsProp = serializedObject.FindProperty("HasMultipleLODsRef");
            AlignAIOnStartProp = serializedObject.FindProperty("AlignAIOnStartRef");
            AlignmentQualityProp = serializedObject.FindProperty("AlignmentQualityRef");
            PickTargetMethodProp = serializedObject.FindProperty("PickTargetMethodRef");
            UseNonAITagProp = serializedObject.FindProperty("UseNonAITagRef");
            WeaponTypeProp = serializedObject.FindProperty("WeaponTypeRef");
            UseRunAttacksProp = serializedObject.FindProperty("UseRunAttacksRef");
            ObstructionDetectionQualityProp = serializedObject.FindProperty("ObstructionDetectionQualityRef");
            AvoidanceQualityProp = serializedObject.FindProperty("AvoidanceQualityRef");
            UseBlockingProp = serializedObject.FindProperty("UseBlockingRef");
            BlockingOddsProp = serializedObject.FindProperty("BlockOdds");
            MaxBlockingAngleProp = serializedObject.FindProperty("MaxBlockAngle");
            UseEquipAnimationProp = serializedObject.FindProperty("UseEquipAnimation");
            AnimatorTypeProp = serializedObject.FindProperty("AnimatorType");
            UseWeaponObjectProp = serializedObject.FindProperty("UseWeaponObject");
            UseHitAnimationsProp = serializedObject.FindProperty("UseHitAnimations");
            TargetObstructedActionProp = serializedObject.FindProperty("TargetObstructedActionRef");
            BloodEffectPositionTypeProp = serializedObject.FindProperty("BloodEffectPositionTypeRef");
            AggroActionProp = serializedObject.FindProperty("AggroActionRef");
            UseAggroProp = serializedObject.FindProperty("UseAggro");
            UseAIAvoidanceProp = serializedObject.FindProperty("UseAIAvoidance");
            AnimatorCullingModeProp = serializedObject.FindProperty("AnimatorCullingMode");
            EnableDebuggingPop = serializedObject.FindProperty("EnableDebugging");
            DrawRaycastsEnabledProp = serializedObject.FindProperty("DrawRaycastsEnabled");
            DebugLogTargetsEnabledProp = serializedObject.FindProperty("DebugLogTargetsEnabled");
            DebugLogObstructionsEnabledProp = serializedObject.FindProperty("DebugLogObstructionsEnabled");
            DebugLogProjectileCollisionsEnabledProp = serializedObject.FindProperty("DebugLogProjectileCollisionsEnabled");
            BackupTypeProp = serializedObject.FindProperty("BackupTypeRef");
            AutoEnableAnimatePhysicsProp = serializedObject.FindProperty("AutoEnableAnimatePhysics");
            SummonsMultipleAIProp = serializedObject.FindProperty("SummonsMultipleAI");
            TurnAnimationTypeProp = serializedObject.FindProperty("TurnAnimationTypeRef");
            DebugLogMissingAnimationsProp = serializedObject.FindProperty("DebugLogMissingAnimations");
            UseCriticalHitsProp = serializedObject.FindProperty("UseCriticalHits");
            AttackOnArrivalProp = serializedObject.FindProperty("AttackOnArrival");
            OffensiveAbilityPickTypeProp = serializedObject.FindProperty("OffensiveAbilityPickType");
            SupportAbilityPickTypeProp = serializedObject.FindProperty("SupportAbilityPickType");
            SummoningAbilityPickTypeProp = serializedObject.FindProperty("SummoningAbilityPickType");
            MeleeAttackPickTypeProp = serializedObject.FindProperty("MeleeAttackPickType");
            MeleeRunAttackPickTypeProp = serializedObject.FindProperty("MeleeRunAttackPickType");
            UseAINameUIOutlineEffectProp = serializedObject.FindProperty("UseAINameUIOutlineEffect");
            UseAILevelUIOutlineEffectProp = serializedObject.FindProperty("UseAILevelUIOutlineEffect");
            UseCustomFontAINameProp = serializedObject.FindProperty("UseCustomFontAIName");
            UseCustomFontAILevelProp = serializedObject.FindProperty("UseCustomFontAILevel");

#if CRUX_PRESENT
            SpawnedWithCruxProp = serializedObject.FindProperty("UseMagicEffectsPackRef");
#endif

            UseRandomRotationOnStartProp = serializedObject.FindProperty("UseRandomRotationOnStartRef");
            UseDeactivateDelayProp = serializedObject.FindProperty("UseDeactivateDelayRef");
            DisableAIWhenNotInViewProp = serializedObject.FindProperty("DisableAIWhenNotInViewRef");
            DeathTypeRefProp = serializedObject.FindProperty("DeathTypeRef");
            UseDroppableWeaponProp = serializedObject.FindProperty("UseDroppableWeapon");

            //string
            AINameProp = serializedObject.FindProperty("AIName");
            AITitleProp = serializedObject.FindProperty("AITitle");
            AILevelProp = serializedObject.FindProperty("AILevel");
            PlayerTagProp = serializedObject.FindProperty("PlayerTag");
            FollowTagProp = serializedObject.FindProperty("FollowTag");
            UITagProp = serializedObject.FindProperty("UITag");
            EmeraldTagProp = serializedObject.FindProperty("EmeraldTag");
            NonAITagProp = serializedObject.FindProperty("NonAITag");
            RagdollTagProp = serializedObject.FindProperty("RagdollTag");
            CameraTagProp = serializedObject.FindProperty("CameraTag");

            //objects
            HealthBarImageProp = serializedObject.FindProperty("HealthBarImage");
            HealthBarBackgroundImageProp = serializedObject.FindProperty("HealthBarBackgroundImage");
            RangedAttackTransformProp = serializedObject.FindProperty("RangedAttackTransform");
            SheatheWeaponProp = serializedObject.FindProperty("SheatheWeapon");
            UnsheatheWeaponProp = serializedObject.FindProperty("UnsheatheWeapon");
            RangedSheatheWeaponProp = serializedObject.FindProperty("RangedSheatheWeapon");
            RangedUnsheatheWeaponProp = serializedObject.FindProperty("RangedUnsheatheWeapon");
            WeaponObjectProp = serializedObject.FindProperty("WeaponObject");
            RangedWeaponObjectProp = serializedObject.FindProperty("RangedWeaponObject");
            AIRendererProp = serializedObject.FindProperty("AIRenderer");
            DroppableWeaponObjectProp = serializedObject.FindProperty("DroppableWeaponObject");
            RangedDroppableWeaponObjectProp = serializedObject.FindProperty("RangedDroppableWeaponObject");
            BlockIdleAnimationProp = serializedObject.FindProperty("BlockIdleAnimation");
            BlockHitAnimationProp = serializedObject.FindProperty("BlockHitAnimation");

            //vector
            HealthBarPosProp = serializedObject.FindProperty("HealthBarPos");
            NameTextFontSizeProp = serializedObject.FindProperty("NameTextFontSize");
            HealthBarScaleProp = serializedObject.FindProperty("HealthBarScale");
            BloodPosOffsetProp = serializedObject.FindProperty("BloodPosOffset");
            AINamePosProp = serializedObject.FindProperty("AINamePos");
            AINameUIOutlineSizeProp = serializedObject.FindProperty("AINameUIOutlineSize");
            AILevelPosProp = serializedObject.FindProperty("AILevelPos");
            AILevelUIOutlineSizeProp = serializedObject.FindProperty("AILevelUIOutlineSize");

            //color
            HealthBarColorProp = serializedObject.FindProperty("HealthBarColor");
            HealthBarBackgroundColorProp = serializedObject.FindProperty("HealthBarBackgroundColor");
            NameTextColorProp = serializedObject.FindProperty("NameTextColor");
            LevelTextColorProp = serializedObject.FindProperty("LevelTextColor");
            AINameUIOutlineColorProp = serializedObject.FindProperty("AINameUIOutlineColor");
            AILevelUIOutlineColorProp = serializedObject.FindProperty("AILevelUIOutlineColor");
            AINameFontProp = serializedObject.FindProperty("AINameFont");
            AILevelFontProp = serializedObject.FindProperty("AILevelFont");

            //bool
            HealthBarsFoldoutProp = serializedObject.FindProperty("HealthBarsFoldout");
            CombatTextFoldoutProp = serializedObject.FindProperty("CombatTextFoldout");
            NameTextFoldoutProp = serializedObject.FindProperty("NameTextFoldout");
            AnimationsUpdatedProp = serializedObject.FindProperty("AnimationsUpdated");
            WaypointsFoldout = serializedObject.FindProperty("WaypointsFoldout");
            WalkFoldout = serializedObject.FindProperty("WalkFoldout");
            RunFoldout = serializedObject.FindProperty("RunFoldout");
            TurnFoldout = serializedObject.FindProperty("TurnFoldout");
            CombatWalkFoldout = serializedObject.FindProperty("CombatWalkFoldout");
            CombatRunFoldout = serializedObject.FindProperty("CombatRunFoldout");
            CombatTurnFoldout = serializedObject.FindProperty("CombatTurnFoldout");

            //Mirror Bools
            MirrorWalkLeftProp = serializedObject.FindProperty("MirrorWalkLeft");
            MirrorWalkRightProp = serializedObject.FindProperty("MirrorWalkRight");
            MirrorRunLeftProp = serializedObject.FindProperty("MirrorRunLeft");
            MirrorRunRightProp = serializedObject.FindProperty("MirrorRunRight");
            MirrorCombatWalkLeftProp = serializedObject.FindProperty("MirrorCombatWalkLeft");
            MirrorCombatWalkRightProp = serializedObject.FindProperty("MirrorCombatWalkRight");
            MirrorCombatRunLeftProp = serializedObject.FindProperty("MirrorCombatRunLeft");
            MirrorCombatRunRightProp = serializedObject.FindProperty("MirrorCombatRunRight");
            MirrorCombatTurnLeftProp = serializedObject.FindProperty("MirrorCombatTurnLeft");
            MirrorCombatTurnRightProp = serializedObject.FindProperty("MirrorCombatTurnRight");
            MirrorRangedCombatWalkLeftProp = serializedObject.FindProperty("MirrorRangedCombatWalkLeft");
            MirrorRangedCombatWalkRightProp = serializedObject.FindProperty("MirrorRangedCombatWalkRight");
            MirrorRangedCombatRunLeftProp = serializedObject.FindProperty("MirrorRangedCombatRunLeft");
            MirrorRangedCombatRunRightProp = serializedObject.FindProperty("MirrorRangedCombatRunRight");
            MirrorRangedCombatTurnLeftProp = serializedObject.FindProperty("MirrorRangedCombatTurnLeft");
            MirrorRangedCombatTurnRightProp = serializedObject.FindProperty("MirrorRangedCombatTurnRight");
            MirrorTurnLeftProp = serializedObject.FindProperty("MirrorTurnLeft");
            MirrorTurnRightProp = serializedObject.FindProperty("MirrorTurnRight");
            ReverseWalkAnimationProp = serializedObject.FindProperty("ReverseWalkAnimation");
            ReverseRangedWalkAnimationProp = serializedObject.FindProperty("ReverseRangedWalkAnimation");

            BehaviorFoldout = serializedObject.FindProperty("BehaviorFoldout");
            ConfidenceFoldout = serializedObject.FindProperty("ConfidenceFoldout");
            WanderFoldout = serializedObject.FindProperty("WanderFoldout");
            CombatStyleFoldout = serializedObject.FindProperty("CombatStyleFoldout");

            Renderer1Prop = serializedObject.FindProperty("Renderer1");
            Renderer2Prop = serializedObject.FindProperty("Renderer2");
            Renderer3Prop = serializedObject.FindProperty("Renderer3");
            Renderer4Prop = serializedObject.FindProperty("Renderer4");

            //Animations
            WalkLeftProp = serializedObject.FindProperty("WalkLeftAnimation");
            WalkStraightProp = serializedObject.FindProperty("WalkStraightAnimation");
            WalkRightProp = serializedObject.FindProperty("WalkRightAnimation");
            CombatWalkLeftProp = serializedObject.FindProperty("CombatWalkLeftAnimation");
            CombatWalkStraightProp = serializedObject.FindProperty("CombatWalkStraightAnimation");
            CombatWalkRightProp = serializedObject.FindProperty("CombatWalkRightAnimation");
            WalkBackProp = serializedObject.FindProperty("CombatWalkBackAnimation");
            RunLeftProp = serializedObject.FindProperty("RunLeftAnimation");
            RunStraightProp = serializedObject.FindProperty("RunStraightAnimation");
            RunRightProp = serializedObject.FindProperty("RunRightAnimation");
            CombatRunLeftProp = serializedObject.FindProperty("CombatRunLeftAnimation");
            CombatRunStraightProp = serializedObject.FindProperty("CombatRunStraightAnimation");
            CombatRunRightProp = serializedObject.FindProperty("CombatRunRightAnimation");
            IdleWarningProp = serializedObject.FindProperty("IdleWarningAnimation");
            IdleCombatProp = serializedObject.FindProperty("CombatIdleAnimation");
            RangedIdleCombatProp = serializedObject.FindProperty("RangedCombatIdleAnimation");
            IdleNonCombatProp = serializedObject.FindProperty("NonCombatIdleAnimation");
            TurnLeftProp = serializedObject.FindProperty("NonCombatTurnLeftAnimation");
            TurnRightProp = serializedObject.FindProperty("NonCombatTurnRightAnimation");
            CombatTurnLeftProp = serializedObject.FindProperty("CombatTurnLeftAnimation");
            CombatTurnRightProp = serializedObject.FindProperty("CombatTurnRightAnimation");
            PutAwayWeaponAnimationProp = serializedObject.FindProperty("PutAwayWeaponAnimation");
            PullOutWeaponAnimationProp = serializedObject.FindProperty("PullOutWeaponAnimation");
            AnimationProfileProp = serializedObject.FindProperty("m_AnimationProfile");

            RangedCombatWalkLeftProp = serializedObject.FindProperty("RangedCombatWalkLeftAnimation");
            RangedCombatWalkStraightProp = serializedObject.FindProperty("RangedCombatWalkStraightAnimation");
            RangedCombatWalkRightProp = serializedObject.FindProperty("RangedCombatWalkRightAnimation");
            RangedWalkBackProp = serializedObject.FindProperty("RangedCombatWalkBackAnimation");
            RangedCombatRunLeftProp = serializedObject.FindProperty("RangedCombatRunLeftAnimation");
            RangedCombatRunStraightProp = serializedObject.FindProperty("RangedCombatRunStraightAnimation");
            RangedCombatRunRightProp = serializedObject.FindProperty("RangedCombatRunRightAnimation");
            RangedIdleWarningProp = serializedObject.FindProperty("RangedIdleWarningAnimation");
            RangedCombatTurnLeftProp = serializedObject.FindProperty("RangedCombatTurnLeftAnimation");
            RangedCombatTurnRightProp = serializedObject.FindProperty("RangedCombatTurnRightAnimation");
            RangedPutAwayWeaponAnimationProp = serializedObject.FindProperty("RangedPutAwayWeaponAnimation");
            RangedPullOutWeaponAnimationProp = serializedObject.FindProperty("RangedPullOutWeaponAnimation");

            //Layer Masks
            DetectionLayerMaskProp = serializedObject.FindProperty("DetectionLayerMask");
            ObstructionDetectionLayerMaskProp = serializedObject.FindProperty("ObstructionDetectionLayerMask");
            AlignmentLayerMaskProp = serializedObject.FindProperty("AlignmentLayerMask");
            AIAvoidanceLayerMaskProp = serializedObject.FindProperty("AIAvoidanceLayerMask");
            UILayerMaskProp = serializedObject.FindProperty("UILayerMask");

            //Events
            DeathEventProp = serializedObject.FindProperty("DeathEvent");
            DamageEventProp = serializedObject.FindProperty("DamageEvent");
            OnDoDamageEventProp = serializedObject.FindProperty("OnDoDamageEvent");
            ReachedDestinationEventProp = serializedObject.FindProperty("ReachedDestinationEvent");
            OnStartEventProp = serializedObject.FindProperty("OnStartEvent");
            OnPlayerDetectionEventProp = serializedObject.FindProperty("OnPlayerDetectionEvent");
            OnEnabledEventProp = serializedObject.FindProperty("OnEnabledEvent");
            OnAttackEventProp = serializedObject.FindProperty("OnAttackEvent");
            OnFleeEventProp = serializedObject.FindProperty("OnFleeEvent");
            OnStartCombatEventProp = serializedObject.FindProperty("OnStartCombatEvent");
            OnKillTargetEventProp = serializedObject.FindProperty("OnKillTargetEvent");
            OnHealEventProp = serializedObject.FindProperty("OnHealEvent");
            OnCriticalHitEventProp = serializedObject.FindProperty("OnCriticalHitEvent");
            #endregion

            #region Editor Icons
            if (TemperamentIcon == null) TemperamentIcon = Resources.Load("TemperamentIcon") as Texture;
            if (SettingsIcon == null) SettingsIcon = Resources.Load("SettingsIcon") as Texture;
            if (DetectTagsIcon == null) DetectTagsIcon = Resources.Load("DetectTagsIcon") as Texture;
            if (UIIcon == null) UIIcon = Resources.Load("UIIcon") as Texture;
            if (SoundIcon == null) SoundIcon = Resources.Load("SoundIcon") as Texture;
            if (WaypointEditorIcon == null) WaypointEditorIcon = Resources.Load("WaypointEditorIcon") as Texture;
            if (AnimationIcon == null) AnimationIcon = Resources.Load("AnimationIcon") as Texture;
            if (DocumentationIcon == null) DocumentationIcon = Resources.Load("DocumentationIcon") as Texture;
            #endregion

            #region Reorderable Lists
            //Put our sound lists into reorderable lists because Unity allows these lists to be serialized
            //Attack Sounds
            AttackSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("AttackSounds"), true, true, true, true);
            AttackSoundsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Attack Sounds List", EditorStyles.boldLabel);
            };
            AttackSoundsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = AttackSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };

            //Injured Sounds
            InjuredSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("InjuredSounds"), true, true, true, true);
            InjuredSoundsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Injured Sounds List", EditorStyles.boldLabel);
            };
            InjuredSoundsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = InjuredSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };

            //Warning Sounds
            WarningSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("WarningSounds"), true, true, true, true);
            WarningSoundsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Warning Sounds List", EditorStyles.boldLabel);
            };
            WarningSoundsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = WarningSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };

            //Death Sounds
            DeathSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("DeathSounds"), true, true, true, true);
            DeathSoundsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Death Sounds List", EditorStyles.boldLabel);
            };
            DeathSoundsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = DeathSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };

            //Footstep Sounds
            FootStepSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("FootStepSounds"), true, true, true, true);
            FootStepSoundsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "FootStep Sounds List", EditorStyles.boldLabel);
            };
            FootStepSoundsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = FootStepSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };

            //Idle Sounds
            IdleSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("IdleSounds"), true, true, true, true);
            IdleSoundsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Idle Sounds List", EditorStyles.boldLabel);
            };
            IdleSoundsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = IdleSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };

            //Impact Sounds
            ImpactSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("ImpactSounds"), true, true, true, true);
            ImpactSoundsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Weapon Impact Sounds List", EditorStyles.boldLabel);
            };
            ImpactSoundsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = ImpactSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };

            //Critical Hit Sounds
            CriticalHitSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("CriticalHitSounds"), true, true, true, true);
            CriticalHitSoundsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Weapon Critical Hit Sounds List", EditorStyles.boldLabel);
            };
            CriticalHitSoundsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = CriticalHitSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };

            //Block Sounds
            BlockSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("BlockingSounds"), true, true, true, true);
            BlockSoundsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Block Sounds List", EditorStyles.boldLabel);
            };
            BlockSoundsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = BlockSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };

            //Hit Animations
            HitAnimationList = new ReorderableList(serializedObject, serializedObject.FindProperty("HitAnimationList"), true, true, true, true);
            HitAnimationList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = HitAnimationList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationSpeed"), GUIContent.none);

                    if (element.FindPropertyRelative("AnimationSpeed").floatValue == 0)
                    {
                        element.FindPropertyRelative("AnimationSpeed").floatValue = 1;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationListsChangedProp.boolValue = true;
                    }
                };

            HitAnimationList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   Speed  " + "     Clip", EditorStyles.boldLabel);
            };
            HitAnimationList.onChangedCallback = (HitAnimationList) =>
            {
                AnimationListsChangedProp.boolValue = true;
            };

            //Combat Hit Animations
            CombatHitAnimationList = new ReorderableList(serializedObject, serializedObject.FindProperty("CombatHitAnimationList"), true, true, true, true);
            CombatHitAnimationList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = CombatHitAnimationList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationSpeed"), GUIContent.none);

                    if (element.FindPropertyRelative("AnimationSpeed").floatValue == 0)
                    {
                        element.FindPropertyRelative("AnimationSpeed").floatValue = 1;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationListsChangedProp.boolValue = true;
                    }
                };

            CombatHitAnimationList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   Speed  " + "     Clip", EditorStyles.boldLabel);
            };
            CombatHitAnimationList.onChangedCallback = (CombatHitAnimationList) =>
            {
                AnimationListsChangedProp.boolValue = true;
            };

            //Ranged Combat Hit Animations
            RangedCombatHitAnimationList = new ReorderableList(serializedObject, serializedObject.FindProperty("RangedCombatHitAnimationList"), true, true, true, true);
            RangedCombatHitAnimationList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = RangedCombatHitAnimationList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationSpeed"), GUIContent.none);

                    if (element.FindPropertyRelative("AnimationSpeed").floatValue == 0)
                    {
                        element.FindPropertyRelative("AnimationSpeed").floatValue = 1;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationListsChangedProp.boolValue = true;
                    }
                };

            RangedCombatHitAnimationList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   Speed  " + "     Clip", EditorStyles.boldLabel);
            };
            RangedCombatHitAnimationList.onChangedCallback = (RangedCombatHitAnimationList) =>
            {
                AnimationListsChangedProp.boolValue = true;
            };

            IdleAnimationList = new ReorderableList(serializedObject, serializedObject.FindProperty("IdleAnimationList"), true, true, true, true);
            IdleAnimationList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = IdleAnimationList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationSpeed"), GUIContent.none);

                    if (element.FindPropertyRelative("AnimationSpeed").floatValue == 0)
                    {
                        element.FindPropertyRelative("AnimationSpeed").floatValue = 1;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationListsChangedProp.boolValue = true;
                    }
                };

            IdleAnimationList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   Speed  " + "     Clip", EditorStyles.boldLabel);
            };
            IdleAnimationList.onChangedCallback = (IdleAnimationList) =>
            {
                AnimationListsChangedProp.boolValue = true;
            };

            //Attack Animations
            AttackAnimationList = new ReorderableList(serializedObject, serializedObject.FindProperty("AttackAnimationList"), true, true, true, true);
            AttackAnimationList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = AttackAnimationList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationSpeed"), GUIContent.none);

                    if (element.FindPropertyRelative("AnimationSpeed").floatValue == 0)
                    {
                        element.FindPropertyRelative("AnimationSpeed").floatValue = 1;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationListsChangedProp.boolValue = true;
                    }
                };

            AttackAnimationList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   Speed  " + "     Clip", EditorStyles.boldLabel);
            };
            AttackAnimationList.onChangedCallback = (AttackAnimationList) =>
            {
                AnimationListsChangedProp.boolValue = true;
            };

            //Ranged Attack Animations
            RangedAttackAnimationList = new ReorderableList(serializedObject, serializedObject.FindProperty("RangedAttackAnimationList"), true, true, true, true);
            RangedAttackAnimationList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = RangedAttackAnimationList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationSpeed"), GUIContent.none);

                    if (element.FindPropertyRelative("AnimationSpeed").floatValue == 0)
                    {
                        element.FindPropertyRelative("AnimationSpeed").floatValue = 1;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationListsChangedProp.boolValue = true;

                    }
                };

            RangedAttackAnimationList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   Speed  " + "     Clip", EditorStyles.boldLabel);
            };
            RangedAttackAnimationList.onChangedCallback = (RangedAttackAnimationList) =>
            {
                AnimationListsChangedProp.boolValue = true;
            };

            //Run Attack Animations
            RunAttackAnimationList = new ReorderableList(serializedObject, serializedObject.FindProperty("RunAttackAnimationList"), true, true, true, true);
            RunAttackAnimationList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = RunAttackAnimationList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationSpeed"), GUIContent.none);

                    if (element.FindPropertyRelative("AnimationSpeed").floatValue == 0)
                    {
                        element.FindPropertyRelative("AnimationSpeed").floatValue = 1;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationListsChangedProp.boolValue = true;
                    }
                };

            RunAttackAnimationList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   Speed  " + "     Clip", EditorStyles.boldLabel);
            };
            RunAttackAnimationList.onChangedCallback = (RunAttackAnimationList) =>
            {
                AnimationListsChangedProp.boolValue = true;
            };

            //Ranged Run Attack Animations
            RangedRunAttackAnimationList = new ReorderableList(serializedObject, serializedObject.FindProperty("RangedRunAttackAnimationList"), true, true, true, true);
            RangedRunAttackAnimationList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = RangedRunAttackAnimationList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationSpeed"), GUIContent.none);

                    if (element.FindPropertyRelative("AnimationSpeed").floatValue == 0)
                    {
                        element.FindPropertyRelative("AnimationSpeed").floatValue = 1;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationListsChangedProp.boolValue = true;
                    }
                };

            RangedRunAttackAnimationList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   Speed  " + "     Clip", EditorStyles.boldLabel);
            };
            RangedRunAttackAnimationList.onChangedCallback = (RangedRunAttackAnimationList) =>
            {
                AnimationListsChangedProp.boolValue = true;
            };

            //Death Animations
            DeathAnimationList = new ReorderableList(serializedObject, serializedObject.FindProperty("DeathAnimationList"), true, true, true, true);
            DeathAnimationList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = DeathAnimationList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationSpeed"), GUIContent.none);

                    if (element.FindPropertyRelative("AnimationSpeed").floatValue == 0)
                    {
                        element.FindPropertyRelative("AnimationSpeed").floatValue = 1;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationListsChangedProp.boolValue = true;
                    }
                };

            DeathAnimationList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   Speed  " + "     Clip", EditorStyles.boldLabel);
            };
            DeathAnimationList.onChangedCallback = (DeathAnimationList) =>
            {
                AnimationListsChangedProp.boolValue = true;
            };

            //Ranged Death Animations
            RangedDeathAnimationList = new ReorderableList(serializedObject, serializedObject.FindProperty("RangedDeathAnimationList"), true, true, true, true);
            RangedDeathAnimationList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = RangedDeathAnimationList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationSpeed"), GUIContent.none);

                    if (element.FindPropertyRelative("AnimationSpeed").floatValue == 0)
                    {
                        element.FindPropertyRelative("AnimationSpeed").floatValue = 1;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationListsChangedProp.boolValue = true;
                    }
                };

            RangedDeathAnimationList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   Speed  " + "     Clip", EditorStyles.boldLabel);
            };
            RangedDeathAnimationList.onChangedCallback = (RangedDeathAnimationList) =>
            {
                AnimationListsChangedProp.boolValue = true;
            };

            //Emote Animations
            EmoteAnimationList = new ReorderableList(serializedObject, serializedObject.FindProperty("EmoteAnimationList"), true, true, true, true);
            EmoteAnimationList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = EmoteAnimationList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, rect.width - 120, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("EmoteAnimationClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AnimationID"), GUIContent.none);
                    if (EditorGUI.EndChangeCheck())
                    {
                        AnimationListsChangedProp.boolValue = true;
                    }
                };

            EmoteAnimationList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   ID  " + "         Emote Animation Clip", EditorStyles.boldLabel);
            };
            EmoteAnimationList.onChangedCallback = (EmoteAnimationList) =>
            {
                AnimationListsChangedProp.boolValue = true;
            };

            //Interact Sounds
            InteractSoundsList = new ReorderableList(serializedObject, serializedObject.FindProperty("InteractSoundList"), true, true, true, true);
            InteractSoundsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = InteractSoundsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, rect.width - 120, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("SoundEffectClip"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("SoundEffectID"), GUIContent.none);
                };

            InteractSoundsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   ID  " + "         Interact Sound Clip", EditorStyles.boldLabel);
            };

            //Item Objects
            ItemList = new ReorderableList(serializedObject, serializedObject.FindProperty("ItemList"), true, true, true, true);
            ItemList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = ItemList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, rect.width - 120, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("ItemObject"), GUIContent.none);
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("ItemID"), GUIContent.none);
                };

            ItemList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "   ID  " + "         Item Object", EditorStyles.boldLabel);
            };

            //Melee Attacks
            MeleeAttacks = new ReorderableList(serializedObject, serializedObject.FindProperty("MeleeAttacks"), true, true, true, true);

            MeleeAttacks.drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, "Melee Attacks", EditorStyles.boldLabel);
            };

            MeleeAttacks.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var element = MeleeAttacks.serializedProperty.GetArrayElementAtIndex(index);

                    if (self.AttackAnimationList.Count > 0 && MeleeEnumAnimations != null)
                    {
                        CustomPopup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), new GUIContent(), element.FindPropertyRelative("AttackAnimaition"), "Attack Animation", MeleeEnumAnimations);
                        EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight), new GUIContent("Attack Animation", "The animation that will be used for this melee attack. Note: Animations are based off of your AI's Attack Animation List."));
                    }
                    else
                    {
                        EditorGUI.Popup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), 0, MeleeBlankOptions);
                        EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight), new GUIContent("Attack Animation"));
                    }

                    if (self.RandomizeDamageRef == EmeraldAISystem.RandomizeDamage.Yes)
                    {
                        MeleeAttacks.elementHeight = EditorGUIUtility.singleLineHeight * 9;
                        element.FindPropertyRelative("MinDamage").intValue = EditorGUI.IntField(new Rect(rect.x, rect.y + 35, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Min Damage", "The minimum damage this attack can do."), element.FindPropertyRelative("MinDamage").intValue);
                        element.FindPropertyRelative("MaxDamage").intValue = EditorGUI.IntField(new Rect(rect.x, rect.y + 60, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Max Damage", "The maximum damage this attack can do."), element.FindPropertyRelative("MaxDamage").intValue);
                        EditorGUI.BeginDisabledGroup(self.MeleeAttackPickType != EmeraldAISystem.MeleeAttackPickTypeEnum.Odds);
                        element.FindPropertyRelative("AttackOdds").intValue = EditorGUI.IntSlider(new Rect(rect.x, rect.y + 85, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Attack Odds",
                            "The odds that this melee attack will use when an AI uses a melee attack (When using the Odds Pick type)."), element.FindPropertyRelative("AttackOdds").intValue, 1, 100);
                        EditorGUI.EndDisabledGroup();
                        EditorGUI.PropertyField(new Rect(rect.x, rect.y + 110, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AttackImpactEffect"), new GUIContent("Attack Impact Effect", "Controls the effect that will play when this AI lands a successful hit with this attack. If no object is applied, this feature will be ignored."));
                    }
                    else
                    {
                        MeleeAttacks.elementHeight = EditorGUIUtility.singleLineHeight * 7.5f;
                        element.FindPropertyRelative("MinDamage").intValue = EditorGUI.IntField(new Rect(rect.x, rect.y + 35, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Damage", "The damage this attack can do."), element.FindPropertyRelative("MinDamage").intValue);
                        EditorGUI.BeginDisabledGroup(self.MeleeAttackPickType != EmeraldAISystem.MeleeAttackPickTypeEnum.Odds);
                        element.FindPropertyRelative("AttackOdds").intValue = EditorGUI.IntSlider(new Rect(rect.x, rect.y + 60, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Attack Odds",
                            "The odds that this melee attack will use when an AI uses a melee attack (When using the Odds Pick type)."), element.FindPropertyRelative("AttackOdds").intValue, 1, 100);
                        EditorGUI.EndDisabledGroup();
                        EditorGUI.PropertyField(new Rect(rect.x, rect.y + 90, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AttackImpactEffect"), new GUIContent("Attack Impact Effect", "Controls the effect that will play when this AI lands a successful hit with this attack. If no object is applied, this feature will be ignored."));
                    }
                };

            MeleeAttacks.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Melee Attacks", EditorStyles.boldLabel);
            };

            //Melee Run Attacks
            MeleeRunAttacks = new ReorderableList(serializedObject, serializedObject.FindProperty("MeleeRunAttacks"), true, true, true, true);

            MeleeRunAttacks.drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, "Melee Run Attacks", EditorStyles.boldLabel);
            };

            MeleeRunAttacks.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var element = MeleeRunAttacks.serializedProperty.GetArrayElementAtIndex(index);

                    if (self.RunAttackAnimationList.Count > 0 && RunMeleeEnumAnimations != null)
                    {
                        CustomPopup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), new GUIContent(), element.FindPropertyRelative("AttackAnimaition"), "Attack Animation", RunMeleeEnumAnimations);
                        EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight), new GUIContent("Attack Animation", "The animation that will be used for this running melee attack. Note: Animations are based off of your AI's Run Attack Animation List."));
                    }
                    else
                    {
                        EditorGUI.Popup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), 0, RunMeleeBlankOptions);
                        EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight), new GUIContent("Attack Animation"));
                    }

                    if (self.RandomizeDamageRef == EmeraldAISystem.RandomizeDamage.Yes)
                    {
                        MeleeRunAttacks.elementHeight = EditorGUIUtility.singleLineHeight * 7;
                        element.FindPropertyRelative("MinDamage").intValue = EditorGUI.IntField(new Rect(rect.x, rect.y + 35, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Min Damage", "The minimum damage this run attack can do."), element.FindPropertyRelative("MinDamage").intValue);
                        element.FindPropertyRelative("MaxDamage").intValue = EditorGUI.IntField(new Rect(rect.x, rect.y + 60, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Max Damage", "The maximum damage this run attack can do."), element.FindPropertyRelative("MaxDamage").intValue);
                        EditorGUI.BeginDisabledGroup(self.MeleeAttackPickType != EmeraldAISystem.MeleeAttackPickTypeEnum.Odds);
                        element.FindPropertyRelative("AttackOdds").intValue = EditorGUI.IntSlider(new Rect(rect.x, rect.y + 85, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Attack Odds",
                            "The odds that this melee attack will use when an AI uses a running melee attack (When using the Odds Pick type)."), element.FindPropertyRelative("AttackOdds").intValue, 1, 100);
                        EditorGUI.EndDisabledGroup();
                    }
                    else
                    {
                        MeleeRunAttacks.elementHeight = EditorGUIUtility.singleLineHeight * 5.5f;
                        element.FindPropertyRelative("MinDamage").intValue = EditorGUI.IntField(new Rect(rect.x, rect.y + 35, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Damage", "The damage this run attack can do."), element.FindPropertyRelative("MinDamage").intValue);
                        EditorGUI.BeginDisabledGroup(self.MeleeAttackPickType != EmeraldAISystem.MeleeAttackPickTypeEnum.Odds);
                        element.FindPropertyRelative("AttackOdds").intValue = EditorGUI.IntSlider(new Rect(rect.x, rect.y + 60, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Attack Odds",
                            "The odds that this melee attack will use when an AI uses a running melee attack (When using the Odds Pick type)."), element.FindPropertyRelative("AttackOdds").intValue, 1, 100);
                        EditorGUI.EndDisabledGroup();
                    }
                };

            MeleeRunAttacks.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Melee Run Attacks", EditorStyles.boldLabel);
            };

            //Offsenive Abilities
            OffensiveAbilities = new ReorderableList(serializedObject, serializedObject.FindProperty("OffensiveAbilities"), true, true, true, true);

            OffensiveAbilities.drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, "Offensive Abilities", EditorStyles.boldLabel);
            };

            OffensiveAbilities.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var element = OffensiveAbilities.serializedProperty.GetArrayElementAtIndex(index);
                    OffensiveAbilities.elementHeight = EditorGUIUtility.singleLineHeight * 5.5f;

                    if (self.RangedAttackAnimationList.Count > 0 && RangedEnumAnimations != null)
                    {
                        //element.FindPropertyRelative("AbilityAnimaition").intValue = EditorGUI.Popup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("AbilityAnimaition").intValue, RangedEnumAnimations);
                        CustomPopup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), new GUIContent(), element.FindPropertyRelative("AbilityAnimaition"), "Ability Animaition", RangedEnumAnimations);
                        EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight), new GUIContent("Ability Animation", "The animation that will be used for this offsensive ability. Note: Animations are based off of your AI's Ranged Attack Animation List."));
                    }
                    else
                    {
                        EditorGUI.Popup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), 0, RangedBlankOptions);
                        EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight), new GUIContent("Ability Animation"));
                    }

                    EditorGUI.ObjectField(new Rect(rect.x, rect.y + 35, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("OffensiveAbility"), new GUIContent("Ability Object", "The ability object that will be used for this offsensive ability."));
                    EditorGUI.BeginDisabledGroup(self.OffensiveAbilityPickType != EmeraldAISystem.OffensiveAbilityPickTypeEnum.Odds);
                    element.FindPropertyRelative("AbilityOdds").intValue = EditorGUI.IntSlider(new Rect(rect.x, rect.y + 60, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Ability Odds", "The odds that this ability will use when an AI uses its offensive abilities when using the Odds Pick type."), element.FindPropertyRelative("AbilityOdds").intValue, 1, 100);
                    EditorGUI.EndDisabledGroup();
                };

            OffensiveAbilities.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Offensive Abilities", EditorStyles.boldLabel);
            };

            //Support Abilities
            SupportAbilities = new ReorderableList(serializedObject, serializedObject.FindProperty("SupportAbilities"), true, true, true, true);

            SupportAbilities.drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, "Support Abilities", EditorStyles.boldLabel);
            };

            SupportAbilities.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var element = SupportAbilities.serializedProperty.GetArrayElementAtIndex(index);
                    SupportAbilities.elementHeight = EditorGUIUtility.singleLineHeight * 5.5f;

                    if (self.RangedAttackAnimationList.Count > 0 && RangedEnumAnimations != null)
                    {
                        CustomPopup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), new GUIContent(), element.FindPropertyRelative("AbilityAnimaition"), "Ability Animaition", RangedEnumAnimations);
                        EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight), new GUIContent("Ability Animation", "The animation that will be used for this offsensive ability. Note: Animations are based off of your AI's Ranged Attack Animation List."));
                    }
                    else
                    {
                        EditorGUI.Popup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), 0, RangedBlankOptions);
                        EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight), new GUIContent("Ability Animation"));
                    }

                    EditorGUI.ObjectField(new Rect(rect.x, rect.y + 35, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("SupportAbility"), new GUIContent("Ability Object", "The ability object that will be used for this offsensive ability."));
                    EditorGUI.BeginDisabledGroup(self.SupportAbilityPickType != EmeraldAISystem.SupportAbilityPickTypeEnum.Odds);
                    element.FindPropertyRelative("AbilityOdds").intValue = EditorGUI.IntSlider(new Rect(rect.x, rect.y + 60, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Ability Odds", "The odds that this ability will use when an AI uses its Support abilities when using the Odds Pick type."), element.FindPropertyRelative("AbilityOdds").intValue, 1, 100);
                    EditorGUI.EndDisabledGroup();
                };

            SupportAbilities.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Support Abilities", EditorStyles.boldLabel);
            };

            //Summoning Abilities
            SummoningAbilities = new ReorderableList(serializedObject, serializedObject.FindProperty("SummoningAbilities"), true, true, true, true);

            SummoningAbilities.drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, "Summoning Abilities", EditorStyles.boldLabel);
            };

            SummoningAbilities.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => {
                    var element = SummoningAbilities.serializedProperty.GetArrayElementAtIndex(index);
                    SummoningAbilities.elementHeight = EditorGUIUtility.singleLineHeight * 5.5f;

                    if (self.RangedAttackAnimationList.Count > 0 && RangedEnumAnimations != null)
                    {
                        CustomPopup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), new GUIContent(), element.FindPropertyRelative("AbilityAnimaition"), "Ability Animaition", RangedEnumAnimations);
                        EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight), new GUIContent("Ability Animation", "The animation that will be used for this offsensive ability. Note: Animations are based off of your AI's Ranged Attack Animation List."));
                    }
                    else
                    {
                        EditorGUI.Popup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), 0, RangedBlankOptions);
                        EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight), new GUIContent("Ability Animation"));
                    }

                    EditorGUI.ObjectField(new Rect(rect.x, rect.y + 35, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("SummoningAbility"), new GUIContent("Ability Object", "The ability object that will be used for this offsensive ability."));
                    EditorGUI.BeginDisabledGroup(self.SummoningAbilityPickType != EmeraldAISystem.SummoningAbilityPickTypeEnum.Odds);
                    element.FindPropertyRelative("AbilityOdds").intValue = EditorGUI.IntSlider(new Rect(rect.x, rect.y + 60, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Ability Odds", "The odds that this ability will use when an AI uses its Summoning abilities when using the Odds Pick type."), element.FindPropertyRelative("AbilityOdds").intValue, 1, 100);
                    EditorGUI.EndDisabledGroup();
                };

            SummoningAbilities.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Summoning Abilities", EditorStyles.boldLabel);
            };

            //Blood Effects List
            BloodEffectsList = new ReorderableList(serializedObject, serializedObject.FindProperty("BloodEffectsList"), true, true, true, true);
            BloodEffectsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Hit Effects List", EditorStyles.boldLabel);
            };
            BloodEffectsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = BloodEffectsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };

            //Factions List
            FactionsList = new ReorderableList(serializedObject, serializedObject.FindProperty("FactionRelationsList"), true, true, true, true);
            FactionsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = FactionsList.serializedProperty.GetArrayElementAtIndex(index);
                    FactionsList.elementHeight = EditorGUIUtility.singleLineHeight * 3.75f;

                    if (element.FindPropertyRelative("RelationTypeRef").intValue == 0)
                    {
                        EditorGUI.DrawRect(new Rect(rect.x - 16, rect.y + 2f, rect.width + 17, EditorGUIUtility.singleLineHeight * 3.5f), new Color(1.0f, 0.0f, 0.0f, 0.15f));
                    }
                    else if (element.FindPropertyRelative("RelationTypeRef").intValue == 1)
                    {
                        EditorGUI.DrawRect(new Rect(rect.x - 16, rect.y + 2f, rect.width + 17, EditorGUIUtility.singleLineHeight * 3.5f), new Color(0.1f, 0.1f, 0.1f, 0.1f));
                    }
                    else if (element.FindPropertyRelative("RelationTypeRef").intValue == 2)
                    {
                        EditorGUI.DrawRect(new Rect(rect.x - 16, rect.y + 2f, rect.width + 17, EditorGUIUtility.singleLineHeight * 3.5f), new Color(0.0f, 1.0f, 0.0f, 0.15f));
                    }
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y + 35, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("RelationTypeRef"), new GUIContent("Relation Type", "The type of relation this AI has with this faction."));

                    CustomPopup(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), new GUIContent(), element.FindPropertyRelative("FactionIndex"), "Faction", EmeraldAISystem.StringFactionList.ToArray());

                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight),
                        new GUIContent("Faction", "Factions are based on all factions within the Faction Manager. An AI can have as many faction relations as needed."));
                };

            FactionsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "AI Faction Relations", EditorStyles.boldLabel);
            };

            //Player Faction
            PlayerFaction = new ReorderableList(serializedObject, serializedObject.FindProperty("PlayerFaction"), false, true, false, false);
            PlayerFaction.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.BeginChangeCheck();
                    var element = PlayerFaction.serializedProperty.GetArrayElementAtIndex(index);
                    PlayerFaction.elementHeight = EditorGUIUtility.singleLineHeight * 4.1f;

                    if (element.FindPropertyRelative("RelationTypeRef").intValue == 0)
                    {
                        EditorGUI.DrawRect(new Rect(rect.x - 2, rect.y + 1f, rect.width + 3, EditorGUIUtility.singleLineHeight * 4f), new Color(1.0f, 0.0f, 0.0f, 0.15f));
                    }
                    else if (element.FindPropertyRelative("RelationTypeRef").intValue == 1)
                    {
                        EditorGUI.DrawRect(new Rect(rect.x - 2, rect.y + 1f, rect.width + 3, EditorGUIUtility.singleLineHeight * 4f), new Color(0.1f, 0.1f, 0.1f, 0.1f));
                    }
                    else if (element.FindPropertyRelative("RelationTypeRef").intValue == 2)
                    {
                        EditorGUI.DrawRect(new Rect(rect.x - 2, rect.y + 1f, rect.width + 3, EditorGUIUtility.singleLineHeight * 4f), new Color(0.0f, 1.0f, 0.0f, 0.15f));
                    }
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y + 35, rect.width, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("RelationTypeRef"), new GUIContent("Relation Type", "The type of relation this AI has with this faction."));

                    //element.FindPropertyRelative("PlayerTag").stringValue = EditorGUI.TagField(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("PlayerTag").stringValue);
                    CustomTag(new Rect(rect.x + 125, rect.y + 10, rect.width - 125, EditorGUIUtility.singleLineHeight), new GUIContent(), element.FindPropertyRelative("PlayerTag"));
                    PlayerTagProp.stringValue = element.FindPropertyRelative("PlayerTag").stringValue;

                    EditorGUI.PrefixLabel(new Rect(rect.x, rect.y + 10, 125, EditorGUIUtility.singleLineHeight),
                        new GUIContent("Player Tag", "The Player Tag is the Unity Tag used to detect player targets."));
                };

            PlayerFaction.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Player Relation", EditorStyles.boldLabel);
            };

            #endregion
        }

        void LoadFactionData()
        {
            EmeraldAISystem.StringFactionList.Clear();
            string path = AssetDatabase.GetAssetPath(Resources.Load("Faction Data"));
            EmeraldAIFactionData FactionData = (EmeraldAIFactionData)AssetDatabase.LoadAssetAtPath(path, typeof(EmeraldAIFactionData));

            if (FactionData != null)
            {
                foreach (string s in FactionData.FactionNameList)
                {
                    if (!EmeraldAISystem.StringFactionList.Contains(s) && s != "")
                    {
                        EmeraldAISystem.StringFactionList.Add(s);
                    }
                }
            }
        }

        void CheckForMissingAnimations()
        {
            EmeraldAISystem self = (EmeraldAISystem)target;
            self.gameObject.GetComponent<EmeraldAIInitializer>().CheckForMissingAnimations();
        }

        void CopyFromNonCombat()
        {
            EmeraldAISystem self = (EmeraldAISystem)target;
            EmeraldAIAnimatorGenerator.CopyFromNonCombat(self);
            serializedObject.Update();
            EditorUtility.SetDirty(self);
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            EmeraldAISystem self = (EmeraldAISystem)target;

            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (!self.AnimatorControllerGenerated)
            {
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("There is currently no generated Animator Controller for this AI. " +
                    "Please go to the Animation tab, apply all needed animations, and create one before using this AI.", MessageType.Warning);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();
            }
            else if (!self.HeadTransform)
            {
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("The AI's Head Transform has not been applied and is needed for accurate raycast calculations, " +
                    "please apply it. This is located under Detections & Tags>Detection Options.", MessageType.Warning);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();
            }
            else if (self.FactionRelationsList.Count == 0 && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
            {
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.HelpBox("This AI needs at least 1 Faction Relation to function properly. " +
                    "Please apply one by going to the Detections & Tags>Faction Options tab.", MessageType.Warning);
                GUI.backgroundColor = Color.white;
                EditorGUILayout.Space();
            }
            else if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged && !self.RangedAttackTransform && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet ||
                self.WeaponTypeRef == EmeraldAISystem.WeaponType.Both && !self.RangedAttackTransform && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
            {
                if (self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Passive && self.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward || self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Cautious && self.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward)
                {
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("There is currently no applied Ranged Attack Transform for this AI. To apply one, go to AI's Settings>Combat>Damage Settings.", MessageType.Warning);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                }
            }
            else if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged && self.OffensiveAbilities.Count == 0 && self.SupportAbilities.Count == 0 && self.SummoningAbilities.Count == 0 && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet ||
                self.WeaponTypeRef == EmeraldAISystem.WeaponType.Both && self.OffensiveAbilities.Count == 0 && self.SupportAbilities.Count == 0 && self.SummoningAbilities.Count == 0 && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
            {
                if (self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Passive && self.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward || self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Cautious && self.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward)
                {
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("There currently aren't any abilities applied to this AI. Please ensure there is at least 1 Ability Object applied to one of the Ability Categories. This " +
                        "can be found under AI's Settings>Combat>Damage Settings.", MessageType.Warning);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                }
            }
            else if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && self.MeleeAttacks.Count == 0 && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet ||
                self.WeaponTypeRef == EmeraldAISystem.WeaponType.Both && self.MeleeAttacks.Count == 0 && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
            {
                if (self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Passive && self.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward || self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Cautious && self.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward)
                {
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("There currently aren't any melee attacks applied to this AI. Please ensure there is at least 1 Melee Attack applied to one of the Melee Attacks List. This " +
                        "can be found under AI's Settings>Combat>Damage Settings.", MessageType.Warning);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                }
            }
            else if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && self.MeleeRunAttacks.Count == 0 && self.UseRunAttacksRef == EmeraldAISystem.UseRunAttacks.Yes && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet ||
                self.WeaponTypeRef == EmeraldAISystem.WeaponType.Both && self.MeleeRunAttacks.Count == 0 && self.UseRunAttacksRef == EmeraldAISystem.UseRunAttacks.Yes && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
            {
                if (self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Passive && self.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward || self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Cautious && self.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward)
                {
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.HelpBox("There currently aren't any melee run attacks applied to this AI. Please ensure there is at least 1 Melee Run Attack applied to one of the Melee Run Attacks List. This " +
                        "can be found under AI's Settings>Combat>Damage Settings.", MessageType.Warning);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                }
            }

            GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            myFoldoutStyle.fontStyle = FontStyle.Bold;
            myFoldoutStyle.fontSize = 12;
            myFoldoutStyle.active.textColor = Color.black;
            myFoldoutStyle.focused.textColor = Color.black;
            myFoldoutStyle.onHover.textColor = Color.black;
            myFoldoutStyle.normal.textColor = Color.black;
            myFoldoutStyle.onNormal.textColor = Color.black;
            myFoldoutStyle.onActive.textColor = Color.black;
            myFoldoutStyle.onFocused.textColor = Color.black;
            Color myStyleColor = Color.black;

            GUIContent[] TabButtons = new GUIContent[8] {new GUIContent("Temperament", TemperamentIcon), new GUIContent("AI's Settings", SettingsIcon), new GUIContent(" Detection \n & Tags", DetectTagsIcon),
            new GUIContent("UI Settings", UIIcon), new GUIContent("Sounds", SoundIcon), new GUIContent(" Waypoint\nEditor", WaypointEditorIcon), new GUIContent("Animations", AnimationIcon), new GUIContent("Docs", DocumentationIcon)};

            GUIStyle SelectionGridStyle = new GUIStyle(EditorStyles.miniButton);
            SelectionGridStyle.fixedHeight = 35;

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
#if UNITY_2019_3_OR_NEWER
            TabNumberProp.intValue = GUILayout.SelectionGrid(TabNumberProp.intValue, TabButtons, 4, SelectionGridStyle, GUILayout.Height(68), GUILayout.Width(EditorGUIUtility.currentViewWidth-50));
#else
            TabNumberProp.intValue = GUILayout.SelectionGrid(TabNumberProp.intValue, TabButtons, 4, EditorStyles.miniButtonRight, GUILayout.Height(68), GUILayout.Width(EditorGUIUtility.currentViewWidth - 50));
#endif
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            var HelpButtonStyle = new GUIStyle(GUI.skin.button);
            HelpButtonStyle.normal.textColor = Color.white;
            HelpButtonStyle.fontStyle = FontStyle.Bold;

            //Behavior Options
            if (TabNumberProp.intValue == 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical("Box");
                var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(new GUIContent(TemperamentIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                EditorGUILayout.LabelField("Temperament Options", style, GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField("The Temperament Options allow you to control an AI's behavior, how it reacts to certain situations, and how it chooses to wander or move to a destination.", EditorStyles.helpBox);
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                BehaviorFoldout.boolValue = CustomEditorProperties.Foldout(BehaviorFoldout.boolValue, "Behavior", true, myFoldoutStyle);

                if (BehaviorFoldout.boolValue)
                {
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("The Behavior Type allows you to define the behavior your AI will have. All AI will only react to targets that have targets' tags set within the AI's Tags List.", EditorStyles.helpBox);

                    EditorGUILayout.PropertyField(BehaviorProp, new GUIContent("Behavior Type"));

                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Passive)
                    {
                        EditorGUILayout.LabelField("Passive - Passive AI will not attack. They will simply wander around. If they are attacked, they will react according to their " +
                            "Confidence Level. If you would like your AI to not attack, even if attacked first, simply enable 'Does Not Attack'.", EditorStyles.helpBox);
                    }
                    else if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Cautious)
                    {
                        EditorGUILayout.LabelField("Cautious - Cautious AI will either flee or act territorial depending on their Confidence Level. Territorial AI will warn targets " +
                            "before attacking their target. An AI is set as territorial if their Confidence Level is set to Brave or higher.", EditorStyles.helpBox);
                    }
                    else if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
                    {
                        EditorGUILayout.LabelField("Companion - Companion AI will follow around a target and help them fight. Companion AI will wander until their follow target is set. " +
                            "This is best done with a script and calling the public function SetTarget.", EditorStyles.helpBox);
                    }
                    else if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Aggressive)
                    {
                        EditorGUILayout.LabelField("Aggressive - Aggressive AI will attack any target that comes within their trigger radius.", EditorStyles.helpBox);
                    }
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                ConfidenceFoldout.boolValue = CustomEditorProperties.Foldout(ConfidenceFoldout.boolValue, "Confidence", true, myFoldoutStyle);

                if (ConfidenceFoldout.boolValue)
                {
                    EditorGUILayout.BeginVertical("Box");
                    if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("(Not Usable with Companion AI)", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    else if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("(Not Usable with Pet AI)", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.LabelField("The Confidence Level gives you more control over how your AI reacts with their Behavior Type.", EditorStyles.helpBox);

                    EditorGUILayout.PropertyField(ConfidenceProp, new GUIContent("Confidence Level"));

                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Cautious)
                    {
                        if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Coward)
                        {
                            EditorGUILayout.LabelField("Coward - AI with a Coward confidence level will flee when they encounter a target.", EditorStyles.helpBox);
                        }
                        else if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Brave)
                        {
                            EditorGUILayout.LabelField("Brave - AI with a Brave confidence level will become territorial when a target enters their trigger radius. " +
                                "If the target doesn't leave its radius before its territorial seconds have been reached, the AI will attack the target. This AI will " +
                                "flee once its health reaches the amount you've set below.", EditorStyles.helpBox);
                        }
                        else if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Foolhardy)
                        {
                            EditorGUILayout.LabelField("Foolhardy - AI with a Foolhardy confidence level will become territorial when a target enters their trigger radius. " +
                                "If the target doesn't leave its radius before its territorial seconds have been reached, the AI will attack the target. " +
                                "This AI will never flee and continue to fight until dead or the target has escaped the AI.", EditorStyles.helpBox);

                        }
                    }
                    else if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Aggressive)
                    {
                        if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Coward)
                        {
                            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                            EditorGUILayout.LabelField("Aggressive AI cannot be set to Coward. This AI will automatically be set to Brave on Start.", EditorStyles.helpBox);
                        }
                        else if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Brave)
                        {
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("Brave - Brave Aggressive AI will fight any target on sight or detection, but attempt to flee once its health " +
                                "reaches the percentage you've set.", EditorStyles.helpBox);
                        }
                        else if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Foolhardy)
                        {
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("Foolhardy - Foolhardy Aggressive AI will fight any target on sight or detection and will never flee. " +
                                "They will continue to fight until dead or the target has escaped the AI.", EditorStyles.helpBox);

                        }
                    }
                    else if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Passive)
                    {
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Coward)
                        {
                            EditorGUILayout.LabelField("Coward - Coward Passive AI will wander according to their wander settings, but only flee when attacked.", EditorStyles.helpBox);
                        }
                        else if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Brave)
                        {
                            EditorGUILayout.LabelField("Brave - Brave Passive AI will wander according to their wander settings, but only attack when attacked. They will " +
                                "attempt to flee once its health reaches the percentage you've set.", EditorStyles.helpBox);
                        }
                        else if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Foolhardy)
                        {
                            EditorGUILayout.LabelField("Foolhardy - Foolhardy Passive AI will wander according to their wander settings, but only attack when attacked. They " +
                                "will never flee and continue to fight until dead or the target has escaped the AI.", EditorStyles.helpBox);

                        }
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    if (self.ConfidenceRef != EmeraldAISystem.ConfidenceType.Brave)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("(Brave Confidence Level Only)", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), HealthPercentageToFleeProp, "Health Level to Flee", 1, 99);
                    CustomHelpLabelField("Controls the percent of health loss that's needed to trigger a flee attempt.", false);

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                WanderFoldout.boolValue = CustomEditorProperties.Foldout(WanderFoldout.boolValue, "Wander Type", true, myFoldoutStyle);

                if (WanderFoldout.boolValue)
                {
                    EditorGUILayout.BeginVertical("Box");
                    if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("(Companion AI will only wander when they don't have a current follower)", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    else if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("(Not Usable with Pet AI)", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.LabelField("Controls the type of wandering technique this AI will use. AI will react to targets according to their Behavior Type.", EditorStyles.helpBox);
                    EditorGUILayout.PropertyField(WanderTypeProp, new GUIContent("Wander Type"));

                    if (self.WanderTypeRef == EmeraldAISystem.WanderType.Dynamic)
                    {
                        CustomHelpLabelField("Dynamic - Allows an AI to randomly wander by dynamically generate waypoints around their Wander Radius.", true);
                    }
                    else if (self.WanderTypeRef == EmeraldAISystem.WanderType.Waypoints)
                    {
                        CustomHelpLabelField("Waypoints - Allows you to define waypoints that the AI will move between.", true);

                        CustomHelpLabelField("You can edit and create Waypoints by going to the Waypoint Editor tab. Would you like to do this now?", false);
                        if (GUILayout.Button("Open the Waypoint Editor"))
                        {
                            TabNumberProp.intValue = 5;
                        }
                    }
                    else if (self.WanderTypeRef == EmeraldAISystem.WanderType.Stationary)
                    {
                        CustomHelpLabelField("Stationary - Allows an AI to stay stationary in the same position and will not move unless a target enters their trigger radius.", true);

                        CustomEditorProperties.CustomIntField(new Rect(), new GUIContent(), StationaryIdleSecondsMinProp, "Min Idle Animation Seconds");
                        CustomHelpLabelField("When using more than 1 idle animation, this controls the minimum amount of seconds needed before switching to the next idle " +
                            "animation. This will be randomized with the Max Idle Animation Seconds.", true);

                        CustomEditorProperties.CustomIntField(new Rect(), new GUIContent(), StationaryIdleSecondsMaxProp, "Max Idle Animation Seconds");
                        CustomHelpLabelField("When using more than 1 idle animation, this controls the maximum amount of seconds needed before switching to the next idle " +
                            "animation. This will be randomized with the Min Idle Animation Seconds.", true);
                    }
                    else if (self.WanderTypeRef == EmeraldAISystem.WanderType.Destination)
                    {
                        CustomHelpLabelField("Destination - Allows an AI to travle to a single destination. Once it reaches it destination, it will stay stationary.", true);

                        if (GUILayout.Button("Reset Destination Point"))
                        {
                            self.SingleDestination = self.transform.position + self.transform.forward * 2;
                        }
                    }

                    EditorGUILayout.Space();

                    if (self.WanderTypeRef == EmeraldAISystem.WanderType.Dynamic)
                    {
                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), WanderRadiusProp, "Dynamic Wander Radius", ((int)self.StoppingDistance + 3), 300);
                        CustomHelpLabelField("Controls the radius that the AI uses to wander. The AI will randomly pick waypoints within this radius.", true);

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaxSlopeLimitProp, "Max Slope Limit", 10, 60);
                        CustomHelpLabelField("Controls the maximum slope that a waypoint can be generated on.", true);
                    }

                    EditorGUILayout.EndVertical();
                }

                if (self.WanderTypeRef == EmeraldAISystem.WanderType.Destination)
                {
                    if (self.SingleDestination == Vector3.zero)
                    {
                        self.SingleDestination = new Vector3(self.transform.position.x, self.transform.position.y, self.transform.position.z + 5);
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                CombatStyleFoldout.boolValue = CustomEditorProperties.Foldout(CombatStyleFoldout.boolValue, "Combat Style", true, myFoldoutStyle);

                if (CombatStyleFoldout.boolValue)
                {
                    EditorGUILayout.BeginVertical("Box");
                    if (self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("(Companion AI Only)", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.LabelField("Controls whether a Companion AI will fight Offensively or Defensively.", EditorStyles.helpBox);
                    EditorGUILayout.PropertyField(CombatTypeProp, new GUIContent("Combat Style"));
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);

                    if (self.CombatTypeRef == EmeraldAISystem.CombatType.Defensive)
                    {
                        EditorGUILayout.LabelField("Defensive - Defensive Companioin AI will only attack a target if it is in active combat mode.", EditorStyles.helpBox);
                    }
                    else if (self.CombatTypeRef == EmeraldAISystem.CombatType.Offensive)
                    {
                        EditorGUILayout.LabelField("Offensive - Offensive Companioin AI will attack any target that is within their trigger radius.", EditorStyles.helpBox);
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                }
            }

            //AI's Settings
            if (TabNumberProp.intValue == 1)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 45));
                var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(new GUIContent(SettingsIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                EditorGUILayout.LabelField("AI's Settings", style, GUILayout.ExpandWidth(true));
                GUILayout.Space(4);
                GUIContent[] AIStatsButtons = new GUIContent[7] { new GUIContent("Stats"), new GUIContent("Movement"), new GUIContent("Combat"), new GUIContent("NavMesh"), new GUIContent("Optimize"), new GUIContent("Events"), new GUIContent("Items") };
                TemperamentTabNumberProp.intValue = GUILayout.Toolbar(TemperamentTabNumberProp.intValue, AIStatsButtons, EditorStyles.miniButton, GUILayout.Height(25), GUILayout.Width(EditorGUIUtility.currentViewWidth - 45));
                GUILayout.Space(2);
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                if (TemperamentTabNumberProp.intValue == 0)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Controls all of an AI's stats such as health, name, and level.", EditorStyles.helpBox);
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(AINameProp, new GUIContent("AI's Name"));
                    CustomHelpLabelField("The name of the AI. This can be displayed with Emerald's built-in UI system or a custom one.", true);
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(AITitleProp, new GUIContent("AI's Title"));
                    CustomHelpLabelField("The title of the AI. This can be displayed with Emerald's built-in UI system or a custom one.", true);
                    EditorGUILayout.Space();

                    CustomEditorProperties.CustomIntField(new Rect(), new GUIContent(), AILevelProp, "AI's Level");
                    CustomHelpLabelField("The level of the AI. This can be displayed with Emerald's built-in UI system or a custom one.", true);
                    EditorGUILayout.Space();

                    CustomEditorProperties.CustomIntField(new Rect(), new GUIContent(), StartingHealthProp, "Health");
                    CustomHelpLabelField("Controls how much health this AI has.", true);
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(RefillHealthTypeProp, new GUIContent("Regenerate Health Type"));
                    CustomHelpLabelField("Controls how your AI regenerates health when not in combat or their max chase/flee distance is met or exceeded.", true);
                    EditorGUILayout.Space();

                    if (self.RefillHealthType == EmeraldAISystem.RefillHealth.OverTime)
                    {
                        CustomEditorProperties.CustomFloatField(new Rect(), new GUIContent(), HealthRegRateProp, "Regen Rate");
                        CustomHelpLabelField("Controls, in seconds, the rate in which health is regenerated.", true);

                        CustomEditorProperties.CustomIntField(new Rect(), new GUIContent(), RegenerateAmountProp, "Regen Amount");
                        CustomHelpLabelField("Controls how much health is refilled after each Regen Rate.", true);
                    }

                    EditorGUILayout.EndVertical();
                }

                if (TemperamentTabNumberProp.intValue == 2)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Combat Settings", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("The Combat section controls all combat related settings. Some AI, such as Cautious AI with a Coward Confidence Type, " +
                        "will not use the Combat section.", EditorStyles.helpBox);

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    GUIContent[] CombatButtons = new GUIContent[3] { new GUIContent("Damage Settings"), new GUIContent("Combat Actions"), new GUIContent("Hit Effect") };
                    CombatTabNumberProp.intValue = GUILayout.Toolbar(CombatTabNumberProp.intValue, CombatButtons, EditorStyles.miniButton, GUILayout.Height(25), GUILayout.Width(EditorGUIUtility.currentViewWidth-60));
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(2);

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    if (CombatTabNumberProp.intValue == 0)
                    {
                        if (!Application.isPlaying)
                        {
                            if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Both)
                            {
                                self.EnableBothWeaponTypes = EmeraldAISystem.YesOrNo.Yes;
                            }
                            else
                            {
                                self.EnableBothWeaponTypes = EmeraldAISystem.YesOrNo.No;
                            }
                        }

                        EditorGUILayout.BeginVertical("Box");

                        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                        EditorGUILayout.BeginVertical("Box");

                        EditorGUILayout.LabelField("Damage Settings", EditorStyles.boldLabel);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndVertical();
                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No)
                        {
                            if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee || self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                            {
                                EditorGUILayout.Space();
                                CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), AttackDistanceProp, "Attack Distance", 0.5f, 200);
                                CustomHelpLabelField("Controls the distance in which an AI will attack.", true);

                                if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                                {
                                    EditorGUILayout.Space();
                                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), AttackHeightProp, "Attack Height", 0.5f, 20f);
                                    CustomHelpLabelField("Controls the height allowed in order for an AI to trigger a melee attack. Note: Setting this value too low can result in the " +
                                        "AI not being able to attack properly. This setting is intended to stop AI from attacking their target that is too far above or below them.", true);

                                    EditorGUILayout.PropertyField(AttackOnArrivalProp, new GUIContent("Attack on Arrival"));
                                    CustomHelpLabelField("Controls whether an AI will attack its target right when it comes within range. After the initial hit, the AI will rely on its attack speed.", true);
                                }

                                CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), TooCloseDistanceProp, "Too Close Distance", 0f, 25);
                                CustomHelpLabelField("Controls the distance for when an AI will backup from its target. This is useful for keeping an AI's targets from getting too close.", true);
                            }
                        }

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MinimumAttackSpeedProp, "Min Attack Speed", 0, 8);
                        CustomHelpLabelField("Controls the minimum amount of time it takes for your AI to attack. This amount is randomized with Maximum Attack Speed.", true);

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaximumAttackSpeedProp, "Max Attack Speed", 0, 8);
                        CustomHelpLabelField("Controls the maximum amount of time it takes for your AI to attack. This amount is randomized with Minimum Attack Speed.", true);

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), SentRagdollForceProp, "Ragdoll Force", 50, 6000);
                        CustomHelpLabelField("Controls the amount of force that is applied to this all of this AI's attacks which affects the target's ragdoll when they die.", false);
                        EditorGUILayout.Space();

                        if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
                        {
                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), SwitchWeaponTypesDistanceProp, "Switch Weapon Type Distance", 2, 15);
                            CustomHelpLabelField("Controls the distance in which the AI will switch to betweem melee and ranged damage. Any distance at or below this amount will be melee" +
                                " and any value greater will be ranged.", true);

                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), SwitchWeaponTypesCooldownProp, "Switch Weapon Type Cooldown", 1, 30);
                            CustomHelpLabelField("Controls the cooldown in which an AI will switch between ranged and melee combat, if the Switch Weapon Type Distance has been met. This" +
                                " is to stop a weapon switch from happening too often.", true);

                            EditorGUILayout.PropertyField(WeaponTypeProp, new GUIContent("Weapon Type"));
                            CustomHelpLabelField("Controls whether your AI will use Ranged combat, Melee combat, or both.", true);

                            var style3 = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                            EditorGUILayout.Space();
                            EditorGUILayout.LabelField("Weapon Types", style3, GUILayout.ExpandWidth(true));
                            EditorGUILayout.Space();
                            GUIContent[] WeaponTypeButtons = new GUIContent[2] { new GUIContent("Melee Settings"), new GUIContent("Ranged Settings") };
                            WeaponTypeControlTabNumberProp.intValue = GUILayout.Toolbar(WeaponTypeControlTabNumberProp.intValue, WeaponTypeButtons, EditorStyles.miniButton, GUILayout.Height(25));
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(WeaponTypeProp, new GUIContent("Weapon Type"));
                            CustomHelpLabelField("Controls whether your AI will use Ranged combat, Melee combat, or both.", true);

                            var style3 = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                            EditorGUILayout.Space();
                            GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                            EditorGUILayout.BeginVertical("Box");
                            if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                            {
                                EditorGUILayout.LabelField("Melee Settings", style3);
                            }
                            else if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                            {
                                EditorGUILayout.LabelField("Ranged Settings", style3);
                            }
                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();
                        }

                        if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged && self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No
                            || self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && WeaponTypeControlTabNumberProp.intValue == 1)
                        {
                            if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
                            {
                                EditorGUILayout.LabelField("Ranged Settings", EditorStyles.boldLabel);
                            }

                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(15);
                            EditorGUILayout.BeginVertical();

                            if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && WeaponTypeControlTabNumberProp.intValue == 1)
                            {
                                EditorGUILayout.Space();
                                CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), RangedAttackDistanceProp, "Ranged Attack Distance", 10, 400);
                                CustomHelpLabelField("Controls the distance in which an AI will attack with ranged attacks.", true);

                                CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), RangedTooCloseDistanceProp, "Ranged Too Close Distance", 0f, 30);
                                CustomHelpLabelField("Controls the distance for when an AI will backup. This is useful for ranged AI keeping their" +
                                    " distance from attackers.", true);
                            }

                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaxFiringAngleProp, "Max Firing Angle", 15, 180);
                            CustomHelpLabelField("Controls the max angle that will allow the AI to fire a projectile.", true);


                            CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), RangedAttackTransformProp, "Ranged Attack Transform", typeof(Transform), true);
                            CustomHelpLabelField("The transfrom that's used to for spawning ranged attacks. This transform can be anything on your AI such as their hand " +
                                "for magic effects or their bow for bow and arrows.", true);

                            EditorGUILayout.PropertyField(TargetObstructedActionProp, new GUIContent("Obstructed Action"));
                            CustomHelpLabelField("Controls what action is done when the AI's target is not visible when using the Ranged Weapon Type.", true);

                            if (self.TargetObstructedActionRef == EmeraldAISystem.TargetObstructedAction.MoveCloserAfterSetSeconds)
                            {
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(15);
                                EditorGUILayout.BeginVertical();

                                CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), ObstructionSecondsProp, "Obstruction Seconds", 1, 10);
                                CustomHelpLabelField("Controls how many seconds before the AI will move closer to its target after it has become obstructed.", false);

                                EditorGUILayout.EndVertical();
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.Space();
                            }

                            CustomEditorProperties.CustomFloatField(new Rect(), new GUIContent(), PlayerYOffsetProp, "Player Offset Y Position");
                            CustomHelpLabelField("Controls the Y position offset when firing projectiles at a player. Depending on the character controller, this may need to be " +
                                "adjusted to ensure the projectiles are not firing too high or too low.", true);

                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                        }

                        if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && WeaponTypeControlTabNumberProp.intValue == 1)
                        {
                            EditorGUILayout.LabelField("Projectiles", EditorStyles.boldLabel);
                            CustomHelpLabelField("Total projectile amounts are based on how many attack animations your AI is currently using.", false);
                        }
                        else if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No
                            || self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && WeaponTypeControlTabNumberProp.intValue == 0)
                        {
                            if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
                            {
                                EditorGUILayout.LabelField("Melee Settings", EditorStyles.boldLabel);
                            }

                            if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && WeaponTypeControlTabNumberProp.intValue == 0)
                            {
                                EditorGUILayout.Space();
                                CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), AttackDistanceProp, "Attack Distance", 0.5f, 40);
                                CustomHelpLabelField("Controls the distance in which an AI will attack with melee attacks.", true);

                                CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), AttackHeightProp, "Attack Height", 0.5f, 20f);
                                CustomHelpLabelField("Controls the height allowed in order for an AI to trigger a melee attack. Note: Setting this value too low can result in the " +
                                    "AI not being able to attack properly. This setting is intended to stop AI from being able to attack too far above or below the player.", true);

                                EditorGUILayout.PropertyField(AttackOnArrivalProp, new GUIContent("Attack on Arrival"));
                                CustomHelpLabelField("Controls whether your AI will attack its target right when it comes within range. After the initial hit, the AI will rely on its attack speed.", true);

                                CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), TooCloseDistanceProp, "Too Close Distance", 0f, 25);
                                CustomHelpLabelField("Controls the distance for when an AI will backup from its target. This is useful for keeping an AI's targets from getting too close.", true);
                            }

                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaxDamageAngleProp, "Max Damage Angle", 45, 180);
                            CustomHelpLabelField("Controls the max angle that will allow attacks to damage targets. For example, if this setting is set to 90, the AI will be able to damage its" +
                                " target while it's within 90 degrees of the AI. Anything greater than this amount will stop damage from being inflicted.", true);

                            EditorGUILayout.LabelField("Damage Amounts", EditorStyles.boldLabel);
                            CustomHelpLabelField("Total damage amounts are based on how many attack animations your AI is currently using.", false);
                        }
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        EditorGUILayout.BeginVertical();

                        if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No
                            || self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && WeaponTypeControlTabNumberProp.intValue == 0)
                        {
                            EditorGUILayout.Space();
                            EditorGUILayout.PropertyField(RandomizeDamageProp, new GUIContent("Use Random Damage"));
                            CustomHelpLabelField("Use Random Damage controls whether your AI will randomize their damage or use a constant damage amount.", true);

                            EditorGUILayout.PropertyField(UseCriticalHitsProp, new GUIContent("Use Critical Hits"));
                            CustomHelpLabelField("Controls whether or not attacks will use critical hits. When a critical hit is successful, the AI's damage is multiplied by the Critical Hit Multiplier.", false);

                            if (self.UseCriticalHits == EmeraldAISystem.YesOrNo.Yes)
                            {
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(15);
                                EditorGUILayout.BeginVertical();

                                CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), CritChanceProp, "Critical Hit Odds", 1, 100);
                                CustomHelpLabelField("Controls the odds percentage in which a critical hit will happen.", true);

                                CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), CritMultiplierProp, "Critical Hit Multiplier", 1.1f, 10);
                                CustomHelpLabelField("Controls the damage multiplier when a critical hit is successful", true);

                                EditorGUILayout.EndVertical();
                                EditorGUILayout.EndHorizontal();
                            }
                        }

                        if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No
                            || self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && WeaponTypeControlTabNumberProp.intValue == 0)
                        {
                            EditorGUILayout.Space();
                            EditorGUILayout.PropertyField(MeleeAttackPickTypeProp, new GUIContent("Pick Type"));
                            CustomHelpLabelField("Controls how Melee Attacks are picked.", false);

                            if (self.MeleeAttackPickType == EmeraldAISystem.MeleeAttackPickTypeEnum.Odds)
                            {
                                CustomHelpLabelField("Odds - Melee Attacks are picked based off of each of the Melee Attack's odds.", true);
                            }
                            else if (self.MeleeAttackPickType == EmeraldAISystem.MeleeAttackPickTypeEnum.Order)
                            {
                                CustomHelpLabelField("Order - Melee Attacks are picked based on the order of the AI's Melee Attacks list.", true);
                            }
                            else if (self.MeleeAttackPickType == EmeraldAISystem.MeleeAttackPickTypeEnum.Random)
                            {
                                CustomHelpLabelField("Random - Melee Attacks are picked randomly from the AI's Melee Attacks list.", true);
                            }
                            EditorGUILayout.Space();

                            CustomHelpLabelField("A list of an AI's melee attacks. You can hover the mouse over each setting to view its tooltip.", false);
                            if (self.AttackAnimationList.Count == 0 || MeleeEnumAnimations == null)
                            {
                                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                EditorGUILayout.HelpBox("Please add at least 1 Melee Attack animation to the Melee Attack Animation list (located under Animations>Combat) to " +
                                    "choose the type of animations these melee attacks will use.", MessageType.Warning);
                                GUI.backgroundColor = Color.white;
                            }
                            MeleeAttacks.DoLayoutList();
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();
                        }

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();

                        //Ranged 
                        if (self.TotalRangedAttackAnimations >= 0)
                        {
                            if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged && self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No
                                || self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && WeaponTypeControlTabNumberProp.intValue == 1)
                            {
                                var style3 = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                                EditorGUILayout.Space();
                                GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                                EditorGUILayout.BeginVertical("Box");
                                EditorGUILayout.LabelField("Abilities", style3);
                                GUI.backgroundColor = Color.white;
                                EditorGUILayout.EndVertical();
                                CustomHelpLabelField("Below you can customize what abilities an AI will use. This can be as simple as an offensive arrow projectile for an " +
                                    "archer or a mage that can utilize all ability categories.", false);

                                GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                                EditorGUILayout.LabelField("Note: You will need to ensure you are applying the Ability Objects to the proper categories. " +
                                    "For a tutorial on creating Ability Objects, please see the tutorial below.", EditorStyles.helpBox);
                                GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
                                if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
                                {
                                    Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Creating-an-AI-Ability-Object");
                                }
                                GUI.backgroundColor = Color.white;
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();

                                EditorGUILayout.Space();
                                EditorGUILayout.Space();

                                EditorGUILayout.Space();
                                GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                                EditorGUILayout.BeginVertical("Box");
                                EditorGUILayout.LabelField("Offensive Abilities", EditorStyles.boldLabel);
                                GUI.backgroundColor = Color.white;
                                EditorGUILayout.EndVertical();

                                //Offensive
                                EditorGUILayout.PropertyField(OffensiveAbilityPickTypeProp, new GUIContent("Pick Type"));
                                CustomHelpLabelField("Controls how Offensive Abilities are picked.", false);

                                if (self.OffensiveAbilityPickType == EmeraldAISystem.OffensiveAbilityPickTypeEnum.Odds)
                                {
                                    CustomHelpLabelField("Odds - Offensive Abilities are picked based off of each of the ability's odds.", true);
                                }
                                else if (self.OffensiveAbilityPickType == EmeraldAISystem.OffensiveAbilityPickTypeEnum.Order)
                                {
                                    CustomHelpLabelField("Order - Offensive Abilities are picked based on the order of the AI's Offensive Abilities list.", true);
                                }
                                else if (self.OffensiveAbilityPickType == EmeraldAISystem.OffensiveAbilityPickTypeEnum.Random)
                                {
                                    CustomHelpLabelField("Random - Offensive Abilities are picked randomly from the AI's Offensive Abilities list.", true);
                                }
                                EditorGUILayout.Space();

                                CustomHelpLabelField("A list of an AI's offensive abilities. You can hover the mouse over each setting to view its tooltip", false);
                                if (self.RangedAttackAnimationList.Count == 0 || RangedEnumAnimations == null)
                                {
                                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                    EditorGUILayout.HelpBox("Please add at least 1 Ranged Attack animation to the Ranged Attack Animation list (located under Animations>Combat) to " +
                                        "choose the type of animations these abilities will use.", MessageType.Warning);
                                    GUI.backgroundColor = Color.white;
                                }
                                OffensiveAbilities.DoLayoutList();
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();

                                EditorGUILayout.Space();
                                EditorGUILayout.Space();

                                EditorGUILayout.Space();
                                GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                                EditorGUILayout.BeginVertical("Box");
                                EditorGUILayout.LabelField("Support Abilities", EditorStyles.boldLabel);
                                GUI.backgroundColor = Color.white;
                                EditorGUILayout.EndVertical();

                                //Support
                                EditorGUILayout.PropertyField(SupportAbilityPickTypeProp, new GUIContent("Pick Type"));
                                CustomHelpLabelField("Controls how Support Abilities are picked.", false);

                                if (self.SupportAbilityPickType == EmeraldAISystem.SupportAbilityPickTypeEnum.Odds)
                                {
                                    CustomHelpLabelField("Odds - Support Abilities are picked based off of each of the ability's odds.", true);
                                }
                                else if (self.SupportAbilityPickType == EmeraldAISystem.SupportAbilityPickTypeEnum.Order)
                                {
                                    CustomHelpLabelField("Order - Support Abilities are picked based on the order of the AI's Support Abilities list.", true);
                                }
                                else if (self.SupportAbilityPickType == EmeraldAISystem.SupportAbilityPickTypeEnum.Random)
                                {
                                    CustomHelpLabelField("Random - Support Abilities are picked randomly from the AI's Support Abilities list.", true);
                                }
                                EditorGUILayout.Space();

                                CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), HealthPercentageToHealProp, "Health Percentage to Heal", 1, 100);
                                CustomHelpLabelField("Controls the health amount needed for this AI to heal cast support abilities such as healing.", true);

                                CustomHelpLabelField("A list of an AI's Support abilities. You can hover the mouse over each setting to view its tooltip", false);
                                if (self.RangedAttackAnimationList.Count == 0 || RangedEnumAnimations == null)
                                {
                                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                    EditorGUILayout.HelpBox("Please add at least 1 Ranged Attack animation to the Ranged Attack Animation list (located under Animations>Combat) to " +
                                        "choose the type of animations these abilities will use.", MessageType.Warning);
                                    GUI.backgroundColor = Color.white;
                                }
                                SupportAbilities.DoLayoutList();
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();

                                EditorGUILayout.Space();
                                EditorGUILayout.Space();

                                EditorGUILayout.Space();
                                GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                                EditorGUILayout.BeginVertical("Box");
                                EditorGUILayout.LabelField("Summon Abilities", EditorStyles.boldLabel);
                                GUI.backgroundColor = Color.white;
                                EditorGUILayout.EndVertical();

                                //Summoning
                                EditorGUILayout.PropertyField(SummoningAbilityPickTypeProp, new GUIContent("Pick Type"));
                                CustomHelpLabelField("Controls how Summoning Abilities are picked.", false);

                                if (self.SummoningAbilityPickType == EmeraldAISystem.SummoningAbilityPickTypeEnum.Odds)
                                {
                                    CustomHelpLabelField("Odds - Summoning Abilities are picked based off of each of the ability's odds.", true);
                                }
                                else if (self.SummoningAbilityPickType == EmeraldAISystem.SummoningAbilityPickTypeEnum.Order)
                                {
                                    CustomHelpLabelField("Order - Summoning Abilities are picked based on the order of the AI's Summoning Abilities list.", true);
                                }
                                else if (self.SummoningAbilityPickType == EmeraldAISystem.SummoningAbilityPickTypeEnum.Random)
                                {
                                    CustomHelpLabelField("Random - Summoning Abilities are picked randomly from the AI's Summoning Abilities list.", true);
                                }
                                EditorGUILayout.Space();

                                EditorGUILayout.PropertyField(SummonsMultipleAIProp, new GUIContent("Summons Multiple AI"));
                                CustomHelpLabelField("Controls whether or not this AI can summon more than 1 after the intilial one dies. " +
                                    "If disabled, this AI will only summon 1 AI, even if the first summoned AI dies.", true);

                                if (self.SummonsMultipleAI == EmeraldAISystem.EnableDisable.Enabled)
                                {
                                    CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaxAllowedSummonedAIProp, "Max Allowed Summoned AI", 1, 3);
                                    CustomHelpLabelField("Controls the maximum allowed AI that this AI can have spanwed at a time.", true);
                                }

                                CustomHelpLabelField("A list of an AI's Summoning abilities. You can hover the mouse over each setting to view its tooltip", false);
                                if (self.RangedAttackAnimationList.Count == 0 || RangedEnumAnimations == null)
                                {
                                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                    EditorGUILayout.HelpBox("Please add at least 1 Ranged Attack animation to the Ranged Attack Animation list (located under Animations>Combat) to " +
                                        "choose the type of animations these abilities will use.", MessageType.Warning);
                                    GUI.backgroundColor = Color.white;
                                }
                                SummoningAbilities.DoLayoutList();
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();
                            }
                        }

                        //Run Attacks
                        if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && WeaponTypeControlTabNumberProp.intValue == 0 || self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                        {
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();
                            EditorGUILayout.PropertyField(UseRunAttacksProp, new GUIContent("Use Run Attacks"));
                            CustomHelpLabelField("Controls whether or not this AI will use run attacks.", true);

                            if (self.UseRunAttacksRef == EmeraldAISystem.UseRunAttacks.Yes)
                            {
                                var style4 = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                                EditorGUILayout.Space();
                                GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                                EditorGUILayout.BeginVertical("Box");
                                EditorGUILayout.LabelField("Run Attack Settings", style4);
                                GUI.backgroundColor = Color.white;
                                EditorGUILayout.EndVertical();
                                EditorGUILayout.Space();

                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(15);
                                EditorGUILayout.BeginVertical();

                                EditorGUILayout.Space();
                                CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MinimumRunAttackSpeedProp, "Min Run Attack Speed", 1, 8);
                                CustomHelpLabelField("Controls the minimum amount of time it takes for your AI to attack with a run attack. This amount is randomized with Maximum Run Attack Speed.", true);

                                CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaximumRunAttackSpeedProp, "Max Run Attack Speed", 1, 8);
                                CustomHelpLabelField("Controls the maximum amount of time it takes for your AI to attack with a run attack. This amount is randomized with Minimum Run Attack Speed.", true);

                                CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), RunAttackDistanceProp, "Run Attack Distance", 1f, 15);
                                CustomHelpLabelField("Controls the additional distance in which an AI will attack with its running animation. This value is based off of your AI's Attack Distance.", true);

                                if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No
                                    || self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && WeaponTypeControlTabNumberProp.intValue == 0)
                                {
                                    EditorGUILayout.Space();
                                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                                    EditorGUILayout.BeginVertical("Box");
                                    EditorGUILayout.LabelField("Run Attack", EditorStyles.boldLabel);
                                    GUI.backgroundColor = Color.white;
                                    EditorGUILayout.EndVertical();
                                    GUI.backgroundColor = Color.white;

                                    EditorGUILayout.Space();
                                    EditorGUILayout.PropertyField(MeleeRunAttackPickTypeProp, new GUIContent("Pick Type"));
                                    CustomHelpLabelField("Controls how Run Melee Attacks are picked.", false);

                                    if (self.MeleeRunAttackPickType == EmeraldAISystem.MeleeRunAttackPickTypeEnum.Odds)
                                    {
                                        CustomHelpLabelField("Odds - Melee Run Attacks are picked based off of each of the Melee Attack's odds.", true);
                                    }
                                    else if (self.MeleeRunAttackPickType == EmeraldAISystem.MeleeRunAttackPickTypeEnum.Order)
                                    {
                                        CustomHelpLabelField("Order - Melee Run Attacks are picked based on the order of the AI's Melee Run Attacks list.", true);
                                    }
                                    else if (self.MeleeRunAttackPickType == EmeraldAISystem.MeleeRunAttackPickTypeEnum.Random)
                                    {
                                        CustomHelpLabelField("Random - Melee Run Attacks are picked randomly from the AI's Melee Run Attacks list.", true);
                                    }
                                    EditorGUILayout.Space();

                                    CustomHelpLabelField("A list of an AI's melee run attacks. You can hover the mouse over each setting to view its tooltip.", false);
                                    if (self.RunAttackAnimationList.Count == 0 || MeleeEnumAnimations == null)
                                    {
                                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                        EditorGUILayout.HelpBox("Please add at least 1 Melee Run Attack animation to the Melee Run Attack Animation list (located under Animations>Combat) to " +
                                            "choose the type of animations these run melee attacks will use.", MessageType.Warning);
                                        GUI.backgroundColor = Color.white;
                                    }
                                    MeleeRunAttacks.DoLayoutList();
                                    EditorGUILayout.Space();
                                    EditorGUILayout.Space();
                                }

                                EditorGUILayout.EndVertical();
                                EditorGUILayout.EndHorizontal();
                            }
                        }

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }

                    if (CombatTabNumberProp.intValue == 1)
                    {
                        EditorGUILayout.BeginVertical("Box");

                        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.LabelField("Combat Actions", EditorStyles.boldLabel);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndVertical();
                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        EditorGUILayout.PropertyField(UseWeaponObjectProp, new GUIContent("Use Weapon Object"));
                        CustomHelpLabelField("Controls whether or not this AI will enable and disable its weapon object when switching between combat and non-combat mode.", true);

                        if (self.UseWeaponObject == EmeraldAISystem.YesOrNo.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(10);
                            EditorGUILayout.BeginVertical();

                            if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
                            {
                                MeleeWeaponString = "Melee ";
                            }
                            else
                            {
                                MeleeWeaponString = "";
                            }

                            if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee ||
                                self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                            {
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), WeaponObjectProp, MeleeWeaponString + "Weapon Object", typeof(GameObject), false);
                                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                                EditorGUILayout.LabelField("The weapon model that the AI uses. This can be enabled and disabled with an Animation Event and " +
                                    "should be an item that's placed on your AI's hand transform.", EditorStyles.helpBox);
                                GUI.backgroundColor = Color.white;
                            }

                            if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged ||
                                self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                            {
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), RangedWeaponObjectProp, "Ranged Weapon Object", typeof(GameObject), false);
                                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                                EditorGUILayout.LabelField("The weapon model that the AI uses. This can be enabled and disabled with an Animation Event and " +
                                    "should be an item that's placed on your AI's hand transform.", EditorStyles.helpBox);
                                GUI.backgroundColor = Color.white;
                                EditorGUILayout.Space();
                            }

                            if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
                            {
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), WeaponObjectProp, "Melee Weapon Object", typeof(GameObject), false);
                                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                                EditorGUILayout.LabelField("The Melee weapon model that the AI uses. This can be enabled and disabled with an Animation Event and " +
                                    "should be an item that's placed on your AI's hand transform.", EditorStyles.helpBox);
                                GUI.backgroundColor = Color.white;
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), RangedWeaponObjectProp, "Ranged Weapon Object", typeof(GameObject), false);
                                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                                EditorGUILayout.LabelField("The Ranged weapon model that the AI uses. This can be enabled and disabled with an Animation Event and " +
                                    "should be an item that's placed on your AI's hand transform.", EditorStyles.helpBox);
                                GUI.backgroundColor = Color.white;
                                EditorGUILayout.Space();
                            }

                            GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                            EditorGUILayout.LabelField("Note: You will need to manually create an EnableWeapon or DisableWeapon Animation Event to use this feature. " +
                                "Please refer to Emerlad's documentation for a tutorial on how to do this.", EditorStyles.helpBox);
                            GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
                            if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
                            {
                                Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Setting-up-an-AIs-Sheathable-Weapons");
                            }
                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(UseDroppableWeaponProp, new GUIContent("Use Droppable Weapon"));
                            CustomHelpLabelField("Controls whether or not this AI will drop a weapon object when it dies.", true);

                            if (self.UseDroppableWeapon == EmeraldAISystem.YesOrNo.Yes)
                            {
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(10);
                                EditorGUILayout.BeginVertical();

                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), DroppableWeaponObjectProp, MeleeWeaponString + "Droppable Weapon Object", typeof(GameObject), false);
                                EditorGUILayout.Space();

                                if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
                                {
                                    CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), RangedDroppableWeaponObjectProp, "Ranged Droppable Weapon Object", typeof(GameObject), false);
                                    EditorGUILayout.Space();
                                }

                                CustomHelpLabelField("The object that is dropped when the AI dies.", false);
                                GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                                EditorGUILayout.LabelField("Note: This should be a prefab/copy of the AI's Weapon Object and should have a collider and Rigidbody attached to it. " +
                                    "The AI's above Weapon Object will automatically be disabled on death.", EditorStyles.helpBox);
                                GUI.backgroundColor = Color.white;
                                EditorGUILayout.Space();

                                EditorGUILayout.EndVertical();
                                EditorGUILayout.EndHorizontal();
                            }

                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                        }

                        if (self.UseHitAnimations == EmeraldAISystem.YesOrNo.No)
                        {
                            GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                            EditorGUILayout.LabelField("Blocking can only be enabled if Use Hit Animation are enabled. This is located in the Animations>Combat tab.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                        }
                        EditorGUI.BeginDisabledGroup(self.UseHitAnimations == EmeraldAISystem.YesOrNo.No);
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(UseBlockingProp, new GUIContent("Use Blocking"));
                        CustomHelpLabelField("Controls whether or not this AI will have the ability to block. AI who use block must have a Block and Block Hit animation " +
                            "as well as have Use Hit Animations enabled.", true);

                        if (self.UseBlockingRef == EmeraldAISystem.YesOrNo.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(10);
                            EditorGUILayout.BeginVertical();

                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), BlockingOddsProp, "Blocking Odds", 1, 100);
                            CustomHelpLabelField("Controls the odds, in percent, of an AI blocking. Currently, your AI will use blocking "
                                + self.BlockOdds + "% of the time while not attacking.", true);

                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MitigationAmountProp, "Block Mitigation", 0, 100);
                            CustomHelpLabelField("Controls the percentage of damage that is mitigated from blocking. Currently, damage will be reduced by " + self.MitigationAmount + "%.", true);

                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaxBlockingAngleProp, "Max Block Angle", 5, 100);
                            CustomHelpLabelField("Controls the maximum angle an AI can block an attack from its attacker. Any damage receieved at or below this angle will be a successful block.", true);

                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUI.EndDisabledGroup();

                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(UseAggroProp, new GUIContent("Use Aggro"));
                        CustomHelpLabelField("Controls whether or not this AI will use the Aggro system. This allows AI to switch targets after a certain amount of hits have been met.", true);

                        if (self.UseAggro == EmeraldAISystem.YesOrNo.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(10);
                            EditorGUILayout.BeginVertical();

                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), TotalAggroHitsProp, "Total Aggro Hits", 1, 100);
                            CustomHelpLabelField("Controls how many hits are needed before an AI switches targets according to their Aggro Action.", true);

                            EditorGUILayout.PropertyField(AggroActionProp, new GUIContent("Aggro Action"));

                            if (self.AggroActionRef == EmeraldAISystem.AggroAction.LastAttacker)
                            {
                                CustomHelpLabelField("Last Attacker - Will switch the AI's current target to the last attacker, after the total aggro hits have been met.", true);
                            }
                            else if (self.AggroActionRef == EmeraldAISystem.AggroAction.RandomAttacker)
                            {
                                CustomHelpLabelField("Random Attacker - Will switch the AI's current target to a random attacker within " +
                                    "the AI's attack radius, after the total aggro hits have been met.", true);
                            }
                            else if (self.AggroActionRef == EmeraldAISystem.AggroAction.ClosestAttacker)
                            {
                                CustomHelpLabelField("Closest Attacker - Will switch the AI's current target to the closest attacker within " +
                                    "the AI's attack radius, after the total aggro hits have been met.", true);
                            }

                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(BackupTypeProp, new GUIContent("Backup Type"));
                        CustomHelpLabelField("Controls how this AI's backup type with the option to disable the back up feature, if desried.", true);

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        if (self.BackupTypeRef == EmeraldAISystem.BackupType.Off)
                        {
                            CustomHelpLabelField("Off - Disables the AI's backup feature.", true);
                        }
                        else if (self.BackupTypeRef == EmeraldAISystem.BackupType.Instant)
                        {
                            CustomHelpLabelField("Instant - Allows the AI to backup instantly whenever a target reaches their 'Too Close Distance'.", true);
                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), BackupSecondsMinProp, "Backup Seconds Min", 1, 8);
                            CustomHelpLabelField("Controls the minimum seconds an AI will backup for.", false);
                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), BackupSecondsMaxProp, "Backup Seconds Max", 1, 8);
                            CustomHelpLabelField("Controls the minimum seconds an AI will backup for.", true);
                        }
                        else if (self.BackupTypeRef == EmeraldAISystem.BackupType.Odds)
                        {
                            CustomHelpLabelField("Odds - Allows the AI to backup whenever a target reaches their 'Too Close Distance', but only if the generated odds have been met.", true);
                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), BackUpOddsProp, "Backup Odds", 1, 99);
                            CustomHelpLabelField("Controls how this AI's backup type with the option to disable the back up feature, if desried.", true);
                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), BackupSecondsMinProp, "Backup Seconds Min", 1, 8);
                            CustomHelpLabelField("Controls the minimum seconds an AI will backup for.", false);
                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), BackupSecondsMaxProp, "Backup Seconds Max", 1, 8);
                            CustomHelpLabelField("Controls the minimum seconds an AI will backup for.", true);
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();

                        if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
                        {
                            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                            EditorGUILayout.LabelField("(Not Usable with Companion AI)", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                        }
                        else if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet)
                        {
                            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                            EditorGUILayout.LabelField("(Not Usable with Pet AI)", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                        }

                        EditorGUILayout.Space();
                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), DeathDelayMinProp, "Min Resume Wandering", 0, 6);
                        CustomHelpLabelField("Controls the minimum amount of time with how quickly an AI will resume wandering according to its Wandering Type after it has engaged " +
                            "in combat. This amount will be randomized with the Maximum Resume Wandering Delay.", true);

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), DeathDelayMaxProp, "Max Resume Wandering", 0, 6);
                        CustomHelpLabelField("Controls the maximum amount of time with how quickly an AI will resume wandering according to its Wandering Type after it has engaged " +
                            "in combat. This amount will be randomized with the Minimum Resume Wandering Delay.", true);

                        if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
                        {
                            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                            EditorGUILayout.LabelField("(Not Usable with Companion AI)", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                        }
                        else if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet)
                        {
                            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                            EditorGUILayout.LabelField("(Not Usable with Pet AI)", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                        }

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), CautiousSecondsProp, "Cautious Seconds", 3, 12);
                        CustomHelpLabelField("Controls the amount of seconds before a Cautious AI will turn aggressive and attack.", true);

                        EditorGUILayout.PropertyField(DeathTypeRefProp, new GUIContent("Death Type"));
                        CustomHelpLabelField("Controls what method an AI uses when they die.", false);

                        if (self.DeathTypeRef == EmeraldAISystem.DeathType.Animation)
                        {
                            CustomHelpLabelField("Animation - Plays a random death animation when an AI dies.", true);
                            EditorGUILayout.PropertyField(SecondsToDisableProp, new GUIContent("Seconds to Disable"));
                            CustomHelpLabelField("Controls how many seconds until your AI is completely disabled. " +
                                "This is to ensure your AI has enough time for its Death Animation to finish playing.", false);
                        }
                        else
                        {
                            CustomHelpLabelField("Ragdoll - Enables an AI's ragdoll components on death allowng its current animation to blend with the ragdoll physics. " +
                                "Note: You must setup an AI's ragdoll components using Unity's built-in system or a custom ragdoll setup system.", false);
                            CustomEditorProperties.CustomTagField(new Rect(), new GUIContent(), RagdollTagProp, "Ragdoll Tag");
                            CustomHelpLabelField("Controls what tag the ragdoll components will be set to when an AI dies.", true);
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }

                    if (CombatTabNumberProp.intValue == 2)
                    {
                        EditorGUILayout.BeginVertical("Box");

                        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.LabelField("Hit Effect", EditorStyles.boldLabel);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndVertical();
                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        EditorGUILayout.PropertyField(UseBloodEffectProp, new GUIContent("Use Hit Effect"));
                        CustomHelpLabelField("Controls whether or not this AI will use a hit effect when it receives melee damage.", true);

                        if (self.UseBloodEffectRef == EmeraldAISystem.UseBloodEffect.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(10);
                            EditorGUILayout.BeginVertical();

                            CustomHelpLabelField("The hit effect that will appear when this AI receives damage.", true);
                            BloodEffectsList.DoLayoutList();
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(BloodEffectTimeoutSecondsProp, new GUIContent("Hit Effect Timeout Seconds"));
                            CustomHelpLabelField("Controls how long the hit effect will be visible before being deactivated.", true);
                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(BloodEffectPositionTypeProp, new GUIContent("Hit Effect Transform Type"));
                            CustomHelpLabelField("Controls the type of transform the Hit Effect Position will use.", false);

                            if (self.BloodEffectPositionTypeRef == EmeraldAISystem.BloodEffectPositionType.BaseTransform)
                            {
                                CustomHelpLabelField("Base Trasnfrom - Will spawn the hit effect according to the base position of the AI.", true);
                                EditorGUILayout.PropertyField(BloodPosOffsetProp, new GUIContent("Hit Effect Position"));
                                CustomHelpLabelField("Controls the offset position of the hit effect using the AI's base position.", true);
                            }
                            else
                            {
                                CustomHelpLabelField("Hit Transform - Will spawn the hit effect according to the AI's Hit Transform position.", true);
                                EditorGUILayout.PropertyField(BloodPosOffsetProp, new GUIContent("Hit Effect Position"));
                                CustomHelpLabelField("Controls the offset position of the hit effect using the AI's Hit Transform position.", true);
                            }
                            EditorGUILayout.Space();

                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                        }

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }
                }

                if (TemperamentTabNumberProp.intValue == 1)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Movement Settings", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.LabelField("Controls all of an AI's movement related settings such as movement speeds, alignment, turning, wait times, and stopping distances. " +
                        "An AI's Movement Type can be adjusted within the Temperament tab.", EditorStyles.helpBox);
                    EditorGUILayout.EndVertical();

                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.BeginVertical("Box");

                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Speed Settings", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.PropertyField(AnimatorTypeProp, new GUIContent("Movement Type"));
                    CustomHelpLabelField("Controls how an AI is moved. This is either driven by the Root Motion animation or by the NavMesh component.", true);

                    if ((EmeraldAISystem.AnimatorTypeState)self.m_LastAnimatorType != self.AnimatorType)
                    {
                        self.m_LastAnimatorType = (int)self.AnimatorType;
                        if (self.AnimatorControllerGenerated)
                        {
                            EmeraldAIAnimatorGenerator.GenerateAnimatorController(self);
                        }
                    }

                    if (self.AnimatorType == EmeraldAISystem.AnimatorTypeState.RootMotion)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        EditorGUILayout.PropertyField(AutoEnableAnimatePhysicsProp, new GUIContent("Auto Enable Animate Physics"));
                        CustomHelpLabelField("Controls whether or not this AI's Animator Controller will use Animate Physics. When enabled, animation quality and animation syncing is " +
                            "improved. Enable this if you are having issues with your animations not syncing properly with your AI's movement. ", true);

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }

                    if (self.AnimatorType == EmeraldAISystem.AnimatorTypeState.NavMeshDriven)
                    {
                        CustomFloatAnimationField(new Rect(), new GUIContent(), WalkSpeedProp, "Walk Speed", 0.5f, 5);
                        CustomHelpLabelField("Controls how fast your AI walks.", true);

                        CustomFloatAnimationField(new Rect(), new GUIContent(), RunSpeedProp, "Run Speed", 0.5f, 10);
                        CustomHelpLabelField("Controls how fast your AI runs.", true);

                        CustomFloatAnimationField(new Rect(), new GUIContent(), WalkBackwardsSpeedProp, "Walk Backwards Speed", 0.5f, 3f);
                        CustomHelpLabelField("Controls how fast your AI walks backwards.", true);

                        //Only auto-update the Animator Controller if inside the Unity Editor as runtime auto-updating is not possible.
#if UNITY_EDITOR
                        if (self.AnimatorControllerGenerated)
                        {
                            if (self.AnimationsUpdated || self.AnimationListsChanged)
                            {
                                EmeraldAIAnimatorGenerator.GenerateAnimatorController(self);
                            }
                        }
#endif
                    }
                    else if (self.AnimatorType == EmeraldAISystem.AnimatorTypeState.RootMotion)
                    {
                        GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                        EditorGUILayout.LabelField("When using Root Motion, an AI's Movement Speed is controlled by its animation speed. Please adjust this under the AI's Animation Settings.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        EditorGUI.BeginDisabledGroup(self.AnimatorType == EmeraldAISystem.AnimatorTypeState.RootMotion);
                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), WalkSpeedProp, "Walk Speed", 0.5f, 5);
                        CustomHelpLabelField("Controls how fast your AI walks.", true);

                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), RunSpeedProp, "Run Speed", 0.5f, 10);
                        CustomHelpLabelField("Controls how fast your AI runs.", true);

                        CustomFloatAnimationField(new Rect(), new GUIContent(), WalkBackwardsSpeedProp, "Walk Backwards Speed", 0.5f, 3f);
                        CustomHelpLabelField("Controls how fast your AI walks backwards.", true);

                        EditorGUI.EndDisabledGroup();
                    }

                    EditorGUILayout.PropertyField(CurrentMovementStateProp, new GUIContent("Movement Animation"));
                    CustomHelpLabelField("Controls the type of animation your AI will use when using waypoints, moving to its destination, or wandering. " +
                        "Note: If needed, this can be changed programmatically during runtime.", true);
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(TurnAnimationTypeProp, new GUIContent("Turn Animation Type"));
                    CustomHelpLabelField("Controls the type of animation your AI will use when turning in its non-combat state. Stationary will use an AI's stationary turn animation. " +
                        "Blend Tree will use the AI's blend tree/walk turn animation.", true);
                    EditorGUILayout.Space();

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), StoppingDistanceProp, "Stopping Distance", 0.25f, 40);
                    CustomHelpLabelField("Controls the distance in which an AI will stop before waypoints and targets.", true);

                    if (self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
                    {
                        EditorGUILayout.Space();
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("(Companion/Pet AI Only)", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), FollowingStoppingDistanceProp, "Following Stopping Distance", 1, 20);
                    CustomHelpLabelField("Controls the distance in which an AI will stop in front of their following targets.", true);

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");

                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Turn Settings", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), NonCombatAngleToTurnProp, "Turning Angle", 5, 90);
                    CustomHelpLabelField("Controls the angle needed to play a turn animation while an AI is not in combat. Emerald can automatically detect whether an AI is " +
                        "turing left or right. Note: You can use a walking animation in place of a turning animation if your AI doesn't one.", true);

                    CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), CombatAngleToTurnProp, "Combat Turning Angle", 1, 90);
                    CustomHelpLabelField("Controls the angle needed to play a turn animation while an AI is in combat. Emerald can automatically detect whether an AI is " +
                        "turing left or right. Note: You can use a walking animation in place of a turning animation if your AI doesn't one.", true);

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), AgentTurnSpeedProp, "Turn Speed", 30, 2500);
                    CustomHelpLabelField("Controls how fast your AI turns while not in combat or when chasing or fleeing from a target. Note: Lower speeds are meant for the Root Motion setting" +
                        " where the turning animations help assist an AI's turning.", true);

                    if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Coward && self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Cautious)
                    {
                        EditorGUILayout.Space();
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("(Not Usable with Cautious Coward AI)", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), StationaryTurningSpeedCombatProp, "Combat Turn Speed", 5, 300);
                    CustomHelpLabelField("Controls how fast your AI turns while in combat and is stationary. This setting does not apply when the AI is in combat and is moving.", true);

                    CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), BackupTurningSpeedProp, "Backup Turn Speed", 30, 300);
                    CustomHelpLabelField("Controls how fast your AI turns while backing up.", true);

                    EditorGUILayout.PropertyField(UseRandomRotationOnStartProp, new GUIContent("Random Roation on Start"));
                    CustomHelpLabelField("Controls whether or not AI will be randomly rotated on Start.", true);

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");

                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Wait Settings", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    CustomEditorProperties.CustomIntField(new Rect(), new GUIContent(), MinimumWaitTimeProp, "Min Wait Time");
                    CustomHelpLabelField("Controls the minimum amount of seconds before generating a new waypoint, when using the Dynamic and Random waypoint Wander Type. This amount is " +
                        "randomized with the Maximim Wait Time.", true);

                    CustomEditorProperties.CustomIntField(new Rect(), new GUIContent(), MaximumWaitTimeProp, "Max Wait Time");
                    CustomHelpLabelField("Controls the maximum amount of seconds before generating a new waypoint, when using the Dynamic and Random waypoint Wander Type. This amount " +
                        "is randomized with the Minimum Wait Time.", true);

                    if (self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
                    {
                        EditorGUILayout.Space();
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("(Companion/Pet AI Only)", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    CustomEditorProperties.CustomIntField(new Rect(), new GUIContent(), MinimumFollowWaitTimeProp, "Min Follow Time");
                    CustomHelpLabelField("Controls how quickly a Companion AI will react to following their target.", true);

                    if (self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
                    {
                        EditorGUILayout.Space();
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("(Companion/Pet AI Only)", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    CustomEditorProperties.CustomIntField(new Rect(), new GUIContent(), MaximumFollowWaitTimeProp, "Max Follow Time");
                    CustomHelpLabelField("Controls how quickly a Companion AI will react to following their target.", true);

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");

                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Alignment Settings", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.PropertyField(AlignAIWithGroundProp, new GUIContent("Align AI?"));
                    CustomHelpLabelField("Aligns the AI to the angle of the terrain and other objects for added realism. Disable this feature for improved performance per AI.", true);

                    if (self.AlignAIWithGroundRef == EmeraldAISystem.AlignAIWithGround.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        EditorGUI.BeginChangeCheck();
                        var layersSelection = EditorGUILayout.MaskField("Alignment Layers", LayerMaskToField(AlignmentLayerMaskProp.intValue), InternalEditorUtility.layers);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(self, "Layers changed");
                            AlignmentLayerMaskProp.intValue = FieldToLayerMask(layersSelection);
                        }
                        CustomHelpLabelField("The layers the AI will use for aligning itself with the angles of surfaces. Any layers not included above will be ignred.", true);

                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(AlignmentQualityProp, new GUIContent("Align Quality"));
                        CustomHelpLabelField("Controls the quality of the Align AI feature by controlling how often it's updated.", true);

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), NonCombatAlignSpeedProp, "Non-Combat Align Speed", 10, 35);
                        CustomHelpLabelField("Controls the speed in which the AI is aligned with the ground while not in combat.", true);

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), CombatAlignSpeedProp, "Combat Align Speed", 10, 40);
                        CustomHelpLabelField("Controls the speed in which the AI is aligned with the ground while in combat.", true);

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaxNormalAngleProp, "Max Angle", 5, 50);
                        CustomHelpLabelField("Controls the maximum angle for an AI to rotate to when aligning with the ground.", true);

                        EditorGUILayout.PropertyField(AlignAIOnStartProp, new GUIContent("Align on Start?"));
                        CustomHelpLabelField("Calculates the Align AI feature on Start.", true);

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();
                }

                if (TemperamentTabNumberProp.intValue == 3)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("NavMesh Settings", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Controls all of an AI's NavMesh related settings.", EditorStyles.helpBox);
                    EditorGUILayout.Space();

                    CustomEditorProperties.CustomFloatField(new Rect(), new GUIContent(), AgentBaseOffsetProp, "Agent Base Offset");
                    CustomHelpLabelField("Controls the NavMesh Agent's base offset. The base offset gives you control over how high your AI will be above the ground. " +
                        "If you AI is a flying AI type, you will want to adjust this value higher so your AI will be above the ground.", true);

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), AgentRadiusProp, "Agent Radius", 0.1f, 8);
                    CustomHelpLabelField("Controls the NavMesh Agent's avoidence radius.", true);

                    CustomEditorProperties.CustomFloatField(new Rect(), new GUIContent(), AgentAccelerationProp, "Agent Acceleration");
                    CustomHelpLabelField("Controls the NavMesh Agent's acceleration speed. The acceleration speed gives you control over how fast your AI will get to its max speed.", true);

                    EditorGUILayout.PropertyField(AvoidanceQualityProp, new GUIContent("Agent Avoidance Quality"));
                    CustomHelpLabelField("Controls the NavMesh Agent's avoidance quality which controls how accurately your AI will navigate through avoidable objects at " +
                        "the cost of perfomance. These objects include other AI and Nav Mesh Obstacles. If you would like to not use this feature, set the Avoidance Quality to None.", true);

                    EditorGUILayout.EndVertical();
                }

                if (TemperamentTabNumberProp.intValue == 4)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Optimization Settings", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Controls all of the settings that can help optimize an AI such as disabling an AI when their model is culled or not visible.", EditorStyles.helpBox);
                    EditorGUILayout.Space();

#if CRUX_PRESENT
                    EditorGUILayout.PropertyField(SpawnedWithCruxProp, new GUIContent("Spawned with Crux"));
                    CustomHelpLabelField("Controls whether or not this AI is being spawned with Crux - Procedural Spawner. This allows Crux to automatically remove and adjust the population of this AI when it's killed.", true);
#endif
                    EditorGUILayout.PropertyField(AnimatorCullingModeProp, new GUIContent("Animator Culling Mode"));
                    CustomHelpLabelField("Controls what type of Culling Mode this AI's Animator will use. Always Animate is recommended when using animated deaths as an AI can sometimes get stuck in T-pose if they die while off-screen.", true);

                    EditorGUILayout.PropertyField(DisableAIWhenNotInViewProp, new GUIContent("Disable when Off-Screen"));
                    CustomHelpLabelField("Controls whether or not this AI is disabled when off screen or is culled.", true);

                    if (self.DisableAIWhenNotInViewRef == EmeraldAISystem.YesOrNo.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        EditorGUILayout.PropertyField(UseDeactivateDelayProp, new GUIContent("Use Deactivate Delay"));
                        CustomHelpLabelField("Controls whether or not there is a delay when using the Disable when Off-Screen feature. If set to No, the AI will be disabled instantly.", true);

                        if (self.UseDeactivateDelayRef == EmeraldAISystem.YesOrNo.Yes)
                        {
                            CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), DeactivateDelayProp, "Deactivate Delay", 1, 30);
                            CustomHelpLabelField("Controls the amount of seconds until the AI will be disabled when either culled or off-screen.", true);
                        }

                        EditorGUILayout.PropertyField(HasMultipleLODsProp, new GUIContent("Has Multiple LODs"));
                        CustomHelpLabelField("Controls whether or not the Disable when Off-Screen feature will use multiple LODs. An AI using this feature must have at have " +
                            "an LOD Group with at least 2 levels. Note: If your AI has multiple LODs, this feature needs to be enabled in order for the Disable when " +
                            "Off-Screen feature to work.", true);

                        if (self.HasMultipleLODsRef == EmeraldAISystem.YesOrNo.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(10);
                            EditorGUILayout.BeginVertical();

                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("Auto Grab LODs will attempt to automatically grab your AI's LODs. Your AI must have a LOD Group component with at least 2 levels to use this feature.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                            if (GUILayout.Button("Auto Grab LODs"))
                            {
                                LODGroup _LODGroup = self.GetComponentInChildren<LODGroup>();

                                if (_LODGroup == null)
                                {
                                    EditorUtility.DisplayDialog("Error", "No LOD Group could be found. Please ensure that your AI has an LOD group that has at least 2 levels. The Multiple LOD Feature has been disabled.", "Okay");
                                    self.HasMultipleLODsRef = EmeraldAISystem.YesOrNo.No;
                                }
                                else if (_LODGroup != null)
                                {
                                    LOD[] AllLODs = _LODGroup.GetLODs();

                                    if (_LODGroup.lodCount <= 4)
                                    {
                                        TotalLODsProp.intValue = (_LODGroup.lodCount - 2);
                                    }

                                    if (_LODGroup.lodCount > 1)
                                    {
                                        for (int i = 0; i < _LODGroup.lodCount; i++)
                                        {
                                            if (i == 0)
                                            {
                                                Renderer1Prop.objectReferenceValue = AllLODs[i].renderers[0];
                                            }
                                            if (i == 1)
                                            {
                                                Renderer2Prop.objectReferenceValue = AllLODs[i].renderers[0];
                                            }
                                            if (i == 2)
                                            {
                                                Renderer3Prop.objectReferenceValue = AllLODs[i].renderers[0];
                                            }
                                            if (i == 3)
                                            {
                                                Renderer4Prop.objectReferenceValue = AllLODs[i].renderers[0];
                                            }
                                        }
                                    }
                                    else if (_LODGroup.lodCount == 1)
                                    {
                                        EditorUtility.DisplayDialog("Warning", "Your AI's LOD Group only has 1 level and it needs to have at least 2. The Multiple LOD feature has been disabled.", "Okay");
                                        self.HasMultipleLODsRef = EmeraldAISystem.YesOrNo.No;
                                    }
                                }
                            }

                            EditorGUILayout.Space();
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(TotalLODsProp, new GUIContent("Total LODs"));
                            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                            EditorGUILayout.LabelField("Controls the amount of LODs the Disable when Off-Screen feature will use. You will need to apply each level from your AI's " +
                                "LOD Group below. If any renderers are missing, this feature will be disabled.", EditorStyles.helpBox);
                            EditorGUILayout.LabelField("This feature only supports 1 model per level. Your AI's main model should be used.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                            EditorGUILayout.Space();

                            if (self.TotalLODsRef == EmeraldAISystem.TotalLODsEnum.Two)
                            {
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), Renderer1Prop, "Renderer 1", typeof(Renderer), true);
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), Renderer2Prop, "Renderer 2", typeof(Renderer), true);
                            }
                            else if (self.TotalLODsRef == EmeraldAISystem.TotalLODsEnum.Three)
                            {
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), Renderer1Prop, "Renderer 1", typeof(Renderer), true);
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), Renderer2Prop, "Renderer 2", typeof(Renderer), true);
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), Renderer3Prop, "Renderer 3", typeof(Renderer), true);
                            }
                            else if (self.TotalLODsRef == EmeraldAISystem.TotalLODsEnum.Four)
                            {
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), Renderer1Prop, "Renderer 1", typeof(Renderer), true);
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), Renderer2Prop, "Renderer 2", typeof(Renderer), true);
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), Renderer3Prop, "Renderer 3", typeof(Renderer), true);
                                CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), Renderer4Prop, "Renderer 4", typeof(Renderer), true);
                            }

                            EditorGUILayout.Space();
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                        }

                        if (self.HasMultipleLODsRef == EmeraldAISystem.YesOrNo.No)
                        {
                            EditorGUILayout.PropertyField(AIRendererProp, new GUIContent("AI Main Renderer"));
                            CustomHelpLabelField("The AI's Main Renderer.", true);
                        }

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();
                }

                if (TemperamentTabNumberProp.intValue == 5)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Controls all of an AI's events.", EditorStyles.helpBox);
                    GUIContent[] EventButtons = new GUIContent[2] { new GUIContent("General"), new GUIContent("Combat") };
                    EventTabNumberProp.intValue = GUILayout.Toolbar(EventTabNumberProp.intValue, EventButtons, EditorStyles.miniButton, GUILayout.Height(25));
                    EditorGUILayout.EndVertical();

                    if (EventTabNumberProp.intValue == 0)
                    {
                        EditorGUILayout.BeginVertical("Box");

                        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.LabelField("General Events", EditorStyles.boldLabel);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndVertical();
                        GUI.backgroundColor = Color.white;

                        CustomHelpLabelField("Triggers an event on Start. This can be useful for initializing custom mechanics and quests as well as spawning animations.", false);
                        EditorGUILayout.PropertyField(OnStartEventProp);

                        CustomHelpLabelField("Triggers an event when this AI is enabled. This can be useful for events that need to be called when an AI is being respawned.", false);
                        EditorGUILayout.PropertyField(OnEnabledEventProp);

                        EditorGUILayout.Space();
                        CustomHelpLabelField("Triggers an event when this AI reaches their destination when using the Destination Wander Type.", false);
                        EditorGUILayout.PropertyField(ReachedDestinationEventProp, new GUIContent("On Reached Destination Event"));

                        EditorGUILayout.Space();
                        CustomHelpLabelField("Triggers an event when this AI detects the player when not in combat mode. This can be useful for quests, initializing dialogue, or greetings. " +
                            "This event is dependent on the AI's Detection Radius and is triggered when the player enters it.", false);
                        CustomHelpLabelField("Controls the Player Detection Event cooldown seconds to stop player detection events from happening too frequently. " +
                            "The Player Detection Cooldown is applied after the first time it comes in contact with an AI. ", false);
                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), PlayerDetectionEventCooldownProp, "Player Detection Cooldown", 1, 60);
                        EditorGUILayout.PropertyField(OnPlayerDetectionEventProp);

                        EditorGUILayout.EndVertical();
                    }

                    if (EventTabNumberProp.intValue == 1)
                    {
                        EditorGUILayout.BeginVertical("Box");

                        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.LabelField("Combat Events", EditorStyles.boldLabel);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndVertical();
                        GUI.backgroundColor = Color.white;

                        CustomHelpLabelField("Triggers an event when the AI first starts combat. This can be useful for playing additional sounds or other added functionality.", false);
                        EditorGUILayout.PropertyField(OnStartCombatEventProp);

                        EditorGUILayout.Space();
                        CustomHelpLabelField("Triggers an event when the AI attacks. This can be useful for attack effects.", false);
                        EditorGUILayout.PropertyField(OnAttackEventProp);

                        EditorGUILayout.Space();
                        CustomHelpLabelField("Triggers an event when the AI is damaged.", false);
                        EditorGUILayout.PropertyField(DamageEventProp, new GUIContent("On Take Damage"));

                        EditorGUILayout.Space();
                        CustomHelpLabelField("Triggers an event when the AI deals damage for both Melee or Ranged weapon types.", false);
                        EditorGUILayout.PropertyField(OnDoDamageEventProp);

                        EditorGUILayout.Space();
                        CustomHelpLabelField("Triggers an event when the AI deals damage and it's a critical hit.", false);
                        EditorGUILayout.PropertyField(OnCriticalHitEventProp);

                        EditorGUILayout.Space();
                        CustomHelpLabelField("Triggers an event when the AI heals.", false);
                        EditorGUILayout.PropertyField(OnHealEventProp);

                        EditorGUILayout.Space();
                        CustomHelpLabelField("Triggers an event when the AI flees. This can be useful for fleeing sounds or other added functionality.", false);
                        EditorGUILayout.PropertyField(OnFleeEventProp);

                        EditorGUILayout.Space();
                        CustomHelpLabelField("Triggers an event when the AI kills a target.", false);
                        EditorGUILayout.PropertyField(OnKillTargetEventProp);

                        EditorGUILayout.Space();
                        CustomHelpLabelField("Triggers an event when the AI dies. This can be useful for triggering loot generation, quest mechanics, or other death related events.", false);
                        EditorGUILayout.PropertyField(DeathEventProp, new GUIContent("On Death Event"));

                        EditorGUILayout.EndVertical();
                    }
                }

                if (TemperamentTabNumberProp.intValue == 6)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Item Settings", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Objects that are attached to your AI that can be enabled or disable through Animation Events or programmatically " +
                    "using the item ID. This can be useful for quests items, animation effects, animation specific items, etc. For more information regarding this, please see the Documentation.", EditorStyles.helpBox);
                    EditorGUILayout.Space();

                    CustomHelpLabelField("Each Item below has an ID number. This ID is used to find that particular item and to either enable or disable it using Emerald AI's API.", true);
                    ItemList.DoLayoutList();

                    EditorGUILayout.EndVertical();
                }
            }

            //Detection
            if (TabNumberProp.intValue == 2)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //GUILayout.Width(EditorGUIUtility.currentViewWidth - 50)

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical("Box");
                var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(new GUIContent(DetectTagsIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                EditorGUILayout.LabelField("Detection & Tags", style, GUILayout.ExpandWidth(true));
                GUILayout.Space(4);
                GUIContent[] DetectionTagsButtons = new GUIContent[3] { new GUIContent("Detection Options"), new GUIContent("Tag & Faction Options"), new GUIContent("Head Look Options") };
                DetectionTagsTabNumberProp.intValue = GUILayout.Toolbar(DetectionTagsTabNumberProp.intValue, DetectionTagsButtons, EditorStyles.miniButton, GUILayout.Height(25), GUILayout.MinWidth(EditorGUIUtility.currentViewWidth - 60));
                GUILayout.Space(1);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                if (DetectionTagsTabNumberProp.intValue == 0)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Detection Options", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Controls various detection related options and settings such as radius distances, target detection, and field of view.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), ProjectileCollisionPointYProp, "Hit Transform", 0, self.transform.localScale.y + 4);
                    CustomHelpLabelField("Controls the transform that other AI will use for detection allowing their raycasts to look at this transform. " +
                        "This transform is also used for ranged combat allowing other AI's projectiles to hit this spot. Make sure the Hit Transform is within the AI's Box Collider to avoid detection issues.", true);
                    EditorGUILayout.Space();

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), DetectionFrequencyProp, "Detection Frequency", 0.1f, 2f);
                    CustomHelpLabelField("Controls how often the AI's detection system updates.", true);

                    if (self.DetectionTypeRef == EmeraldAISystem.DetectionType.LineOfSight && self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("Companion AI cannot use the Line of Sight Detection Type. It will automatically be switched to Trigger on Start.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    else if (self.DetectionTypeRef == EmeraldAISystem.DetectionType.LineOfSight && self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Passive)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("Passive AI cannot use the Line of Sight Detection Type. It will automatically be switched to Trigger on Start.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.PropertyField(DetectionTypeProp, new GUIContent("Detection Type"));
                    CustomHelpLabelField("Controls the type of detection that is used for detecting targets.", true);

                    if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("Companion AI will automatically use the Closest Pick Target Method.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.PropertyField(PickTargetMethodProp, new GUIContent("Pick Target Method"));

                    CustomHelpLabelField("Controls the type of method used to pick an AI's first target.", false);

                    if (self.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.Closest)
                    {
                        CustomHelpLabelField("Closest - When a target is first detected or seen, the AI will search for the nearest target within range sometimes resulting " +
                            "in the AI picking a different target that may not currently be seen. However, this usually ends up with the best results and keeps AI evenly " +
                            "distibuted during larger battles. AI will not initialize an attack unless one is seen first.", true);
                    }
                    else if (self.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.FirstDetected)
                    {
                        CustomHelpLabelField("First Detected - Picks the target that is first seen or detected. This is the most realistic, but can sometimes result in " +
                            "multiple AI picking the same target.", true);
                    }
                    else if (self.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.Random)
                    {
                        CustomHelpLabelField("Random - Picks a random target from all available targets within an AI's detection radius upon entering combat. " +
                            "This helps the AI target picking to feel more dynamic and less precise so the same, most logical target, isn’t always picked.", true);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(HeadTransformProp, new GUIContent("Head Transform"));
                    CustomHelpLabelField("The head transform of your AI. This is used for accurate head looking and raycast calculations related to sight and obstruction detection. " +
                        "This should be your AI's head object within its bone objects.", false);

                    CustomHelpLabelField("Attempts to automatically find the AI's head transform. If a transform is found, it is recommended that it is checked to ensure it's correct. " +
                        "If a suitable transform isn't found, no object will be applied and you will need to apply one manually.", false);
                    if (GUILayout.Button("Auto Find Head Transform"))
                    {
                        //Search all the transforms within an AI. The only object that is suitable is a transform with no Mesh or Skinned Mesh Renderer that contains the name head.
                        foreach (Transform t in self.GetComponentsInChildren<Transform>())
                        {
                            if (t.name.Contains("head") || t.name.Contains("Head") || t.name.Contains("HEAD"))
                            {
                                if (t.GetComponent<MeshRenderer>() == null && t.GetComponent<SkinnedMeshRenderer>() == null)
                                {
                                    HeadTransformProp.objectReferenceValue = t;
                                }
                            }
                        }
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUI.BeginChangeCheck();
                    var layersSelection = EditorGUILayout.MaskField("Obstruction Ignore Layers", LayerMaskToField(ObstructionDetectionLayerMaskProp.intValue), InternalEditorUtility.layers);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(self, "Layers changed");
                        ObstructionDetectionLayerMaskProp.intValue = FieldToLayerMask(layersSelection);
                    }
                    CustomHelpLabelField("The layers that should be ignored when an AI is using its obstruction detection for attacking with melee and ranged attacks. " +
                        "These are objects that may prevent an AI from seeing the player target object. If your player has nothing that will block the AI's sight, you can " +
                        "set the layermask to Nothing.", true);

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(ObstructionDetectionQualityProp, new GUIContent("Detection Quality"));
                    CustomHelpLabelField("Controls the quality of the Obstruction Detection feature by controlling how often it's updated", true);

                    if (self.DetectionTypeRef == EmeraldAISystem.DetectionType.LineOfSight)
                    {
                        EditorGUILayout.Space();
                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), fieldOfViewAngleProp, "Field of View", 1, 360);
                        CustomHelpLabelField("Controls the field of view your AI uses to detect targets.", true);

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), ExpandedFieldOfViewAngleProp, "Expanded Field of View", 1, 360);
                        CustomHelpLabelField("Controls the field of view after your AI has been damaged and no target has been found to allow the AI a better " +
                            "opportunity to find its attacker.", true);
                        EditorGUILayout.Space();

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), DetectionRadiusProp, "Detection Distance", 1, 100);
                        CustomHelpLabelField("Controls the distance of the field of view as well as the AI's detection radius. When enabled, AI can go into " +
                            "'Alert Mode' when an target is near, but not visible.", true);

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), ExpandedDetectionRadiusProp, "Expanded Detection Distance", 1, 200);
                        CustomHelpLabelField("Controls the distance of the field of view, as well as the AI's detection radius, after your AI has been damaged, but no " +
                            "target has found. This allows the AI a better opportunity to find its attacker.", true);
                    }

                    if (self.DetectionTypeRef == EmeraldAISystem.DetectionType.Trigger)
                    {
                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), DetectionRadiusProp, "Detection Distance", 1, 100);
                        CustomHelpLabelField("Controls the distance of the AI's trigger radius. When a valid target is within this radius, " +
                            "the AI will react according to its Behavior Type.", true);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Coward && self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Cautious)
                    {
                        EditorGUILayout.PropertyField(MaxChaseDistanceTypeProp, new GUIContent("Distance Type"));
                        CustomHelpLabelField("Controls the AI's target for detecting the distance to stop fleeing.", true);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(MaxChaseDistanceTypeProp, new GUIContent("Distance Type"));
                        CustomHelpLabelField("Controls the AI's target for detecting the distance to stop attacking.", true);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    if (MaxChaseDistanceProp.intValue <= DetectionRadiusProp.intValue)
                    {
                        MaxChaseDistanceProp.intValue = DetectionRadiusProp.intValue + 10;
                    }

                    if (self.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Cautious)
                    {
                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaxChaseDistanceProp, "Chase Distance", DetectionRadiusProp.intValue + 5, 8000);
                        CustomHelpLabelField("Controls the maximum amount of distance the AI will travel before giving up on their target. This distance can be based " +
                            "on either distance away from its target or its starting position.", true);
                        //EditorGUILayout.Space();
                    }
                    else if (self.ConfidenceRef == EmeraldAISystem.ConfidenceType.Coward && self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Cautious)
                    {
                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaxChaseDistanceProp, "Flee Range", DetectionRadiusProp.intValue + 5, 200);
                        CustomHelpLabelField("Controls the maximum amount of distance the AI will travel before they will stop fleeing. This distance can be " +
                            "based on either distance away from its target or its starting position.", true);
                        EditorGUILayout.Space();
                    }

                    CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), ExpandedChaseDistanceProp, "Expanded Chase Distance", DetectionRadiusProp.intValue + 10, 300);
                    CustomHelpLabelField("Controls the max chase distance after your AI has been damaged so the AI can reach its attacker. " +
                        "If you want an to chase or flee from a distance attacker, you will want to increase this value.", true);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Avoidance Options", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(UseAIAvoidanceProp, new GUIContent("Use Object Avoidance"));
                    CustomHelpLabelField("Controls whether or not this AI will avoid objects of the appropriate layer such as other AI and players. " +
                        "Note: This is different avoidance than Unity's NavMesh avoidance and should typically be used for other AI and players.", true);

                    if (self.UseAIAvoidance == EmeraldAISystem.YesOrNo.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        EditorGUI.BeginChangeCheck();
                        var AIAvoidanceLayersSelection = EditorGUILayout.MaskField("AI Avoidance Layers", LayerMaskToField(AIAvoidanceLayerMaskProp.intValue), InternalEditorUtility.layers);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(self, "Layers changed");
                            AIAvoidanceLayerMaskProp.intValue = FieldToLayerMask(AIAvoidanceLayersSelection);
                        }
                        CustomHelpLabelField("The layers the AI will use for avoiding objects such as other AI and players. Note: This feature cannot be used with Companion or Pet AI.", true);

                        EditorGUILayout.Space();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                if (DetectionTagsTabNumberProp.intValue == 1)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Tag and Faction Settings", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("The Tag Options allow you to set Unity Tags and Layers that an AI uses for detection. The Faction Options allow you to control which Factions your AI " +
                        "sees as enemies or allies, including the relations with the AI and the player.", EditorStyles.helpBox);

                    GUIContent[] CombatButtons = new GUIContent[2] { new GUIContent("Tag Options"), new GUIContent("Faction Options") };
                    FactionsAndTagTabNumberProp.intValue = GUILayout.Toolbar(FactionsAndTagTabNumberProp.intValue, CombatButtons, EditorStyles.miniButton, GUILayout.Height(25));

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    if (FactionsAndTagTabNumberProp.intValue == 0)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.LabelField("Tag Options", EditorStyles.boldLabel);
                        EditorGUILayout.HelpBox("Here you can setup your AI's Unity Tags and Layers (These are the tags are layers at the top of this game object). " +
                            "Emerald AI needs these Unity Tags and Layers for its detection system so ensure they are setup correctly.", MessageType.None, true);

                        GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                        EditorGUILayout.LabelField("For a tutorial on setting up an AI's layers and tags, please see the tutorial below.", EditorStyles.helpBox);
                        GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
                        if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
                        {
                            Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Setting-up-an-AIs-Layers-and-Tags");
                        }
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        CustomEditorProperties.CustomTagField(new Rect(), new GUIContent(), EmeraldTagProp, "Emerald Unity Tag");
                        CustomHelpLabelField("The Unity Tag used to define other Emerald AI objects. This is the tag that was created using Unity's Tag pulldown at the top of " +
                            "the gameobject.", true);
                        EditorGUILayout.Space();

                        EditorGUI.BeginChangeCheck();
                        var layersSelection = EditorGUILayout.MaskField("Detection Layers", LayerMaskToField(DetectionLayerMaskProp.intValue), InternalEditorUtility.layers);
                        CustomHelpLabelField("The Detection Layers controls what layers this AI can detect as possible targets, if the AI has the appropriate Emerald Unity Tag.", false);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(self, "Layers changed");
                            DetectionLayerMaskProp.intValue = FieldToLayerMask(layersSelection);
                        }

                        if (DetectionLayerMaskProp.intValue == 0 || DetectionLayerMaskProp.intValue == 1)
                        {
                            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                            EditorGUILayout.LabelField("The Detection Layers cannot contain Nothing, Default, or Everything.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        if (self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion && self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
                        {
                            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                            EditorGUILayout.LabelField("(Companion AI Only)", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                        }
                        EditorGUILayout.HelpBox("The Follower Tag controls the Tag that this AI will follow. This happens when this AI's trigger radius hits the said tag. This feature does not have to be used. " +
                            "If you'd like to manually set the AI's follower, you can do so programmatically.", MessageType.None, true);
                        CustomEditorProperties.CustomTagField(new Rect(), new GUIContent(), FollowTagProp, "Follower Tag");
                        CustomHelpLabelField("If you would like to not use this feature, you can set the Follower Tag to Untagged.", true);
                        EditorGUILayout.Space();

                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(UseNonAITagProp, new GUIContent("Use Non-AI Tag?"));
                        CustomHelpLabelField("Controls whether or not this AI will attack a non-player object with the given tag.", true);

                        if (self.UseNonAITagRef == EmeraldAISystem.UseNonAITag.Yes)
                        {
                            EditorGUILayout.HelpBox("The Non-AI Unity Tag is another type of tag AI can use for the behavior types such as avoid cars, areas of water, or other avoidable objects that are not AI objects.", MessageType.None, true);
                            CustomEditorProperties.CustomTagField(new Rect(), new GUIContent(), NonAITagProp, "Non-AI Tag");
                            GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                            EditorGUILayout.LabelField("Note: The layer of a Non-AI object must be included in an AI's Detection Layers.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        EditorGUILayout.EndVertical();
                    }

                    if (FactionsAndTagTabNumberProp.intValue == 1)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.LabelField("Faction Options", EditorStyles.boldLabel);
                        EditorGUILayout.HelpBox("The Faction Options allow you to control which Factions your AI " +
                        "sees as enemies or allies. These options also allow you to control the relations with the AI and the player.", MessageType.None, true);

                        GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                        EditorGUILayout.LabelField("For a tutorial on setting up an AI's faction relations, please see the tutorial below.", EditorStyles.helpBox);
                        GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
                        if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
                        {
                            Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Using-Factions-and-Faction-Manager");
                        }
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        CustomEditorProperties.CustomEnum(new Rect(), new GUIContent(), CurrentFactionProp, "Faction");
                        CustomHelpLabelField("An AI's Faction is the name used to control combat reaction with other AI. This is the name other AI will use when " +
                            "looking for opposing targets.", true);

                        CustomHelpLabelField("Factions can be created and removed using the Faction Manager. ", false);
                        if (GUILayout.Button("Open Faction Manager"))
                        {
                            EditorWindow APS = EditorWindow.GetWindow(typeof(EmeraldAIFactionManager));
                            APS.minSize = new Vector2(600f, 775f);
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Player Relation", EditorStyles.boldLabel);
                        CustomHelpLabelField("Controls how this AI sees the player. You can hover the mouse over each setting to view its tooltip.", false);
                        GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                        EditorGUILayout.HelpBox("You must define your Player's Unity Tag separately using the Player Tag below. This allows the AI to determine if the target is another AI or a player target.", MessageType.None, true);
                        GUI.backgroundColor = Color.white;

                        if (self.PlayerFaction.Count > 0)
                        {
                            if (self.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Enemy)
                            {
                                CustomHelpLabelField("Enemy - AI with a Player Relation of Enemy will attack, engage, or flee from all players when they're detected (Reacting according to their Behavior Type).", false);
                            }
                            else if (self.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Neutral)
                            {
                                CustomHelpLabelField("Neutral - AI with a Player Relation of Neautral will ignore all players, unless they are attacked. When this happens, AI will react according to their Behavior Type.", false);
                            }
                            else if (self.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Friendly)
                            {
                                CustomHelpLabelField("Friendly - AI with a Player Relation of Friendly will ignore all players, even if they are attacked.", false);
                            }
                        }

                        PlayerFaction.DoLayoutList();
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        EditorGUILayout.LabelField("AI Faction Relations", EditorStyles.boldLabel);
                        CustomHelpLabelField("Controls which factions this AI sees as enemies and allies. You can hover the mouse over each setting to view its tooltip.", false);
                        GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                        EditorGUILayout.LabelField("Note: The AI Faction Relations use an AI's Faction not Unity tags. You can add and remove factions through the Faction Manager. " +
                            "This can be opened by pressing the button below.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.Space();
                        if (GUILayout.Button("Open Faction Manager"))
                        {
                            EditorWindow APS = EditorWindow.GetWindow(typeof(EmeraldAIFactionManager));
                            APS.minSize = new Vector2(600f, 775f);
                        }
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        FactionsList.DoLayoutList();
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.EndVertical();
                    }
                }

                if (DetectionTagsTabNumberProp.intValue == 2)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Head Look Options", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Controls the AI's Head Look settings.", EditorStyles.helpBox);
                    GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                    EditorGUILayout.LabelField("Note: Only models with a Humanoid Animation Type can use this feature. " +
                        "The Non-Combat head look feature is only usable on players. The Combat head look feature works on all targets, including other AI.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(UseHeadLookProp, new GUIContent("Use Head Look"));
                    CustomHelpLabelField("Enables or disables the head look feature.", true);

                    if (self.UseHeadLookRef == EmeraldAISystem.YesOrNo.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), LookSmootherProp, "Head Look Speed", 0.1f, 3);
                        CustomHelpLabelField("Controls the speed of the AI's head look.", true);
                        EditorGUILayout.Space();

                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), MaxLookAtDistanceProp, "Max Head Look Distance", 5, 200);
                        CustomHelpLabelField("Controls the max distance an AI will use the head look feature.", true);

                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), HeadLookYOffsetProp, "Player Y Offset", -4, 4);
                        CustomHelpLabelField("Controls the Y offset of the AI's look at position when looking at the Player. If an AI is looking too high or too " +
                            "low at their target, you can adjust it with this setting.", true);

                        EditorGUILayout.LabelField("Non-Combat", EditorStyles.boldLabel);
                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), NonCombatLookAtLimitProp, "Angle Limit", 10, 90);
                        CustomHelpLabelField("Controls the max angle an will use the look at feature while not in combat.", true);

                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), HeadLookWeightNonCombatProp, "Head Weight", 0, 1);
                        CustomHelpLabelField("Controls the Head Look intensity of the AI's head while in not combat.", true);

                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), BodyLookWeightNonCombatProp, "Body Weight", 0, 1);
                        CustomHelpLabelField("Controls the Head Look intensity of the AI's body while in not combat.", true);
                        EditorGUILayout.Space();

                        EditorGUILayout.LabelField("Combat", EditorStyles.boldLabel);
                        CustomEditorProperties.CustomIntSlider(new Rect(), new GUIContent(), CombatLookAtLimitProp, "Angle Limit", 10, 90);
                        CustomHelpLabelField("Controls the max angle an will use the look at feature while in combat.", true);

                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), HeadLookWeightCombatProp, "Combat Head Weight", 0, 1);
                        CustomHelpLabelField("Controls the Head Look intensity of the AI's head while in combat.", true);

                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), BodyLookWeightCombatProp, "Combat Body Weight", 0, 1);
                        CustomHelpLabelField("Controls the Head Look intensity of the AI's body while in combat.", true);

                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), YAimOffsetProp, "Y Axis Offset", -10, 10);
                        CustomHelpLabelField("Controls the Y offset of the AI's body and arms in the instance that they are off when aiming at targets.", true);

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.EndHorizontal();
                }

            }

            if (TabNumberProp.intValue == 3)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical("Box");
                var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(new GUIContent(UIIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                EditorGUILayout.LabelField("UI Settings", style, GUILayout.ExpandWidth(true));
                GUILayout.Space(2);
                EditorGUILayout.LabelField("Controls the use and settings of Emerald's built-in Health Bars and Combat Text. In order for UI to be visible, a player of the appropriate tag must enter an AI's trigger radius. " +
                    "You can set an AI's UI Tag under the Detection and Tag tab.", EditorStyles.helpBox);
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.Space();

                GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                EditorGUILayout.LabelField("In order for the UI system to work correctly, you will need to assign a Tag and Layer. This is typically your Player's Tag and Layer. " +
                    "This is used to make the UI system more efficient by only running when the appropriate objects are detected. You will also need to apply your player's camera Tag so the UI can be properly positioned.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;

                EditorGUILayout.Space();
                CustomEditorProperties.CustomTagField(new Rect(), new GUIContent(), CameraTagProp, "Camera Tag");
                CustomHelpLabelField("The Camera Tag is the Unity Tag that your player uses. The Camera is needed to properly position the UI.", true);

                CustomEditorProperties.CustomTagField(new Rect(), new GUIContent(), UITagProp, "UI Tag");
                CustomHelpLabelField("The UI Tag is the Unity Tag that will trigger the AI's UI, when enabled.", true);

                EditorGUI.BeginChangeCheck();
                var layersSelectionUI = EditorGUILayout.MaskField("UI Layers", LayerMaskToField(UILayerMaskProp.intValue), InternalEditorUtility.layers);
                CustomHelpLabelField("The UI Layers controls what layers this AI will detect to enable the their UI, if the object also has the appropriate UI Tag. This is typically used for players.", false);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(self, "Layers changed");
                    UILayerMaskProp.intValue = FieldToLayerMask(layersSelectionUI);
                }

                if (UILayerMaskProp.intValue == 0 || UILayerMaskProp.intValue == 1)
                {
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.LabelField("The UI Layers cannot contain Nothing, Default, or Everything.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(MaxUIScaleSizeProp, new GUIContent("Max UI Scale"));
                CustomHelpLabelField("Controls the max size the UI will be scaled when the player is getting further away from an AI's UI.", true);
                EditorGUILayout.Space();

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();

                NameTextFoldoutProp.boolValue = CustomEditorProperties.Foldout(NameTextFoldoutProp.boolValue, "Name Text Settings", true, myFoldoutStyle);

                if (NameTextFoldoutProp.boolValue)
                {
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(DisplayAINameProp, new GUIContent("Display AI Name"));
                    CustomHelpLabelField("Enables or disables the display of the AI's name. When enabled, the AI's name will be visible above its health bar.", true);

                    if (self.DisplayAINameRef == EmeraldAISystem.DisplayAIName.Yes)
                    {

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        EditorGUILayout.PropertyField(UseCustomFontAINameProp, new GUIContent("Use Custom Name Font"));
                        CustomHelpLabelField("Controls whether or not the Name Text font can be customized.", false);
                        EditorGUILayout.Space();

                        if (self.UseCustomFontAIName == EmeraldAISystem.YesOrNo.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(10);
                            EditorGUILayout.BeginVertical();
                            EditorGUILayout.PropertyField(AINameFontProp, new GUIContent("Name Font"));
                            CustomHelpLabelField("Controls the font of the AI's Name Text.", true);
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                        }

                        EditorGUILayout.PropertyField(AINamePosProp, new GUIContent("AI Name Position"));
                        CustomHelpLabelField("Controls the position of the AI's name text.", true);
                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(NameTextFontSizeProp, new GUIContent("AI Name Font Size"));
                        CustomHelpLabelField("Controls the size of the AI's name text.", true);
                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(NameTextColorProp, new GUIContent("AI Name Color"));
                        CustomHelpLabelField("Controls the color of the AI's name text.", true);
                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(UseAINameUIOutlineEffectProp, new GUIContent("Use Outline on Name Text"));
                        CustomHelpLabelField("Controls whether or not the AI's Name UI will use an Outline Effect.", true);
                        EditorGUILayout.Space();

                        if (self.UseAINameUIOutlineEffect == EmeraldAISystem.YesOrNo.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(10);
                            EditorGUILayout.BeginVertical();

                            EditorGUILayout.PropertyField(AINameUIOutlineColorProp, new GUIContent("Name Text Outline Color"));
                            CustomHelpLabelField("Controls the color of the AI's Name Text Outline.", true);
                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(AINameUIOutlineSizeProp, new GUIContent("Name Text Outline Size"));
                            CustomHelpLabelField("Controls the size of the AI's Name Text Outline.", true);
                            EditorGUILayout.Space();

                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                        }

                        EditorGUILayout.PropertyField(DisplayAITitleProp, new GUIContent("Display AI Title"));
                        CustomHelpLabelField("Enables or disables the display of the AI's title. When enabled, the AI's title will be visible above its health bar.", false);

                        if (self.DisplayAITitleRef == EmeraldAISystem.DisplayAITitle.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(10);
                            EditorGUILayout.BeginVertical();
                            EditorGUILayout.PropertyField(AINameLineSpacingProp, new GUIContent("Name Line Spacing"));
                            CustomHelpLabelField("Controls the line spacing between the AI's Name and the AI's Title.", true);
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                        }

                        EditorGUILayout.Space();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }

                    if (self.CreateHealthBarsRef == EmeraldAISystem.CreateHealthBars.No)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("You must have Auto Create Health Bars enabled to use this feature.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.PropertyField(DisplayAILevelProp, new GUIContent("Display AI Level"));
                    CustomHelpLabelField("Enables or disables the display of the AI's level. When enabled, the AI's level will be visible to the left of its health bar.", true);

                    if (self.DisplayAILevelRef == EmeraldAISystem.DisplayAILevel.Yes)
                    {

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        EditorGUILayout.PropertyField(UseCustomFontAILevelProp, new GUIContent("Use Custom Level Font"));
                        CustomHelpLabelField("Controls whether or not the Level Text font can be customized.", true);
                        EditorGUILayout.Space();

                        if (self.UseCustomFontAILevel == EmeraldAISystem.YesOrNo.Yes)
                        {
                            EditorGUILayout.PropertyField(AILevelFontProp, new GUIContent("Level Font"));
                            CustomHelpLabelField("Controls the font of the AI's Level Text.", true);
                            EditorGUILayout.Space();
                        }

                        EditorGUILayout.PropertyField(AILevelPosProp, new GUIContent("AI Level Position"));
                        CustomHelpLabelField("Controls the position of the AI's level text.", true);
                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(LevelTextColorProp, new GUIContent("Level Color"));
                        CustomHelpLabelField("Controls the color of the AI's Level Text.", true);
                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(UseAILevelUIOutlineEffectProp, new GUIContent("Use Outline on Level Text"));
                        CustomHelpLabelField("Controls whether or not the AI's Level UI will use an Outline Effect.", true);
                        EditorGUILayout.Space();

                        if (self.UseAILevelUIOutlineEffect == EmeraldAISystem.YesOrNo.Yes)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(10);
                            EditorGUILayout.BeginVertical();

                            EditorGUILayout.PropertyField(AILevelUIOutlineColorProp, new GUIContent("Level Text Outline Color"));
                            CustomHelpLabelField("Controls the color of the AI's Level Text Outline.", true);
                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(AILevelUIOutlineSizeProp, new GUIContent("Level Text Outline Size"));
                            CustomHelpLabelField("Controls the size of the AI's Level Text Outline.", true);
                            EditorGUILayout.Space();

                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                        }

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                HealthBarsFoldoutProp.boolValue = CustomEditorProperties.Foldout(HealthBarsFoldoutProp.boolValue, "Health Bar Settings", true, myFoldoutStyle);

                if (HealthBarsFoldoutProp.boolValue)
                {
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(CreateHealthBarsProp, new GUIContent("Auto Create Health Bars"));
                    CustomHelpLabelField("Enables or disables the use of Emerald automatically creating health bars for your AI. Enabling this will open up additional settings.", true);
                    EditorGUILayout.Space();

                    if (self.CreateHealthBarsRef == EmeraldAISystem.CreateHealthBars.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        EditorGUILayout.PropertyField(HealthBarPosProp, new GUIContent("Health Bar Position"));
                        CustomHelpLabelField("Controls the starting position of the AI's created health bar.", true);
                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(HealthBarScaleProp, new GUIContent("Health Bar Scale"));
                        CustomHelpLabelField("Controls the scale of the AI's created health bar.", true);
                        EditorGUILayout.Space();

                        if (self.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet)
                        {
                            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                            EditorGUILayout.LabelField("Not Usable with Pet AI. Health bars will be disabled.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                        }

                        EditorGUILayout.PropertyField(HealthBarColorProp, new GUIContent("Health Bar Color"));
                        CustomHelpLabelField("Controls the color of the AI's health bar.", true);
                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(HealthBarBackgroundColorProp, new GUIContent("Background Color"));
                        CustomHelpLabelField("Controls the background color of the AI's health bar.", true);
                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(CustomizeHealthBarProp, new GUIContent("Customize Health Bar?"));
                        CustomHelpLabelField("Gives you controls over using custom sprites for the AI's health bar.", true);
                        EditorGUILayout.Space();

                        if (self.CustomizeHealthBarRef == EmeraldAISystem.CustomizeHealthBar.Yes)
                        {
                            EditorGUILayout.LabelField("Health Bar Sprites", EditorStyles.boldLabel);
                            EditorGUILayout.Space();

                            CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), HealthBarImageProp, "Bar", typeof(Sprite), true);
                            CustomHelpLabelField("Customizes the health bar sprite for the AI's health bar.", true);

                            CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), HealthBarBackgroundImageProp, "Bar Background", typeof(Sprite), true);
                            CustomHelpLabelField("Customizes the health bar's background sprite for the AI's health bar.", true);
                        }

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                CombatTextFoldoutProp.boolValue = CustomEditorProperties.Foldout(CombatTextFoldoutProp.boolValue, "Combat Text Settings", true, myFoldoutStyle);

                if (CombatTextFoldoutProp.boolValue)
                {
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.Space();

                    CustomHelpLabelField("The Combat Text System has been improved and is now handled globally through the Combat Text Manager.", false);
                    var ButtonStyle = new GUIStyle(GUI.skin.button);
                    if (GUILayout.Button("Open Combat Text Manager", ButtonStyle))
                    {
                        EditorWindow CTM = EditorWindow.GetWindow(typeof(EmeraldAICombatTextManager), true, "Combat Text Manager");
                        CTM.minSize = new Vector2(600f, 725f);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                }
            }

            if (TabNumberProp.intValue == 4)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginVertical("Box");
                var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(new GUIContent(SoundIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                EditorGUILayout.LabelField("Sounds", style, GUILayout.ExpandWidth(true));
                GUILayout.Space(2);
                GUIContent[] SoundButtons = new GUIContent[2] { new GUIContent("General"), new GUIContent("Combat") };
                SoundTabNumberProp.intValue = GUILayout.Toolbar(SoundTabNumberProp.intValue, SoundButtons, EditorStyles.miniButton, GUILayout.Height(25), GUILayout.Width(EditorGUIUtility.currentViewWidth - 45));
                GUILayout.Space(1);
                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //General
                if (SoundTabNumberProp.intValue == 0)
                {
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("General Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.LabelField("Emerald AI's general sounds such as footstep, idle, and other sounds an AI can play with Animation Events.", EditorStyles.helpBox);
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Idle Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), IdleVolumeProp, "Idle Volume", 0, 1);
                    CustomHelpLabelField("Controls the volume of idle sounds.", true);

                    CustomHelpLabelField("Controls how many idle sounds this AI will use.", true);

                    IdleSoundsList.DoLayoutList();

                    if (self.IdleSounds.Count != 0)
                    {
                        EditorGUILayout.Space();
                        CustomEditorProperties.CustomIntField(new Rect(), new GUIContent(), IdleSoundsSecondsMinProp, "Min Idle Sound Seconds");
                        CustomHelpLabelField("Controls the minimum amount of seconds needed before playing a random idle sound from the list below. This amuont will be " +
                            "randomized with the Max Idle Sound Seconds.", true);

                        CustomEditorProperties.CustomIntField(new Rect(), new GUIContent(), IdleSoundsSecondsMaxProp, "Max Idle Sound Seconds");
                        CustomHelpLabelField("Controls the maximum amount of seconds needed before playing a random idle sound from the list below. This amuont will be " +
                            "randomized with the Min Idle Sound Seconds.", true);
                    }
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Footstep Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), WalkFootstepVolumeProp, "Walk Footsteps Volume", 0, 1);
                    CustomHelpLabelField("Controls the volume of walk footstep sounds.", false);
                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), RunFootstepVolumeProp, "Run Footsteps Volume", 0, 1);
                    CustomHelpLabelField("Controls the volume of Run footstep sounds.", true);

                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("Controls how many footstep sounds this AI will use.", EditorStyles.helpBox);
                    GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                    EditorGUILayout.LabelField("Note: You will need to manually create a WalkFootstepSound and/or RunFootstepSound Animation Event to use this feature. " +
                        "Please refer to Emerlad's documentation for a tutorial on how to do this.", EditorStyles.helpBox);
                    GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
                    if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
                    {
                        Application.OpenURL("https://www.youtube.com/watch?feature=player_embedded&v=pL5Z-f8COcY");
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    FootStepSoundsList.DoLayoutList();

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Interact Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    CustomHelpLabelField("Various sounds that can be called through Animation Events or programmatically using the sound effect ID. " +
                        "This can be useful for quests, dialouge, animation sound effects, etc.", true);

                    InteractSoundsList.DoLayoutList();
                    EditorGUILayout.EndVertical();
                }

                if (SoundTabNumberProp.intValue == 1)
                {
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Combat Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.LabelField("Emerald AI's sounds that play during combat such as attack sounds, injured sounds, death sounds, and more. " +
                        "Some sounds will need to be played with an Animation Event.", EditorStyles.helpBox);
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Equip and Unequip Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.LabelField("Controls the sounds that play when the AI is equiping or unequiping their weapon, when Use Equip Animation is enabled.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                    if (self.UseEquipAnimation == EmeraldAISystem.YesOrNo.No)
                    {
                        GUI.backgroundColor = new Color(1f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("Use Equip Animations needs to be enabled in order to use this feature. This can be found under the Combat Animation Settings.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUI.BeginDisabledGroup(self.UseEquipAnimation == EmeraldAISystem.YesOrNo.No);
                    if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee || self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
                    {
                        EditorGUILayout.Space();
                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), EquipVolumeProp, "Equip Volume", 0, 1);
                        CustomHelpLabelField("Controls the volume of the Equip Sound.", false);
                        CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), UnsheatheWeaponProp, "Equip Sound", typeof(AudioClip), true);
                        CustomHelpLabelField("The sound that plays when this AI is equiping their weapon.", true);
                        EditorGUILayout.Space();
                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), UnequipVolumeProp, "Unequip Volume", 0, 1);
                        CustomHelpLabelField("Controls the volume of the Unequip Sound.", false);
                        CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), SheatheWeaponProp, "Unequip Sound", typeof(AudioClip), true);
                        CustomHelpLabelField("The sound that plays when this AI is unequiping their weapon.", true);
                    }
                    if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged || self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
                    {
                        EditorGUILayout.Space();
                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), RangedEquipVolumeProp, "Ranged Equip Volume", 0, 1);
                        CustomHelpLabelField("Controls the volume of the Equip Sound.", false);
                        CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), RangedUnsheatheWeaponProp, "Ranged Equip Sound", typeof(AudioClip), true);
                        CustomHelpLabelField("The sound that plays when this AI is equiping their weapon.", true);
                        EditorGUILayout.Space();
                        CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), RangedUnequipVolumeProp, "Ranged Unequip Volume", 0, 1);
                        CustomHelpLabelField("Controls the volume of the Unequip Sound.", false);
                        CustomEditorProperties.CustomObjectField(new Rect(), new GUIContent(), RangedSheatheWeaponProp, "Ranged Unequip Sound", typeof(AudioClip), true);
                        CustomHelpLabelField("The sound that plays when this AI is unequiping their weapon.", true);
                    }
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Attack Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                    EditorGUILayout.LabelField("Note: Attack Sounds are used through Animation Events. For a tutorial on this, please press the 'See Tutorial' button below.", EditorStyles.helpBox);
                    GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
                    if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
                    {
                        Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Emerald-AI-Animation-Events#attack-sound-animation-events");
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), AttackVolumeProp, "Attack Volume", 0, 1);
                    CustomHelpLabelField("Controls the volume of Attack Sounds.", true);

                    CustomHelpLabelField("Controls how many attack sounds this AI will use.", true);

                    AttackSoundsList.DoLayoutList();
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Injured Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), InjuredVolumeProp, "Injured Volume", 0, 1);
                    CustomHelpLabelField("Controls the volume of Injured Sounds.", true);

                    CustomHelpLabelField("Controls how many injured sounds this AI will use.", true);

                    InjuredSoundsList.DoLayoutList();
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Block Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), BlockVolumeProp, "Block Volume", 0, 1);
                    CustomHelpLabelField("Controls the volume of Block Sounds.", true);

                    CustomHelpLabelField("Controls the sound that happens when this AI is hit while blocking. " +
                        "Note: Blocking must be enabled with the proper animations setup in order for this to work.", true);
                    BlockSoundsList.DoLayoutList();
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Weapon Impact Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), ImpactVolumeProp, "Impact Volume", 0, 1);
                    CustomHelpLabelField("Controls the volume of Impact Sounds.", true);

                    CustomHelpLabelField("Controls the sound that happens when this AI hits its target when using the Melee Weapon Type. This is typically the AI's weapon impact sound.", true);

                    ImpactSoundsList.DoLayoutList();
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Weapon Critical Hit Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), CriticalHitVolumeProp, "Critical Hit Volume", 0, 1);
                    CustomHelpLabelField("Controls the volume of Critical Hit Sounds.", true);

                    CustomHelpLabelField("Controls the sound that happens when this AI hits its target and it's a critical hit when using the Melee Weapon type. If you do not want to use this feature, simple leave this sound list empty.", true);

                    CriticalHitSoundsList.DoLayoutList();
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Death Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    if (self.DeathTypeRef == EmeraldAISystem.DeathType.Animation)
                    {
                        GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                        EditorGUILayout.LabelField("Note: Death Sounds are used through Animation Events. For a tutorial on this, please press the 'See Tutorial' button below.", EditorStyles.helpBox);
                        GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
                        if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
                        {
                            Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Emerald-AI-Animation-Events#death-sound-animation-events-animation-death-only");
                        }
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                    }

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), DeathVolumeProp, "Death Volume", 0, 1);
                    CustomHelpLabelField("Controls the volume of Death Sounds.", true);

                    CustomHelpLabelField("Controls how many death sounds this AI will use.", true);
                    DeathSoundsList.DoLayoutList();

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.BeginVertical("Box");
                    GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.25f);
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField("Warning Sounds", EditorStyles.boldLabel);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndVertical();
                    GUI.backgroundColor = Color.white;

                    GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                    EditorGUILayout.LabelField("Note: Warning Sounds are only used by Cautious Behavior Types with a Confidence greater than Coward and are done through Animation Events. For a tutorial on this, please press the 'See Tutorial' button below.", EditorStyles.helpBox);
                    GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
                    if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
                    {
                        Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Emerald-AI-Animation-Events#warning-sound-animation-events");
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    CustomEditorProperties.CustomFloatSlider(new Rect(), new GUIContent(), WarningVolumeProp, "Warning Volume", 0, 1);
                    CustomHelpLabelField("Controls the volume of Warning Sounds.", true);

                    CustomHelpLabelField("Controls how many warning sounds this AI will use.", true);

                    WarningSoundsList.DoLayoutList();
                    EditorGUILayout.EndVertical();
                }
            }

            if (TabNumberProp.intValue == 5)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                if (self.WanderTypeRef != EmeraldAISystem.WanderType.Waypoints)
                {
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.LabelField("You can only use the Waypoint Editor if your AI's Wander Type is set to Waypoints, would you like to enable this AI to use Waypoints?", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                    if (GUILayout.Button("Enable Waypoints"))
                    {
                        WanderTypeProp.intValue = 1;
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                }


                if (self.WanderTypeRef == EmeraldAISystem.WanderType.Waypoints)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.BeginVertical("Box");
                    var style4 = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                    EditorGUILayout.LabelField(new GUIContent(WaypointEditorIcon), style4, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                    EditorGUILayout.LabelField("Waypoint Editor", style4, GUILayout.ExpandWidth(true));
                    GUILayout.Space(2);
                    EditorGUILayout.HelpBox("Here you can define waypoints for your AI to follow. Simply press the 'Add Waypoint' button to create a waypoint. The AI will " +
                        "follow each created waypoint in the order they are created. A line will be drawn to visually represent this.", MessageType.None, true);
                    EditorGUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    if (self.WaypointsList != null && Selection.objects.Length == 1)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.LabelField("Controls what an AI will do when it reaches its last waypoint.", EditorStyles.helpBox);
                        EditorGUILayout.PropertyField(WaypointTypeProp, new GUIContent("Waypoint Type"));
                        GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                        GUI.backgroundColor = Color.white;

                        if (self.WaypointTypeRef == (EmeraldAISystem.WaypointType.Loop))
                        {
                            CustomHelpLabelField("Loop - When an AI reaches its last waypoint, it will set the first waypoint as its next waypoint thus creating a loop.", false);
                        }
                        else if (self.WaypointTypeRef == (EmeraldAISystem.WaypointType.Reverse))
                        {
                            CustomHelpLabelField("Reverse - When an AI reaches its last waypoint, it will reverse the AI's waypoints making the last waypoint its first. " +
                                "This allows AI to patorl back and forth through narrow or one way areas.", false);
                        }
                        else if (self.WaypointTypeRef == (EmeraldAISystem.WaypointType.Random))
                        {
                            CustomHelpLabelField("Random - This allows an AI to patrol randomly through all waypoints. An AI will idle each time it reaches a waypoint " +
                                "for as long as its wait time seconds are set.", false);
                        }

                        EditorGUILayout.Space();

                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        if (GUILayout.Button("Add Waypoint"))
                        {
                            Vector3 newPoint = new Vector3(0, 0, 0);

                            if (self.WaypointsList.Count == 0)
                            {
                                newPoint = new Vector3(self.transform.position.x, self.transform.position.y, self.transform.position.z + 5);
                            }
                            else if (self.WaypointsList.Count > 0)
                            {
                                newPoint = new Vector3(self.WaypointsList[self.WaypointsList.Count - 1].x, self.WaypointsList[self.WaypointsList.Count - 1].y, self.WaypointsList[self.WaypointsList.Count - 1].z + 5);
                            }

                            self.WaypointsList.Add(newPoint);
                            EditorUtility.SetDirty(self);
                        }

                        var style = new GUIStyle(GUI.skin.button);
                        style.normal.textColor = Color.red;

                        if (GUILayout.Button("Clear All Waypoints", style) && EditorUtility.DisplayDialog("Clear Waypoints?", "Are you sure you want to clear all of this AI's waypoints? This process cannot be undone.", "Yes", "Cancel"))
                        {
                            self.WaypointsList.Clear();
                            EditorUtility.SetDirty(self);
                        }
                        GUI.contentColor = Color.white;
                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        WaypointsFoldout.boolValue = CustomEditorProperties.Foldout(WaypointsFoldout.boolValue, "Waypoints", true, myFoldoutStyle);

                        if (WaypointsFoldout.boolValue)
                        {
                            EditorGUILayout.BeginVertical("Box");
                            EditorGUILayout.LabelField("Waypoints", EditorStyles.boldLabel);
                            EditorGUILayout.LabelField("All of this AI's current waypoints. Waypoints can be individually removed by pressing the ''Remove Point'' button.", EditorStyles.helpBox);
                            EditorGUILayout.Space();

                            if (self.WaypointsList.Count > 0)
                            {
                                for (int j = 0; j < self.WaypointsList.Count; ++j)
                                {
                                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                                    EditorGUILayout.LabelField("Waypoint " + (j + 1), EditorStyles.toolbarButton);
                                    GUI.backgroundColor = Color.white;

                                    if (GUILayout.Button("Remove Point", EditorStyles.miniButton, GUILayout.Height(18)))
                                    {
                                        self.WaypointsList.RemoveAt(j);
                                        --j;
                                        EditorUtility.SetDirty(self);
                                    }
                                    GUILayout.Space(10);
                                }
                            }


                            EditorGUILayout.EndVertical();
                        }

                    }
                    else if (self.WaypointsList != null && Selection.objects.Length > 1)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("Waypoints do not support multi-object editing. If you'd like to edit an AI's waypoints, please only have 1 AI selected at a time.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndVertical();
                    }
                }
            }

            if (TabNumberProp.intValue == 6)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Only auto-update the Animator Controller if inside the Unity Editor as runtime auto-updating is not possible.
#if UNITY_EDITOR
                if (self.AnimatorControllerGenerated)
                {
                    if (self.AnimationsUpdated || self.AnimationListsChanged)
                    {
                        EmeraldAIAnimatorGenerator.GenerateAnimatorController(self);
                        UpdateAbilityAnimationEnums();
                    }
                }
#endif

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(1);
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 55));
                var style3 = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(new GUIContent(AnimationIcon), style3, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                EditorGUILayout.LabelField("Animation Settings", style3, GUILayout.ExpandWidth(true));

                if (!self.AnimatorControllerGenerated)
                {
                    EditorGUILayout.LabelField("To create an Animator Controller, press the 'Create Animator Controller' button below. " +
                        "This will create an Animator Controller and assign the animations you've entered below.", EditorStyles.helpBox);
                }
                else if (self.AnimatorControllerGenerated)
                {
                    GUI.backgroundColor = new Color(0.1f, 1f, 0.1f, 0.5f);
                    EditorGUILayout.LabelField("The Animator Controller is automatically updated as changes are made.", EditorStyles.helpBox);
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.LabelField("Important - Please remember all animation slots that are enabled must have animations applied to avoid errors. Please ensure you have applied all " +
                        "of the neccesary animations before using this AI. (This is a reminder, not an error message)", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;

                    GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                    EditorGUILayout.LabelField("Note: For a quick tutorial on applying animations, please see the tutorial below. ", EditorStyles.helpBox);
                    GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
                    if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
                    {
                        Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Setting-up-an-AI's-Animations");
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                    EditorGUILayout.LabelField("Note: You can press the 'Check for Missing Animations' button below to have Emerald AI debug log missing animations to the Unity Console so you don't have to manually " +
                        "look through each animation tab. If no errors are displayed, your AI is not missing any animations.", EditorStyles.helpBox);
                    GUI.backgroundColor = new Color(0.9f, 0, 0, 0.8f);
                    if (GUILayout.Button("Check for Missing Animations", HelpButtonStyle, GUILayout.Height(20)))
                    {
                        CheckForMissingAnimations();
                    }
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Note: If you have multiple AI using the same Animator Controller, such as duplicated or copied AI, you will " +
                        "need to have all said objects selected when updating the Animator Controller.", EditorStyles.helpBox);
                }

                GUILayout.Space(6);
                GUI.backgroundColor = Color.white;
                GUILayout.Space(2);
                GUIContent[] AnimationButtons = new GUIContent[4] { new GUIContent("Idle"), new GUIContent("Movement"), new GUIContent("Combat"), new GUIContent("Emotes") };
                AnimationTabNumberProp.intValue = GUILayout.Toolbar(AnimationTabNumberProp.intValue, AnimationButtons, EditorStyles.miniButton, GUILayout.Height(20), GUILayout.Width(EditorGUIUtility.currentViewWidth - 55));
                GUILayout.Space(1);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                if (AnimationTabNumberProp.intValue == 0)
                {
                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 39));
                    EditorGUILayout.LabelField("Idle Animations", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("The Idle Animations section allows you to set all idle animations that this AI will use when wandering and the idle animations that will be used in combat. ", EditorStyles.helpBox);
                    EditorGUILayout.Space();

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Idle Animations", EditorStyles.boldLabel);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("Controls the idle animations that will randomly play when the AI is wandering or grazing. A max of 6 can be used.", EditorStyles.helpBox);
                    GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                    GUI.backgroundColor = Color.white;
                    IdleAnimationList.DoLayoutList();
                    EditorGUILayout.Space();

                    //Idle
                    if (self.IdleAnimationList.Count == 6)
                    {
                        IdleAnimationList.displayAdd = false;
                    }
                    else
                    {
                        IdleAnimationList.displayAdd = true;
                    }

                    CustomAnimationField(new Rect(), new GUIContent(), IdleNonCombatProp, "Idle Non-Combat", typeof(AnimationClip), false);
                    CustomHelpLabelField("Controls the default idle animation.", false);
                    if (self.NonCombatIdleAnimation != null)
                    {
                        var settings = AnimationUtility.GetAnimationClipSettings(self.NonCombatIdleAnimation);
                        if (!settings.loopTime)
                        {
                            GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                            EditorGUILayout.LabelField("The 'Idle Non-Combat' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                            GUI.backgroundColor = Color.white;
                        }
                    }
                    CustomFloatAnimationField(new Rect(), new GUIContent(), IdleNonCombatAnimationSpeedProp, "Animation Speed", 0.1f, 2);

                    if (self.WeaponTypeRef != EmeraldAISystem.WeaponType.Ranged)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        CustomAnimationField(new Rect(), new GUIContent(), IdleCombatProp, "Idle Combat", typeof(AnimationClip), false);
                        CustomHelpLabelField("Controls the idle animation that the AI will play while an AI is in Combat Mode.", false);
                        if (self.CombatIdleAnimation != null)
                        {
                            var settings = AnimationUtility.GetAnimationClipSettings(self.CombatIdleAnimation);
                            if (!settings.loopTime)
                            {
                                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                EditorGUILayout.LabelField("The 'Idle Combat' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                                GUI.backgroundColor = Color.white;
                            }
                        }
                        CustomFloatAnimationField(new Rect(), new GUIContent(), IdleCombatAnimationSpeedProp, "Animation Speed", 0.1f, 2);
                    }

                    if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes || self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        CustomAnimationField(new Rect(), new GUIContent(), RangedIdleCombatProp, "Ranged Idle Combat", typeof(AnimationClip), false);
                        CustomHelpLabelField("Controls the ranged idle animation that the AI will play while an AI is in Combat Mode.", false);
                        if (self.RangedCombatIdleAnimation != null)
                        {
                            var settings = AnimationUtility.GetAnimationClipSettings(self.RangedCombatIdleAnimation);
                            if (!settings.loopTime)
                            {
                                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                EditorGUILayout.LabelField("The 'Ranged Idle Combat' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                                GUI.backgroundColor = Color.white;
                            }
                        }
                        CustomFloatAnimationField(new Rect(), new GUIContent(), RangedIdleCombatAnimationSpeedProp, "Animation Speed", 0.1f, 2);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    if (self.BehaviorRef != EmeraldAISystem.CurrentBehavior.Cautious)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("(Cautious AI Only)", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.PropertyField(UseWarningAnimationProp, new GUIContent("Use Warning"));
                    CustomHelpLabelField("Controls whether or not this AI will play a warning animation and sound if they feel threatened", true);

                    if (self.UseWarningAnimationRef == EmeraldAISystem.YesOrNo.Yes)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes || self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                        {
                            CustomAnimationField(new Rect(), new GUIContent(), IdleWarningProp, "Idle Warning", typeof(AnimationClip), false);
                            CustomHelpLabelField("Controls the animation that the AI will play to warn a target that they will attack, if the target doesn't leave their attack radius soon.", false);
                            CustomFloatAnimationField(new Rect(), new GUIContent(), IdleWarningAnimationSpeedProp, "Animation Speed", 0.1f, 2);
                            EditorGUILayout.Space();
                        }

                        if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes || self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                        {
                            CustomAnimationField(new Rect(), new GUIContent(), RangedIdleWarningProp, "Ranged Idle Warning", typeof(AnimationClip), false);
                            CustomHelpLabelField("Controls the animation that the AI will play to warn a target that they will attack, if the target doesn't leave their attack radius soon.", false);
                            CustomFloatAnimationField(new Rect(), new GUIContent(), RangedIdleWarningAnimationSpeedProp, "Animation Speed", 0.1f, 2);
                        }

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                }

                if (AnimationTabNumberProp.intValue == 1)
                {
                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 53));
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Movement Animations", style3, GUILayout.ExpandWidth(true));
                    EditorGUILayout.Space();
                    GUIContent[] MovementButtons = new GUIContent[2] { new GUIContent("Non-Combat Movement"), new GUIContent("Combat Movement") };
                    MovementTabNumberProp.intValue = GUILayout.Toolbar(MovementTabNumberProp.intValue, MovementButtons, EditorStyles.miniButton, GUILayout.Height(25), GUILayout.Width(EditorGUIUtility.currentViewWidth - 53));

                    if (MovementTabNumberProp.intValue == 1)
                    {
                        EditorGUILayout.Space();
                        if (GUILayout.Button("Copy from non-Combat Movement"))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "If your AI has the same combat and non-combat movement animations, you can use this so you don't have to manually re-apply the same animations. " +
                                "Are you sure you want to copy all animations from the non-Combat Movement section to the Combat Movement?", "Yes", "Cancel"))
                            {
                                CopyFromNonCombat();
                            }
                        }
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    if (MovementTabNumberProp.intValue == 0)
                    {
                        WalkFoldout.boolValue = CustomEditorProperties.Foldout(WalkFoldout.boolValue, "Walk Animations", true, myFoldoutStyle);

                        if (WalkFoldout.boolValue)
                        {
                            EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 40));
                            CustomFloatAnimationField(new Rect(), new GUIContent(), NonCombatWalkAnimationSpeedProp, "Walk Speed", 0.5f, 2);
                            CustomHelpLabelField("Controls how fast your AI's Walk straight, left, and right animations play. When using Root Motion, this will also " +
                                "control your AI's walk movement speed.", true);

                            //Walk Straight
                            CustomAnimationField(new Rect(), new GUIContent(), WalkStraightProp, "Walk Straight Animation", typeof(AnimationClip), false);
                            CustomHelpLabelField("The walk animation that plays when your AI is walking straight when not in combat.", false);
                            if (self.WalkStraightAnimation != null)
                            {
                                var settings = AnimationUtility.GetAnimationClipSettings(self.WalkStraightAnimation);
                                if (!settings.loopTime)
                                {
                                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                    EditorGUILayout.LabelField("The 'Walk Straight' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                                    GUI.backgroundColor = Color.white;
                                }
                            }
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();

                            //Walk Left
                            CustomAnimationField(new Rect(), new GUIContent(), WalkLeftProp, "Walk Left Animation", typeof(AnimationClip), false);
                            CustomHelpLabelField("The walk animation that plays when your AI is walking left when not in combat.", false);
                            CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorWalkLeftProp, "Mirror Walk Left");
                            CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                            if (self.WalkLeftAnimation != null)
                            {
                                var settings = AnimationUtility.GetAnimationClipSettings(self.WalkLeftAnimation);
                                if (!settings.loopTime)
                                {
                                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                    EditorGUILayout.LabelField("The 'Walk Left' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                                    GUI.backgroundColor = Color.white;
                                }
                            }
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();

                            //Walk Right
                            CustomAnimationField(new Rect(), new GUIContent(), WalkRightProp, "Walk Right Animation", typeof(AnimationClip), false);
                            CustomHelpLabelField("The walk animation that plays when your AI is walking right when not in combat.", false);
                            CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorWalkRightProp, "Mirror Walk Right");
                            CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                            if (self.WalkRightAnimation != null)
                            {
                                var settings = AnimationUtility.GetAnimationClipSettings(self.WalkRightAnimation);
                                if (!settings.loopTime)
                                {
                                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                    EditorGUILayout.LabelField("The 'Walk Right' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                                    GUI.backgroundColor = Color.white;
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }

                        EditorGUILayout.Space();

                        RunFoldout.boolValue = CustomEditorProperties.Foldout(RunFoldout.boolValue, "Run Animations", true, myFoldoutStyle);

                        if (RunFoldout.boolValue)
                        {
                            EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 40));
                            CustomFloatAnimationField(new Rect(), new GUIContent(), NonCombatRunAnimationSpeedProp, "Run Speed", 0.5f, 2);
                            CustomHelpLabelField("Controls how fast your AI's Run straight, left, and right animations play. When using Root Motion, " +
                                "this will also control your AI's run movement speed.", true);

                            //Run Straight
                            CustomAnimationField(new Rect(), new GUIContent(), RunStraightProp, "Run Straight Animation", typeof(AnimationClip), false);
                            CustomHelpLabelField("The run animation that plays when your AI is running straight when not in combat.", false);
                            if (self.RunStraightAnimation != null)
                            {
                                var settings = AnimationUtility.GetAnimationClipSettings(self.RunStraightAnimation);
                                if (!settings.loopTime)
                                {
                                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                    EditorGUILayout.LabelField("The 'Run Straight' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                                    GUI.backgroundColor = Color.white;
                                }
                            }

                            EditorGUILayout.Space();
                            EditorGUILayout.Space();

                            //Run Left
                            CustomAnimationField(new Rect(), new GUIContent(), RunLeftProp, "Run Left Animation", typeof(AnimationClip), false);
                            CustomHelpLabelField("The run animation that plays when your AI is running left when not in combat.", false);
                            CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorRunLeftProp, "Mirror Run Left");
                            CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                            if (self.RunLeftAnimation != null)
                            {
                                var settings = AnimationUtility.GetAnimationClipSettings(self.RunLeftAnimation);
                                if (!settings.loopTime)
                                {
                                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                    EditorGUILayout.LabelField("The 'Run Left' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                                    GUI.backgroundColor = Color.white;
                                }
                            }

                            EditorGUILayout.Space();
                            EditorGUILayout.Space();

                            //Run Right
                            CustomAnimationField(new Rect(), new GUIContent(), RunRightProp, "Run Right Animation", typeof(AnimationClip), false);
                            CustomHelpLabelField("The run animation that plays when your AI is running right when not in combat.", false);
                            CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorRunRightProp, "Mirror Run Right");
                            CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                            if (self.RunRightAnimation != null)
                            {
                                var settings = AnimationUtility.GetAnimationClipSettings(self.RunRightAnimation);
                                if (!settings.loopTime)
                                {
                                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                    EditorGUILayout.LabelField("The 'Run Right' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                                    GUI.backgroundColor = Color.white;
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }

                        EditorGUILayout.Space();

                        TurnFoldout.boolValue = CustomEditorProperties.Foldout(TurnFoldout.boolValue, "Turn Animations", true, myFoldoutStyle);

                        if (TurnFoldout.boolValue)
                        {
                            EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 40));
                            //Turn Left
                            CustomAnimationField(new Rect(), new GUIContent(), TurnLeftProp, "Turn Left", typeof(AnimationClip), false);
                            CustomHelpLabelField("The animation clip for turning right when not in combat.", false);
                            CustomFloatAnimationField(new Rect(), new GUIContent(), TurnLeftAnimationSpeedProp, "Turn Left Animation Speed", 0.1f, 2);
                            CustomHelpLabelField("The speed in which the turn right animation will play.", false);
                            CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorTurnLeftProp, "Mirror Turn Left");
                            CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                            if (self.NonCombatTurnLeftAnimation != null)
                            {
                                var settings = AnimationUtility.GetAnimationClipSettings(self.NonCombatTurnLeftAnimation);
                                if (!settings.loopTime)
                                {
                                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                    EditorGUILayout.LabelField("The 'Turn Left' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                                    GUI.backgroundColor = Color.white;
                                }
                            }
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();

                            //Turn Right
                            CustomAnimationField(new Rect(), new GUIContent(), TurnRightProp, "Turn Right", typeof(AnimationClip), false);
                            CustomHelpLabelField("The animation clip for turning right when not in combat.", false);
                            CustomFloatAnimationField(new Rect(), new GUIContent(), TurnRightAnimationSpeedProp, "Turn Right Animation Speed", 0.1f, 2);
                            CustomHelpLabelField("The speed in which the turn right animation will play.", false);
                            CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorTurnRightProp, "Mirror Turn Right");
                            CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                            if (self.NonCombatTurnRightAnimation != null)
                            {
                                var settings = AnimationUtility.GetAnimationClipSettings(self.NonCombatTurnRightAnimation);
                                if (!settings.loopTime)
                                {
                                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                                    EditorGUILayout.LabelField("The 'Turn Right' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                                    GUI.backgroundColor = Color.white;
                                }
                            }
                            EditorGUILayout.Space();
                            EditorGUILayout.EndVertical();
                        }
                    }

                    if (MovementTabNumberProp.intValue == 1)
                    {
                        if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
                        {
                            EditorGUILayout.BeginVertical("Box");
                            EditorGUILayout.Space();
                            EditorGUILayout.LabelField("Movement Types", style3, GUILayout.ExpandWidth(true));
                            EditorGUILayout.Space();
                            GUIContent[] WeaponTypeButtons = new GUIContent[2] { new GUIContent("Melee Movement"), new GUIContent("Ranged Movement") };
                            WeaponTypeTabNumberProp.intValue = GUILayout.Toolbar(WeaponTypeTabNumberProp.intValue, WeaponTypeButtons, EditorStyles.miniButton, GUILayout.Height(25));
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();

                            if (WeaponTypeTabNumberProp.intValue == 0)
                            {
                                DefaultCombatMovement();
                            }
                            else if (WeaponTypeTabNumberProp.intValue == 1)
                            {
                                RangedCombatMovement();
                            }
                        }
                        else
                        {
                            if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No)
                            {
                                DefaultCombatMovement();
                            }
                            else if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged && self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No)
                            {
                                RangedCombatMovement();
                            }
                        }
                    }
                }

                if (AnimationTabNumberProp.intValue == 2)
                {
                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 40));
                    EditorGUILayout.Space();

                    if (self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
                    {
                        GUIContent[] WeaponTypeButtons = new GUIContent[2] { new GUIContent("Melee Animations"), new GUIContent("Ranged Animations") };
                        WeaponTypeCombatTabNumberProp.intValue = GUILayout.Toolbar(WeaponTypeCombatTabNumberProp.intValue, WeaponTypeButtons, EditorStyles.miniButton, GUILayout.Height(25));
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();

                        if (WeaponTypeCombatTabNumberProp.intValue == 0)
                        {
                            DefaultCombatAnimations();
                        }
                        else if (WeaponTypeCombatTabNumberProp.intValue == 1)
                        {
                            RangedCombatAnimations();
                        }
                    }
                    else
                    {
                        if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No)
                        {
                            DefaultCombatAnimations();
                        }
                        else if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged && self.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No)
                        {
                            RangedCombatAnimations();
                        }
                    }

                    EditorGUILayout.EndVertical();
                }

                if (AnimationTabNumberProp.intValue == 3)
                {
                    EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 40));
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Emote Animations", EditorStyles.boldLabel);
                    GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                    EditorGUILayout.LabelField("Controls the emote animations that will play when an AI's PlayEmoteAnimation function is called and passing the emote ID as the parameter. The speed of each animation can be adjusted by changing the speed parameter. A max of 10 can be used.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                    EmoteAnimationList.DoLayoutList();

                    //Emote
                    if (self.EmoteAnimationList.Count == 10)
                    {
                        EmoteAnimationList.displayAdd = false;
                    }
                    else
                    {
                        EmoteAnimationList.displayAdd = true;
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 38));
                GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                EditorGUILayout.LabelField("Animation Profile", EditorStyles.boldLabel);
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.LabelField("Animation Profiles allow you to export and import animation clips between AI to avoid having to apply them manually. This is useful if you " +
                    "have multiple AI that all use the same animations and share the same rigging.", EditorStyles.helpBox);
                GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                EditorGUILayout.LabelField("Note: The animations must be compatible with this model and share the same rigging. Some settings, such as animation specific settings," +
                    " will also be imported.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
                var buttonstyle = new GUIStyle(GUI.skin.button);
                EditorGUILayout.Space();

                //Import
                CustomHelpLabelField("Imports all animations from the current Animation Profile.", false);
                EditorGUILayout.PropertyField(AnimationProfileProp);

                if (GUILayout.Button("Import Animation Profile", buttonstyle))
                {
                    var option = EditorUtility.DisplayDialog(
                    "Import Animation Profile",
                    "Importing an Animation Profile will replace all of this AI's current animations with the ones from the Animation Profile. This process cannot be undone. You can choose to " +
                    "import with the AI's existing Animator Controller or use the Animator Controller from the Animation Profile.",
                    "Import",
                    "Cancel");

                    if (option)
                    {
                        var option2 = EditorUtility.DisplayDialogComplex(
                        "Import Animation Profile",
                        "Would you like to import the Animator Controller with this Animation Profile?",
                        "Yes",
                        "No", "Cancel");

                        if (option2 == 0 && self.m_AnimationProfile != null) //Import with Animator Controller
                        {
                            AnimationProfileExporter.ImportAnimationProfile(self, self.m_AnimationProfile, serializedObject, true);
                            AnimationsUpdatedProp.boolValue = false;
                            EditorUtility.SetDirty(self);
                            UpdateAbilityAnimationEnums();

                            if (Selection.gameObjects.Length > 1)
                            {
                                foreach (GameObject G in Selection.gameObjects)
                                {
                                    if (G.GetComponent<EmeraldAISystem>() != null)
                                    {
                                        EmeraldAISystem EmeraldComponent = G.GetComponent<EmeraldAISystem>();
                                        AnimationProfileExporter.ImportAnimationProfile(EmeraldComponent, EmeraldComponent.m_AnimationProfile, serializedObject, true);
                                    }
                                }
                            }
                        }
                        else if (option2 == 1 && self.m_AnimationProfile != null) //Import without Animator Controller
                        {
                            AnimationProfileExporter.ImportAnimationProfile(self, self.m_AnimationProfile, serializedObject, false);
                            AnimationsUpdatedProp.boolValue = false;
                            EditorUtility.SetDirty(self);
                            UpdateAbilityAnimationEnums();

                            if (Selection.gameObjects.Length > 1)
                            {
                                foreach (GameObject G in Selection.gameObjects)
                                {
                                    if (G.GetComponent<EmeraldAISystem>() != null)
                                    {
                                        EmeraldAISystem EmeraldComponent = G.GetComponent<EmeraldAISystem>();
                                        AnimationProfileExporter.ImportAnimationProfile(EmeraldComponent, EmeraldComponent.m_AnimationProfile, serializedObject, false);
                                    }
                                }
                            }
                        }
                        else if (option2 != 2 || self.m_AnimationProfile == null && option2 != 2) //Import Failed
                        {
                            EditorUtility.DisplayDialog(
                    "Import Error",
                    "There is currently no Animation Profile in this AI's Animation Profile slot. Please apply one and try again.",
                    "Okay");
                        }
                    }
                }
                EditorGUILayout.Space();

                //Export
                if (self.AnimatorControllerGenerated)
                {
                    CustomHelpLabelField("Exports all animations to an Animation Profile to be imported and shared with other AI so animations don't have to be applied manually.", false);
                    if (GUILayout.Button("Export Animation Profile", buttonstyle))
                    {
                        string SavePath = EditorUtility.SaveFilePanelInProject("Save Animation Profile", "New Animation Profile", "asset", "Please enter a file name to save the file to");
                        if (SavePath != string.Empty)
                        {
                            AnimationProfileExporter.ExportAnimationProfile(SavePath, self);
                        }
                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 38));
                if (!self.AnimatorControllerGenerated)
                {
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.LabelField("In order for this AI to have animations, you must create an Animator Controller for it. To do so, press the 'Create Animator Controller' button below.'", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }

                if (self.AnimatorControllerGenerated)
                {
                    GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                    EditorGUILayout.LabelField("Clears the current Animator Controller so a new one can be created.", EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;
                }

                if (!self.AnimatorControllerGenerated)
                {
                    if (GUILayout.Button("Create Animator Controller"))
                    {
                        self.FilePath = EditorUtility.SaveFilePanelInProject("Save as OverrideController", "New OverrideController", "overrideController", "Please enter a file name to save the file to");
                        if (self.FilePath != string.Empty)
                        {
                            string UserFilePath = self.FilePath;
                            string SourceFilePath = AssetDatabase.GetAssetPath(Resources.Load("Emerald Animator Controller"));
                            AssetDatabase.CopyAsset(SourceFilePath, UserFilePath);
                            self.AIAnimator = self.gameObject.GetComponent<Animator>();
                            self.AIAnimator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath(UserFilePath, typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

                            if (Selection.gameObjects.Length > 1)
                            {
                                foreach (GameObject G in Selection.gameObjects)
                                {
                                    if (G.GetComponent<EmeraldAISystem>() != null)
                                    {
                                        EmeraldAISystem EmeraldComponent = G.GetComponent<EmeraldAISystem>();
                                        EmeraldComponent.AIAnimator = G.GetComponent<Animator>();
                                        EmeraldComponent.AIAnimator.runtimeAnimatorController = self.AIAnimator.runtimeAnimatorController;
                                        EmeraldComponent.AnimatorControllerGenerated = true;
                                    }
                                }
                            }

                            EmeraldAIAnimatorGenerator.GenerateAnimatorController(self);
                            serializedObject.Update();
                            self.AnimatorControllerGenerated = true;
                            AnimationsUpdatedProp.boolValue = false;
                            EditorUtility.SetDirty(self);
                            UpdateAbilityAnimationEnums();
                        }
                    }
                }

                if (self.AnimatorControllerGenerated)
                {
                    var style = new GUIStyle(GUI.skin.button);
                    style.normal.textColor = Color.red;
                    if (GUILayout.Button("Clear Animator Controller", style) && EditorUtility.DisplayDialog("Clear Animator Controller?", "Are you sure you want to clear this AI's Animator Controller? This process cannot be undone.", "Yes", "Cancel"))
                    {
                        foreach (GameObject G in Selection.gameObjects)
                        {
                            if (G.GetComponent<EmeraldAISystem>() != null)
                            {
                                G.GetComponent<EmeraldAISystem>().AIAnimator = null;
                                G.GetComponent<EmeraldAISystem>().AnimatorControllerGenerated = false;
                            }
                        }
                    }
                    GUI.contentColor = Color.white;
                    GUI.backgroundColor = Color.white;
                }

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }

            if (TabNumberProp.intValue == 7)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                //EditorGUILayout.BeginVertical("Box", GUILayout.Width(85 * Screen.width / Screen.dpi));
                EditorGUILayout.BeginVertical("Box");
                var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField(new GUIContent(DocumentationIcon), style, GUILayout.ExpandWidth(true), GUILayout.Height(32));
                EditorGUILayout.LabelField("Documentation", style, GUILayout.ExpandWidth(true));
                GUILayout.Space(2);
                EditorGUILayout.LabelField("Emerald's Docs can all be found below. This is to give users easy access to tutorials, script references, and documentation all from " +
                    "within the Emerald Editor. Each section is online so users always get the most up to date material.", EditorStyles.helpBox);
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Documentation", EditorStyles.boldLabel);
                CustomHelpLabelField("Contains detailed guides and tutorials to help get you started and familiar with Emerald.", false);
                if (GUILayout.Button("Documentation", GUILayout.Height(28)))
                {
                    Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki");
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Integration Tutorials", EditorStyles.boldLabel);
                CustomHelpLabelField("Written tutorials covering the integration of some of the top character controller systems such as UFPS, RFPS, Game Kit Controller, " +
                    "Invector Third Person Controller, and Ootii Motion Controller.", false);
                if (GUILayout.Button("Integration Tutorials", GUILayout.Height(28)))
                {
                    Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Integration-Tutorials");
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Video Tutorials", EditorStyles.boldLabel);
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
                EditorGUILayout.LabelField("Various video tutorials covering a wide range of features and implementations.", EditorStyles.helpBox);
                GUI.backgroundColor = Color.white;
                if (GUILayout.Button("Video Tutorials", GUILayout.Height(28)))
                {
                    Application.OpenURL("https://www.youtube.com/playlist?list=PLlyiPBj7FznY7q4bdDQgGYgUByYpeCe07");
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Scripting Reference", EditorStyles.boldLabel);
                CustomHelpLabelField("All of Emerald AI's API.", false);
                if (GUILayout.Button("Scripting Reference", GUILayout.Height(28)))
                {
                    Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Emerald-AI-API");
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Emerald AI Forums", EditorStyles.boldLabel);
                CustomHelpLabelField("The forums are a great way to get quick support as well being kept update to date on upcoming updates.", false);
                if (GUILayout.Button("Emerald AI Forums", GUILayout.Height(28)))
                {
                    Application.OpenURL("https://forum.unity.com/threads/update-coming-soon-emerald-ai-dynamic-wildlife-breeding-predators-prey-herds-npcs-more.336521/");
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Customer Support", EditorStyles.boldLabel);
                CustomHelpLabelField("All the contact information you will need to get quick support.", false);
                if (GUILayout.Button("Customer Support", GUILayout.Height(28)))
                {
                    Application.OpenURL("http://www.blackhorizonstudios.com/contact/");
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Report a Bug", EditorStyles.boldLabel);
                CustomHelpLabelField("If you've encountered a bug, you can fill out a bug report here. This allows bugs to be well documented so they can be fixed as soon as possible.", false);
                if (GUILayout.Button("Report a Bug", GUILayout.Height(28)))
                {
                    Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdzHNlLSCyv2k3LPT5STMSRuWPLFIanci0rTuC7BjQQgAoDgA/viewform?usp=sf_link");
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(EnableDebuggingPop, new GUIContent("Enable Debugging"));
                CustomHelpLabelField("Enables certain debugging options to assist development or help find issues.", true);

                if (self.EnableDebugging == EmeraldAISystem.YesOrNo.Yes)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    EditorGUILayout.BeginVertical();

                    EditorGUILayout.PropertyField(DrawRaycastsEnabledProp, new GUIContent("Draw Raycasts"));
                    CustomHelpLabelField("Allows raycasts to be draw, while in the Unity Editor. " +
                        "This can be useful for ensuring raycast are being positioned correctly.", true);

                    EditorGUILayout.PropertyField(DebugLogTargetsEnabledProp, new GUIContent("Debug Log Targets"));
                    CustomHelpLabelField("Allows the target objects to be displayed in the Unity Console. " +
                        "This can be useful for ensure the proper object is being hit when the AI is targeting an object.", true);

                    EditorGUILayout.PropertyField(DebugLogObstructionsEnabledProp, new GUIContent("Debug Log Obstructions"));
                    CustomHelpLabelField("Allows the AI's obstructions to be displayed in the Unity Console. " +
                        "This can be useful for ensure the proper object is being hit when the AI is targeting an object.", true);

                    EditorGUILayout.PropertyField(DebugLogProjectileCollisionsEnabledProp, new GUIContent("Debug Log Projectile Collisions"));
                    CustomHelpLabelField("Allows the AI's projectile collisions to be displayed in the Unity Console. " +
                        "This can be useful for ensure the proper object is being hit when the AI is firing its projectile.", true);

                    EditorGUILayout.PropertyField(DebugLogMissingAnimationsProp, new GUIContent("Debug Log Missing Animations and Events"));
                    CustomHelpLabelField("Controls whether or not Emerald AI will assist users by Debug Logging which animaions are missing and if they need certain Animation Events " +
                        "in order for everything to function properly. It is recommended to keep this enabled, but it isn't required.", true);

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }

        //Draw all of our Editor related controls, settings, and effects
        void OnSceneGUI()
        {
            EmeraldAISystem self = (EmeraldAISystem)target;

            if (TabNumberProp.intValue == 2 && DetectionTagsTabNumberProp.intValue == 0)
            {
                Handles.color = new Color(255, 0, 0, 0.5f);
                self.m_ProjectileCollisionPoint = new Vector3(self.transform.position.x, self.transform.position.y + self.ProjectileCollisionPointY, self.transform.position.z);
                Handles.SphereHandleCap(0, self.m_ProjectileCollisionPoint, Quaternion.identity, 0.5f, EventType.Repaint);
                GUIStyle style = new GUIStyle();
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.white;
                style.alignment = TextAnchor.MiddleCenter;
                Handles.Label(self.m_ProjectileCollisionPoint + Vector3.right / 5 + Vector3.up / 2, "Hit Transform", style);
            }

            //Draw two arcs each with 50% of the field of view in opposite directions
            Handles.color = new Color(255, 0, 0, 0.07f);
            if (self.DetectionTypeRef == EmeraldAISystem.DetectionType.LineOfSight && TabNumberProp.intValue == 2 && DetectionTagsTabNumberProp.intValue == 0)
            {
                Handles.DrawSolidArc(self.transform.position, self.transform.up, self.transform.forward, self.fieldOfViewAngle / 2, (float)self.DetectionRadius);
                Handles.DrawSolidArc(self.transform.position, self.transform.up, self.transform.forward, -self.fieldOfViewAngle / 2, (float)self.DetectionRadius);
                Handles.color = Color.white;
            }
            else if (self.DetectionTypeRef == EmeraldAISystem.DetectionType.Trigger && TabNumberProp.intValue == 2 && DetectionTagsTabNumberProp.intValue == 0)
            {
                Handles.DrawSolidDisc(self.transform.position, self.transform.up, (float)self.DetectionRadius);
                Handles.color = Color.white;
            }

            if (self.WanderTypeRef == EmeraldAISystem.WanderType.Dynamic && TabNumberProp.intValue == 0)
            {
                Handles.color = new Color(0, 255, 0, 1.0f);
                Handles.DrawWireDisc(self.transform.position, self.transform.up, (float)self.WanderRadius);
                Handles.color = Color.white;
            }

            if (TabNumberProp.intValue == 3)
            {
                if (self.DisplayAINameRef == EmeraldAISystem.DisplayAIName.Yes)
                {
                    Handles.color = self.NameTextColor;
                    Handles.DrawLine(new Vector3(self.transform.localPosition.x, self.transform.localPosition.y, self.transform.localPosition.z),
                        new Vector3(self.AINamePos.x, self.AINamePos.y, self.AINamePos.z) + self.transform.localPosition);
                    Handles.color = Color.white;
                }

                if (self.CreateHealthBarsRef == EmeraldAISystem.CreateHealthBars.Yes)
                {
                    Handles.color = self.HealthBarColor;
                    Handles.DrawLine(new Vector3(self.transform.localPosition.x + 0.25f, self.transform.localPosition.y, self.transform.localPosition.z),
                        new Vector3(self.HealthBarPos.x + 0.25f, self.HealthBarPos.y, self.HealthBarPos.z) + self.transform.localPosition);
                    Handles.color = Color.white;
                }
            }

            if (TabNumberProp.intValue == 5 && self.WanderTypeRef == EmeraldAISystem.WanderType.Waypoints)
            {
                if (self.WaypointsList.Count > 0 && self.WaypointsList != null)
                {
                    Handles.color = Color.blue;
                    Handles.DrawLine(self.transform.position, self.WaypointsList[0]);
                    Handles.color = Color.white;

                    Handles.color = Color.green;
                    if (self.WaypointTypeRef != (EmeraldAISystem.WaypointType.Random))
                    {
                        for (int i = 0; i < self.WaypointsList.Count - 1; i++)
                        {
                            Handles.DrawLine(self.WaypointsList[i], self.WaypointsList[i + 1]);
                        }
                    }
                    else if (self.WaypointTypeRef == (EmeraldAISystem.WaypointType.Random))
                    {
                        for (int i = 0; i < self.WaypointsList.Count; i++)
                        {
                            for (int j = (i + 1); j < self.WaypointsList.Count; j++)
                            {
                                Handles.DrawLine(self.WaypointsList[i], self.WaypointsList[j]);
                            }
                        }
                    }
                    Handles.color = Color.white;

                    Handles.color = Color.green;
                    if (self.WaypointTypeRef == (EmeraldAISystem.WaypointType.Loop))
                    {
                        Handles.DrawLine(self.WaypointsList[0], self.WaypointsList[self.WaypointsList.Count - 1]);
                    }
                    Handles.color = Color.white;

                    Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
                    for (int i = 0; i < self.WaypointsList.Count; i++)
                    {
                        Handles.SphereHandleCap(0, self.WaypointsList[i], Quaternion.identity, 0.5f, EventType.Repaint);
                        CustomEditorProperties.DrawString("Waypoint " + (i + 1), self.WaypointsList[i] + Vector3.up, Color.white);
                    }

                    Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
                    for (int i = 0; i < self.WaypointsList.Count; i++)
                    {
                        self.WaypointsList[i] = Handles.PositionHandle(self.WaypointsList[i], Quaternion.identity);
                    }

#if UNITY_EDITOR
                    EditorUtility.SetDirty(self);
#endif
                }
            }

            if (TabNumberProp.intValue == 0 && self.WanderTypeRef == EmeraldAISystem.WanderType.Destination && self.SingleDestination != Vector3.zero)
            {
                Handles.color = Color.green;
                Handles.DrawLine(self.transform.position, self.SingleDestination);
                Handles.color = Color.white;

                Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
                Handles.SphereHandleCap(0, self.SingleDestination, Quaternion.identity, 0.5f, EventType.Repaint);
                CustomEditorProperties.DrawString("Destination Point", self.SingleDestination + Vector3.up, Color.white);

                Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
                self.SingleDestination = Handles.PositionHandle(self.SingleDestination, Quaternion.identity);

#if UNITY_EDITOR
                EditorUtility.SetDirty(self);
#endif
            }
        }

        void DefaultCombatMovement()
        {
            EmeraldAISystem self = (EmeraldAISystem)target;
            GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            myFoldoutStyle.fontStyle = FontStyle.Bold;
            myFoldoutStyle.fontSize = 12;
            myFoldoutStyle.active.textColor = Color.black;
            myFoldoutStyle.focused.textColor = Color.black;
            myFoldoutStyle.onHover.textColor = Color.black;
            myFoldoutStyle.normal.textColor = Color.black;
            myFoldoutStyle.onNormal.textColor = Color.black;
            myFoldoutStyle.onActive.textColor = Color.black;
            myFoldoutStyle.onFocused.textColor = Color.black;
            Color myStyleColor = Color.black;

            CombatWalkFoldout.boolValue = CustomEditorProperties.Foldout(CombatWalkFoldout.boolValue, "Combat Walk Animations", true, myFoldoutStyle);

            if (CombatWalkFoldout.boolValue)
            {
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 40));
                CustomFloatAnimationField(new Rect(), new GUIContent(), CombatWalkAnimationSpeedProp, "Combat Walk Speed", 0.5f, 2);
                CustomHelpLabelField("Controls how fast your AI's Combat Walk straight, left, and right animations play. When using Root Motion, " +
                    "this will also control your AI's walk movement speed.", true);

                //Combat Walk Straight
                CustomAnimationField(new Rect(), new GUIContent(), CombatWalkStraightProp, "Combat Walk Straight Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The walk animation that plays when your AI is walking straight when not in combat.", false);
                if (self.CombatWalkStraightAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.CombatWalkStraightAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Walk Straight' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Combat Walk Left
                CustomAnimationField(new Rect(), new GUIContent(), CombatWalkLeftProp, "Combat Walk Left Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The walk animation that plays when your AI is walking left when in combat.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorCombatWalkLeftProp, "Mirror Combat Walk Left");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.CombatWalkLeftAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.CombatWalkLeftAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Walk Left' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Combat Walk Right
                CustomAnimationField(new Rect(), new GUIContent(), CombatWalkRightProp, "Combat Walk Right Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The walk animation that plays when your AI is walking right when in combat.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorCombatWalkRightProp, "Mirror Combat Walk Right");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.CombatWalkRightAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.CombatWalkRightAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Walk Right' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Walk Back
                CustomAnimationField(new Rect(), new GUIContent(), WalkBackProp, "Combat Walk Back Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The walk animation that plays when your AI is walking backwards when in combat.", false);
                EditorGUILayout.PropertyField(ReverseWalkAnimationProp, new GUIContent("Reverse Walk Back"));
                CustomHelpLabelField("Reverses the Combat Walk Back animation. This is useful if a model doesn't have a walk backwards animation. " +
                    "The model's walk animation can be used and will be reversed.", false);
                if (self.CombatWalkBackAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.CombatWalkBackAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Walk Back' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();

            CombatRunFoldout.boolValue = CustomEditorProperties.Foldout(CombatRunFoldout.boolValue, "Combat Run Animations", true, myFoldoutStyle);

            if (CombatRunFoldout.boolValue)
            {
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 40));
                CustomFloatAnimationField(new Rect(), new GUIContent(), CombatRunAnimationSpeedProp, "Run Speed", 0.5f, 2);
                CustomHelpLabelField("Controls how fast your AI's Combat Run straight, left, and right animations play. When using Root Motion, " +
                    "this will also control your AI's run movement speed.", true);

                //Combat Run Straight
                CustomAnimationField(new Rect(), new GUIContent(), CombatRunStraightProp, "Combat Run Straight Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The run animation that plays when your AI is running straight when in combat.", false);
                if (self.CombatRunStraightAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.CombatRunStraightAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Run Straight' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Combat Run Left
                CustomAnimationField(new Rect(), new GUIContent(), CombatRunLeftProp, "Combat Run Left Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The run animation that plays when your AI is running left when in combat.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorCombatRunLeftProp, "Mirror Combat Run Left");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.CombatRunLeftAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.CombatRunLeftAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Run Left' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Combat Run Right
                CustomAnimationField(new Rect(), new GUIContent(), CombatRunRightProp, "Combat Run Right Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The run animation that plays when your AI is running right when in combat.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorCombatRunRightProp, "Mirror Combat Run Right");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.CombatRunRightAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.CombatRunRightAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Run Right' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();

            CombatTurnFoldout.boolValue = CustomEditorProperties.Foldout(CombatTurnFoldout.boolValue, "Combat Turn Animations", true, myFoldoutStyle);

            if (CombatTurnFoldout.boolValue)
            {
                EditorGUILayout.BeginVertical("Box", GUILayout.Width(EditorGUIUtility.currentViewWidth - 40));
                //Combat Turn Left
                CustomAnimationField(new Rect(), new GUIContent(), CombatTurnLeftProp, "Combat Turn Left", typeof(AnimationClip), false);
                CustomHelpLabelField("The turn animation that plays when your AI is turning left when in combat.", false);
                CustomFloatAnimationField(new Rect(), new GUIContent(), CombatTurnLeftAnimationSpeedProp, "Turn Left Animation Speed", 0.1f, 2);
                CustomHelpLabelField("The speed in which the combat turn left animation will play.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorCombatTurnLeftProp, "Mirror Combat Turn Left");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.CombatTurnLeftAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.CombatTurnLeftAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Turn Left' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Combat Turn Right
                CustomAnimationField(new Rect(), new GUIContent(), CombatTurnRightProp, "Combat Turn Right", typeof(AnimationClip), false);
                CustomHelpLabelField("The turn animation that plays when your AI is turning right when in combat.", false);
                CustomFloatAnimationField(new Rect(), new GUIContent(), CombatTurnRightAnimationSpeedProp, "Turn Right Animation Speed", 0.1f, 2);
                CustomHelpLabelField("The speed in which the combat turn left animation will play.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorCombatTurnRightProp, "Mirror Combat Turn Right");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.CombatTurnRightAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.CombatTurnRightAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Turn Right' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }
        }

        void RangedCombatMovement()
        {
            EmeraldAISystem self = (EmeraldAISystem)target;
            GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            myFoldoutStyle.fontStyle = FontStyle.Bold;
            myFoldoutStyle.fontSize = 12;
            myFoldoutStyle.active.textColor = Color.black;
            myFoldoutStyle.focused.textColor = Color.black;
            myFoldoutStyle.onHover.textColor = Color.black;
            myFoldoutStyle.normal.textColor = Color.black;
            myFoldoutStyle.onNormal.textColor = Color.black;
            myFoldoutStyle.onActive.textColor = Color.black;
            myFoldoutStyle.onFocused.textColor = Color.black;
            Color myStyleColor = Color.black;

            CombatWalkFoldout.boolValue = CustomEditorProperties.Foldout(CombatWalkFoldout.boolValue, "Combat Walk Animations", true, myFoldoutStyle);

            if (CombatWalkFoldout.boolValue)
            {
                EditorGUILayout.BeginVertical("Box");
                CustomFloatAnimationField(new Rect(), new GUIContent(), RangedCombatWalkAnimationSpeedProp, "Combat Walk Speed", 0.5f, 2);
                CustomHelpLabelField("Controls how fast your AI's Combat Walk straight, left, and right animations play. When using Root Motion, " +
                    "this will also control your AI's walk movement speed.", true);

                //Combat Walk Straight
                CustomAnimationField(new Rect(), new GUIContent(), RangedCombatWalkStraightProp, "Combat Walk Straight Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The walk animation that plays when your AI is walking straight when not in combat.", false);
                if (self.RangedCombatWalkStraightAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.RangedCombatWalkStraightAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Walk Straight' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Combat Walk Left
                CustomAnimationField(new Rect(), new GUIContent(), RangedCombatWalkLeftProp, "Combat Walk Left Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The walk animation that plays when your AI is walking left when in combat.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorRangedCombatWalkLeftProp, "Mirror Combat Walk Left");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.RangedCombatWalkLeftAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.RangedCombatWalkLeftAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Walk Left' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Combat Walk Right
                CustomAnimationField(new Rect(), new GUIContent(), RangedCombatWalkRightProp, "Combat Walk Right Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The walk animation that plays when your AI is walking right when in combat.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorRangedCombatWalkRightProp, "Mirror Combat Walk Right");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.RangedCombatWalkRightAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.RangedCombatWalkRightAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Walk Right' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Walk Back
                CustomAnimationField(new Rect(), new GUIContent(), RangedWalkBackProp, "Combat Walk Back Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The walk animation that plays when your AI is walking backwards when in combat.", false);
                EditorGUILayout.PropertyField(ReverseRangedWalkAnimationProp, new GUIContent("Reverse Walk Back"));
                CustomHelpLabelField("Reverses the Combat Walk Back animation. This is useful if a model doesn't have a walk backwards animation. " +
                    "The model's walk animation can be used and will be reversed.", false);
                if (self.RangedCombatWalkBackAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.RangedCombatWalkBackAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Walk Back' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();

            CombatRunFoldout.boolValue = CustomEditorProperties.Foldout(CombatRunFoldout.boolValue, "Combat Run Animations", true, myFoldoutStyle);

            if (CombatRunFoldout.boolValue)
            {
                EditorGUILayout.BeginVertical("Box");
                CustomFloatAnimationField(new Rect(), new GUIContent(), RangedCombatRunAnimationSpeedProp, "Run Speed", 0.5f, 2);
                CustomHelpLabelField("Controls how fast your AI's Combat Run straight, left, and right animations play. When using Root Motion, " +
                    "this will also control your AI's run movement speed.", true);

                //Combat Run Straight
                CustomAnimationField(new Rect(), new GUIContent(), RangedCombatRunStraightProp, "Combat Run Straight Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The run animation that plays when your AI is running straight when in combat.", false);
                if (self.RangedCombatRunStraightAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.RangedCombatRunStraightAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Run Straight' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Combat Run Left
                CustomAnimationField(new Rect(), new GUIContent(), RangedCombatRunLeftProp, "Combat Run Left Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The run animation that plays when your AI is running left when in combat.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorRangedCombatRunLeftProp, "Mirror Combat Run Left");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.RangedCombatRunLeftAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.RangedCombatRunLeftAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Run Left' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Combat Run Right
                CustomAnimationField(new Rect(), new GUIContent(), RangedCombatRunRightProp, "Combat Run Right Animation", typeof(AnimationClip), false);
                CustomHelpLabelField("The run animation that plays when your AI is running right when in combat.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorRangedCombatRunRightProp, "Mirror Combat Run Right");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.RangedCombatRunRightAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.RangedCombatRunRightAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Run Right' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();

            CombatTurnFoldout.boolValue = CustomEditorProperties.Foldout(CombatTurnFoldout.boolValue, "Combat Turn Animations", true, myFoldoutStyle);

            if (CombatTurnFoldout.boolValue)
            {
                EditorGUILayout.BeginVertical("Box");
                //Combat Turn Left
                CustomAnimationField(new Rect(), new GUIContent(), RangedCombatTurnLeftProp, "Combat Turn Left", typeof(AnimationClip), false);
                CustomHelpLabelField("The turn animation that plays when your AI is turning left when in combat.", false);
                CustomFloatAnimationField(new Rect(), new GUIContent(), RangedCombatTurnLeftAnimationSpeedProp, "Turn Left Animation Speed", 0.1f, 2);
                CustomHelpLabelField("The speed in which the combat turn left animation will play.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorRangedCombatTurnLeftProp, "Mirror Combat Turn Left");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.RangedCombatTurnLeftAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.RangedCombatTurnLeftAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Turn Left' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //Combat Turn Right
                CustomAnimationField(new Rect(), new GUIContent(), RangedCombatTurnRightProp, "Combat Turn Right", typeof(AnimationClip), false);
                CustomHelpLabelField("The turn animation that plays when your AI is turning right when in combat.", false);
                CustomFloatAnimationField(new Rect(), new GUIContent(), RangedCombatTurnRightAnimationSpeedProp, "Turn Right Animation Speed", 0.1f, 2);
                CustomHelpLabelField("The speed in which the combat turn left animation will play.", false);
                CustomBoolAnimationField(new Rect(), new GUIContent(), MirrorRangedCombatTurnRightProp, "Mirror Combat Turn Right");
                CustomHelpLabelField("Mirroring an animation allows you to play an animation in the opposite direction.", false);

                if (self.RangedCombatTurnRightAnimation != null)
                {
                    var settings = AnimationUtility.GetAnimationClipSettings(self.RangedCombatTurnRightAnimation);
                    if (!settings.loopTime)
                    {
                        GUI.backgroundColor = new Color(10f, 0.0f, 0.0f, 0.25f);
                        EditorGUILayout.LabelField("The 'Combat Turn Right' animation must be set to loop. To do so, go to your animation settings and set 'Loop Time' to true.", EditorStyles.helpBox);
                        GUI.backgroundColor = Color.white;
                    }
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }
        }

        void DefaultCombatAnimations()
        {
            EmeraldAISystem self = (EmeraldAISystem)target;
            GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            myFoldoutStyle.fontStyle = FontStyle.Bold;
            myFoldoutStyle.fontSize = 12;
            myFoldoutStyle.active.textColor = Color.black;
            myFoldoutStyle.focused.textColor = Color.black;
            myFoldoutStyle.onHover.textColor = Color.black;
            myFoldoutStyle.normal.textColor = Color.black;
            myFoldoutStyle.onNormal.textColor = Color.black;
            myFoldoutStyle.onActive.textColor = Color.black;
            myFoldoutStyle.onFocused.textColor = Color.black;
            Color myStyleColor = Color.black;

            var HelpButtonStyle = new GUIStyle(GUI.skin.button);
            HelpButtonStyle.normal.textColor = Color.white;
            HelpButtonStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.PropertyField(UseEquipAnimationProp, new GUIContent("Use Equip Animations"));
            CustomHelpLabelField("Enables or disables the use of equip and unequip animations.", false);

            if (self.UseEquipAnimation == EmeraldAISystem.YesOrNo.Yes)
            {
                GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                EditorGUILayout.LabelField("Note: This requires an AI to have an EnableWeapon and DisableWeapon Animation Events setup on both their equip and unequip animations. " +
                    "For a guide on how to do this, refer to the Emerald AI Documentation, if you haven't yet set them up and would like to use this feature.", EditorStyles.helpBox);
                GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
                if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
                {
                    Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Setting-up-an-AIs-Sheathable-Weapons");
                }
                GUI.backgroundColor = Color.white;

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField("Equip Animations", EditorStyles.boldLabel);

                //Pullout Weapon
                CustomAnimationField(new Rect(), new GUIContent(), PullOutWeaponAnimationProp, "Equip Weapon", typeof(AnimationClip), false);
                CustomHelpLabelField("The animation that plays when the AI is pulling out their weapon.", false);

                //Put Away Weapon
                CustomAnimationField(new Rect(), new GUIContent(), PutAwayWeaponAnimationProp, "Unequip Weapon", typeof(AnimationClip), false);
                CustomHelpLabelField("The animation that plays when the AI is putting away their weapon.", false);

                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Attack Animations", EditorStyles.boldLabel);
            CustomHelpLabelField("Controls the attack animations that an will use when the AI in combat. A max of 6 can be used. Attack animations should have 'Loop Time' unchecked.", false);
            GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
            EditorGUILayout.LabelField("Note: You will need to manually create an Animation Event on your AI's attack animations to allow your AI to cause Damage. " +
                "Please refer to Emerlad's documentation for a tutorial on how to do this.", EditorStyles.helpBox);
            GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
            if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
            {
                if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Creating-Attack-Animation-Events");
                }
                else
                {
                    Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Creating-Attack-Animation-Events");
                }
            }

            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();
            AttackAnimationList.DoLayoutList();
            EditorGUILayout.Space();

            //Attack
            if (self.AttackAnimationList.Count == 6)
            {
                AttackAnimationList.displayAdd = false;
            }
            else
            {
                AttackAnimationList.displayAdd = true;
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(UseRunAttacksProp, new GUIContent("Use Run Attacks"));
            CustomHelpLabelField("Controls whether or not this AI will use run attacks.", true);

            if (self.UseRunAttacksRef == EmeraldAISystem.UseRunAttacks.Yes)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Run Attack Animations", EditorStyles.boldLabel);
                CustomHelpLabelField("Controls the run attack animations that will randomly play when the AI in combat and moving. A max of 3 can be used. Runt Attack animations should have 'Loop Time' unchecked.", false);
                RunAttackAnimationList.DoLayoutList();

                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            //Run Attack
            if (self.RunAttackAnimationList.Count == 3)
            {
                RunAttackAnimationList.displayAdd = false;
            }
            else
            {
                RunAttackAnimationList.displayAdd = true;
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(UseHitAnimationsProp, new GUIContent("Use Hit Animation"));
            CustomHelpLabelField("Controls whether or not this AI will use hit animations for both combat and non-combat.", true);

            if (self.UseHitAnimations == EmeraldAISystem.YesOrNo.Yes)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Blocking Animations", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(UseBlockingProp, new GUIContent("Use Blocking"));
                CustomHelpLabelField("Controls whether or not this AI will have the ability to block. AI who use block must have a Block and Block Hit animation.", true);

                if (self.UseBlockingRef == EmeraldAISystem.YesOrNo.Yes)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    EditorGUILayout.BeginVertical();

                    CustomAnimationField(new Rect(), new GUIContent(), BlockIdleAnimationProp, "Block Animation", typeof(AnimationClip), false);
                    CustomHelpLabelField("The animation that plays when your AI is blocking.", false);

                    CustomAnimationField(new Rect(), new GUIContent(), BlockHitAnimationProp, "Block Impact Animation", typeof(AnimationClip), false);
                    CustomHelpLabelField("The animation that plays when your AI is blocking and is hit with an attack.", false);

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Combat Hit Animations", EditorStyles.boldLabel);
                CustomHelpLabelField("Controls the animations that will randomly play when an AI receives damage when in combat.", false);
                CombatHitAnimationList.DoLayoutList();

                //Combat Hit
                if (self.CombatHitAnimationList.Count == 6)
                {
                    CombatHitAnimationList.displayAdd = false;
                }
                else
                {
                    CombatHitAnimationList.displayAdd = true;
                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Hit Animations", EditorStyles.boldLabel);
                CustomHelpLabelField("Controls the animations that will randomly play when an AI receives damage when not in combat.", false);
                HitAnimationList.DoLayoutList();
                EditorGUILayout.Space();

                //Hit
                if (self.HitAnimationList.Count == 6)
                {
                    HitAnimationList.displayAdd = false;
                }
                else
                {
                    HitAnimationList.displayAdd = true;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Death Animations", EditorStyles.boldLabel);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the animations that will randomly play when an AI receives damage. The speed of each animation can be " +
                "adjusted by changing the speed parameter.", EditorStyles.helpBox);
            GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
            EditorGUILayout.LabelField("Note: Death animations will only work when using the Animation Death Type. This setting is located under AI's " +
                "Settings>Combat>Combat Actions & Effect Settings.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            DeathAnimationList.DoLayoutList();
            EditorGUILayout.Space();

            //Death
            if (self.DeathAnimationList.Count == 6)
            {
                DeathAnimationList.displayAdd = false;
            }
            else
            {
                DeathAnimationList.displayAdd = true;
            }
        }

        void RangedCombatAnimations()
        {
            EmeraldAISystem self = (EmeraldAISystem)target;
            GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            myFoldoutStyle.fontStyle = FontStyle.Bold;
            myFoldoutStyle.fontSize = 12;
            myFoldoutStyle.active.textColor = Color.black;
            myFoldoutStyle.focused.textColor = Color.black;
            myFoldoutStyle.onHover.textColor = Color.black;
            myFoldoutStyle.normal.textColor = Color.black;
            myFoldoutStyle.onNormal.textColor = Color.black;
            myFoldoutStyle.onActive.textColor = Color.black;
            myFoldoutStyle.onFocused.textColor = Color.black;
            Color myStyleColor = Color.black;

            var HelpButtonStyle = new GUIStyle(GUI.skin.button);
            HelpButtonStyle.normal.textColor = Color.white;
            HelpButtonStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.PropertyField(UseEquipAnimationProp, new GUIContent("Use Equip Animations"));
            CustomHelpLabelField("Enables or disables the use of equip and unequip animations.", false);

            if (self.UseEquipAnimation == EmeraldAISystem.YesOrNo.Yes)
            {
                GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
                EditorGUILayout.LabelField("Note: This requires an AI to have an EnableWeapon and DisableWeapon Animation Events setup on both their equip and unequip animations. " +
                    "For a guide on how to do this, refer to the Emerald AI Documentation, if you haven't yet set them up and would like to use this feature.", EditorStyles.helpBox);
                GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
                if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
                {
                    Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Setting-up-an-AIs-Sheathable-Weapons");
                }
                GUI.backgroundColor = Color.white;

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField("Ranged Equip Animations", EditorStyles.boldLabel);

                //Pullout Weapon
                CustomAnimationField(new Rect(), new GUIContent(), RangedPullOutWeaponAnimationProp, "Equip Weapon", typeof(AnimationClip), false);
                CustomHelpLabelField("The animation that plays when the AI is pulling out their weapon.", false);

                //Put Away Weapon
                CustomAnimationField(new Rect(), new GUIContent(), RangedPutAwayWeaponAnimationProp, "Unequip Weapon", typeof(AnimationClip), false);
                CustomHelpLabelField("The animation that plays when the AI is putting away their weapon.", false);

                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Ranged Attack Animations", EditorStyles.boldLabel);
            CustomHelpLabelField("Controls the attack animations that an AI will use for its abilities. A max of 6 can be used. Ranged Attack animations should have 'Loop Time' unchecked.", false);
            GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
            EditorGUILayout.LabelField("Note: You will need to manually create an Animation Event on your AI's attack animations to allow your AI to cause Damage. " +
                "Please refer to Emerlad's documentation for a tutorial on how to do this.", EditorStyles.helpBox);
            GUI.backgroundColor = new Color(0, 0.65f, 0, 0.8f);
            if (GUILayout.Button("See Tutorial", HelpButtonStyle, GUILayout.Height(20)))
            {
                if (self.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Creating-Attack-Animation-Events");
                }
                else
                {
                    Application.OpenURL("https://github.com/Black-Horizon-Studios/Emerald-AI/wiki/Creating-Attack-Animation-Events");
                }
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();
            RangedAttackAnimationList.DoLayoutList();
            EditorGUILayout.Space();

            //Attack
            if (self.RangedAttackAnimationList.Count == 6)
            {
                RangedAttackAnimationList.displayAdd = false;
            }
            else
            {
                RangedAttackAnimationList.displayAdd = true;
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(UseHitAnimationsProp, new GUIContent("Use Hit Animation"));
            CustomHelpLabelField("Controls whether or not this AI will use hit animations for when in combat.", true);

            if (self.UseHitAnimations == EmeraldAISystem.YesOrNo.Yes)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField("Ranged Combat Hit Animations", EditorStyles.boldLabel);
                CustomHelpLabelField("Controls the animations that will randomly play when an AI receives damage when in combat.", false);
                RangedCombatHitAnimationList.DoLayoutList();

                //Combat Hit
                if (self.RangedCombatHitAnimationList.Count == 6)
                {
                    RangedCombatHitAnimationList.displayAdd = false;
                }
                else
                {
                    RangedCombatHitAnimationList.displayAdd = true;
                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Hit Animations", EditorStyles.boldLabel);
                CustomHelpLabelField("Controls the animations that will randomly play when an AI receives damage when not in combat.", false);
                HitAnimationList.DoLayoutList();
                EditorGUILayout.Space();

                //Hit
                if (self.HitAnimationList.Count == 6)
                {
                    HitAnimationList.displayAdd = false;
                }
                else
                {
                    HitAnimationList.displayAdd = true;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Ranged Death Animations", EditorStyles.boldLabel);
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField("Controls the death animations that will randomly play when an AI dies. The speed of each animation can be " +
                "adjusted by changing the speed parameter.", EditorStyles.helpBox);
            GUI.backgroundColor = new Color(1f, 1, 0.25f, 0.25f);
            EditorGUILayout.LabelField("Note: Death animations will only work when using the Animation Death Type. This setting is located under AI's " +
                "Settings>Combat>Combat Actions & Effect Settings.", EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            RangedDeathAnimationList.DoLayoutList();
            EditorGUILayout.Space();

            //Death
            if (self.RangedDeathAnimationList.Count == 6)
            {
                RangedDeathAnimationList.displayAdd = false;
            }
            else
            {
                RangedDeathAnimationList.displayAdd = true;
            }

            EditorGUILayout.Space();
        }

        #region Local Properties

        void CustomPopup(Rect position, GUIContent label, SerializedProperty property, string nameOfLabel, string[] names)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            string[] enumNamesList = names;
            var newValue = EditorGUI.Popup(position, property.intValue, enumNamesList);

            if (EditorGUI.EndChangeCheck())
                property.intValue = newValue;

            EditorGUI.EndProperty();
        }

        void CustomTag(Rect position, GUIContent label, SerializedProperty property)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.TagField(position, property.stringValue);

            if (EditorGUI.EndChangeCheck())
                property.stringValue = newValue;

            EditorGUI.EndProperty();
        }

        void CustomFloatAnimationField(Rect position, GUIContent label, SerializedProperty property, string Name, float Min, float Max)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUILayout.Slider(Name, property.floatValue, Min, Max);

            if (newValue != property.floatValue)
            {
                AnimationsUpdatedProp.boolValue = true;
            }

            if (EditorGUI.EndChangeCheck())
                property.floatValue = newValue;

            EditorGUI.EndProperty();
        }

        void CustomBoolAnimationField(Rect position, GUIContent label, SerializedProperty property, string Name)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUILayout.Toggle(Name, property.boolValue);

            if (newValue != property.boolValue)
            {
                AnimationsUpdatedProp.boolValue = true;
            }

            if (EditorGUI.EndChangeCheck())
                property.boolValue = newValue;

            EditorGUI.EndProperty();
        }

        void CustomAnimationField(Rect position, GUIContent label, SerializedProperty property, string Name, Type typeOfObject, bool IsEssential)
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

            if (newValue != property.objectReferenceValue)
            {
                AnimationsUpdatedProp.boolValue = true;
            }

            if (EditorGUI.EndChangeCheck())
            {
                property.objectReferenceValue = newValue;
            }


            EditorGUI.EndProperty();
        }

        void CustomHelpLabelField(string TextInfo, bool UseSpace)
        {
            GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.19f);
            EditorGUILayout.LabelField(TextInfo, EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            if (UseSpace)
            {
                EditorGUILayout.Space();
            }
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
        #endregion
    }
}