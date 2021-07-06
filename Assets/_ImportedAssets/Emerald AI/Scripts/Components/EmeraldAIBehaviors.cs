using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI
{
    /// <summary>
    /// This script handles all of Emerald AI's behaviors and states.
    /// </summary>
    public class EmeraldAIBehaviors : MonoBehaviour
    {
        [HideInInspector]
        public EmeraldAISystem EmeraldComponent;
        float BlockTimer;
        bool BackupDelayActive;
        float BackupDistance;
        bool SearchDelayActive = false;
        int StartingDetectionRadius;

        void Start()
        {
            EmeraldComponent = GetComponent<EmeraldAISystem>();
            StartingDetectionRadius = EmeraldComponent.DetectionRadius;
        }

        /// <summary>
        /// Handles the Aggressive Behavior Type
        /// </summary>
        public void AggressiveBehavior()
        {
            if (EmeraldComponent.CurrentTarget)
            {               
                Vector3 CurrentTargetPos = EmeraldComponent.CurrentTarget.position;
                CurrentTargetPos.y = 0;
                Vector3 CurrentPos = transform.position;
                CurrentPos.y = 0;
                EmeraldComponent.DistanceFromTarget = Vector3.Distance(CurrentTargetPos, CurrentPos);
            }

            EmeraldComponent.ObstructionDetectionUpdateTimer += Time.deltaTime;
            if (!EmeraldComponent.IsTurning && EmeraldComponent.ObstructionDetectionUpdateTimer >= EmeraldComponent.ObstructionDetectionUpdateSeconds)
            {                  
                EmeraldComponent.EmeraldDetectionComponent.CheckForObstructions();
                EmeraldComponent.ObstructionDetectionUpdateTimer = 0;
            }

            if (!EmeraldComponent.BackingUp && EmeraldComponent.AIAgentActive && !EmeraldComponent.Attacking && EmeraldComponent.CurrentTarget)
            {
                AttackState();

                //If our target exceeds the max chase distance, clear the target and resume wander type by returning to the default state.
                if (EmeraldComponent.MaxChaseDistanceTypeRef == EmeraldAISystem.MaxChaseDistanceType.TargetDistance && EmeraldComponent.DistanceFromTarget > EmeraldComponent.MaxChaseDistance)
                {
                    DefaultState();
                }
                else if (EmeraldComponent.MaxChaseDistanceTypeRef == EmeraldAISystem.MaxChaseDistanceType.StartingDistance && Vector3.Distance(EmeraldComponent.StartingDestination, transform.position) > EmeraldComponent.MaxChaseDistance)
                {
                    EmeraldComponent.ReturningToStartInProgress = true;
                    DefaultState();
                }

                //If using blocking, attempt to trigger the blocking state
                if (EmeraldComponent.UseBlockingRef == EmeraldAISystem.YesOrNo.Yes)
                {
                    BlockState();
                }

                //Monitor the distance away from the current target, 
                //if the backup range is met, trigger the backup state.
                if (EmeraldComponent.BackupTypeRef != EmeraldAISystem.BackupType.Off)
                {
                    if (!EmeraldComponent.Attacking && !BackupDelayActive)
                    {
                        CalculateBackupState();
                    }                
                }
            }

            //Backs AI up when true
            if (EmeraldComponent.BackingUp)
            {
                BackupState();                
            }

            //If our AI target dies, search for another target
            if (EmeraldComponent.TargetTypeRef == EmeraldAISystem.TargetType.AI && !SearchDelayActive)
            {
                if (EmeraldComponent.TargetEmerald != null)
                {
                    if (EmeraldComponent.TargetEmerald.CurrentHealth <= 0 && !EmeraldComponent.DeathDelayActive)
                    {
                        EmeraldComponent.OnKillTargetEvent.Invoke();

                        EmeraldComponent.EmeraldDetectionComponent.m_LookLerpValue = 0;
                        EmeraldComponent.DestinationAdjustedAngle = 100;
                        EmeraldComponent.AIAnimator.ResetTrigger("Attack");
                        
                        if (EmeraldComponent.TargetTypeRef == EmeraldAISystem.TargetType.AI)
                        {
                            EmeraldComponent.EmeraldDetectionComponent.PreviousTarget = EmeraldComponent.TargetEmerald.HitPointTransform;
                        }   
                        else
                        {
                            EmeraldComponent.EmeraldDetectionComponent.PreviousTarget = EmeraldComponent.CurrentTarget;
                        }

                        //Delay the SearchForTarget function by 0.75 seconds
                        SearchDelayActive = true;
                        Invoke("DelaySearch", 0.75f);
                    }
                }
            }
            else if (EmeraldComponent.TargetTypeRef == EmeraldAISystem.TargetType.Player) //If our player target dies, search for another target
            {
                if (EmeraldComponent.CurrentTarget == null)
                {
                    if (!EmeraldComponent.DeathDelayActive)
                    {
                        EmeraldComponent.OnKillTargetEvent.Invoke();

                        EmeraldComponent.EmeraldDetectionComponent.PreviousTarget = EmeraldComponent.CurrentTarget;
                        EmeraldComponent.EmeraldDetectionComponent.m_LookLerpValue = 0;
                        EmeraldComponent.DestinationAdjustedAngle = 100;
                        EmeraldComponent.AIAnimator.ResetTrigger("Attack");
                        EmeraldComponent.EmeraldDetectionComponent.SearchForTarget();
                    }
                }
            }

            //If the AI's target becomes null, return to the default state.
            if (EmeraldComponent.CurrentTarget == null && EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.Active && !EmeraldComponent.ReturningToStartInProgress)
            {
                EmeraldComponent.DeathDelayActive = true;
            }
        }

        /// <summary>
        /// Invoked to delay the SearchForTarget function which allows an AI not immediately attack another target after having just killed one.
        /// </summary>
        public void DelaySearch ()
        {
            SearchDelayActive = false;
            EmeraldComponent.EmeraldDetectionComponent.SearchForTarget();
        }

        /// <summary>
        /// Handles the Companion Behavior Type
        /// </summary>
        public void CompanionBehavior()
        {
            if (EmeraldComponent.CompanionTargetRef != null && EmeraldComponent.CurrentTarget == null)
            {
                if (EmeraldComponent.TargetTypeRef == EmeraldAISystem.TargetType.AI)
                {
                    if (EmeraldComponent.TargetEmerald != null && EmeraldComponent.TargetEmerald.BehaviorRef == EmeraldAISystem.CurrentBehavior.Aggressive ||
                        EmeraldComponent.TargetEmerald != null && EmeraldComponent.TargetEmerald.BehaviorRef == EmeraldAISystem.CurrentBehavior.Cautious)
                    {
                        if (EmeraldComponent.CombatTypeRef == EmeraldAISystem.CombatType.Defensive)
                        {
                            if (EmeraldComponent.TargetEmerald.CombatStateRef == EmeraldAISystem.CombatState.Active)
                            {
                                EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.AttackDistance;
                                EmeraldComponent.EmeraldDetectionComponent.PreviousTarget = EmeraldComponent.CompanionTargetRef;
                                EmeraldComponent.CurrentTarget = EmeraldComponent.CompanionTargetRef;
                                EmeraldComponent.EmeraldBehaviorsComponent.ActivateCombatState();
                            }
                        }
                        else if (EmeraldComponent.CombatTypeRef == EmeraldAISystem.CombatType.Offensive)
                        {
                            EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.AttackDistance;
                            EmeraldComponent.EmeraldDetectionComponent.PreviousTarget = EmeraldComponent.CompanionTargetRef;
                            EmeraldComponent.CurrentTarget = EmeraldComponent.CompanionTargetRef;
                            EmeraldComponent.EmeraldBehaviorsComponent.ActivateCombatState();
                        }
                    }
                }
                else
                {
                    EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.AttackDistance;
                    EmeraldComponent.EmeraldDetectionComponent.PreviousTarget = EmeraldComponent.CompanionTargetRef;
                    EmeraldComponent.CurrentTarget = EmeraldComponent.CompanionTargetRef;
                    EmeraldComponent.EmeraldBehaviorsComponent.ActivateCombatState();
                }
            }
            else if (EmeraldComponent.CurrentTarget != null)
            {
                EmeraldComponent.ObstructionDetectionUpdateTimer += Time.deltaTime;
                if (!EmeraldComponent.IsTurning && EmeraldComponent.ObstructionDetectionUpdateTimer >= EmeraldComponent.ObstructionDetectionUpdateSeconds)
                {
                    EmeraldComponent.EmeraldDetectionComponent.CheckForObstructions();
                    EmeraldComponent.ObstructionDetectionUpdateTimer = 0;
                }

                if (!EmeraldComponent.BackingUp && EmeraldComponent.AIAgentActive && !EmeraldComponent.Attacking && EmeraldComponent.CurrentTarget)
                {
                    Vector3 CurrentTargetPos = EmeraldComponent.CurrentTarget.position;
                    CurrentTargetPos.y = 0;
                    Vector3 CurrentPos = transform.position;
                    CurrentPos.y = 0;
                    EmeraldComponent.DistanceFromTarget = Vector3.Distance(CurrentTargetPos, CurrentPos);

                    AttackState();

                    //If our target exceeds the max chase distance, clear the target and resume wander type by returning to the default state.
                    if (EmeraldComponent.DistanceFromTarget > EmeraldComponent.MaxChaseDistance)
                    {
                        DefaultState();
                    }

                    //If using blocking, attempt to trigger the blocking state
                    if (EmeraldComponent.UseBlockingRef == EmeraldAISystem.YesOrNo.Yes)
                    {
                        BlockState();
                    }

                    //Monitor the distance away from the current target, 
                    //if the backup range is met, trigger the backup state.
                    if (EmeraldComponent.BackupTypeRef != EmeraldAISystem.BackupType.Off)
                    {
                        if (!EmeraldComponent.Attacking && !BackupDelayActive)
                        {
                            CalculateBackupState();
                        }
                    }
                }

                //Backs AI up when true
                if (EmeraldComponent.BackingUp)
                {
                    BackupState();
                }

                //If our AI target dies, search for another target
                if (EmeraldComponent.TargetTypeRef == EmeraldAISystem.TargetType.AI)
                {
                    if (EmeraldComponent.TargetEmerald != null)
                    {
                        if (EmeraldComponent.TargetEmerald.CurrentHealth <= 0 && !EmeraldComponent.DeathDelayActive)
                        {
                            EmeraldComponent.OnKillTargetEvent.Invoke();

                            EmeraldComponent.DestinationAdjustedAngle = 100;
                            EmeraldComponent.AIAnimator.ResetTrigger("Attack");

                            if (EmeraldComponent.TargetTypeRef == EmeraldAISystem.TargetType.AI)
                            {
                                EmeraldComponent.EmeraldDetectionComponent.PreviousTarget = EmeraldComponent.TargetEmerald.HitPointTransform;
                            }
                            else
                            {
                                EmeraldComponent.EmeraldDetectionComponent.PreviousTarget = EmeraldComponent.CurrentTarget;
                            }

                            EmeraldComponent.EmeraldDetectionComponent.SearchForTarget();
                        }
                    }
                }
            }
            else if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.Active && EmeraldComponent.CompanionTargetRef == null && EmeraldComponent.CurrentTarget == null)
            {
                DefaultState();
            }
        }

        /// <summary>
        /// Handles the Coward Behavior Type
        /// </summary>
        public void CowardBehavior()
        {
            if (EmeraldComponent.CurrentTarget)
            {
                Vector3 CurrentTargetPos = EmeraldComponent.CurrentTarget.position;
                CurrentTargetPos.y = 0;
                Vector3 CurrentPos = transform.position;
                CurrentPos.y = 0;
                EmeraldComponent.DistanceFromTarget = Vector3.Distance(CurrentTargetPos, CurrentPos);

                FleeState();

                //If our target exceeds the max chase distance, clear the target and resume wander type by returning to the default state.
                if (EmeraldComponent.DistanceFromTarget > EmeraldComponent.MaxChaseDistance)
                {
                    if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Dynamic)
                    {
                        EmeraldComponent.StartingDestination = this.transform.position + transform.forward * 8;
                    }

                    DefaultState();
                }

                //If our AI target dies, search for another target
                if (EmeraldComponent.TargetTypeRef == EmeraldAISystem.TargetType.AI)
                {
                    if (EmeraldComponent.TargetEmerald != null)
                    {
                        if (EmeraldComponent.TargetEmerald.CurrentHealth <= 0)
                        {
                            EmeraldComponent.EmeraldDetectionComponent.PreviousTarget = EmeraldComponent.CurrentTarget;
                            EmeraldComponent.EmeraldDetectionComponent.SearchForTarget();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Coward Behavior Type
        /// </summary>
        public void CautiousBehavior()
        {
            if (EmeraldComponent.CurrentTarget)
            {
                Vector3 CurrentTargetPos = EmeraldComponent.CurrentTarget.position;
                CurrentTargetPos.y = 0;
                Vector3 CurrentPos = transform.position;
                CurrentPos.y = 0;
                EmeraldComponent.DistanceFromTarget = Vector3.Distance(CurrentTargetPos, CurrentPos);

                //If our target exceeds the Detection Radius distance, clear the target and resume wander type by returning to the default state.
                //Also, reset the CautiousTimer to 0.
                if (EmeraldComponent.DistanceFromTarget > EmeraldComponent.DetectionRadius)
                {
                    DefaultState();
                    EmeraldComponent.CautiousTimer = 0;
                }
            }

            EmeraldComponent.CautiousTimer += Time.deltaTime;

            if (EmeraldComponent.CautiousTimer >= EmeraldComponent.CautiousSeconds)
            {
                EmeraldComponent.BehaviorRef = EmeraldAISystem.CurrentBehavior.Aggressive;
                EmeraldComponent.CautiousTimer = 0;
            }

            if (EmeraldComponent.UseWarningAnimationRef == EmeraldAISystem.YesOrNo.Yes && !EmeraldComponent.WarningAnimationTriggered && EmeraldComponent.CautiousTimer > 2)
            {
                EmeraldComponent.AIAnimator.SetTrigger("Warning");
                EmeraldComponent.WarningAnimationTriggered = true;
            }
        }


        /// <summary>
        /// Controls what happens when the AI dies
        /// </summary>
        public void DeadState()
        {
            EmeraldComponent.IsDead = true;
            EmeraldComponent.DeathEvent.Invoke();

            //If our AI has a summoner, remove self from their TotalSummonedAI.
            if (EmeraldComponent.CurrentSummoner != null)
            {
                if (EmeraldComponent.CurrentSummoner.SummonsMultipleAI == EmeraldAISystem.EnableDisable.Enabled)
                {
                    EmeraldComponent.CurrentSummoner.TotalSummonedAI--;
                }
                EmeraldComponent.CurrentSummoner = null;
            }

            if (EmeraldComponent.m_NavMeshAgent.enabled)
            {
                EmeraldComponent.m_NavMeshAgent.ResetPath();
                EmeraldComponent.m_NavMeshAgent.isStopped = true;
                EmeraldComponent.m_NavMeshAgent.enabled = false;
            }

            EmeraldComponent.EmeraldInitializerComponent.InitializeAIDeath();          

            if (EmeraldComponent.DeathTypeRef == EmeraldAISystem.DeathType.Ragdoll)
            {
                EmeraldComponent.EmeraldInitializerComponent.EnableRagdoll();
            }
            else if (EmeraldComponent.DeathTypeRef == EmeraldAISystem.DeathType.Animation)
            {
                EmeraldComponent.EmeraldDetectionComponent.enabled = false;
                EmeraldComponent.EmeraldEventsManagerComponent.enabled = false;
                EmeraldComponent.AIBoxCollider.enabled = false;
                EmeraldComponent.AIAnimator.ResetTrigger("Hit");
                EmeraldComponent.AIAnimator.ResetTrigger("Attack");
                EmeraldComponent.AIAnimator.SetTrigger("Dead");
                EmeraldComponent.AIAnimator.SetInteger("Death Index", Random.Range(1, EmeraldComponent.TotalDeathAnimations + 1));
            }

            if (EmeraldComponent.CreateHealthBarsRef == EmeraldAISystem.CreateHealthBars.Yes || EmeraldComponent.DisplayAINameRef == EmeraldAISystem.DisplayAIName.Yes)
            {
                if (EmeraldComponent.HealthBarCanvas != null)
                {
                    EmeraldComponent.HealthBar.GetComponent<EmeraldAI.Utility.EmeraldAIHealthBar>().FadeOut();
                }
            }
        }

        /// <summary>
        /// Controls our AI's fleeing logic and functionality
        /// </summary>
        public void FleeState()
        {
            if (EmeraldComponent.m_NavMeshAgent.remainingDistance <= EmeraldComponent.StoppingDistance)
            {
                Vector3 direction = (EmeraldComponent.CurrentTarget.position - transform.position).normalized;
                Vector3 GeneratedDestination = transform.position + -direction * 30 + Random.insideUnitSphere * 10;
                GeneratedDestination.y = transform.position.y;
                EmeraldComponent.m_NavMeshAgent.destination = GeneratedDestination;
            }
        }

        /// <summary>
        /// Keeps track of whether or not certain animations are currently playing
        /// </summary>
        public void CheckAnimationStates()
        {           
            if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.Active)
            {
                if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    if (EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Attack1Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Attack2Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Attack3Animation || 
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Attack4Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Attack5Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Attack6Animation)
                    {
                        EmeraldComponent.Attacking = true;

                        if (EmeraldComponent.CurrentBlockingState == EmeraldAISystem.BlockingState.Blocking)
                        {
                            EmeraldComponent.CurrentBlockingState = EmeraldAISystem.BlockingState.NotBlocking;
                            EmeraldComponent.AIAnimator.SetBool("Blocking", false);
                            EmeraldComponent.AIAnimator.ResetTrigger("Hit");
                        }
                    }
                    else
                    {
                        EmeraldComponent.Attacking = false;
                    }
                }
                else if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                {
                    if (EmeraldComponent.CurrentAnimationClip == EmeraldComponent.RangedAttack1Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.RangedAttack2Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.RangedAttack3Animation)
                    {
                        EmeraldComponent.Attacking = true;
                    }
                    else
                    {
                        EmeraldComponent.Attacking = false;
                    }
                }
            }
            
            if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.NotActive)
            {
                if (EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Hit1Animation || 
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Hit2Animation || 
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Hit3Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Hit4Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Hit5Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Hit6Animation)
                {
                    EmeraldComponent.GettingHit = true;
                }
                else
                {
                    EmeraldComponent.GettingHit = false;
                }

                if (EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Emote1Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Emote2Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Emote3Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Emote4Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Emote5Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Emote6Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Emote7Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Emote8Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Emote9Animation ||
                    EmeraldComponent.CurrentAnimationClip == EmeraldComponent.Emote10Animation)
                {
                    EmeraldComponent.EmoteAnimationActive = true;
                }
                else
                {
                    EmeraldComponent.EmoteAnimationActive = false;
                }
            }
            else if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.Active)
            {
                if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    if (EmeraldComponent.CurrentAnimationClip == EmeraldComponent.CombatHit1Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.CombatHit2Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.CombatHit3Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.CombatHit4Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.CombatHit5Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.CombatHit6Animation)
                    {
                        EmeraldComponent.GettingHit = true;
                        EmeraldComponent.CurrentBlockingState = EmeraldAISystem.BlockingState.NotBlocking;
                        EmeraldComponent.AIAnimator.SetBool("Blocking", false);
                    }
                    else
                    {
                        EmeraldComponent.GettingHit = false;
                    }
                }
                else if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                {
                    if (EmeraldComponent.CurrentAnimationClip == EmeraldComponent.RangedCombatHit1Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.RangedCombatHit2Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.RangedCombatHit3Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.RangedCombatHit4Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.RangedCombatHit5Animation ||
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.RangedCombatHit6Animation)
                    {
                        EmeraldComponent.GettingHit = true;
                    }
                    else
                    {
                        EmeraldComponent.GettingHit = false;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the AI back to its default state
        /// </summary>
        public void DefaultState()
        {
            EmeraldComponent.BehaviorRef = (EmeraldAISystem.CurrentBehavior)EmeraldComponent.StartingBehaviorRef;
            EmeraldComponent.ConfidenceRef = (EmeraldAISystem.ConfidenceType)EmeraldComponent.StartingConfidenceRef;
            EmeraldComponent.CombatStateRef = EmeraldAISystem.CombatState.NotActive;
            EmeraldComponent.CurrentDetectionRef = EmeraldAISystem.CurrentDetection.Unaware;
            EmeraldComponent.CurrentBlockingState = EmeraldAISystem.BlockingState.NotBlocking;
            EmeraldComponent.AttackTimer = 0;
            EmeraldComponent.RunAttackTimer = 0;
            EmeraldComponent.Attacking = false;
            EmeraldComponent.WarningAnimationTriggered = false;
            EmeraldComponent.AIAnimator.SetBool("Turn Right", false);
            EmeraldComponent.AIAnimator.SetBool("Turn Left", false);
            EmeraldComponent.AIAnimator.SetBool("Blocking", false);
            EmeraldComponent.AIAnimator.ResetTrigger("Hit");
            EmeraldComponent.AIAnimator.ResetTrigger("Attack");
            EmeraldComponent.AIAnimator.SetBool("Combat State Active", false);
            EmeraldComponent.EmeraldEventsManagerComponent.ClearTarget();
            EmeraldComponent.CurrentMovementState = EmeraldComponent.StartingMovementState;
            EmeraldComponent.FirstTimeInCombat = true;

            if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion)
            {
                EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.StoppingDistance;             
            }
            else
            {
                EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.FollowingStoppingDistance;
            }

            EmeraldComponent.m_NavMeshAgent.updateRotation = true;
            EmeraldComponent.AIAnimator.SetBool("Walk Backwards", false);
            EmeraldComponent.BackingUp = false;
            EmeraldComponent.BackingUpTimer = 0;
            EmeraldComponent.CautiousTimer = 0;
            EmeraldComponent.DeathDelayTimer = 0;
            EmeraldComponent.DeathDelayActive = false;

            if (EmeraldComponent.UseEquipAnimation == EmeraldAISystem.YesOrNo.Yes)
            {
                StartCoroutine(DelayReturnToDestination(1));
            }
            else
            {
                StartCoroutine(DelayReturnToDestination(0));
            }

            if (EmeraldComponent.RefillHealthType == EmeraldAISystem.RefillHealth.Instantly)
            {
                EmeraldComponent.CurrentHealth = EmeraldComponent.StartingHealth;
            }
            else if (EmeraldComponent.RefillHealthType == EmeraldAISystem.RefillHealth.OverTime)
            {
                StartCoroutine(RefillHeathOverTime());
            } 
            
            if (EmeraldComponent.ReturningToStartInProgress)
            {
                StartCoroutine(ReturnToStartComplete());
            }
        }

        //Wait until the AI has returned to its starting destination to reset its detection radius. 
        //This is only used by AI who use the Starting Position Distance Type.
        IEnumerator ReturnToStartComplete()
        {
            EmeraldComponent.CurrentMovementState = EmeraldAISystem.MovementState.Run;
            EmeraldComponent.m_NavMeshAgent.SetDestination(EmeraldComponent.StartingDestination);
            EmeraldComponent.DetectionRadius = 1;
            yield return new WaitUntil(() => EmeraldComponent.m_NavMeshAgent.remainingDistance <= EmeraldComponent.m_NavMeshAgent.stoppingDistance && !EmeraldComponent.m_NavMeshAgent.pathPending);
            EmeraldComponent.DetectionRadius = StartingDetectionRadius;
            EmeraldComponent.CurrentMovementState = EmeraldComponent.StartingMovementState;
            EmeraldComponent.ReturningToStartInProgress = false;
        }

        IEnumerator RefillHeathOverTime ()
        {
            while (EmeraldComponent.CurrentHealth < EmeraldComponent.StartingHealth)
            {
                EmeraldComponent.RegenerateTimer += Time.deltaTime;
                if (EmeraldComponent.RegenerateTimer >= EmeraldComponent.HealthRegRate)
                {
                    EmeraldComponent.CurrentHealth += EmeraldComponent.RegenerateAmount;
                    EmeraldComponent.RegenerateTimer = 0;
                }

                if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.Active)
                {
                    break;
                }

                yield return null;
            }

            if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.NotActive)
            {
                EmeraldComponent.CurrentHealth = EmeraldComponent.StartingHealth;
            }
        }

        IEnumerator DelayReturnToDestination (float DelaySeconds)
        {
            yield return new WaitForSeconds(DelaySeconds);
            if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion)
            {
                if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Dynamic)
                {
                    EmeraldComponent.GenerateWaypoint();
                }
                else if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Stationary && EmeraldComponent.m_NavMeshAgent.enabled)
                {
                    EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.StartingDestination;
                    EmeraldComponent.ReturnToStationaryPosition = true;
                }
                else if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Waypoints && EmeraldComponent.m_NavMeshAgent.enabled)
                {
                    EmeraldComponent.m_NavMeshAgent.ResetPath();
                    if (EmeraldComponent.WaypointTypeRef != EmeraldAISystem.WaypointType.Random)
                    {
                        EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.WaypointsList[EmeraldComponent.WaypointIndex];
                    }
                    else
                    {
                        EmeraldComponent.WaypointTimer = 1;
                    }
                }
                else if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Destination && EmeraldComponent.m_NavMeshAgent.enabled)
                {
                    EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.SingleDestination;
                    EmeraldComponent.ReturnToStationaryPosition = true;
                }
            }
        }

        /// <summary>
        /// Activates our AI's Combat State
        /// </summary>
        public void ActivateCombatState()
        {
            EmeraldComponent.AIAnimator.ResetTrigger("Hit");
            EmeraldComponent.CombatStateRef = EmeraldAISystem.CombatState.Active;
            EmeraldComponent.AIAnimator.SetBool("Idle Active", false);
            EmeraldComponent.AIAnimator.SetBool("Combat State Active", true);
            EmeraldComponent.CurrentMovementState = EmeraldAISystem.MovementState.Run;

            if (EmeraldComponent.ConfidenceRef == EmeraldAISystem.ConfidenceType.Coward)
            {
                EmeraldComponent.OnFleeEvent.Invoke();
            }
        }

        /// <summary>
        /// Calculates our AI's attacks
        /// </summary>
        public void AttackState()
        {
            if (EmeraldComponent.DistanceFromTarget > EmeraldComponent.StoppingDistance && !EmeraldComponent.DeathDelayActive)
            {               
                if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.CurrentTarget.position;
                }
                else if (EmeraldComponent.TargetObstructed && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged && 
                    EmeraldComponent.TargetObstructedActionRef != EmeraldAISystem.TargetObstructedAction.StayStationary)
                {
                    EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.CurrentTarget.position;
                }
                else if (!EmeraldComponent.TargetObstructed && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                {
                    EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.CurrentTarget.position;
                }
            }

            if (EmeraldComponent.TargetObstructed && EmeraldComponent.TargetObstructedActionRef == EmeraldAISystem.TargetObstructedAction.MoveCloserAfterSetSeconds)
            {
                EmeraldComponent.ObstructionTimer += Time.deltaTime;

                if (EmeraldComponent.ObstructionTimer >= EmeraldComponent.ObstructionSeconds && EmeraldComponent.m_NavMeshAgent.stoppingDistance > 10)
                {
                    EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.CurrentTarget.position;
                    EmeraldComponent.m_NavMeshAgent.stoppingDistance -= 5;
                    EmeraldComponent.ObstructionTimer = 0;
                }
            }

            if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.Active && !EmeraldComponent.DeathDelayActive && !EmeraldComponent.m_SwitchingWeaponTypes)
            {
                if (EmeraldComponent.m_NavMeshAgent.remainingDistance < EmeraldComponent.m_NavMeshAgent.stoppingDistance && !EmeraldComponent.IsMoving && !EmeraldComponent.Attacking)
                {                    
                    EmeraldComponent.AttackTimer += Time.deltaTime;                    

                    if (EmeraldComponent.AttackTimer >= (EmeraldComponent.AttackSpeed) && 
                        !EmeraldComponent.GettingHit && EmeraldComponent.CurrentBlockingState == EmeraldAISystem.BlockingState.NotBlocking)
                    {
                        //Get the distance between the target and the AI. Negate the x and z axes to get the y axis height between the two objects.
                        //This is used to stop AI from being able to trigger attacks that exceed the AI's Attack Height.
                        Vector3 m_TargetPos = EmeraldComponent.CurrentTarget.position;
                        m_TargetPos.x = 0;
                        m_TargetPos.z = 0;
                        Vector3 m_CurrentPos = EmeraldComponent.HitPointTransform.position;
                        m_CurrentPos.x = 0;
                        m_CurrentPos.z = 0;
                        float m_TargetHeight = Vector3.Distance(m_TargetPos, m_CurrentPos);

                        if (!EmeraldComponent.TargetObstructed && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged || 
                            EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && !EmeraldComponent.m_NavMeshAgent.pathPending && 
                            EmeraldComponent.DestinationAdjustedAngle <= EmeraldComponent.MaxDamageAngle && m_TargetHeight <= EmeraldComponent.AttackHeight && !EmeraldComponent.TargetObstructed)
                        {
                            float angle = EmeraldComponent.TargetAngle();                           
                            if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged && angle > EmeraldComponent.MaxFiringAngle || EmeraldComponent.CurrentAggroHits >= EmeraldComponent.TotalAggroHits || EmeraldComponent.IsRunAttack)
                            {
                                EmeraldComponent.AttackTimer = 0;
                                EmeraldComponent.IsRunAttack = false;
                                return;
                            }

                            EmeraldComponent.GeneratedBlockOdds = Random.Range(1, 101);
                            EmeraldComponent.GeneratedBackupOdds = Random.Range(1, 101);
                            EmeraldComponent.AIAnimator.ResetTrigger("Hit");
                            EmeraldComponent.CurrentBlockingState = EmeraldAISystem.BlockingState.NotBlocking;
                            EmeraldComponent.AIAnimator.SetBool("Blocking", false);

                            if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged ||
                                EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                            {
                                float AbilityOddsRoll = Random.Range(0f, 1f);
                                AbilityOddsRoll = AbilityOddsRoll * 100;

                                if (!EmeraldComponent.m_AbilityPicked && EmeraldComponent.SupportAbilities.Count > 0 && !EmeraldComponent.HealingCooldownActive && 
                                    ((float)EmeraldComponent.CurrentHealth / EmeraldComponent.StartingHealth*100) < EmeraldComponent.HealthPercentageToHeal)
                                {
                                    EmeraldAICombatManager.GenerateSupportAbility(EmeraldComponent);
                                    EmeraldComponent.AIAnimator.SetInteger("Attack Index", EmeraldComponent.CurrentAnimationIndex+1);
                                    EmeraldComponent.AIAnimator.SetTrigger("Attack");
                                    EmeraldComponent.m_AbilityPicked = true;
                                }
                                else if (!EmeraldComponent.m_AbilityPicked && EmeraldComponent.TotalSummonedAI < EmeraldComponent.MaxAllowedSummonedAI && EmeraldComponent.SummoningAbilities.Count > 0)
                                {
                                    EmeraldAICombatManager.GenerateSummoningAbility(EmeraldComponent);
                                    EmeraldComponent.AIAnimator.SetInteger("Attack Index", EmeraldComponent.CurrentAnimationIndex+1);
                                    EmeraldComponent.AIAnimator.SetTrigger("Attack");
                                    EmeraldComponent.m_AbilityPicked = true;
                                }
                                else if (!EmeraldComponent.m_AbilityPicked && EmeraldComponent.OffensiveAbilities.Count > 0)                             
                                {                                    
                                    if (!EmeraldComponent.EmeraldDetectionComponent.m_LookAtInProgress && EmeraldComponent.UseHeadLookRef == EmeraldAISystem.YesOrNo.Yes ||
                                        EmeraldComponent.UseHeadLookRef == EmeraldAISystem.YesOrNo.No)
                                    {
                                        EmeraldAICombatManager.GenerateOffensiveAbility(EmeraldComponent);
                                        EmeraldComponent.m_InitialTargetPosition = EmeraldComponent.CurrentTarget.position;
                                        EmeraldComponent.AIAnimator.SetInteger("Attack Index", EmeraldComponent.CurrentAnimationIndex+1);
                                        EmeraldComponent.AIAnimator.SetTrigger("Attack");
                                        EmeraldComponent.m_AbilityPicked = true;
                                    }
                                    else
                                    {
                                        //If our AI is in the process of rotating with the look at feature, return. 
                                        return;
                                    }
                                }
                            }

                            if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                            {
                                EmeraldAICombatManager.GenerateMeleeAttack(EmeraldComponent);
                                EmeraldComponent.AIAnimator.SetInteger("Attack Index", EmeraldComponent.CurrentAnimationIndex + 1);
                                EmeraldComponent.AIAnimator.SetTrigger("Attack");
                            }

                            EmeraldComponent.AttackTimer = 0;
                            float RandomOffset = Random.Range(0.1f, 0.5f); //Add a small random offset to the attack speed to prevent attacks from firing simultaneously
                            RandomOffset = Mathf.Round(RandomOffset * 10f) / 10f; //Round said offset
                            EmeraldComponent.AttackSpeed = Random.Range(EmeraldComponent.MinimumAttackSpeed, EmeraldComponent.MaximumAttackSpeed + 1) + RandomOffset;
                        }
                        else if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && m_TargetHeight > EmeraldComponent.AttackHeight)
                        {
                            EmeraldComponent.AttackTimer = 0;
                        }
                    }
                }
                else if (EmeraldComponent.m_NavMeshAgent.remainingDistance > EmeraldComponent.m_NavMeshAgent.stoppingDistance && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee 
                    && EmeraldComponent.CurrentBlockingState == EmeraldAISystem.BlockingState.NotBlocking)
                {
                    if (EmeraldComponent.AttackOnArrival == EmeraldAISystem.EnableDisable.Enabled)
                    {
                        EmeraldComponent.AttackTimer = EmeraldComponent.AttackSpeed - 0.25f;
                    }
                }

                if (EmeraldComponent.UseRunAttacksRef == EmeraldAISystem.UseRunAttacks.Yes)
                {
                    if (Vector3.Distance(EmeraldComponent.CurrentTarget.position, transform.position) <= (EmeraldComponent.m_NavMeshAgent.stoppingDistance + 
                        EmeraldComponent.RunAttackDistance) && EmeraldComponent.IsMoving)
                    {
                        EmeraldComponent.RunAttackTimer += Time.deltaTime;

                        if (EmeraldComponent.RunAttackTimer >= EmeraldComponent.RunAttackSpeed)
                        {
                            EmeraldComponent.CurrentBlockingState = EmeraldAISystem.BlockingState.NotBlocking;
                            EmeraldComponent.AIAnimator.SetBool("Blocking", false);
                            EmeraldComponent.AIAnimator.ResetTrigger("Hit");
                            if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No ||
                                EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                            {
                                EmeraldAICombatManager.GenerateMeleeRunAttack(EmeraldComponent);
                                EmeraldComponent.AIAnimator.SetInteger("Run Attack Index", EmeraldComponent.CurrentRunAttackAnimationIndex + 1);
                                EmeraldComponent.AIAnimator.SetTrigger("Run Attack");
                            }
                            else
                            {
                                EmeraldAICombatManager.GenerateMeleeRunAttack(EmeraldComponent);
                                EmeraldComponent.AIAnimator.SetInteger("Run Attack Index", EmeraldComponent.CurrentRunAttackAnimationIndex + 1);
                                EmeraldComponent.AIAnimator.SetTrigger("Run Attack");
                            }
                            EmeraldComponent.RunAttackSpeed = Random.Range(EmeraldComponent.MinimumRunAttackSpeed, EmeraldComponent.MaximumRunAttackSpeed);
                            EmeraldComponent.RunAttackTimer = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates and applies the Block State
        /// </summary>
        public void BlockState()
        {
            //Activates blocking, when the appropriate conditions are met.
            if (!EmeraldComponent.BackingUp && !EmeraldComponent.IsMoving && EmeraldComponent.CurrentTarget && !EmeraldComponent.DeathDelayActive)
            {
                if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.Active && !EmeraldComponent.Attacking && !EmeraldComponent.GettingHit && EmeraldComponent.AIAgentActive && 
                    EmeraldComponent.m_NavMeshAgent.remainingDistance <= EmeraldComponent.m_NavMeshAgent.stoppingDistance && !EmeraldComponent.m_NavMeshAgent.pathPending && !EmeraldComponent.AIAnimator.GetBool("Attack"))
                {
                    if (EmeraldComponent.AttackTimer < EmeraldComponent.AttackSpeed - 0.25f && EmeraldComponent.GeneratedBlockOdds <= EmeraldComponent.BlockOdds && 
                        EmeraldComponent.CurrentAnimationClip == EmeraldComponent.CombatIdleAnimation && EmeraldComponent.CurrentBlockingState == EmeraldAISystem.BlockingState.NotBlocking)
                    {
                        EmeraldComponent.AIAnimator.ResetTrigger("Attack");
                        EmeraldComponent.CurrentBlockingState = EmeraldAISystem.BlockingState.Blocking;
                        EmeraldComponent.AIAnimator.SetBool("Blocking", true);
                    }
                }
            }

            if (EmeraldComponent.CurrentBlockingState == EmeraldAISystem.BlockingState.Blocking)
            {
                BlockTimer += Time.deltaTime;

                //Disables blocking, when the appropriate conditions are met.
                if (BlockTimer >= 3 || EmeraldComponent.BackingUp)
                {
                    EmeraldComponent.CurrentBlockingState = EmeraldAISystem.BlockingState.NotBlocking;
                    EmeraldComponent.AIAnimator.SetBool("Blocking", false);

                    if (BlockTimer >= 3)
                    {
                        EmeraldComponent.AIAnimator.SetBool("Block Cooldown Active", true);
                        Invoke("ActivateBlockCooldown", 0.75f);
                        BlockTimer = 0;
                    }
                }
            }
        }

        void ActivateBlockCooldown ()
        {
            EmeraldComponent.AIAnimator.SetBool("Block Cooldown Active", false);
        }

        /// <summary>
        /// Calculates and applies our AI's Backup State
        /// </summary>
        public void BackupState ()
        {
            EmeraldComponent.BackingUpTimer += Time.deltaTime;
            EmeraldComponent.m_NavMeshAgent.stoppingDistance = 2;
            EmeraldComponent.BackingUp = true;

            if (EmeraldComponent.AnimatorType == EmeraldAISystem.AnimatorTypeState.NavMeshDriven)
            {
                if (EmeraldComponent.Attacking)
                {
                    EmeraldComponent.BackingUpTimer = 0;
                    EmeraldComponent.m_NavMeshAgent.speed = 0;
                }
                else if (!EmeraldComponent.Attacking)
                {
                    EmeraldComponent.m_NavMeshAgent.speed = EmeraldComponent.WalkBackwardsSpeed;
                }
            }

            Vector3 direction = (EmeraldComponent.CurrentTarget.position - transform.position).normalized;
            Vector3 GeneratedDestination = transform.position + -direction * 10;
            GeneratedDestination.y = transform.position.y;
            EmeraldComponent.m_NavMeshAgent.destination = GeneratedDestination;

            //Get the angle between the current target and the AI. If using the alignment feature,
            //adjust the angle to include the rotation difference of the AI's current surface angle.
            Vector3 Direction = new Vector3(EmeraldComponent.CurrentTarget.position.x, 0, EmeraldComponent.CurrentTarget.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            float TargetAngle = Vector3.Angle(transform.forward, Direction);
            Vector3 DestinationDirection = Direction;

            if (EmeraldComponent.AlignAIWithGroundRef == EmeraldAISystem.AlignAIWithGround.Yes)
            {
                float RoationDifference = transform.localEulerAngles.x;
                RoationDifference = (RoationDifference > 180) ? RoationDifference - 360 : RoationDifference;
                EmeraldComponent.DestinationAdjustedAngle = Mathf.Abs(TargetAngle) - Mathf.Abs(RoationDifference);
            }
            else if (EmeraldComponent.AlignAIWithGroundRef == EmeraldAISystem.AlignAIWithGround.No)
            {
                EmeraldComponent.DestinationAdjustedAngle = Mathf.Abs(TargetAngle);
            }

            if (EmeraldComponent.AlignAIWithGroundRef == EmeraldAISystem.AlignAIWithGround.Yes)
            {
                Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                Quaternion qGround = Quaternion.FromToRotation(Vector3.up, EmeraldComponent.SurfaceNormal) * qTarget;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qGround, Time.deltaTime * EmeraldComponent.BackupTurningSpeed);
            }
            else if (EmeraldComponent.AlignAIWithGroundRef == EmeraldAISystem.AlignAIWithGround.No)
            {
                Quaternion qTarget = Quaternion.LookRotation(DestinationDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qTarget, Time.deltaTime * EmeraldComponent.BackupTurningSpeed);
            }

            //Get the distance while backing up. If the hit point disatnce becomes less than or equal to 1, it will stop the back up process.
            RaycastHit HitBehind;
            if (Physics.Raycast(EmeraldComponent.HeadTransform.position, -transform.forward, out HitBehind, EmeraldComponent.BackupDistance))
            {
                if (HitBehind.collider != null && HitBehind.collider.gameObject != this.gameObject)
                {                  
                    BackupDistance = HitBehind.distance;
                }
            }

            //Return if these conditions are met to stop an AI from backing up
            if (EmeraldComponent.CurrentTarget == null || EmeraldComponent.DeathDelayActive || BackupDistance <= 1)
            {
                EmeraldComponent.AIAnimator.SetBool("Walk Backwards", false);
                EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.AttackDistance;
                EmeraldComponent.m_NavMeshAgent.updateRotation = true;
                EmeraldComponent.m_NavMeshAgent.destination = transform.position;                
                EmeraldComponent.BackingUp = false;
                EmeraldComponent.BackingUpTimer = 0;
                EmeraldComponent.AttackTimer = 0;
                return;
            }

            if (EmeraldComponent.BackingUpTimer >= EmeraldComponent.BackingUpSeconds ||
            EmeraldComponent.m_NavMeshAgent.remainingDistance <= EmeraldComponent.m_NavMeshAgent.stoppingDistance && !EmeraldComponent.m_NavMeshAgent.pathPending) 
            {
                EmeraldComponent.AIAnimator.ResetTrigger("Hit");
                EmeraldComponent.AIAnimator.SetBool("Walk Backwards", false);
                EmeraldComponent.AttackTimer = 0;
            }

            if (EmeraldComponent.BackingUpTimer >= EmeraldComponent.BackingUpSeconds + 0.5f)
            {
                float CurrentDistance = Vector3.Distance(EmeraldComponent.CurrentTarget.position, transform.position);
                
                if (CurrentDistance > 3 && CurrentDistance < 8 && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    EmeraldComponent.AttackTimer = 0;
                    EmeraldComponent.CurrentBlockingState = EmeraldAISystem.BlockingState.NotBlocking;
                    EmeraldComponent.AIAnimator.SetBool("Blocking", false);
                    EmeraldComponent.AIAnimator.SetBool("Walk Backwards", false);
                    EmeraldComponent.AIAnimator.ResetTrigger("Hit");                  

                    if (EmeraldComponent.UseRunAttacksRef == EmeraldAISystem.UseRunAttacks.Yes)
                    {
                        EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.CurrentTarget.position;
                        EmeraldAICombatManager.GenerateMeleeRunAttack(EmeraldComponent);
                        EmeraldComponent.AIAnimator.SetInteger("Run Attack Index", EmeraldComponent.CurrentRunAttackAnimationIndex + 1);
                        EmeraldComponent.AIAnimator.SetTrigger("Run Attack");
                        EmeraldComponent.IsRunAttack = true;
                    }
                }
                else
                {
                    EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.CurrentTarget.position;
                }

                if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                {
                    EmeraldComponent.AttackTimer = EmeraldComponent.AttackSpeed;
                }

                EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.AttackDistance;
                EmeraldComponent.m_NavMeshAgent.updateRotation = true;
                EmeraldComponent.BackingUp = false;
                EmeraldComponent.BackingUpTimer = 0;
                EmeraldComponent.BackingUpSeconds = Random.Range(EmeraldComponent.BackingUpSecondsMin, EmeraldComponent.BackingUpSecondsMax+1);
                EmeraldComponent.GeneratedBackupOdds = Random.Range(1, 101);
                BackupDelayActive = true;
                Invoke("BackupDelay", 1);
            }
        }

        void BackupDelay ()
        {
            BackupDelayActive = false;
        }

        /// <summary>
        /// Calculates backing our AI up, when the appropriate conditions are met
        /// </summary>
        public void CalculateBackupState()
        {
            if (EmeraldComponent.CurrentTarget != null && !BackupDelayActive)
            {
                if (EmeraldComponent.BackupTypeRef == EmeraldAISystem.BackupType.Instant || 
                    EmeraldComponent.BackupTypeRef == EmeraldAISystem.BackupType.Odds && EmeraldComponent.GeneratedBackupOdds <= EmeraldComponent.BackupOdds && EmeraldComponent.AttackTimer > 0.5f)
                {
                    if (EmeraldComponent.DistanceFromTarget <= EmeraldComponent.TooCloseDistance && !EmeraldComponent.m_NavMeshAgent.pathPending && !EmeraldComponent.Attacking)
                    {
                        float AdjustedAngle = EmeraldComponent.TargetAngle();                     

                        if (AdjustedAngle <= 60 && !EmeraldComponent.AIAnimator.GetBool("Turn Left") && !EmeraldComponent.AIAnimator.GetBool("Turn Right"))
                        {
                            //Do a quick raycast to see if behind the AI is clear before calling the backup state.
                            RaycastHit HitBehind;
                            if (Physics.Raycast(EmeraldComponent.HeadTransform.position, -transform.forward, out HitBehind, EmeraldComponent.BackupDistance))
                            {
                                if (HitBehind.collider != null && HitBehind.distance > 3)
                                {                                  
                                    if (EmeraldComponent.AnimatorType == EmeraldAISystem.AnimatorTypeState.NavMeshDriven)
                                    {
                                        EmeraldComponent.m_NavMeshAgent.speed = EmeraldComponent.WalkBackwardsSpeed;
                                    }
                                    EmeraldComponent.CurrentBlockingState = EmeraldAISystem.BlockingState.NotBlocking;
                                    EmeraldComponent.AIAnimator.SetBool("Blocking", false);
                                    EmeraldComponent.m_NavMeshAgent.updateRotation = false;
                                    Vector3 diff = transform.position - EmeraldComponent.CurrentTarget.position;
                                    diff.y = 0.0f;
                                    EmeraldComponent.BackupDestination = EmeraldComponent.CurrentTarget.position + diff.normalized * HitBehind.distance;
                                    EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.BackupDestination;
                                    EmeraldComponent.AIAnimator.ResetTrigger("Hit");
                                    EmeraldComponent.AIAnimator.ResetTrigger("Attack");
                                    EmeraldComponent.AIAnimator.SetBool("Turn Left", false);
                                    EmeraldComponent.AIAnimator.SetBool("Turn Right", false);
                                    EmeraldComponent.AIAnimator.SetBool("Walk Backwards", true);
                                    BackupDistance = EmeraldComponent.BackupDistance;
                                    EmeraldComponent.BackingUp = true;
                                }
                            }
                            else
                            {
                                if (EmeraldComponent.AnimatorType == EmeraldAISystem.AnimatorTypeState.NavMeshDriven)
                                {
                                    EmeraldComponent.m_NavMeshAgent.speed = EmeraldComponent.WalkBackwardsSpeed;
                                }
                                EmeraldComponent.CurrentBlockingState = EmeraldAISystem.BlockingState.NotBlocking;
                                EmeraldComponent.AIAnimator.SetBool("Blocking", false);
                                EmeraldComponent.m_NavMeshAgent.updateRotation = false;
                                Vector3 diff = transform.position - EmeraldComponent.CurrentTarget.position;
                                diff.y = 0.0f;
                                EmeraldComponent.BackupDestination = EmeraldComponent.CurrentTarget.position + diff.normalized * EmeraldComponent.BackupDistance;
                                EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.BackupDestination;
                                EmeraldComponent.AIAnimator.ResetTrigger("Hit");
                                EmeraldComponent.AIAnimator.ResetTrigger("Attack");
                                EmeraldComponent.AIAnimator.SetBool("Turn Left", false);
                                EmeraldComponent.AIAnimator.SetBool("Turn Right", false);
                                EmeraldComponent.AIAnimator.SetBool("Walk Backwards", true);
                                BackupDistance = EmeraldComponent.BackupDistance;
                                EmeraldComponent.BackingUp = true;
                            }
                        }
                    }
                }
            }
            else
            {
                //If our target dies or is lost, search for another target.
                EmeraldComponent.EmeraldDetectionComponent.SearchForTarget();
            }
        }
    }
}
