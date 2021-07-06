using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI
{
    [CreateAssetMenu(fileName = "New Ability Object", menuName = "Emerald AI/Create/Ability Object")]
    [System.Serializable]
    public class EmeraldAIAbility : ScriptableObject
    {
        public string AbilityName = "New Ability";
        public string AbilityDescription = "Ability Description";
        public int AbilityDamage = 5;
        public int AbilityDamagePerIncrement = 5;
        public float CriticalHitOdds = 6.25f;
        public float CriticalHitMultiplierMin = 1.1f;
        public float CriticalHitMultiplierMax = 1.3f;
        public int AbilityImpactDamage = 5;
        public int AbilitySupportAmount = 5;
        public float AbilityDamageIncrement = 1;
        public int AbilityCooldown = 10;
        public int AbilityLength = 8;
        public int SummonRadius = 10;
        public int ProjectileSpeed = 30;
        public int ProjectileTimeoutSeconds = 6;
        public float ColliderRadius = 0.05f;

        public float TimeoutTime = 4.5f;
        public float CollisionTime = 0;
        public float HeatSeekingSeconds = 1;

        public float CastEffectTimeoutSeconds = 1;
        public float AbilityEffectTimeoutSeconds = 5;
        public float SummonEffectTimeoutSeconds = 3;
        public float CollisionTimeout = 3;
        public float DamageOvertimeTimeout = 3;

        public Texture2D AbilityIcon;
        public GameObject AbilityEffect;
        public GameObject CastEffect;
        public GameObject SummonEffect;
        public GameObject SummonObject;
        public GameObject DamageOverTimeEffect;
        public AudioClip DamageOverTimeSound;
        public GameObject CollisionEffect;
        public AudioClip ImpactSound;
        public AudioClip SummonSound;
        public AudioClip CastSound;

        public Yes_No UseRandomizedTrajectory = Yes_No.No;
        public Yes_No UseGravity = Yes_No.No;
        public float TrajectoryXOffsetMin = 0;
        public float TrajectoryXOffsetMax = 0;
        public float TrajectoryYOffsetMin = 0;
        public float TrajectoryYOffsetMax = 0;
        public float TrajectoryZOffsetMin = 0;
        public float TrajectoryZOffsetMax = 0;

        public EffectOnCollision EffectOnCollisionRef = EffectOnCollision.No;
        public EffectOnCollision SoundOnCollisionRef = EffectOnCollision.No;
        public enum EffectOnCollision
        {
            No = 0,
            Yes = 1
        }

        public HeatSeeking HeatSeekingRef = HeatSeeking.No;
        public enum HeatSeeking
        {
            No = 0,
            Yes = 1
        }

        public AbilityTypeEnum AbilityType;
        public enum AbilityTypeEnum
        {
            Damage = 0,
            Support = 1,
            Summon = 2
        }

        public DamageTypeEnum DamageType;
        public enum DamageTypeEnum
        {
            Instant = 0,
            OverTime = 1
        }

        public SupportTypeEnum SupportType;
        public enum SupportTypeEnum
        {
            Instant = 0,
            OverTime = 1
        }

        public enum Yes_No { No = 0, Yes = 1 };
        public Yes_No AbilityStacksRef = Yes_No.No;
        public Yes_No UseDamageOverTimeEffectRef = Yes_No.No;
        public Yes_No UseDamageOverTimeSoundRef = Yes_No.No;
        public Yes_No UseCastSound = Yes_No.No;
        public Yes_No UseCriticalHits = Yes_No.No;
        public Yes_No UseCustomAbilityIcon = Yes_No.No;
        public Yes_No UseCastEffect = Yes_No.No;

        public enum ArrowObject { No = 0, Yes = 1 };
        public ArrowObject ArrowProjectileRef = ArrowObject.No;

        //Editor variables
        public int AbilityEditorTabs = 0;
    }
}