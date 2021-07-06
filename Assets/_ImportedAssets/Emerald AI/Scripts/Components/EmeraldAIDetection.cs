using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Utility
{
    public class EmeraldAIDetection : MonoBehaviour
    {
        [HideInInspector]
        public float YOffSet = 0;
        [HideInInspector]
        public Transform PreviousTarget;
        [HideInInspector]
        public bool SearchForRandomTarget = false;
        EmeraldAISystem EmeraldComponent;
        bool AvoidanceTrigger;
        float AvoidanceTimer;       
        float AvoidanceSeconds = 1.25f;
        Vector3 TargetDirection;
        [HideInInspector]
        public GameObject CurrentObstruction;
        Color C = Color.white;
        [HideInInspector]
        public float m_LookLerpValue;
        [HideInInspector]
        public float m_TurnLerpValue;
        [HideInInspector]
        public float LookWeightLerp2;
        [HideInInspector]
        public float LookHeight;
        public enum PlayerDetectionRef { Detected, NotDetected };
        [HideInInspector]
        public PlayerDetectionRef PlayerDetection = PlayerDetectionRef.NotDetected;
        bool m_PlayerDetected = false;
        [HideInInspector]
        public float PlayerDetectionCooldown;
        bool m_FirstTimeDetectingPlayer;
        [HideInInspector]
        public bool m_LookAtInProgress;
        [HideInInspector]
        public float m_PreviousHeight;
        [HideInInspector]
        public float DetectionTimer;

        void Start()
        {
            EmeraldComponent = GetComponent<EmeraldAISystem>();
        }

        void Update()
        {
            //Update our OverlapShere function based on the DetectionFrequency
            if (EmeraldComponent.TargetDetectionActive)
            {
                DetectionTimer += Time.deltaTime;

                if (DetectionTimer >= EmeraldComponent.DetectionFrequency)
                {
                    UpdateAIDetection();
                    DetectionTimer = 0;
                }
            }           
        }

        void FixedUpdate()
        {
            if (EmeraldComponent.UseAIAvoidance == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.m_NavMeshAgent.enabled && EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.NotActive)
            {
                AIAvoidance();
            }

            if (EmeraldComponent.DetectionTypeRef == EmeraldAISystem.DetectionType.LineOfSight && EmeraldComponent.OptimizedStateRef == EmeraldAISystem.OptimizedState.Inactive && EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.NotActive)
            {
                LineOfSightDetection();
            }

            if (EmeraldComponent.UseHeadLookRef == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.CurrentTarget != null)
            {
                if (EmeraldComponent.AIAnimator.layerCount == 2)
                {
                    EmeraldComponent.m_LayerCurrentState = EmeraldComponent.AIAnimator.GetCurrentAnimatorStateInfo(1);
                }

                float CurrentDistance = Vector3.Distance(EmeraldComponent.CurrentTarget.position, transform.position);
                float AdjustedAngle = EmeraldComponent.TargetAngle();

                if (!EmeraldComponent.DeathDelayActive)
                {
                    LookWeightLerp2 += Time.deltaTime * EmeraldComponent.LookSmoother;
                }

                if (CurrentDistance <= EmeraldComponent.MaxLookAtDistance && AdjustedAngle <= EmeraldComponent.LookAtLimit && LookWeightLerp2 > 1)
                {
                    EmeraldComponent.lookWeight = Mathf.Lerp(EmeraldComponent.lookWeight, 1f, Time.deltaTime * EmeraldComponent.LookSmoother);
                }
                else
                {
                    if (CurrentDistance > EmeraldComponent.MaxLookAtDistance || AdjustedAngle > EmeraldComponent.LookAtLimit)
                    {
                        if (!m_LookAtInProgress)
                        {
                            EmeraldComponent.lookWeight = Mathf.Lerp(EmeraldComponent.lookWeight, 0, Time.deltaTime * EmeraldComponent.LookSmoother);
                        }
                    }
                }
            }
            else if (EmeraldComponent.UseHeadLookRef == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.HeadLookRef != null)
            {              
                if (EmeraldComponent.AIAnimator.layerCount == 2)
                {
                    EmeraldComponent.m_LayerCurrentState = EmeraldComponent.AIAnimator.GetCurrentAnimatorStateInfo(1);
                }

                float CurrentDistance = Vector3.Distance(EmeraldComponent.HeadLookRef.position, transform.position);               
                float AdjustedAngle = EmeraldComponent.HeadLookAngle();

                if (CurrentDistance <= EmeraldComponent.MaxLookAtDistance && AdjustedAngle <= EmeraldComponent.LookAtLimit)
                {
                    EmeraldComponent.lookWeight = Mathf.Lerp(EmeraldComponent.lookWeight, 1f, Time.deltaTime * EmeraldComponent.LookSmoother);
                }
                else
                {                  
                    EmeraldComponent.lookWeight = Mathf.Lerp(EmeraldComponent.lookWeight, 0, Time.deltaTime * EmeraldComponent.LookSmoother);
                }
            }
            else if (EmeraldComponent.UseHeadLookRef == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.HeadLookRef == null)
            {              
                if (EmeraldComponent.AIAnimator.layerCount == 2)
                {
                    EmeraldComponent.m_LayerCurrentState = EmeraldComponent.AIAnimator.GetCurrentAnimatorStateInfo(1);
                }

                LookWeightLerp2 += Time.deltaTime * EmeraldComponent.LookSmoother;

                if (LookWeightLerp2 > 1)
                {
                    EmeraldComponent.lookWeight = Mathf.Lerp(EmeraldComponent.lookWeight, 0, Time.deltaTime * EmeraldComponent.LookSmoother);
                }
            }
        }

        //Handles all of the AI's Look At features.
        private void OnAnimatorIK()
        {
            if (EmeraldComponent != null && EmeraldComponent.UseHeadLookRef == EmeraldAISystem.YesOrNo.Yes)
            {
                if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.NotActive)
                {
                    EmeraldComponent.AIAnimator.SetLookAtWeight(EmeraldComponent.lookWeight * EmeraldComponent.HeadLookWeightNonCombat, EmeraldComponent.lookWeight * EmeraldComponent.BodyLookWeightNonCombat);
                }
                else if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.Active)
                {
                    EmeraldComponent.AIAnimator.SetLookAtWeight(EmeraldComponent.lookWeight, EmeraldComponent.lookWeight * EmeraldComponent.BodyLookWeightCombat, EmeraldComponent.lookWeight * EmeraldComponent.HeadLookWeightCombat);
                }

                //Handles all of the AI's looking calculations for AI
                if (EmeraldComponent.TargetTypeRef == EmeraldAISystem.TargetType.AI)
                {
                    if (EmeraldComponent.CurrentTarget != null && PreviousTarget != null && EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.Active && !EmeraldComponent.DeathDelayActive)
                    {
                        //Subtract the offset based on the distance of the target as the offset is needed less as the target gets closer to the AI. 
                        float m_TargetAimOffsetDistance = Vector3.Distance(EmeraldComponent.CurrentTarget.position,transform.position) / EmeraldComponent.MaxLookAtDistance;
                        m_TargetAimOffsetDistance = Mathf.Clamp(m_TargetAimOffsetDistance, 0, 1);

                        //Subtract the offset based on the distance of the previous target as the offset is needed less as the target gets closer to the AI. 
                        float m_PreviousTargetAimOffsetDistance = Vector3.Distance(PreviousTarget.position, transform.position) / EmeraldComponent.MaxLookAtDistance;
                        m_PreviousTargetAimOffsetDistance = Mathf.Clamp(m_PreviousTargetAimOffsetDistance, 0, 1);

                        //Apply the offset to the AI's hit point and do the same for using the previous target
                        Vector3 CurrentTargetPos = EmeraldComponent.TargetEmerald.HitPointTransform.position - new Vector3(0, m_TargetAimOffsetDistance * YOffSet, 0);
                        Vector3 PreviousTargetPos = new Vector3(PreviousTarget.position.x, PreviousTarget.position.y - m_PreviousTargetAimOffsetDistance * YOffSet, PreviousTarget.position.z);

                        LookHeight = PreviousTargetPos.y;

                        if (!EmeraldComponent.DeathDelayActive)
                        {
                            m_TurnLerpValue += Time.deltaTime * EmeraldComponent.LookSmoother;
                        }

                        //Lerp the previous target position with the current one based on the AI's look at speed to allow for smooth blending between targets. 
                        if (m_TurnLerpValue <= 1 && !EmeraldComponent.DeathDelayActive)
                        {
                            EmeraldComponent.AIAnimator.SetLookAtPosition(Vector3.Lerp(PreviousTargetPos, CurrentTargetPos, m_TurnLerpValue));
                            m_LookAtInProgress = true;

                            //If the AI is killed during the lerping process, reset the lerp value to start over using the newly assigned target.
                            if (EmeraldComponent.TargetEmerald.CurrentHealth <= 0)
                            {                               
                                m_TurnLerpValue = 0;
                                LookWeightLerp2 = 0;
                            }
                        }
                        else if (m_TurnLerpValue > 1)
                        {
                            EmeraldComponent.AIAnimator.SetLookAtPosition(CurrentTargetPos);
                            m_LookAtInProgress = false;
                        }

                        //Draws a ray to the current target for Emerald AI's Debug Tools
                        if (EmeraldComponent.EnableDebugging == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.DrawRaycastsEnabled == EmeraldAISystem.YesOrNo.Yes)
                        {
                            Debug.DrawRay(new Vector3(EmeraldComponent.HeadTransform.position.x, EmeraldComponent.HeadTransform.position.y, EmeraldComponent.HeadTransform.position.z),
                              new Vector3(CurrentTargetPos.x, CurrentTargetPos.y + m_TargetAimOffsetDistance * YOffSet, CurrentTargetPos.z) - EmeraldComponent.HeadTransform.position, Color.yellow);
                        }
                    }
                    else if (EmeraldComponent.HeadLookRef != null && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion)
                    {                       
                        if (PreviousTarget != null)
                        {
                            m_LookLerpValue += Time.deltaTime;

                            Vector3 PreviousTargetPos = new Vector3(PreviousTarget.position.x, PreviousTarget.position.y +
                                PreviousTarget.localScale.y / 2 + (EmeraldComponent.HeadLookYOffset), PreviousTarget.position.z);

                            PreviousTargetPos.y = LookHeight;

                            EmeraldComponent.AIAnimator.SetLookAtPosition(PreviousTargetPos);
                        }
                        else
                        {                            
                            EmeraldComponent.AIAnimator.SetLookAtPosition(new Vector3(EmeraldComponent.HeadLookRef.position.x, EmeraldComponent.HeadLookRef.position.y +
                            EmeraldComponent.HeadLookRef.localScale.y / 2 + (EmeraldComponent.HeadLookYOffset), EmeraldComponent.HeadLookRef.position.z));
                        }
                    }
                    else if (EmeraldComponent.HeadLookRef == null && EmeraldComponent.DeathDelayActive)
                    {
                        //Subtract the offset based on the distance of the target as the offset is needed less as the target gets closer to the AI. 
                        float m_TargetAimOffsetDistance = Vector3.Distance(EmeraldComponent.CurrentTarget.position, transform.position) / EmeraldComponent.MaxLookAtDistance;
                        m_TargetAimOffsetDistance = Mathf.Clamp(m_TargetAimOffsetDistance, 0, 1);

                        //Apply the offset to the AI's hit point and do the same for using the previous target
                        Vector3 CurrentTargetPos = EmeraldComponent.TargetEmerald.HitPointTransform.position - new Vector3(0, m_TargetAimOffsetDistance * YOffSet, 0);

                        EmeraldComponent.AIAnimator.SetLookAtPosition(CurrentTargetPos);
                    }
                }
                //Player
                else
                {
                    if (EmeraldComponent.CurrentTarget != null && PreviousTarget != null && EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.Active)
                    {                       
                        //Subtract the offset based on the distance of the target as the offset is needed less as the target gets closer to the AI. 
                        float m_TargetAimOffsetDistance = Vector3.Distance(EmeraldComponent.CurrentTarget.position, transform.position) / EmeraldComponent.MaxLookAtDistance;
                        m_TargetAimOffsetDistance = Mathf.Clamp(m_TargetAimOffsetDistance, 0, 1);

                        //Subtract the offset based on the distance of the previous target as the offset is needed less as the target gets closer to the AI. 
                        float m_PreviousTargetAimOffsetDistance = Vector3.Distance(PreviousTarget.position, transform.position) / EmeraldComponent.MaxLookAtDistance;
                        m_PreviousTargetAimOffsetDistance = Mathf.Clamp(m_PreviousTargetAimOffsetDistance, 0, 1);

                        //Apply the offset to the AI's hit point and do the same for using the previous target
                        Vector3 CurrentTargetPos = new Vector3(EmeraldComponent.CurrentTarget.position.x, EmeraldComponent.CurrentTarget.position.y + EmeraldComponent.CurrentTarget.localScale.y / 2 + (EmeraldComponent.HeadLookYOffset), 
                            EmeraldComponent.CurrentTarget.position.z) - new Vector3(0, m_TargetAimOffsetDistance * YOffSet, 0);
                        Vector3 PreviousTargetPos = new Vector3(PreviousTarget.position.x, PreviousTarget.position.y - m_PreviousTargetAimOffsetDistance * YOffSet, PreviousTarget.position.z);

                        LookHeight = PreviousTargetPos.y;

                        if (!EmeraldComponent.DeathDelayActive)
                        {                            
                            m_TurnLerpValue += Time.deltaTime * EmeraldComponent.LookSmoother;
                        }

                        //Lerp the previous target position with the current one based on the AI's look at speed to allow for smooth blending between targets. 
                        if (m_TurnLerpValue <= 1 && !EmeraldComponent.DeathDelayActive)
                        {
                            EmeraldComponent.AIAnimator.SetLookAtPosition(Vector3.Lerp(PreviousTargetPos, CurrentTargetPos, m_TurnLerpValue));
                            m_LookAtInProgress = true;
                        }
                        else if (m_TurnLerpValue > 1)
                        {
                            EmeraldComponent.AIAnimator.SetLookAtPosition(CurrentTargetPos);
                            m_LookAtInProgress = false;
                        }

                        //Draws a ray to the current target for Emerald AI's Debug Tools
                        if (EmeraldComponent.EnableDebugging == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.DrawRaycastsEnabled == EmeraldAISystem.YesOrNo.Yes)
                        {
                            Debug.DrawRay(new Vector3(EmeraldComponent.HeadTransform.position.x, EmeraldComponent.HeadTransform.position.y, EmeraldComponent.HeadTransform.position.z),
                              new Vector3(CurrentTargetPos.x, CurrentTargetPos.y + m_TargetAimOffsetDistance * YOffSet, CurrentTargetPos.z) - EmeraldComponent.HeadTransform.position, Color.yellow);
                        }
                    }
                }

                if (EmeraldComponent.HeadLookRef != null && EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.NotActive)
                {
                    if (PreviousTarget != null)
                    {
                        m_TurnLerpValue += Time.deltaTime * EmeraldComponent.LookSmoother;

                        //Lerp the previous target position with the current one based on the AI's look at speed to allow for smooth blending between targets. 
                        if (m_TurnLerpValue <= 1)
                        {
                            EmeraldComponent.AIAnimator.SetLookAtPosition(Vector3.Lerp(PreviousTarget.position, EmeraldComponent.HeadLookRef.position +
                                new Vector3(0, EmeraldComponent.HeadLookRef.localScale.y / 2 + (EmeraldComponent.HeadLookYOffset), 0), m_TurnLerpValue));
                        }
                        else
                        {
                            PreviousTarget = null;
                        }
                    }
                    else
                    {
                        EmeraldComponent.AIAnimator.SetLookAtPosition(new Vector3(EmeraldComponent.HeadLookRef.position.x, EmeraldComponent.HeadLookRef.position.y +
                        EmeraldComponent.HeadLookRef.localScale.y / 2 + (EmeraldComponent.HeadLookYOffset), EmeraldComponent.HeadLookRef.position.z));
                    }
                }
            }
        }

        //Assigns a target or follower based on the passed parameter and an AI's settings
        public void DetectTarget(GameObject C)
        {
            if (EmeraldComponent.UseHeadLookRef == EmeraldAISystem.YesOrNo.Yes && C.CompareTag(EmeraldComponent.PlayerTag) && EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.NotActive)
            {
                EmeraldComponent.HeadLookRef = C.transform;              
            }

            if (EmeraldComponent.DetectionTypeRef == EmeraldAISystem.DetectionType.Trigger)
            {
                if (C.CompareTag(EmeraldComponent.PlayerTag))
                {
                    EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.Player;
                    EmeraldComponent.CurrentDetectionRef = EmeraldAISystem.CurrentDetection.Alert;

                    if (EmeraldComponent.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Enemy && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion
                        && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Passive && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
                    {
                        EmeraldComponent.EmeraldBehaviorsComponent.ActivateCombatState();

                        //Pick our target depending on the AI's options
                        if (EmeraldComponent.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.FirstDetected)
                        {
                            if (PreviousTarget == null)
                            {
                                PreviousTarget = C.transform;
                            }

                            EmeraldComponent.CurrentTarget = C.transform;
                            DetectTargetType(EmeraldComponent.CurrentTarget);
                            EmeraldComponent.OnStartCombatEvent.Invoke();
                        }
                        else if (EmeraldComponent.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.Closest)
                        {
                            SearchForTarget();
                        }
                    }
                    else if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion && !C.CompareTag(EmeraldComponent.PlayerTag))
                    {
                        EmeraldComponent.CompanionTargetRef = C.transform;
                    }
                    else if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Passive)
                    {
                        EmeraldComponent.PassiveTargetRef = C.transform;
                    }
                }
                else if (C.CompareTag(EmeraldComponent.EmeraldTag))
                {
                    if (C.GetComponent<EmeraldAISystem>() != null)
                    {
                        EmeraldComponent.ReceivedFaction = C.GetComponent<EmeraldAISystem>().CurrentFaction;
                    }
                    if (EmeraldComponent.AIFactionsList.Contains(EmeraldComponent.ReceivedFaction) && EmeraldComponent.FactionRelations[EmeraldComponent.AIFactionsList.IndexOf(EmeraldComponent.ReceivedFaction)] == 0)
                    {
                        if (C.GetComponent<EmeraldAISystem>() != null)
                        {
                            EmeraldComponent.TargetEmerald = C.GetComponent<EmeraldAISystem>();
                            EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.AI;
                        }
                        EmeraldComponent.CurrentDetectionRef = EmeraldAISystem.CurrentDetection.Alert;

                        if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Passive && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
                        {
                            EmeraldComponent.CombatStateRef = EmeraldAISystem.CombatState.Active;
                            EmeraldComponent.AIAnimator.SetBool("Idle Active", false);
                            EmeraldComponent.AIAnimator.SetBool("Combat State Active", true);
                            EmeraldComponent.CurrentMovementState = EmeraldAISystem.MovementState.Run;

                            //Pick our target depending on the AI's options
                            if (EmeraldComponent.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.FirstDetected)
                            {
                                if (PreviousTarget == null)
                                {
                                    PreviousTarget = C.transform;
                                }

                                EmeraldComponent.CurrentTarget = C.transform;
                            }
                            else if (EmeraldComponent.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.Closest)
                            {
                                SearchForTarget();
                            }
                            else if (EmeraldComponent.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.Random)
                            {
                                SearchForTarget();
                                Invoke("StartRandomTarget", 0.9f);
                                Invoke("SearchForTarget", 1);
                            }
                        }
                        else if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
                        {
                            EmeraldComponent.CompanionTargetRef = C.transform;
                        }
                        else if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Passive)
                        {
                            EmeraldComponent.PassiveTargetRef = C.transform;
                        }
                    }
                }
                else if (C.tag == EmeraldComponent.NonAITag && EmeraldComponent.UseNonAITagRef == EmeraldAISystem.UseNonAITag.Yes)
                {
                    if (C.GetComponent<EmeraldAISystem>() == null)
                    {
                        EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.NonAITarget;
                    }
                    EmeraldComponent.CurrentDetectionRef = EmeraldAISystem.CurrentDetection.Alert;

                    if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Passive && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
                    {
                        EmeraldComponent.CombatStateRef = EmeraldAISystem.CombatState.Active;
                        EmeraldComponent.AIAnimator.SetBool("Idle Active", false);
                        EmeraldComponent.AIAnimator.SetBool("Combat State Active", true);
                        EmeraldComponent.CurrentMovementState = EmeraldAISystem.MovementState.Run;

                        //Pick our target depending on the AI's options
                        if (EmeraldComponent.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.FirstDetected)
                        {
                            if (PreviousTarget == null)
                            {
                                PreviousTarget = C.transform;
                            }

                            EmeraldComponent.CurrentTarget = C.transform;
                        }
                        else if (EmeraldComponent.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.Closest)
                        {
                            SearchForTarget();
                        }
                    }
                    else if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
                    {
                        EmeraldComponent.CompanionTargetRef = C.transform;
                    }
                    else if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Passive)
                    {
                        EmeraldComponent.PassiveTargetRef = C.transform;
                    }
                }
            }
            else if (EmeraldComponent.DetectionTypeRef == EmeraldAISystem.DetectionType.LineOfSight)
            {
                if (C.CompareTag(EmeraldComponent.PlayerTag) && !EmeraldComponent.LineOfSightTargets.Contains(C.transform) && EmeraldComponent.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Enemy)
                {
                    EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.Player;
                    EmeraldComponent.CurrentDetectionRef = EmeraldAISystem.CurrentDetection.Alert;
                    EmeraldComponent.LineOfSightTargets.Add(C.transform);
                }
                else if (C.CompareTag(EmeraldComponent.EmeraldTag))
                {
                    if (C.GetComponent<EmeraldAISystem>() != null)
                    {
                        EmeraldComponent.ReceivedFaction = C.GetComponent<EmeraldAISystem>().CurrentFaction;
                    }
                    if (EmeraldComponent.AIFactionsList.Contains(EmeraldComponent.ReceivedFaction) && EmeraldComponent.FactionRelations[EmeraldComponent.AIFactionsList.IndexOf(EmeraldComponent.ReceivedFaction)] == 0 
                        && !EmeraldComponent.LineOfSightTargets.Contains(C.transform))
                    {
                        if (C.GetComponent<EmeraldAISystem>() != null)
                        {
                            EmeraldComponent.TargetEmerald = C.GetComponent<EmeraldAISystem>();
                            EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.AI;
                        }
                        EmeraldComponent.CurrentDetectionRef = EmeraldAISystem.CurrentDetection.Alert;
                        EmeraldComponent.LineOfSightTargets.Add(C.transform);
                    }
                }
                else if (C.tag == EmeraldComponent.NonAITag && EmeraldComponent.UseNonAITagRef == EmeraldAISystem.UseNonAITag.Yes && !EmeraldComponent.LineOfSightTargets.Contains(C.transform))
                {
                    if (C.GetComponent<EmeraldAISystem>() == null)
                    {
                        EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.NonAITarget;
                    }
                    EmeraldComponent.CurrentDetectionRef = EmeraldAISystem.CurrentDetection.Alert;
                    EmeraldComponent.LineOfSightTargets.Add(C.transform);
                }
            }
        }

        //Handles the avoiding of other AI by casting a raycast from the AI's head transform is. If a target of the appropriate layer is hit, 
        //alter the AI's destination for briefly to allow both AI to move past each other without colliding.
        void AIAvoidance()
        {
            RaycastHit HitForward;            
            if (EmeraldComponent.IsMoving && Physics.Raycast(EmeraldComponent.HeadTransform.position, 
                transform.forward, out HitForward, (EmeraldComponent.StoppingDistance*2+1), EmeraldComponent.AIAvoidanceLayerMask) && !AvoidanceTrigger)
            {
                if (HitForward.transform != transform)
                {
                    EmeraldComponent.TargetDestination = transform.position + HitForward.transform.right / Random.Range(-5,6) * (EmeraldComponent.StoppingDistance*2+1);
                    EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.TargetDestination;
                    AvoidanceTrigger = true;
                    AvoidanceTimer = 0;
                }
            }
            else if (AvoidanceTrigger)
            {
                if (EmeraldComponent.CurrentMovementState == EmeraldAISystem.MovementState.Walk)
                {
                    AvoidanceSeconds = 1;
                }
                else
                {
                    AvoidanceSeconds = 0.75f;
                }

                AvoidanceTimer += Time.deltaTime;
                if (AvoidanceTimer > AvoidanceSeconds && EmeraldComponent.CurrentTarget == null)
                {
                    if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Dynamic)
                    {
                        EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.NewDestination;
                    }
                    else if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Destination)
                    {
                        EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.SingleDestination;
                    }
                    else if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Stationary)
                    {
                        EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.StartingDestination;
                    }
                    if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Waypoints)
                    {
                        EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.WaypointsList[EmeraldComponent.WaypointIndex];
                    }

                    AvoidanceTimer = 0;
                    AvoidanceTrigger = false;
                }
            }
        }

        void UpdateAIUI ()
        {
            if (EmeraldComponent.CreateHealthBarsRef == EmeraldAISystem.CreateHealthBars.Yes || EmeraldComponent.DisplayAINameRef == EmeraldAISystem.DisplayAIName.Yes)
            {
                Collider[] CurrentlyDetectedTargets = Physics.OverlapSphere(transform.position, EmeraldComponent.DetectionRadius, EmeraldComponent.UILayerMask);

                foreach (Collider C in CurrentlyDetectedTargets)
                {                   
                    if (C.CompareTag(EmeraldComponent.UITag))
                    {
                        EmeraldComponent.SetUI(true);
                        return;
                    }
                    else
                    {
                        EmeraldComponent.SetUI(false);
                    }
                }

                if (CurrentlyDetectedTargets.Length == 0)
                {
                    EmeraldComponent.SetUI(false);
                }
            }
        }

        //Handles all of an AI's target detection by using a Physics.OverlapSphere (instead of a Trigger Collider). This allows users to specifiy which layers will be considered
        void UpdateAIDetection ()
        {
            //Do a separate OverlapSphere with only the UI Layer. If the detected object has the UITag, enable the UI system.
            UpdateAIUI();

            if (EmeraldComponent != null && EmeraldComponent.CurrentTarget == null && !EmeraldComponent.ReturningToStartInProgress || 
                EmeraldComponent != null && EmeraldComponent.DeathDelayActive && !EmeraldComponent.ReturningToStartInProgress)
            {
                Collider[] CurrentlyDetectedTargets = Physics.OverlapSphere(transform.position, EmeraldComponent.DetectionRadius, EmeraldComponent.DetectionLayerMask);
                m_PlayerDetected = false;

                foreach (Collider C in CurrentlyDetectedTargets)
                {
                    //Check for a companion target, if a detected object is found that is the proper tag, assign it as the active follower.
                    if (C.CompareTag(EmeraldComponent.FollowTag) && C.tag != "Untagged" && EmeraldComponent.CurrentFollowTarget == null)
                    {
                        if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion || EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet)
                        {
                            EmeraldComponent.CurrentFollowTarget = C.transform;
                            EmeraldComponent.CurrentMovementState = EmeraldAISystem.MovementState.Run;
                            EmeraldComponent.StartingMovementState = EmeraldAISystem.MovementState.Run;
                        }
                    }

                    if (EmeraldComponent.CurrentTarget == null && C.gameObject != this.gameObject)
                    {
                        if (C.CompareTag(EmeraldComponent.EmeraldTag) || C.CompareTag(EmeraldComponent.PlayerTag) && EmeraldComponent.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Enemy || 
                            C.tag == EmeraldComponent.NonAITag && EmeraldComponent.UseNonAITagRef == EmeraldAISystem.UseNonAITag.Yes)
                        {
                            if (EmeraldComponent.DetectionTypeRef == EmeraldAISystem.DetectionType.Trigger)
                            {
                                if (EmeraldComponent.DeathDelayActive)
                                {
                                    EmeraldComponent.DeathDelayActive = false;
                                }

                                DetectTarget(C.gameObject);
                            }
                            else if (EmeraldComponent.DetectionTypeRef == EmeraldAISystem.DetectionType.LineOfSight && EmeraldComponent.CurrentTarget == null)
                            {
                                DetectTarget(C.gameObject);
                            }
                        }
                    }
                    else if (C.CompareTag(EmeraldComponent.EmeraldTag) && EmeraldComponent.DeathDelayActive && EmeraldComponent.CurrentTarget != null)
                    {
                        if (EmeraldComponent.TargetEmerald != null)
                        {
                            if (EmeraldComponent.TargetEmerald.CurrentHealth <= 0)
                            {
                                EmeraldComponent.DeathDelayActive = false;
                                SearchForTarget();
                            }
                        }
                    }

                    if (EmeraldComponent.CurrentTarget == null && C.gameObject != this.gameObject)
                    {
                        if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.NotActive && C.CompareTag(EmeraldComponent.PlayerTag))
                        {
                            m_PlayerDetected = true;
                        }

                        if (m_PlayerDetected && PlayerDetection == PlayerDetectionRef.NotDetected && PlayerDetectionCooldown > 5 || 
                            m_PlayerDetected && PlayerDetection == PlayerDetectionRef.NotDetected && !m_FirstTimeDetectingPlayer)
                        {
                            EmeraldComponent.OnPlayerDetectionEvent.Invoke();
                            PlayerDetection = PlayerDetectionRef.Detected;
                            m_FirstTimeDetectingPlayer = true;
                            PlayerDetectionCooldown = 0;
                        }

                        if (EmeraldComponent.UseHeadLookRef == EmeraldAISystem.YesOrNo.Yes && C.CompareTag(EmeraldComponent.PlayerTag) &&
                            EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.NotActive && EmeraldComponent.HeadLookRef == null && EmeraldComponent.CurrentTarget == null)
                        {
                            DetectTargetType(C.transform);
                            EmeraldComponent.HeadLookRef = C.transform;
                        }
                    }
                }

                if (!m_PlayerDetected)
                {
                    PlayerDetectionCooldown += EmeraldComponent.DetectionFrequency;
                    PlayerDetection = PlayerDetectionRef.NotDetected;
                }
            }
        }

        //Calculates our AI's line of sight mechanics.
        //For each target that is within the AI's trigger radius, and they are within the AI's
        //line of sight, set the first seen target as the CurrentTarget.
        public void LineOfSightDetection ()
        {
            if (EmeraldComponent.CurrentDetectionRef == EmeraldAISystem.CurrentDetection.Alert && EmeraldComponent.CurrentTarget == null && EmeraldComponent.CurrentHealth > 0)
            {
                foreach (Transform T in EmeraldComponent.LineOfSightTargets.ToArray())
                {
                    Vector3 direction = (new Vector3(T.position.x, T.position.y + T.localScale.y / 2, T.position.z)) - EmeraldComponent.HeadTransform.position;
                    float angle = Vector3.Angle(new Vector3(direction.x, 0, direction.z), transform.forward);

                    if (EmeraldComponent.EnableDebugging == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.DrawRaycastsEnabled == EmeraldAISystem.YesOrNo.Yes)
                    {
                        Debug.DrawRay(EmeraldComponent.HeadTransform.position, direction, new Color(1, 0.549f, 0));
                    }

                    if (angle < EmeraldComponent.fieldOfViewAngle * 0.5f)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(EmeraldComponent.HeadTransform.position, direction, out hit, EmeraldComponent.DetectionRadius))
                        {
                            if (hit.collider.CompareTag(EmeraldComponent.EmeraldTag) || hit.collider.CompareTag(EmeraldComponent.PlayerTag) && EmeraldComponent.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Enemy || 
                                hit.collider.tag == EmeraldComponent.NonAITag && EmeraldComponent.UseNonAITagRef == EmeraldAISystem.UseNonAITag.Yes)
                            {
                                if (hit.collider.CompareTag(EmeraldComponent.EmeraldTag))
                                {
                                    EmeraldComponent.ReceivedFaction = hit.collider.GetComponent<EmeraldAISystem>().CurrentFaction;
                                }
                                if (EmeraldComponent.AIFactionsList.Contains(EmeraldComponent.ReceivedFaction) && EmeraldComponent.FactionRelations[EmeraldComponent.AIFactionsList.IndexOf(EmeraldComponent.ReceivedFaction)] == 0 
                                    || hit.collider.CompareTag(EmeraldComponent.PlayerTag) && EmeraldComponent.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Enemy
                                    || hit.collider.tag == EmeraldComponent.NonAITag && EmeraldComponent.UseNonAITagRef == EmeraldAISystem.UseNonAITag.Yes)
                                {
                                    if (EmeraldComponent.AIFactionsList.Contains(EmeraldComponent.ReceivedFaction))
                                    {
                                        EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.AI;
                                        EmeraldComponent.TargetEmerald = hit.collider.GetComponent<EmeraldAISystem>();
                                    }
                                    else if (hit.collider.CompareTag(EmeraldComponent.PlayerTag) && EmeraldComponent.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Enemy)
                                    {
                                        EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.Player;
                                    }
                                    else if (hit.collider.tag == EmeraldComponent.NonAITag && EmeraldComponent.UseNonAITagRef == EmeraldAISystem.UseNonAITag.Yes)
                                    {
                                        EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.NonAITarget;
                                    }

                                    SetLineOfSightTarget(hit.collider.transform);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetLineOfSightTarget (Transform LineOfSightTarget)
        {
            EmeraldComponent.LineOfSightRef = LineOfSightTarget;
            //Pick our target depending on the AI's options
            if (EmeraldComponent.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.FirstDetected || EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Cautious)
            {
                if (EmeraldComponent.DeathDelayActive)
                {
                    EmeraldComponent.DeathDelayActive = false;
                }

                EmeraldComponent.CurrentTarget = EmeraldComponent.LineOfSightRef;
                SetDetectedTarget(EmeraldComponent.CurrentTarget);
            }
            else if (EmeraldComponent.PickTargetMethodRef == EmeraldAISystem.PickTargetMethod.Closest)
            {
                SearchForTarget();
            }

            EmeraldComponent.EmeraldBehaviorsComponent.ActivateCombatState();

            if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
            {
                EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.StoppingDistance;
            }
        }

        public void CheckForObstructions ()
        {          
            if (EmeraldComponent.CurrentTarget != null && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
            {
                if (EmeraldComponent.TargetTypeRef == EmeraldAISystem.TargetType.AI && EmeraldComponent.TargetEmerald != null)
                {
                    TargetDirection = EmeraldComponent.TargetEmerald.HitPointTransform.position - EmeraldComponent.HeadTransform.position;
                }
                else
                {
                    TargetDirection = (new Vector3(EmeraldComponent.CurrentTarget.position.x, EmeraldComponent.CurrentTarget.position.y + EmeraldComponent.CurrentTarget.localScale.y / 2 + (EmeraldComponent.PlayerYOffset), EmeraldComponent.CurrentTarget.position.z)) - EmeraldComponent.HeadTransform.position;
                }

                float angle = Vector3.Angle(new Vector3(TargetDirection.x, 0, TargetDirection.z), new Vector3(transform.forward.x, 0, transform.forward.z));
                RaycastHit hit;

                if (EmeraldComponent.EnableDebugging == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.DrawRaycastsEnabled == EmeraldAISystem.YesOrNo.Yes)
                {
                    Debug.DrawRay(new Vector3(EmeraldComponent.HeadTransform.position.x, EmeraldComponent.HeadTransform.position.y, EmeraldComponent.HeadTransform.position.z), TargetDirection, C);
                }

                //Check for obstructions and incrementally lower our AI's stopping distance until one is found. If none are found when the distance has reached 5 or below, search for a new target to see if there is a better option
                if (Physics.Raycast(EmeraldComponent.HeadTransform.position, (TargetDirection), out hit, EmeraldComponent.DistanceFromTarget + 2, ~EmeraldComponent.ObstructionDetectionLayerMask))
                {                   
                    if (EmeraldComponent.CurrentTarget != null && angle > 45 && EmeraldComponent.m_NavMeshAgent.stoppingDistance > 5 && hit.collider.gameObject != this.gameObject && hit.collider.gameObject != EmeraldComponent.CurrentTarget.gameObject
                        || EmeraldComponent.CurrentTarget != null && hit.collider.gameObject != EmeraldComponent.CurrentTarget.gameObject && hit.collider.gameObject != this.gameObject && EmeraldComponent.m_NavMeshAgent.stoppingDistance > 5)
                    {
                        if (!hit.transform.IsChildOf(EmeraldComponent.CurrentTarget)) //Added
                        {
                            C = Color.red;
                            EmeraldComponent.TargetObstructed = true;

                            if (hit.collider.gameObject != CurrentObstruction)
                            {
                                CurrentObstruction = hit.collider.gameObject;
                                if (EmeraldComponent.EnableDebugging == EmeraldAISystem.YesOrNo.Yes &&
                                    EmeraldComponent.DebugLogObstructionsEnabled == EmeraldAISystem.YesOrNo.Yes && !EmeraldComponent.DeathDelayActive)
                                {
                                    Debug.Log("<b>" + "<color=green>" + gameObject.name + "'s Current Obstruction: " + "</color>" + "<color=red>" + hit.collider.gameObject.name + "</color>" + "</b>");
                                }
                            }

                            if (EmeraldComponent.m_NavMeshAgent.stoppingDistance > EmeraldComponent.StoppingDistance && !EmeraldComponent.BackingUp && !EmeraldComponent.IsTurning && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                            {
                                if (!EmeraldComponent.Attacking && hit.collider.tag != EmeraldComponent.EmeraldTag && hit.collider.tag != EmeraldComponent.PlayerTag)
                                {
                                    if (EmeraldComponent.TargetObstructedActionRef == EmeraldAISystem.TargetObstructedAction.MoveCloserImmediately)
                                    {
                                        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                                        EmeraldComponent.m_NavMeshAgent.CalculatePath(EmeraldComponent.CurrentTarget.position, path);

                                        if (path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
                                        {
                                            EmeraldComponent.m_NavMeshAgent.stoppingDistance -= 5;
                                        }
                                        else if (path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial && EmeraldComponent.TargetObstructed)
                                        {
                                            EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.StoppingDistance;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (EmeraldComponent.CurrentTarget != null && hit.collider.gameObject == EmeraldComponent.CurrentTarget.gameObject && !EmeraldComponent.IsTurning ||
                        EmeraldComponent.CurrentTarget != null && hit.transform.IsChildOf(EmeraldComponent.CurrentTarget) && !EmeraldComponent.IsTurning)
                    {
                        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                        EmeraldComponent.m_NavMeshAgent.CalculatePath(EmeraldComponent.CurrentTarget.position, path);

                        C = Color.green;
                        EmeraldComponent.TargetObstructed = false;
                        EmeraldComponent.CurrentMovementState = EmeraldAISystem.MovementState.Run;
                        if (!EmeraldComponent.BackingUp && path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete)
                        {
                            EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.AttackDistance;
                        }
                        EmeraldComponent.ObstructionTimer = 0;
                    }
                }
                else
                {
                    C = Color.red;
                    EmeraldComponent.TargetObstructed = true;
                }
            }
            else if (EmeraldComponent.CurrentTarget != null && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
            {
                if (EmeraldComponent.TargetTypeRef == EmeraldAISystem.TargetType.AI && EmeraldComponent.TargetEmerald != null)
                {
                    TargetDirection = EmeraldComponent.TargetEmerald.HitPointTransform.position - EmeraldComponent.HeadTransform.position;
                }
                else
                {
                    TargetDirection = (new Vector3(EmeraldComponent.CurrentTarget.position.x, EmeraldComponent.CurrentTarget.position.y + EmeraldComponent.CurrentTarget.localScale.y / 2 + (EmeraldComponent.PlayerYOffset), EmeraldComponent.CurrentTarget.position.z)) - EmeraldComponent.HeadTransform.position;
                }

                RaycastHit hit;

                if (EmeraldComponent.EnableDebugging == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.DrawRaycastsEnabled == EmeraldAISystem.YesOrNo.Yes)
                {
                    Debug.DrawRay(new Vector3(EmeraldComponent.HeadTransform.position.x, EmeraldComponent.HeadTransform.position.y, EmeraldComponent.HeadTransform.position.z), TargetDirection, C);
                }

                //Check for obstructions and incrementally lower our AI's stopping distance until one is found. If none are found when the distance has reached 5 or below, search for a new target to see if there is a better option
                if (Physics.Raycast(EmeraldComponent.HeadTransform.position, (TargetDirection), out hit, EmeraldComponent.DistanceFromTarget + 2, ~EmeraldComponent.ObstructionDetectionLayerMask))
                {
                    if (!hit.transform.IsChildOf(EmeraldComponent.CurrentTarget)) // || hit.collider.gameObject != this.gameObject && hit.collider.transform != EmeraldComponent.CurrentTarget
                    {
                        C = Color.red;
                        EmeraldComponent.TargetObstructed = true;

                        if (hit.collider.gameObject != CurrentObstruction)
                        {
                            CurrentObstruction = hit.collider.gameObject;
                            if (EmeraldComponent.EnableDebugging == EmeraldAISystem.YesOrNo.Yes &&
                                EmeraldComponent.DebugLogObstructionsEnabled == EmeraldAISystem.YesOrNo.Yes && !EmeraldComponent.DeathDelayActive)
                            {
                                Debug.Log("<b>" + "<color=green>" + gameObject.name + "'s Current Obstruction: " + "</color>" + "<color=red>" + hit.collider.gameObject.name + "</color>" + "</b>");
                            }
                        }
                    }
                    else
                    {
                        C = Color.green;
                        EmeraldComponent.TargetObstructed = false;
                    }
                }
                else
                {
                    C = Color.green;
                    EmeraldComponent.TargetObstructed = false;
                }
            }
        }

        //Find colliders within range using a Physics.OverlapSphere. Mask the Physics.OverlapSphere to the user set layers. 
        //This will allow the Physics.OverlapSphere to only get relevent colliders.
        //Once found, use Emerald's custom tag system to find matches for potential targets. Once found, apply them to a list for potential targets.
        //Finally, search through each target in the list and set the nearest one as our current target.
        public void SearchForTarget ()
        {        
            Collider[] Col = Physics.OverlapSphere(transform.position, EmeraldComponent.DetectionRadius, EmeraldComponent.DetectionLayerMask);
            EmeraldComponent.CollidersInArea = Col;

            EmeraldComponent.potentialTargets.Clear();

            foreach (Collider C in EmeraldComponent.CollidersInArea)
            {
                if (C.gameObject != this.gameObject && !EmeraldComponent.potentialTargets.Contains(C.gameObject))
                {                  
                    if (C.gameObject.GetComponent<EmeraldAISystem>() != null)
                    {
                        EmeraldAISystem EmeraldRef = C.gameObject.GetComponent<EmeraldAISystem>();
                        if (EmeraldComponent.AIFactionsList.Contains(EmeraldRef.CurrentFaction) && 
                            EmeraldComponent.FactionRelations[EmeraldComponent.AIFactionsList.IndexOf(EmeraldRef.CurrentFaction)] == 0 && 
                            EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion)
                        {
                            EmeraldComponent.potentialTargets.Add(C.gameObject);
                        }
                        else if (EmeraldComponent.AIFactionsList.Contains(EmeraldRef.CurrentFaction) && 
                            EmeraldComponent.FactionRelations[EmeraldComponent.AIFactionsList.IndexOf(EmeraldRef.CurrentFaction)] == 0 && 
                            EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion && EmeraldRef.CombatStateRef == EmeraldAISystem.CombatState.Active)
                        {
                            EmeraldComponent.potentialTargets.Add(C.gameObject);
                        }
                        else if (EmeraldComponent.AIFactionsList.Contains(EmeraldRef.CurrentFaction) && 
                            EmeraldComponent.FactionRelations[EmeraldComponent.AIFactionsList.IndexOf(EmeraldRef.CurrentFaction)] == 0 && 
                            EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion && EmeraldRef.CombatStateRef == EmeraldAISystem.CombatState.NotActive)
                        {
                            EmeraldComponent.CompanionTargetRef = C.transform;
                            EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.AI;
                            EmeraldComponent.TargetEmerald = C.gameObject.GetComponent<EmeraldAISystem>();
                        }
                    }
                    else if (C.gameObject.CompareTag(EmeraldComponent.PlayerTag) && EmeraldComponent.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Enemy)
                    {
                        if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion || EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion && C.gameObject.transform != EmeraldComponent.CurrentFollowTarget)
                        {
                            EmeraldComponent.potentialTargets.Add(C.gameObject);
                        }
                    }
                    else if (C.gameObject.tag == EmeraldComponent.NonAITag && EmeraldComponent.UseNonAITagRef == EmeraldAISystem.UseNonAITag.Yes)
                    {
                        EmeraldComponent.potentialTargets.Add(C.gameObject);
                    }
                }
            }

            //Search for a random target (Only usable through the Aggro feature)
            if (SearchForRandomTarget && EmeraldComponent.potentialTargets.Count > 0 && EmeraldComponent.m_NavMeshAgent.enabled)
            {
                EmeraldComponent.CurrentTarget = EmeraldComponent.potentialTargets[Random.Range(0, EmeraldComponent.potentialTargets.Count)].transform;
                EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.CurrentTarget.position;

                if (EmeraldComponent.AIFactionsList.Contains(EmeraldComponent.ReceivedFaction) && EmeraldComponent.FactionRelations[EmeraldComponent.AIFactionsList.IndexOf(EmeraldComponent.ReceivedFaction)] == 0)
                {
                    EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.AI;
                    EmeraldComponent.TargetEmerald = EmeraldComponent.CurrentTarget.GetComponent<EmeraldAISystem>();
                }
                else if (EmeraldComponent.CurrentTarget.tag == EmeraldComponent.PlayerTag && EmeraldComponent.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Enemy)
                {
                    EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.Player;
                    EmeraldComponent.TargetEmerald = null;
                }
                else if (EmeraldComponent.CurrentTarget.tag == EmeraldComponent.NonAITag && EmeraldComponent.UseNonAITagRef == EmeraldAISystem.UseNonAITag.Yes)
                {
                    EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.NonAITarget;
                    EmeraldComponent.TargetEmerald = null;
                }

                SearchForRandomTarget = false;
                return;
            }

            //No targets were found within the AI's trigger radius. Set AI back to its default state.
            if (EmeraldComponent.potentialTargets.Count == 0)
            {
                EmeraldComponent.BackingUp = false;
                EmeraldComponent.AIAnimator.SetBool("Walk Backwards", false);
                if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
                {
                    EmeraldComponent.CompanionTargetRef = null;                 

                    if (EmeraldComponent.CurrentFollowTarget != null)
                    {
                        EmeraldComponent.m_NavMeshAgent.ResetPath();                    
                    }
                }

                EmeraldComponent.DeathDelay = Random.Range(EmeraldComponent.DeathDelayMin, EmeraldComponent.DeathDelayMax + 1);
                EmeraldComponent.m_NavMeshAgent.SetDestination(this.transform.position);
                if (!EmeraldComponent.ReturningToStartInProgress)
                    EmeraldComponent.DeathDelayActive = true;
                m_LookLerpValue = 0;
            }

            float previousDistance = Mathf.Infinity;
            float currentDistance;
            Transform TempTarget = null;

            foreach (GameObject target in EmeraldComponent.potentialTargets.ToArray())
            {
                if (!SearchForRandomTarget && target != null)
                {
                    if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion)
                    {
                        EmeraldComponent.distanceBetween = target.transform.position - transform.position;
                    }
                    else if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion && EmeraldComponent.CurrentFollowTarget != null)
                    {
                        EmeraldComponent.distanceBetween = target.transform.position - EmeraldComponent.CurrentFollowTarget.position;
                    }
                    currentDistance = EmeraldComponent.distanceBetween.sqrMagnitude;

                    if (currentDistance < previousDistance)
                    {
                        TempTarget = target.transform;
                        previousDistance = currentDistance;                        
                    }
                }
            }

            if (PreviousTarget == null)
            {
                PreviousTarget = TempTarget;
            }
            else
            {
                PreviousTarget = EmeraldComponent.CurrentTarget;
            }

            if (TempTarget != null && TempTarget != EmeraldComponent.CurrentTarget)
            {               
                SetDetectedTarget(TempTarget);
            }
        }

        /// <summary>
        /// Internal use only - For assigning targets directly, use EmeraldAIEventsManager.SetCombatTarget(Transform Target)
        /// </summary>
        public void SetDetectedTarget (Transform DetectedTarget)
        {
            EmeraldComponent.AIAnimator.SetBool("Turn Left", false);
            EmeraldComponent.AIAnimator.SetBool("Turn Right", false);
            EmeraldComponent.CurrentTarget = DetectedTarget;

            //If our combat state is not active, activate it.
            if (EmeraldComponent.CombatStateRef == EmeraldAISystem.CombatState.NotActive)
            {
                EmeraldComponent.EmeraldBehaviorsComponent.ActivateCombatState();
            }

            if (EmeraldComponent.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward)
            {
                EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.AttackDistance;
                EmeraldComponent.OnStartCombatEvent.Invoke();
            }

            EmeraldComponent.potentialTargets.Clear();

            //Once a target has been found, reduce the Detection Radius back to the defaul value.
            EmeraldComponent.DetectionRadius = EmeraldComponent.StartingDetectionRadius;
            EmeraldComponent.MaxChaseDistance = EmeraldComponent.StartingChaseDistance;
            EmeraldComponent.fieldOfViewAngle = EmeraldComponent.fieldOfViewAngleRef;

            if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Aggressive)
            {
                if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.CurrentTarget.position;
                }
            }

            SearchForRandomTarget = false;
            DetectTargetType(EmeraldComponent.CurrentTarget);

            if (EmeraldComponent.EnableDebugging == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.DebugLogTargetsEnabled == EmeraldAISystem.YesOrNo.Yes && !EmeraldComponent.DeathDelayActive)
            {
                if (EmeraldComponent.CurrentTarget != null)
                {
                    Debug.Log("<b>" + "<color=green>" + gameObject.name + "'s Current Target: " + "</color>" + "<color=red>" + EmeraldComponent.CurrentTarget.gameObject.name + "</color>" + "</b>" + "  |" +
                        "<b>" + "<color=green>" + "  Target Type: " + "</color>" + "<color=red>" + EmeraldComponent.TargetTypeRef + "</color>" + "</b>");
                }
            }
        }

        /// <summary>
        /// Used for detecting and assigning the AI's target type.
        /// </summary>
        public void DetectTargetType (Transform Target, bool? OverrideFactionRequirement = false)
        {
            if (Target != null)
            {
                m_TurnLerpValue = 0;
                LookWeightLerp2 = 0;
                EmeraldComponent.FirstTimeInCombat = false;
                EmeraldComponent.IsTurning = false;

                if (Target.tag == EmeraldComponent.EmeraldTag)
                {
                    EmeraldComponent.ReceivedFaction = Target.GetComponent<EmeraldAISystem>().CurrentFaction;
                }

                if (Target.tag == EmeraldComponent.EmeraldTag && EmeraldComponent.AIFactionsList.Contains(EmeraldComponent.ReceivedFaction) &&
                    EmeraldComponent.FactionRelations[EmeraldComponent.AIFactionsList.IndexOf(EmeraldComponent.ReceivedFaction)] == 0 || 
                    Target.tag == EmeraldComponent.EmeraldTag && (bool)OverrideFactionRequirement)
                {
                    EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.AI;
                    EmeraldComponent.TargetEmerald = Target.GetComponent<EmeraldAISystem>();
                }
                else if (Target.tag == EmeraldComponent.PlayerTag && EmeraldComponent.PlayerFaction[0].RelationTypeRef == EmeraldAISystem.PlayerFactionClass.RelationType.Enemy)
                {
                    EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.Player;
                    EmeraldComponent.TargetEmerald = null;
                }
                else if (Target.tag == EmeraldComponent.NonAITag && EmeraldComponent.UseNonAITagRef == EmeraldAISystem.UseNonAITag.Yes)
                {
                    EmeraldComponent.TargetTypeRef = EmeraldAISystem.TargetType.NonAITarget;
                    EmeraldComponent.TargetEmerald = null;
                }
            }
        }

        public void StartRandomTarget ()
        {
            SearchForRandomTarget = true;
        }
    }
}
