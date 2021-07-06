using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace EmeraldAI.Utility
{
    public class EmeraldAIInitializer : MonoBehaviour
    {
        EmeraldAISystem EmeraldComponent;

        public void Initialize()
        {
            SetupEmeraldAISettings();
            SetupEmeraldAIObjectPool();
            SetupNavMeshAgent();
            SetupAdditionalComponents();
            SetupOptimizationSettings();
            SetupHealthBar();
            SetupCombatText();
            SetupAnimator();
            if (GetComponent<LocationBasedDamage>() == null)
                DisableRagdoll();
            else if (GetComponent<LocationBasedDamage>() != null)
                GetComponent<LocationBasedDamage>().InitializeLocationBasedDamage();
            SetupAudio();
            if (EmeraldComponent.DebugLogMissingAnimations == EmeraldAISystem.EnableDisable.Enabled)
            {
                CheckAnimationEvents();
                CheckForMissingAnimations();
            }
            InitializeWeaponTypeAnimation();
        }

        void Start()
        {
            //Invoke our AI's On Start Event
            EmeraldComponent.OnStartEvent.Invoke();
        }

        void OnEnable()
        {
            //When the AI is enabled, and it has been killed, reset the AI to its default settings. 
            //This is intended for being used with Object Pooling or spawning systems such as Crux.
            if (EmeraldComponent != null && EmeraldComponent.IsDead)
            {
                EmeraldComponent.EmeraldEventsManagerComponent.ResetAI();
            }
        }

        public void AlignOnStart()
        {
            RaycastHit HitDown;
            if (Physics.Raycast(new Vector3(transform.localPosition.x, transform.localPosition.y + 0.25f, transform.localPosition.z), -transform.up, out HitDown, 2, EmeraldComponent.AlignmentLayerMask))
            {
                if (HitDown.transform != this.transform)
                {
                    Vector3 Normal = HitDown.normal;
                    Normal.x = Mathf.Clamp(Normal.x, -EmeraldComponent.MaxNormalAngle, EmeraldComponent.MaxNormalAngle);
                    Normal.z = Mathf.Clamp(Normal.z, -EmeraldComponent.MaxNormalAngle, EmeraldComponent.MaxNormalAngle);

                    transform.rotation = Quaternion.FromToRotation(transform.up, Normal) * transform.rotation;
                }
            }
        }

        public void DisableRagdoll()
        {
            foreach (Rigidbody R in transform.GetComponentsInChildren<Rigidbody>())
            {
                R.isKinematic = true;
            }

            foreach (Collider C in transform.GetComponentsInChildren<Collider>())
            {
                C.enabled = false;
            }

            GetComponent<BoxCollider>().enabled = true;
        }

        public void EnableRagdoll()
        {
            EmeraldComponent.AIBoxCollider.enabled = false;

            foreach (Collider C in transform.GetComponentsInChildren<Collider>())
            {
                if (C.transform != this.transform)
                {
                    C.tag = EmeraldComponent.RagdollTag;
                    C.enabled = true;
                }
            }

            foreach (Rigidbody R in transform.GetComponentsInChildren<Rigidbody>())
            {
                EmeraldComponent.AIAnimator.enabled = false;
                R.isKinematic = false;
            }

            if (EmeraldComponent.UseDroppableWeapon == EmeraldAISystem.YesOrNo.Yes)
            {
                EmeraldComponent.EmeraldEventsManagerComponent.CreateDroppableWeapon();
            }
        }

        void SetupEmeraldAIObjectPool ()
        {
            if (EmeraldAISystem.ObjectPool == null)
            {
                EmeraldAISystem.ObjectPool = new GameObject();
                EmeraldAISystem.ObjectPool.name = "Emerald Object Pool";
            }
        }

        void SetupAudio()
        {
            EmeraldComponent.m_AudioSource = GetComponent<AudioSource>();

            EmeraldComponent.m_SecondaryAudioSource = gameObject.AddComponent<AudioSource>();
            EmeraldComponent.m_SecondaryAudioSource.spatialBlend = EmeraldComponent.m_AudioSource.spatialBlend;
            EmeraldComponent.m_SecondaryAudioSource.minDistance = EmeraldComponent.m_AudioSource.minDistance;
            EmeraldComponent.m_SecondaryAudioSource.maxDistance = EmeraldComponent.m_AudioSource.maxDistance;
            EmeraldComponent.m_SecondaryAudioSource.rolloffMode = EmeraldComponent.m_AudioSource.rolloffMode;

            EmeraldComponent.m_EventAudioSource = gameObject.AddComponent<AudioSource>();
            EmeraldComponent.m_EventAudioSource.spatialBlend = EmeraldComponent.m_AudioSource.spatialBlend;
            EmeraldComponent.m_EventAudioSource.minDistance = EmeraldComponent.m_AudioSource.minDistance;
            EmeraldComponent.m_EventAudioSource.maxDistance = EmeraldComponent.m_AudioSource.maxDistance;
            EmeraldComponent.m_EventAudioSource.rolloffMode = EmeraldComponent.m_AudioSource.rolloffMode;
        }

        void SetupOptimizationSettings ()
        {
            if (EmeraldComponent.DisableAIWhenNotInViewRef == EmeraldAISystem.YesOrNo.Yes)
            {
                if (EmeraldComponent.DisableAIWhenNotInViewRef == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.HasMultipleLODsRef == EmeraldAISystem.YesOrNo.No)
                {
                    if (EmeraldComponent.AIRenderer != null && EmeraldComponent.UseDeactivateDelayRef == EmeraldAISystem.YesOrNo.No &&
                        EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
                    {
                        EmeraldComponent.AIRenderer.gameObject.AddComponent<VisibilityCheck>();
                        GetComponentInChildren<VisibilityCheck>().EmeraldComponent = GetComponentInChildren<EmeraldAISystem>();
                    }
                    else if (EmeraldComponent.AIRenderer != null && EmeraldComponent.UseDeactivateDelayRef == EmeraldAISystem.YesOrNo.Yes &&
                        EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
                    {
                        EmeraldComponent.AIRenderer.gameObject.AddComponent<VisibilityCheckDelay>();
                        GetComponentInChildren<VisibilityCheckDelay>().EmeraldComponent = GetComponentInChildren<EmeraldAISystem>();
                    }
                    else if (EmeraldComponent.HasMultipleLODsRef == EmeraldAISystem.YesOrNo.No && EmeraldComponent.AIRenderer == null)
                    {
                        EmeraldComponent.DisableAIWhenNotInViewRef = EmeraldAISystem.YesOrNo.No;
                    }
                }

                if (EmeraldComponent.HasMultipleLODsRef == EmeraldAISystem.YesOrNo.Yes)
                {
                    if (EmeraldComponent.TotalLODsRef == EmeraldAISystem.TotalLODsEnum.Two)
                    {
                        if (EmeraldComponent.Renderer1 == null || EmeraldComponent.Renderer2 == null)
                        {
                            EmeraldComponent.DisableAIWhenNotInViewRef = EmeraldAISystem.YesOrNo.No;
                            EmeraldComponent.HasMultipleLODsRef = EmeraldAISystem.YesOrNo.No;
                        }
                    }
                    else if (EmeraldComponent.TotalLODsRef == EmeraldAISystem.TotalLODsEnum.Three)
                    {
                        if (EmeraldComponent.Renderer1 == null || EmeraldComponent.Renderer2 == null || EmeraldComponent.Renderer3 == null)
                        {
                            EmeraldComponent.DisableAIWhenNotInViewRef = EmeraldAISystem.YesOrNo.No;
                            EmeraldComponent.HasMultipleLODsRef = EmeraldAISystem.YesOrNo.No;
                        }
                    }
                    else if (EmeraldComponent.TotalLODsRef == EmeraldAISystem.TotalLODsEnum.Four)
                    {
                        if (EmeraldComponent.Renderer1 == null || EmeraldComponent.Renderer2 == null ||
                            EmeraldComponent.Renderer3 == null || EmeraldComponent.Renderer4 == null)
                        {
                            EmeraldComponent.DisableAIWhenNotInViewRef = EmeraldAISystem.YesOrNo.No;
                            EmeraldComponent.HasMultipleLODsRef = EmeraldAISystem.YesOrNo.No;
                        }
                    }
                }
            }
            else if (EmeraldComponent.DisableAIWhenNotInViewRef == EmeraldAISystem.YesOrNo.No)
            {
                EmeraldComponent.OptimizedStateRef = EmeraldAISystem.OptimizedState.Inactive;
            }

            if (EmeraldComponent.AlignmentQualityRef == EmeraldAISystem.AlignmentQuality.Low)
            {
                EmeraldComponent.RayCastUpdateSeconds = 0.3f;
            }
            else if (EmeraldComponent.AlignmentQualityRef == EmeraldAISystem.AlignmentQuality.Medium)
            {
                EmeraldComponent.RayCastUpdateSeconds = 0.2f;
            }
            else if (EmeraldComponent.AlignmentQualityRef == EmeraldAISystem.AlignmentQuality.High)
            {
                EmeraldComponent.RayCastUpdateSeconds = 0.1f;
            }

            if (EmeraldComponent.ObstructionDetectionQualityRef == EmeraldAISystem.ObstructionDetectionQuality.Low)
            {
                EmeraldComponent.ObstructionDetectionUpdateSeconds = 0.6f;
            }
            else if (EmeraldComponent.ObstructionDetectionQualityRef == EmeraldAISystem.ObstructionDetectionQuality.Medium)
            {
                EmeraldComponent.ObstructionDetectionUpdateSeconds = 0.3f;
            }
            else if (EmeraldComponent.ObstructionDetectionQualityRef == EmeraldAISystem.ObstructionDetectionQuality.High)
            {
                EmeraldComponent.ObstructionDetectionUpdateSeconds = 0.1f;
            }

            if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion || EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet)
            {
                EmeraldComponent.OptimizedStateRef = EmeraldAISystem.OptimizedState.Inactive;
            }
        }

        void SetupFactions ()
        {
            for (int i = 0; i < EmeraldComponent.FactionRelationsList.Count; i++)
            {
                EmeraldComponent.AIFactionsList.Add(EmeraldComponent.FactionRelationsList[i].FactionIndex);
                EmeraldComponent.FactionRelations.Add((int)EmeraldComponent.FactionRelationsList[i].RelationTypeRef);
            }
        }

        void SetupAdditionalComponents ()
        {
            EmeraldComponent.EmeraldDetectionComponent = GetComponent<EmeraldAIDetection>();
            EmeraldComponent.EmeraldEventsManagerComponent = GetComponent<EmeraldAIEventsManager>();
            EmeraldComponent.EmeraldBehaviorsComponent = GetComponent<EmeraldAIBehaviors>();
        }

        void SetupNavMeshAgent ()
        {
            if (EmeraldComponent.m_NavMeshAgent == null)
            {
                gameObject.AddComponent<NavMeshAgent>();
                EmeraldComponent.m_NavMeshAgent = GetComponent<NavMeshAgent>();
            }

            EmeraldComponent.AIPath = new NavMeshPath();
            EmeraldComponent.m_NavMeshAgent.CalculatePath(transform.position, EmeraldComponent.AIPath);

            if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
            {
                EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.StoppingDistance;
            }
            else if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion || EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet)
            {               
                if (EmeraldComponent.CurrentTarget == null)
                {
                    EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.FollowingStoppingDistance;
                }
            }

            EmeraldComponent.m_NavMeshAgent.radius = EmeraldComponent.AgentRadius;
            EmeraldComponent.m_NavMeshAgent.baseOffset = EmeraldComponent.AgentBaseOffset;
            EmeraldComponent.m_NavMeshAgent.angularSpeed = EmeraldComponent.AgentTurnSpeed;
            EmeraldComponent.m_NavMeshAgent.acceleration = EmeraldComponent.AgentAcceleration;
            EmeraldComponent.m_NavMeshAgent.updateUpAxis = false;

            if (EmeraldComponent.AvoidanceQualityRef == EmeraldAISystem.AvoidanceQuality.None)
            {
                EmeraldComponent.m_NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            }
            else if (EmeraldComponent.AvoidanceQualityRef == EmeraldAISystem.AvoidanceQuality.Low)
            {
                EmeraldComponent.m_NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
            }
            else if (EmeraldComponent.AvoidanceQualityRef == EmeraldAISystem.AvoidanceQuality.Medium)
            {
                EmeraldComponent.m_NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
            }
            else if (EmeraldComponent.AvoidanceQualityRef == EmeraldAISystem.AvoidanceQuality.Good)
            {
                EmeraldComponent.m_NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
            }
            else if (EmeraldComponent.AvoidanceQualityRef == EmeraldAISystem.AvoidanceQuality.High)
            {
                EmeraldComponent.m_NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            }

            if (EmeraldComponent.m_NavMeshAgent.enabled)
            {
                if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion || EmeraldComponent.CurrentFollowTarget == null)
                {
                    if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Destination)
                    {
                        EmeraldComponent.m_NavMeshAgent.autoBraking = false;
                        EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.SingleDestination;
                        EmeraldComponent.CheckPath(EmeraldComponent.SingleDestination);
                    }
                    else if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Waypoints)
                    {
                        if (EmeraldComponent.WaypointTypeRef != EmeraldAISystem.WaypointType.Random)
                        {
                            if (EmeraldComponent.WaypointsList.Count > 0)
                            {
                                EmeraldComponent.m_NavMeshAgent.stoppingDistance = 0.1f;
                                EmeraldComponent.m_NavMeshAgent.autoBraking = false;
                                EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.WaypointsList[EmeraldComponent.WaypointIndex];
                            }
                        }
                        else if (EmeraldComponent.WaypointTypeRef == EmeraldAISystem.WaypointType.Random)
                        {
                            if (EmeraldComponent.WaypointsList.Count > 0)
                            {
                                EmeraldComponent.WaypointIndex = Random.Range(0, EmeraldComponent.WaypointsList.Count);
                                EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.WaypointsList[EmeraldComponent.WaypointIndex];
                                EmeraldComponent.m_NavMeshAgent.autoBraking = false;
                            }
                        }

                        if (EmeraldComponent.WaypointsList.Count == 0)
                        {
                            EmeraldComponent.WanderTypeRef = EmeraldAISystem.WanderType.Stationary;
                            EmeraldComponent.m_NavMeshAgent.stoppingDistance = EmeraldComponent.StoppingDistance;
                        }
                    }
                    else if (EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Stationary || EmeraldComponent.WanderTypeRef == EmeraldAISystem.WanderType.Dynamic)
                    {
                        EmeraldComponent.m_NavMeshAgent.autoBraking = false;
                        EmeraldComponent.m_NavMeshAgent.destination = EmeraldComponent.StartingDestination;
                    }
                }
            }

            if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion || EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet)
            {
                if (EmeraldComponent.CurrentFollowTarget != null)
                {
                    EmeraldComponent.CurrentMovementState = EmeraldAISystem.MovementState.Run;
                    EmeraldComponent.StartingMovementState = EmeraldAISystem.MovementState.Run;
                }
                EmeraldComponent.UseAIAvoidance = EmeraldAISystem.YesOrNo.No;
            }
        }

        void SetupAnimator ()
        {
            EmeraldComponent.AIAnimator = GetComponent<Animator>();

            if (EmeraldComponent.AIAnimator.layerCount >= 2)
                EmeraldComponent.AIAnimator.SetLayerWeight(1, 1);

            EmeraldComponent.StartingLookAtPosition = transform.position + transform.forward;

            SetupFactions();

            if (EmeraldComponent.ReverseWalkAnimation && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
            {
                EmeraldComponent.AIAnimator.SetFloat("Backup Speed", -1f);
            }
            else if (!EmeraldComponent.ReverseWalkAnimation && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
            {
                EmeraldComponent.AIAnimator.SetFloat("Backup Speed", 1);
            }

            if (EmeraldComponent.ReverseRangedWalkAnimation && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
            {
                EmeraldComponent.AIAnimator.SetFloat("Backup Speed", -1f);
            }
            else if (!EmeraldComponent.ReverseRangedWalkAnimation && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
            {
                EmeraldComponent.AIAnimator.SetFloat("Backup Speed", 1);
            }

            if (EmeraldComponent.UseEquipAnimation == EmeraldAISystem.YesOrNo.Yes)
            {
                EmeraldComponent.AIAnimator.SetBool("Animate Weapon State", true);
            }
            else if (EmeraldComponent.UseEquipAnimation == EmeraldAISystem.YesOrNo.No)
            {
                EmeraldComponent.AIAnimator.SetBool("Animate Weapon State", false);
            }

            if (EmeraldComponent.UseHitAnimations == EmeraldAISystem.YesOrNo.Yes)
            {
                EmeraldComponent.AIAnimator.SetBool("Use Hit", true);
            }
            else
            {
                EmeraldComponent.AIAnimator.SetBool("Use Hit", false);
            }

            if (EmeraldComponent.AnimatorType == EmeraldAISystem.AnimatorTypeState.RootMotion)
            {
                EmeraldComponent.m_NavMeshAgent.speed = 0;
                EmeraldComponent.AIAnimator.applyRootMotion = true;

                if (EmeraldComponent.AutoEnableAnimatePhysics == EmeraldAISystem.YesOrNo.Yes)
                {
                    EmeraldComponent.AIAnimator.updateMode = AnimatorUpdateMode.AnimatePhysics;
                }
            }
            else
            {
                EmeraldComponent.AIAnimator.applyRootMotion = false;
            }

            if (EmeraldComponent.AIAnimator.layerCount >= 2)
            {
                EmeraldComponent.AIAnimator.SetLayerWeight(1, 1);
            }

            if (EmeraldComponent.UseEquipAnimation == EmeraldAISystem.YesOrNo.Yes)
            {
                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes || 
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    if (EmeraldComponent.PullOutWeaponAnimation == null || EmeraldComponent.PutAwayWeaponAnimation == null)
                    {
                        EmeraldComponent.AIAnimator.SetBool("Animate Weapon State", false);
                    }
                }

                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                {
                    if (EmeraldComponent.RangedPullOutWeaponAnimation == null || EmeraldComponent.RangedPutAwayWeaponAnimation == null)
                    {
                        EmeraldComponent.AIAnimator.SetBool("Animate Weapon State", false);
                    }
                }
            }

            EmeraldComponent.AIAnimator.SetInteger("Idle Index", Random.Range(1, EmeraldComponent.TotalIdleAnimations + 1));
        }

        void SetupHealthBar ()
        {
            if (EmeraldComponent.CreateHealthBarsRef == EmeraldAISystem.CreateHealthBars.Yes && EmeraldComponent.HealthBarCanvas == null || EmeraldComponent.DisplayAINameRef == EmeraldAISystem.DisplayAIName.Yes && EmeraldComponent.HealthBarCanvas == null)
            {
                EmeraldComponent.HealthBarCanvas = Resources.Load("AI Health Bar Canvas") as GameObject;
            }

            if (EmeraldComponent.CreateHealthBarsRef == EmeraldAISystem.CreateHealthBars.Yes && EmeraldComponent.HealthBarCanvas != null || EmeraldComponent.DisplayAINameRef == EmeraldAISystem.DisplayAIName.Yes && EmeraldComponent.HealthBarCanvas != null)
            {
                EmeraldComponent.HealthBar = Instantiate(EmeraldComponent.HealthBarCanvas, Vector3.zero, Quaternion.identity) as GameObject;
                GameObject HealthBarParent = new GameObject();
                HealthBarParent.name = "HealthBarParent";
                HealthBarParent.transform.SetParent(this.transform);
                HealthBarParent.transform.localPosition = new Vector3(0, 0, 0);

                EmeraldComponent.HealthBar.transform.SetParent(HealthBarParent.transform);
                EmeraldComponent.HealthBar.transform.localPosition = EmeraldComponent.HealthBarPos;
                EmeraldComponent.HealthBar.AddComponent<EmeraldAIHealthBar>();
                EmeraldAIHealthBar HealthBarScript = EmeraldComponent.HealthBar.GetComponent<EmeraldAIHealthBar>();
                EmeraldComponent.m_HealthBarComponent = HealthBarScript;
                HealthBarScript.canvas = EmeraldComponent.HealthBar.GetComponent<Canvas>();
                HealthBarScript.EmeraldComponent = GetComponent<EmeraldAISystem>();
                EmeraldComponent.HealthBar.name = "AI Health Bar Canvas";

                GameObject HealthBarChild = EmeraldComponent.HealthBar.transform.Find("AI Health Bar Background").gameObject;
                HealthBarChild.transform.localScale = EmeraldComponent.HealthBarScale;

                Image HealthBarRef = HealthBarChild.transform.Find("AI Health Bar").GetComponent<Image>();
                HealthBarRef.color = EmeraldComponent.HealthBarColor;

                Image HealthBarBackgroundImageRef = HealthBarChild.GetComponent<Image>();
                HealthBarBackgroundImageRef.color = EmeraldComponent.HealthBarBackgroundColor;

                EmeraldComponent.HealthBarCanvasRef = EmeraldComponent.HealthBar.GetComponent<Canvas>();

                if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet || EmeraldComponent.CreateHealthBarsRef == EmeraldAISystem.CreateHealthBars.No)
                {
                    HealthBarChild.GetComponent<Image>().enabled = false;
                    HealthBarRef.gameObject.SetActive(false);
                }

                if (EmeraldComponent.CustomizeHealthBarRef == EmeraldAISystem.CustomizeHealthBar.Yes && EmeraldComponent.HealthBarBackgroundImage != null && EmeraldComponent.HealthBarImage != null)
                {
                    HealthBarBackgroundImageRef.sprite = EmeraldComponent.HealthBarBackgroundImage;
                    HealthBarRef.sprite = EmeraldComponent.HealthBarImage;
                }

                //Displays and colors our AI's name text, if enabled.
                if (EmeraldComponent.DisplayAINameRef == EmeraldAISystem.DisplayAIName.Yes)
                {
                    EmeraldComponent.AINameUI = EmeraldComponent.HealthBar.transform.Find("AI Name Text").gameObject.GetComponent<Text>();

                    if (EmeraldComponent.UseAINameUIOutlineEffect == EmeraldAISystem.YesOrNo.Yes)
                    {
                        Outline AINameOutline = EmeraldComponent.AINameUI.GetComponent<Outline>();
                        AINameOutline.effectDistance = EmeraldComponent.AINameUIOutlineSize;
                        AINameOutline.effectColor = EmeraldComponent.AINameUIOutlineColor;
                    }
                    else
                    {
                        EmeraldComponent.AINameUI.GetComponent<Outline>().enabled = false;
                    }

                    if (EmeraldComponent.DisplayAITitleRef == EmeraldAISystem.DisplayAITitle.Yes)
                    {                       
                        EmeraldComponent.AIName = EmeraldComponent.AIName + "\\n" + EmeraldComponent.AITitle;
                        EmeraldComponent.AIName = EmeraldComponent.AIName.Replace("\\n", "\n");
                        EmeraldComponent.AINamePos.y += 0.25f;

                        if (EmeraldComponent.UseAINameUIOutlineEffect == EmeraldAISystem.YesOrNo.Yes)
                            EmeraldComponent.AINameUI.lineSpacing = EmeraldComponent.AINameLineSpacing;
                    }

                    EmeraldComponent.AINameUI.transform.localPosition = new Vector3(EmeraldComponent.AINamePos.x, EmeraldComponent.AINamePos.y - EmeraldComponent.HealthBarPos.y, EmeraldComponent.AINamePos.z);
                    EmeraldComponent.AINameUI.text = EmeraldComponent.AIName;
                    EmeraldComponent.AINameUI.fontSize = EmeraldComponent.NameTextFontSize;
                    EmeraldComponent.AINameUI.color = EmeraldComponent.NameTextColor;

                    if (EmeraldComponent.UseCustomFontAIName == EmeraldAISystem.YesOrNo.Yes)
                        EmeraldComponent.AINameUI.font = EmeraldComponent.AINameFont;
                }

                //Displays and colors our AI's level text, if enabled.
                if (EmeraldComponent.DisplayAILevelRef == EmeraldAISystem.DisplayAILevel.Yes)
                {
                    EmeraldComponent.AILevelUI = EmeraldComponent.HealthBar.transform.Find("AI Level Text").gameObject.GetComponent<Text>();
                    EmeraldComponent.AILevelUI.text = "   "+EmeraldComponent.AILevel.ToString();
                    EmeraldComponent.AILevelUI.color = EmeraldComponent.LevelTextColor;
                    EmeraldComponent.AILevelUI.transform.localPosition = new Vector3(EmeraldComponent.AILevelPos.x, EmeraldComponent.AILevelPos.y, EmeraldComponent.AILevelPos.z);

                    if (EmeraldComponent.UseCustomFontAILevel == EmeraldAISystem.YesOrNo.Yes)
                        EmeraldComponent.AILevelUI.font = EmeraldComponent.AILevelFont;

                    if (EmeraldComponent.UseAINameUIOutlineEffect == EmeraldAISystem.YesOrNo.Yes)
                    {
                        Outline AINameOutline = EmeraldComponent.AINameUI.GetComponent<Outline>();
                        AINameOutline.effectDistance = EmeraldComponent.AINameUIOutlineSize;
                        AINameOutline.effectColor = EmeraldComponent.AINameUIOutlineColor;
                    }
                    else
                    {
                        EmeraldComponent.AILevelUI.GetComponent<Outline>().enabled = false;
                    }
                }

                //Add disable to return to start and slight delay
                if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet)
                {
                    EmeraldComponent.HealthBarCanvasRef.enabled = false;
                    if (EmeraldComponent.CreateHealthBarsRef == EmeraldAISystem.CreateHealthBars.No)
                    {
                        HealthBarBackgroundImageRef.gameObject.SetActive(false);
                    }
                    if (EmeraldComponent.AINameUI != null && EmeraldComponent.DisplayAINameRef == EmeraldAISystem.DisplayAIName.Yes)
                    {
                        EmeraldComponent.AINameUI.gameObject.SetActive(false);
                    }
                    if (EmeraldComponent.AILevelUI != null && EmeraldComponent.DisplayAILevelRef == EmeraldAISystem.DisplayAILevel.Yes)
                    {
                        EmeraldComponent.AILevelUI.gameObject.SetActive(false);
                    }
                }
                else if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion || EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet)
                {
                    EmeraldComponent.SetUI(true);
                }
            }
        }

        void SetupCombatText ()
        {
            if (EmeraldAISystem.CombatTextSystemObject == null)
            {
                GameObject m_CombatTextSystem = Instantiate((GameObject)Resources.Load("Combat Text System") as GameObject, Vector3.zero, Quaternion.identity);
                m_CombatTextSystem.name = "Combat Text System";
                GameObject m_CombatTextCanvas = Instantiate((GameObject)Resources.Load("Combat Text Canvas") as GameObject, Vector3.zero, Quaternion.identity);
                m_CombatTextCanvas.name = "Combat Text Canvas";
                EmeraldAISystem.CombatTextSystemObject = m_CombatTextCanvas;
                CombatTextSystem.Instance.CombatTextCanvas = m_CombatTextCanvas;
                CombatTextSystem.Instance.Initialize();
            }
        }

        void SetupEmeraldAISettings ()
        {
            EmeraldComponent = GetComponent<EmeraldAISystem>();
            EmeraldComponent.HitPointTransform = new GameObject("AI Hit Transform").transform;
            EmeraldComponent.HitPointTransform.SetParent(transform);
            EmeraldComponent.HitPointTransform.localPosition = new Vector3(0, EmeraldComponent.ProjectileCollisionPointY/transform.localScale.y, 0);
            EmeraldComponent.m_NavMeshAgent = GetComponent<NavMeshAgent>();
            EmeraldComponent.RandomOffset = Random.insideUnitSphere * EmeraldComponent.FollowingStoppingDistance;
            EmeraldComponent.StartingRunSpeed = EmeraldComponent.RunSpeed;
            EmeraldComponent.StartingRunAnimationSpeed = EmeraldComponent.RunAnimationSpeed;
            EmeraldComponent.TargetObstructed = true;
            EmeraldComponent.StartingTag = gameObject.tag;
            EmeraldComponent.StartingLayer = gameObject.layer;
            EmeraldComponent.fieldOfViewAngleRef = EmeraldComponent.fieldOfViewAngle;
            EmeraldComponent.StartingMovementState = EmeraldComponent.CurrentMovementState;
            EmeraldComponent.StartingDetectionRadius = EmeraldComponent.DetectionRadius;
            EmeraldComponent.StartingDestination = transform.position;
            EmeraldComponent.StartingChaseDistance = EmeraldComponent.MaxChaseDistance;
            EmeraldComponent.BackingUpSeconds = Random.Range(EmeraldComponent.BackingUpSecondsMin, EmeraldComponent.BackingUpSecondsMax + 1);
            EmeraldComponent.WaitTime = Random.Range(EmeraldComponent.MinimumWaitTime, EmeraldComponent.MaximumWaitTime + 1);
            EmeraldComponent.IdleSoundsSeconds = Random.Range(EmeraldComponent.IdleSoundsSecondsMin, EmeraldComponent.IdleSoundsSecondsMax + 1);
            EmeraldComponent.StationaryIdleSeconds = Random.Range(EmeraldComponent.StationaryIdleSecondsMin, EmeraldComponent.StationaryIdleSecondsMax + 1);
            EmeraldComponent.CurrentHealth = EmeraldComponent.StartingHealth;
            EmeraldComponent.AIBoxCollider = GetComponent<BoxCollider>();
            EmeraldComponent.m_AudioSource = GetComponent<AudioSource>();
            EmeraldComponent.DeathDelay = Random.Range(EmeraldComponent.DeathDelayMin, EmeraldComponent.DeathDelayMax + 1);
            EmeraldComponent.AttackTimer = EmeraldComponent.AttackSpeed;
            EmeraldComponent.GeneratedBlockOdds = Random.Range(1, 101);
            EmeraldComponent.GeneratedBackupOdds = Random.Range(1, 101);
            EmeraldComponent.CombatStateRef = EmeraldAISystem.CombatState.NotActive;
            EmeraldComponent.StartingBehaviorRef = (int)EmeraldComponent.BehaviorRef;
            EmeraldComponent.StartingConfidenceRef = (int)EmeraldComponent.ConfidenceRef;
            EmeraldComponent.AttackSpeed = Random.Range(EmeraldComponent.MinimumAttackSpeed, EmeraldComponent.MaximumAttackSpeed + 1);
            EmeraldComponent.RunAttackSpeed = Random.Range(EmeraldComponent.MinimumRunAttackSpeed, EmeraldComponent.MaximumRunAttackSpeed + 1);
            EmeraldComponent.TargetDetectionActive = true;
            EmeraldComponent.StartingAttackDistance = EmeraldComponent.AttackDistance;
            EmeraldComponent.StartingTooCloseDistance = EmeraldComponent.TooCloseDistance;
            EmeraldComponent.FirstTimeInCombat = true;
            EmeraldComponent.BackupDistance = (int)EmeraldComponent.StoppingDistance + 2;
            EmeraldComponent.GetComponent<EmeraldAIDetection>().YOffSet = EmeraldComponent.YAimOffset*-1;

            if (EmeraldComponent.AnimatorCullingMode == EmeraldAISystem.AnimatorCullingModes.AlwaysAnimate)
                EmeraldComponent.AIAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            else if (EmeraldComponent.AnimatorCullingMode == EmeraldAISystem.AnimatorCullingModes.CullUpdateTransforms)
                EmeraldComponent.AIAnimator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;

            if (EmeraldComponent.MeleeAttacks.Count > 0)
            {
                EmeraldComponent.GetDamageAmount();
            }

            if (EmeraldComponent.UseHitAnimations == EmeraldAISystem.YesOrNo.No && EmeraldComponent.UseBlockingRef == EmeraldAISystem.YesOrNo.Yes)
            {
                EmeraldComponent.UseBlockingRef = EmeraldAISystem.YesOrNo.No;
            }

            if (EmeraldComponent.ConfidenceRef == EmeraldAISystem.ConfidenceType.Coward)
            {
                EmeraldComponent.UseEquipAnimation = EmeraldAISystem.YesOrNo.No;
            }

            if (EmeraldComponent.SummonsMultipleAI == EmeraldAISystem.EnableDisable.Disabled)
            {
                EmeraldComponent.MaxAllowedSummonedAI = 1;
            }

            //If the user forgot to add a head transform, create a temporary one to avoid an error and to still allow the AI to function.
            if (EmeraldComponent.HeadTransform == null)
            {
                Transform TempHeadTransform = new GameObject("AI Head Transform").transform;
                TempHeadTransform.SetParent(transform);
                TempHeadTransform.localPosition = new Vector3(0,1,0);
                EmeraldComponent.HeadTransform = TempHeadTransform;
            }

            if (EmeraldComponent.AttackSpeed < 1)
            {
                EmeraldComponent.AttackSpeed = 1;
            }

            if (EmeraldComponent.RunAttackSpeed < 1)
            {
                EmeraldComponent.RunAttackSpeed = 1;
            }

            if (EmeraldComponent.RangedAttackTransform == null)
            {
                EmeraldComponent.RangedAttackTransform = this.transform;
            }

            EmeraldComponent.MaxNormalAngle = ((float)EmeraldComponent.MaxNormalAngleEditor / 9 * 0.1f);

            //Companion AI cannot use the Line of Sight Detection Type. This is due to them possibly missing targets.
            if (EmeraldComponent.DetectionTypeRef == EmeraldAISystem.DetectionType.LineOfSight && EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion
                || EmeraldComponent.DetectionTypeRef == EmeraldAISystem.DetectionType.LineOfSight && EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Passive)
            {
                EmeraldComponent.DetectionTypeRef = EmeraldAISystem.DetectionType.Trigger;
            }             
            
            if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Companion)
            {
                EmeraldComponent.PlayerFaction[0].RelationTypeRef = EmeraldAISystem.PlayerFactionClass.RelationType.Friendly;
            }

            if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Aggressive && EmeraldComponent.ConfidenceRef == EmeraldAISystem.ConfidenceType.Coward)
            {
                EmeraldComponent.ConfidenceRef = EmeraldAISystem.ConfidenceType.Brave;
            }

            if (EmeraldComponent.BehaviorRef == EmeraldAISystem.CurrentBehavior.Pet)
            {
                if (gameObject.tag != "Untagged")
                {
                    gameObject.tag = "Untagged";
                }
                if (gameObject.layer != 0)
                {
                    gameObject.layer = 0;
                }
                EmeraldComponent.PlayerFaction[0].RelationTypeRef = EmeraldAISystem.PlayerFactionClass.RelationType.Friendly;
            }

            if (EmeraldComponent.UseRandomRotationOnStartRef == EmeraldAISystem.YesOrNo.Yes)
            {
                transform.rotation = Quaternion.AngleAxis(Random.Range(5, 360), Vector3.up);
            }

            if (EmeraldComponent.AlignAIOnStartRef == EmeraldAISystem.YesOrNo.Yes && EmeraldComponent.AlignAIWithGroundRef == EmeraldAISystem.AlignAIWithGround.Yes)
            {
                AlignOnStart();
            }
        }

        /// <summary>
        /// Double check all essential animations to esnure they have the proper events. If not, notify the user which animations 
        /// are missing as well as which events are needed. This is to avoid confusion as to why some functionality may not be wroking correctly.
        /// </summary>
        void CheckAnimationEvents ()
        {
            if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet || EmeraldComponent.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward)
            {
                //Attack Animations
                for (int l = 0; l < EmeraldComponent.AttackAnimationList.Count; l++)
                {
                    bool AttackAnimationEventFound = false;
                    bool AnimationEventErrorDisplayed = false;

                    if (EmeraldComponent.AttackAnimationList[l].AnimationClip != null)
                    {
                        for (int i = 0; i < EmeraldComponent.AttackAnimationList[l].AnimationClip.events.Length; i++)
                        {
                            if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee &&
                                EmeraldComponent.AttackAnimationList[l].AnimationClip.events[i].functionName == "SendEmeraldDamage" || 
                                EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee &&
                                EmeraldComponent.AttackAnimationList[l].AnimationClip.events[i].functionName == "EmeraldAttackEvent")
                            {
                                AttackAnimationEventFound = true;
                            }
                        }

                        if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && !AttackAnimationEventFound && !AnimationEventErrorDisplayed)
                        {
                            Debug.Log("<b>" + "<color=red>" + gameObject.name + "'s Attack " + (l + 1) + " is missing a EmeraldAttackEvent Animation Event. " +
                                    "Please add one or see Emerald AI's Documentation for a guide on how to do so." + "</color>" + "</b>");
                            AnimationEventErrorDisplayed = true;
                        }
                    }
                }

                //Ranged Attack Animations
                for (int l = 0; l < EmeraldComponent.RangedAttackAnimationList.Count; l++)
                {
                    bool AttackAnimationEventFound = false;
                    bool AnimationEventErrorDisplayed = false;

                    if (EmeraldComponent.RangedAttackAnimationList[l].AnimationClip != null)
                    {
                        for (int i = 0; i < EmeraldComponent.RangedAttackAnimationList[l].AnimationClip.events.Length; i++)
                        {
                            if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged &&
                                EmeraldComponent.RangedAttackAnimationList[l].AnimationClip.events[i].functionName == "CreateEmeraldProjectile" ||
                                EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged &&
                                EmeraldComponent.RangedAttackAnimationList[l].AnimationClip.events[i].functionName == "EmeraldAttackEvent")
                            {
                                AttackAnimationEventFound = true;
                            }
                        }

                        if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged && !AttackAnimationEventFound && !AnimationEventErrorDisplayed)
                        {
                            Debug.Log("<b>" + "<color=red>" + gameObject.name + "'s Attack " + (l + 1) + " is missing a EmeraldAttackEvent Animation Event. " +
                                "Please add one or see Emerald AI's Documentation for a guide on how to do so." + "</color>" + "</b>");
                            AnimationEventErrorDisplayed = true;
                        }
                    }
                }

                //Run Attack Animations
                if (EmeraldComponent.UseRunAttacksRef == EmeraldAISystem.UseRunAttacks.Yes)
                {
                    for (int l = 0; l < EmeraldComponent.RunAttackAnimationList.Count; l++)
                    {
                        bool RunAttackAnimationEventFound = false;
                        bool RunAnimationEventErrorDisplayed = false;

                        if (EmeraldComponent.RunAttackAnimationList[l].AnimationClip != null)
                        {
                            for (int i = 0; i < EmeraldComponent.RunAttackAnimationList[l].AnimationClip.events.Length; i++)
                            {
                                if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee &&
                                    EmeraldComponent.RunAttackAnimationList[l].AnimationClip.events[i].functionName == "SendEmeraldDamage")
                                {
                                    RunAttackAnimationEventFound = true;
                                }
                                else if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee &&
                                    EmeraldComponent.RunAttackAnimationList[l].AnimationClip.events[i].functionName == "EmeraldAttackEvent")
                                {
                                    RunAttackAnimationEventFound = true;
                                }
                            }

                            if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee && !RunAttackAnimationEventFound && !RunAnimationEventErrorDisplayed ||
                                EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes && !RunAttackAnimationEventFound && !RunAnimationEventErrorDisplayed)
                            {
                                Debug.Log("<b>" + "<color=red>" + gameObject.name + "'s Run Attack " + (l + 1) + " is missing a EmeraldAttackEvent Animation Event. " +
                                        "Please add one or see Emerald AI's Documentation for a guide on how to do so." + "</color>" + "</b>");
                                RunAnimationEventErrorDisplayed = true;
                            }
                        }
                    }
                }

                //Equip Animations
                if (EmeraldComponent.UseEquipAnimation == EmeraldAISystem.YesOrNo.Yes)
                {
                    if (EmeraldComponent.PullOutWeaponAnimation != null)
                    {
                        bool EquipAnimationEventFound = false;
                        bool AnimationEventErrorDisplayed = false;

                        for (int i = 0; i < EmeraldComponent.PullOutWeaponAnimation.events.Length; i++)
                        {
                            if (EmeraldComponent.PullOutWeaponAnimation.events[i].functionName == "EnableWeapon")
                            {
                                EquipAnimationEventFound = true;
                            }
                        }

                        if (!EquipAnimationEventFound && !AnimationEventErrorDisplayed)
                        {
                            Debug.Log("<b>" + "<color=red>" + gameObject.name + "'s Equip Animation is missing an EnableWeapon Animation Event. " +
                                "Please add one or see Emerald AI's Documentation for a guide on how to do so." + "</color>" + "</b>");
                            AnimationEventErrorDisplayed = true;
                        }
                    }

                    if (EmeraldComponent.PutAwayWeaponAnimation != null)
                    {
                        bool UnequipAnimationEventFound = false;
                        bool AnimationEventErrorDisplayed = false;

                        for (int i = 0; i < EmeraldComponent.PutAwayWeaponAnimation.events.Length; i++)
                        {
                            if (EmeraldComponent.PutAwayWeaponAnimation.events[i].functionName == "DisableWeapon")
                            {
                                UnequipAnimationEventFound = true;
                            }
                        }

                        if (!UnequipAnimationEventFound && !AnimationEventErrorDisplayed)
                        {
                            Debug.Log("<b>" + "<color=red>" + gameObject.name + "'s Unequip Animation is missing a DisableWeapon Animation Event. " +
                                "Please add one or see Emerald AI's Documentation for a guide on how to do so." + "</color>" + "</b>");
                            AnimationEventErrorDisplayed = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check all of an AI's animations. If there are any missing. Debug.Log an error message notifying the user which animation is missing.
        /// </summary>
        public void CheckForMissingAnimations()
        {
            EmeraldComponent = GetComponent<EmeraldAISystem>();

            //Check animations Lists
            for (int l = 0; l < EmeraldComponent.IdleAnimationList.Count; l++)
            {
                if (EmeraldComponent.IdleAnimationList[l].AnimationClip == null)
                {
                    Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Idle Animation " + (l + 1) + " is missing. Please assign an animation to this slot or remove it to avoid errors." + "</color>" + "</b>");
                }
            }
            
            if (EmeraldComponent.UseHitAnimations == EmeraldAISystem.YesOrNo.Yes)
            {
                for (int l = 0; l < EmeraldComponent.HitAnimationList.Count; l++)
                {
                    if (EmeraldComponent.HitAnimationList[l].AnimationClip == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Hit Animation " + (l + 1) + " is missing. Please assign an animation to this slot or remove it to avoid errors." + "</color>" + "</b>");
                    }
                }

                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    for (int l = 0; l < EmeraldComponent.CombatHitAnimationList.Count; l++)
                    {
                        if (EmeraldComponent.CombatHitAnimationList[l].AnimationClip == null)
                        {
                            Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Combat Hit Animation " + (l + 1) + " is missing. Please assign an animation to this slot or remove it to avoid errors." + "</color>" + "</b>");
                        }
                    }
                }

                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                {
                    for (int l = 0; l < EmeraldComponent.RangedCombatHitAnimationList.Count; l++)
                    {
                        if (EmeraldComponent.RangedCombatHitAnimationList[l].AnimationClip == null)
                        {
                            Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Hit Animation " + (l + 1) + " is missing. Please assign an animation to this slot or remove it to avoid errors." + "</color>" + "</b>");
                        }
                    }
                }
            }

            if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
            {
                for (int l = 0; l < EmeraldComponent.RangedAttackAnimationList.Count; l++)
                {
                    if (EmeraldComponent.RangedAttackAnimationList[l].AnimationClip == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Attack Animation " + (l + 1) + " is missing. Please assign an animation to this slot or remove it to avoid errors." + "</color>" + "</b>");
                    }
                }

                for (int l = 0; l < EmeraldComponent.RangedRunAttackAnimationList.Count; l++)
                {
                    if (EmeraldComponent.RangedRunAttackAnimationList[l].AnimationClip == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Run Attack Animation " + (l + 1) + " is missing. Please assign an animation to this slot or remove it to avoid errors." + "</color>" + "</b>");
                    }
                }
            }

            if (EmeraldComponent.DeathTypeRef == EmeraldAISystem.DeathType.Animation)
            {
                if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee || EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Both)
                {
                    for (int l = 0; l < EmeraldComponent.DeathAnimationList.Count; l++)
                    {
                        if (EmeraldComponent.DeathAnimationList[l].AnimationClip == null)
                        {
                            Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Death Animation " + (l + 1) + " is missing. Please assign an animation to this slot or remove it to avoid errors." + "</color>" + "</b>");
                        }
                    }
                }

                if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged || EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Both)
                {
                    for (int l = 0; l < EmeraldComponent.RangedDeathAnimationList.Count; l++)
                    {
                        if (EmeraldComponent.RangedDeathAnimationList[l].AnimationClip == null)
                        {
                            Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Death Animation " + (l + 1) + " is missing. Please assign an animation to this slot or remove it to avoid errors." + "</color>" + "</b>");
                        }
                    }
                }
            }

            if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Pet || EmeraldComponent.ConfidenceRef != EmeraldAISystem.ConfidenceType.Coward)
            {
                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    if (EmeraldComponent.CombatIdleAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Idle Combat Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }
                }

                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                {
                    if (EmeraldComponent.RangedCombatIdleAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Idle Combat Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }
                }

                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    for (int l = 0; l < EmeraldComponent.AttackAnimationList.Count; l++)
                    {
                        if (EmeraldComponent.AttackAnimationList[l].AnimationClip == null)
                        {
                            Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Attack Animation " + (l + 1) + " is missing. Please assign an animation to this slot or remove it to avoid errors." + "</color>" + "</b>");
                        }
                    }
                }

                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    if (EmeraldComponent.CombatWalkStraightAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Combat Walk Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.CombatWalkLeftAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Combat Walk Left Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.CombatWalkRightAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Combat Walk Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.CombatRunStraightAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Combat Run Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.CombatRunLeftAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Combat Run Left Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.CombatRunRightAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Combat Run Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.CombatTurnLeftAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Combat Turn Left Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.CombatTurnRightAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Combat Turn Right Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }
                }

                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                {
                    if (EmeraldComponent.RangedCombatWalkStraightAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Combat Walk Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.RangedCombatWalkLeftAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Combat Walk Left Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.RangedCombatWalkRightAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Combat Walk Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.RangedCombatRunStraightAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Combat Run Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.RangedCombatRunLeftAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Combat Run Left Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.RangedCombatRunRightAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Combat Run Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.RangedCombatTurnLeftAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Combat Turn Left Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.RangedCombatTurnRightAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Combat Turn Right Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }
                }
            }

            if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee || 
                EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
            {
                if (EmeraldComponent.UseRunAttacksRef == EmeraldAISystem.UseRunAttacks.Yes)
                {
                    for (int l = 0; l < EmeraldComponent.RunAttackAnimationList.Count; l++)
                    {
                        if (EmeraldComponent.RunAttackAnimationList[l].AnimationClip == null)
                        {
                            Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Run Attack Animation " + (l + 1) + " is missing. Please assign an animation to this slot or remove it to avoid errors." + "</color>" + "</b>");
                        }
                    }
                }
            }

            //Double Check Single Animations
            if (EmeraldComponent.UseEquipAnimation == EmeraldAISystem.YesOrNo.Yes)
            {
                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes || 
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    if (EmeraldComponent.PullOutWeaponAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Equip Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.PutAwayWeaponAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Unequip Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }
                }

                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                {
                    if (EmeraldComponent.RangedPullOutWeaponAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Equip Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }

                    if (EmeraldComponent.RangedPutAwayWeaponAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Unequip Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }
                }
            }

            if (EmeraldComponent.NonCombatIdleAnimation == null)
            {
                Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Idle Non-Combat Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
            }

            if (EmeraldComponent.UseWarningAnimationRef == EmeraldAISystem.YesOrNo.Yes)
            {
                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    if (EmeraldComponent.IdleWarningAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Warning Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }
                }

                if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes ||
                    EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No && EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                {
                    if (EmeraldComponent.RangedIdleWarningAnimation == null)
                    {
                        Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Ranged Warning Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
                    }
                }
            }

            if (EmeraldComponent.WalkStraightAnimation == null)
            {
                Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Walk Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
            }

            if (EmeraldComponent.WalkLeftAnimation == null)
            {
                Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Walk Left Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
            }

            if (EmeraldComponent.WalkRightAnimation == null)
            {
                Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Walk Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
            }

            if (EmeraldComponent.RunStraightAnimation == null)
            {
                Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Run Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
            }

            if (EmeraldComponent.RunLeftAnimation == null)
            {
                Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Run Left Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
            }

            if (EmeraldComponent.RunRightAnimation == null)
            {
                Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Run Straight Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
            }

            if (EmeraldComponent.NonCombatTurnLeftAnimation == null)
            {
                Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Non-Combat Turn Left Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
            }

            if (EmeraldComponent.NonCombatTurnRightAnimation == null)
            {
                Debug.LogError("<b>" + "<color=red>" + gameObject.name + "'s Non-Combat Turn Right Animation is missing. Please assign an animation to this slot to avoid errors." + "</color>" + "</b>");
            }
        }

        public void InitializeWeaponTypeAnimation ()
        {
            if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.No)
            {
                if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Melee)
                {
                    EmeraldComponent.AIAnimator.SetInteger("Weapon Type State", 0);
                }
                else if (EmeraldComponent.WeaponTypeRef == EmeraldAISystem.WeaponType.Ranged)
                {
                    EmeraldComponent.AIAnimator.SetInteger("Weapon Type State", 1);
                }
            }
            else
            {
                EmeraldComponent.WeaponTypeRef = EmeraldAISystem.WeaponType.Melee;
                EmeraldComponent.AIAnimator.SetInteger("Weapon Type State", 1);
            }

            /*
            else if (EmeraldComponent.EnableBothWeaponTypes == EmeraldAISystem.YesOrNo.Yes)
            {
                if (EmeraldComponent.StartingWeaponTypeRef == EmeraldAISystem.StartingWeaponType.Melee)
                {
                    EmeraldComponent.AIAnimator.SetInteger("Weapon Type State", 1);
                }
                else if (EmeraldComponent.StartingWeaponTypeRef == EmeraldAISystem.StartingWeaponType.Ranged)
                {
                    EmeraldComponent.AIAnimator.SetInteger("Weapon Type State", 0);
                }
            }
            */
        }

        /// <summary>
        /// Initialize an AI's death
        /// </summary>
        public void InitializeAIDeath ()
        {
            //Get a random ragdoll component to apply force to
            if (EmeraldComponent.DeathTypeRef == EmeraldAISystem.DeathType.Ragdoll)
            {
                Rigidbody[] RandomRagdollComponent = transform.GetComponentsInChildren<Rigidbody>();
                EmeraldComponent.RagdollTransform = RandomRagdollComponent[Random.Range(0, RandomRagdollComponent.Length)].transform;
                EmeraldComponent.EmeraldEventsManagerComponent.PlayDeathSound();
            }
            
            StartCoroutine(InitializeAIDeathCoroutine());
        }

        IEnumerator InitializeAIDeathCoroutine ()
        {
            float CurrentForce = 0;

            //Crux support
            #if CRUX_PRESENT
            if (EmeraldComponent.UseMagicEffectsPackRef == EmeraldAISystem.UseMagicEffectsPack.Yes)
            {
                Crux.CruxSystem.Instance.RemoveObjectFromPopulation(gameObject);
            }
            #endif

            if (EmeraldComponent.DeathTypeRef == EmeraldAISystem.DeathType.Ragdoll)
            {
                while (CurrentForce <= 0.1f && EmeraldComponent.RagdollTransform != null && EmeraldComponent.ForceTransform != null)
                {
                    CurrentForce += Time.deltaTime;
                    EmeraldComponent.RagdollTransform.GetComponentInChildren<Rigidbody>().AddForce((EmeraldComponent.ForceTransform.forward * EmeraldComponent.ReceivedRagdollForceAmount) +
                        EmeraldComponent.ForceTransform.up * EmeraldComponent.ReceivedRagdollForceAmount * 1.5f);
                    yield return null;
                }

                GetComponent<EmeraldAISystem>().enabled = false;
                EmeraldComponent.EmeraldDetectionComponent.enabled = false;
                EmeraldComponent.EmeraldEventsManagerComponent.enabled = false;
                EmeraldComponent.AIBoxCollider.enabled = false;
            }
            else if (EmeraldComponent.DeathTypeRef == EmeraldAISystem.DeathType.Animation)
            {
                while (CurrentForce <= EmeraldComponent.SecondsToDisable)
                {
                    CurrentForce += Time.deltaTime;                   
                    yield return null;
                }

                EmeraldComponent.AIAnimator.enabled = false;
                GetComponent<EmeraldAISystem>().enabled = false;
            }           
        }
    }
}
