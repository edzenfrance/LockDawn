using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Utility
{
    [CreateAssetMenu(fileName = "New Animation Profile", menuName = "Emerald AI/Create/Animation Profile")]
    public class AnimationProfile : ScriptableObject
    {
        public RuntimeAnimatorController AIAnimator;
        public AnimationClip Attack1Animation, Attack2Animation, Attack3Animation, Attack4Animation, Attack5Animation, Attack6Animation, RunAttack1Animation, RunAttack2Animation, RunAttack3Animation;
        public AnimationClip Idle1Animation, Idle2Animation, Idle3Animation, Idle4Animation, Idle5Animation, Idle6Animation, IdleWarningAnimation;
        public AnimationClip NonCombatTurnLeftAnimation, NonCombatTurnRightAnimation, CombatTurnLeftAnimation, CombatTurnRightAnimation;
        public AnimationClip NonCombatIdleAnimation, WalkLeftAnimation, WalkStraightAnimation, WalkRightAnimation, RunLeftAnimation, RunStraightAnimation, RunRightAnimation;
        public AnimationClip CombatIdleAnimation, CombatWalkLeftAnimation, CombatWalkStraightAnimation, CombatWalkBackAnimation, CombatWalkRightAnimation, CombatRunLeftAnimation, CombatRunStraightAnimation, CombatRunRightAnimation;
        public AnimationClip Hit1Animation, Hit2Animation, Hit3Animation, Hit4Animation, Hit5Animation, Hit6Animation, CombatHit1Animation, CombatHit2Animation, CombatHit3Animation, CombatHit4Animation, 
            CombatHit5Animation, CombatHit6Animation, BlockIdleAnimation, BlockHitAnimation, PutAwayWeaponAnimation, PullOutWeaponAnimation;
        public AnimationClip Death1Animation, Death2Animation, Death3Animation, Death4Animation, Death5Animation, Death6Animation, 
            RangedDeath1Animation, RangedDeath2Animation, RangedDeath3Animation, RangedDeath4Animation, RangedDeath5Animation, RangedDeath6Animation;

        public AnimationClip RangedCombatIdleAnimation, RangedCombatWalkLeftAnimation, RangedCombatWalkStraightAnimation, RangedCombatWalkBackAnimation, RangedCombatWalkRightAnimation,
            RangedCombatRunLeftAnimation, RangedCombatRunStraightAnimation, RangedCombatRunRightAnimation, RangedIdleWarningAnimation, RangedPutAwayWeaponAnimation, RangedPullOutWeaponAnimation;
        public AnimationClip RangedAttack1Animation, RangedAttack2Animation, RangedAttack3Animation, RangedAttack4Animation, RangedAttack5Animation, RangedAttack6Animation, RangedRunAttack1Animation, RangedRunAttack2Animation, RangedRunAttack3Animation;
        public AnimationClip RangedCombatHit1Animation, RangedCombatHit2Animation, RangedCombatHit3Animation, RangedCombatHit4Animation, RangedCombatHit5Animation, RangedCombatHit6Animation, RangedCombatTurnLeftAnimation, RangedCombatTurnRightAnimation;

        public AnimationClip Emote1Animation, Emote2Animation, Emote3Animation, Emote4Animation, Emote5Animation, Emote6Animation, Emote7Animation, Emote8Animation, Emote9Animation, Emote10Animation;

        public float Idle1AnimationSpeed = 1;
        public float Idle2AnimationSpeed = 1;
        public float Idle3AnimationSpeed = 1;
        public float Idle4AnimationSpeed = 1;
        public float Idle5AnimationSpeed = 1;
        public float Idle6AnimationSpeed = 1;
        public float IdleWarningAnimationSpeed = 1;
        public float RangedIdleWarningAnimationSpeed = 1;
        public float IdleCombatAnimationSpeed = 1;
        public float RangedIdleCombatAnimationSpeed = 1;
        public float IdleNonCombatAnimationSpeed = 1;
        public float Attack1AnimationSpeed = 1;
        public float Attack2AnimationSpeed = 1;
        public float Attack3AnimationSpeed = 1;
        public float Attack4AnimationSpeed = 1;
        public float Attack5AnimationSpeed = 1;
        public float Attack6AnimationSpeed = 1;
        public float RangedAttack1AnimationSpeed = 1;
        public float RangedAttack2AnimationSpeed = 1;
        public float RangedAttack3AnimationSpeed = 1;
        public float RangedAttack4AnimationSpeed = 1;
        public float RangedAttack5AnimationSpeed = 1;
        public float RangedAttack6AnimationSpeed = 1;
        public float RunAttack1AnimationSpeed = 1;
        public float RunAttack2AnimationSpeed = 1;
        public float RunAttack3AnimationSpeed = 1;
        public float RangedRunAttack1AnimationSpeed = 1;
        public float RangedRunAttack2AnimationSpeed = 1;
        public float RangedRunAttack3AnimationSpeed = 1;
        public float TurnLeftAnimationSpeed = 1;
        public float TurnRightAnimationSpeed = 1;
        public float CombatTurnLeftAnimationSpeed = 1;
        public float CombatTurnRightAnimationSpeed = 1;
        public float RangedCombatTurnLeftAnimationSpeed = 1;
        public float RangedCombatTurnRightAnimationSpeed = 1;
        public float Death1AnimationSpeed = 1;
        public float Death2AnimationSpeed = 1;
        public float Death3AnimationSpeed = 1;
        public float Death4AnimationSpeed = 1;
        public float Death5AnimationSpeed = 1;
        public float Death6AnimationSpeed = 1;
        public float RangedDeath1AnimationSpeed = 1;
        public float RangedDeath2AnimationSpeed = 1;
        public float RangedDeath3AnimationSpeed = 1;
        public float RangedDeath4AnimationSpeed = 1;
        public float RangedDeath5AnimationSpeed = 1;
        public float RangedDeath6AnimationSpeed = 1;
        public float Emote1AnimationSpeed = 1;
        public float Emote2AnimationSpeed = 1;
        public float Emote3AnimationSpeed = 1;
        public float Emote4AnimationSpeed = 1;
        public float Emote5AnimationSpeed = 1;
        public float Emote6AnimationSpeed = 1;
        public float Emote7AnimationSpeed = 1;
        public float Emote8AnimationSpeed = 1;
        public float Emote9AnimationSpeed = 1;
        public float Emote10AnimationSpeed = 1;
        public float WalkAnimationSpeed = 1;
        public float RunAnimationSpeed = 1;
        public float NonCombatWalkAnimationSpeed = 1;
        public float NonCombatRunAnimationSpeed = 1;
        public float CombatWalkAnimationSpeed = 1;
        public float CombatRunAnimationSpeed = 1;
        public float RangedCombatWalkAnimationSpeed = 1;
        public float RangedCombatRunAnimationSpeed = 1;
        public float Hit1AnimationSpeed = 1;
        public float Hit2AnimationSpeed = 1;
        public float Hit3AnimationSpeed = 1;
        public float Hit4AnimationSpeed = 1;
        public float Hit5AnimationSpeed = 1;
        public float Hit6AnimationSpeed = 1;
        public float CombatHit1AnimationSpeed = 1;
        public float CombatHit2AnimationSpeed = 1;
        public float CombatHit3AnimationSpeed = 1;
        public float CombatHit4AnimationSpeed = 1;
        public float CombatHit5AnimationSpeed = 1;
        public float CombatHit6AnimationSpeed = 1;
        public float RangedCombatHit1AnimationSpeed = 1;
        public float RangedCombatHit2AnimationSpeed = 1;
        public float RangedCombatHit3AnimationSpeed = 1;
        public float RangedCombatHit4AnimationSpeed = 1;
        public float RangedCombatHit5AnimationSpeed = 1;
        public float RangedCombatHit6AnimationSpeed = 1;

        public bool MirrorWalkLeft = false;
        public bool MirrorWalkRight = false;
        public bool MirrorRunLeft = false;
        public bool MirrorRunRight = false;
        public bool MirrorCombatWalkLeft = false;
        public bool MirrorCombatWalkRight = false;
        public bool MirrorCombatRunLeft = false;
        public bool MirrorCombatRunRight = false;
        public bool MirrorCombatTurnLeft = false;
        public bool MirrorCombatTurnRight = false;
        public bool MirrorRangedCombatWalkLeft = false;
        public bool MirrorRangedCombatWalkRight = false;
        public bool MirrorRangedCombatRunLeft = false;
        public bool MirrorRangedCombatRunRight = false;
        public bool MirrorRangedCombatTurnLeft = false;
        public bool MirrorRangedCombatTurnRight = false;
        public bool MirrorTurnLeft = false;
        public bool MirrorTurnRight = false;
        public bool ReverseWalkAnimation = false;
        public bool ReverseRangedWalkAnimation = false;

        public enum YesOrNo { No = 0, Yes = 1 };
        public YesOrNo UseWarningAnimationRef = YesOrNo.No;
        public YesOrNo UseRunAttacksRef = YesOrNo.No;
        public YesOrNo UseHitAnimations = YesOrNo.Yes;
        public YesOrNo UseEquipAnimation = YesOrNo.No;
        public YesOrNo UseBlockingRef = YesOrNo.No;
        public enum WeaponType { Melee = 0, Ranged = 1, Both = 2 };
        public WeaponType WeaponTypeRef = WeaponType.Melee;
        public AnimatorTypeState AnimatorType = AnimatorTypeState.NavMeshDriven;
        public enum AnimatorTypeState { RootMotion, NavMeshDriven }

        [System.Serializable]
        public class EmoteAnimationClass
        {
            public EmoteAnimationClass(int NewAnimationID, AnimationClip NewEmoteAnimationClip)
            {
                AnimationID = NewAnimationID;
                EmoteAnimationClip = NewEmoteAnimationClip;
            }

            public int AnimationID = 1;
            public AnimationClip EmoteAnimationClip;
        }
        public List<EmoteAnimationClass> EmoteAnimationList = new List<EmoteAnimationClass>();

        [System.Serializable]
        public class AnimationClass
        {
            public AnimationClass(float NewAnimationSpeed, AnimationClip NewAnimationClip)
            {
                AnimationSpeed = NewAnimationSpeed;
                AnimationClip = NewAnimationClip;
            }

            public float AnimationSpeed = 1;
            public AnimationClip AnimationClip;
        }
        public List<AnimationClass> IdleAnimationList = new List<AnimationClass>();
        public List<AnimationClass> AttackAnimationList = new List<AnimationClass>();
        public List<AnimationClass> RunAttackAnimationList = new List<AnimationClass>();
        public List<AnimationClass> DeathAnimationList = new List<AnimationClass>();      
        public List<AnimationClass> CombatHitAnimationList = new List<AnimationClass>();
        public List<AnimationClass> HitAnimationList = new List<AnimationClass>();

        public List<AnimationClass> RangedAttackAnimationList = new List<AnimationClass>();
        public List<AnimationClass> RangedCombatHitAnimationList = new List<AnimationClass>();
        public List<AnimationClass> RangedRunAttackAnimationList = new List<AnimationClass>();
        public List<AnimationClass> RangedDeathAnimationList = new List<AnimationClass>();
    }
}