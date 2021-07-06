using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Utility
{
    public class EmeraldAIProjectile : MonoBehaviour
    {
        [HideInInspector]
        public EmeraldAISystem EmeraldSystem;
        [HideInInspector]
        public EmeraldAISystem TargetEmeraldSystem;
        [HideInInspector]
        public Transform StartingTarget;
        [HideInInspector]
        public EmeraldAIPlayerDamage m_EmeraldAIPlayerDamage;
        [HideInInspector]
        public Vector3 ProjectileDirection;
        [HideInInspector]
        public float TimeoutTimer;
        [HideInInspector]
        public float CollisionTimer;
        [HideInInspector]
        public bool Collided;
        [HideInInspector]
        public SphereCollider ProjectileCollider;
        [HideInInspector]
        public float ColliderRadius = 0.1f;
        [HideInInspector]
        public float CollisionTimeout;
        [HideInInspector]
        public float DamageOvertimeTimeout;
        GameObject SpawnedEffect;
        GameObject CollisionSound;
        GameObject CollisionSoundObject;
        Vector3 LastDirection;
        float m_TargetDeadTimer;

        //Customizable variables
        [HideInInspector]
        public int Damage = 1;
        [HideInInspector]
        public string AbilityName = "";
        [HideInInspector]
        public int AbilityImpactDamage = 1;
        [HideInInspector]
        public float AbilityDamageIncrement = 1;
        [HideInInspector]
        public int AbilityDamagePerIncrement = 1;
        [HideInInspector]
        public int AbilitySupportAmount = 5;
        [HideInInspector]
        public int AbilityLength = 3;
        [HideInInspector]
        public GameObject DamageOverTimeEffect;
        [HideInInspector]
        public AudioClip DamageOverTimeSound;
        [HideInInspector]
        public int RagdollForce = 100;
        [HideInInspector]
        public float TimeoutTime = 4.5f;
        [HideInInspector]
        public float CollisionTime = 0;
        [HideInInspector]
        public int ProjectileSpeed = 30;
        [HideInInspector]
        public enum EffectOnCollision { No = 0, Yes = 1 };
        [HideInInspector]
        public EffectOnCollision EffectOnCollisionRef = EffectOnCollision.No;
        [HideInInspector]
        public EffectOnCollision SoundOnCollisionRef = EffectOnCollision.No;
        [HideInInspector]
        public enum HeatSeeking { No = 0, Yes = 1 };
        [HideInInspector]
        public HeatSeeking HeatSeekingRef = HeatSeeking.No;
        [HideInInspector]
        public AudioClip ImpactSound;
        [HideInInspector]
        public GameObject CollisionEffect;
        [HideInInspector]
        public Vector3 AdditionalHeight;
        [HideInInspector]
        public float HeatSeekingSeconds = 1;
        [HideInInspector]
        public float HeatSeekingTimer = 0;
        [HideInInspector]
        public bool HeatSeekingFinished = false;
        [HideInInspector]
        public Transform ProjectileCurrentTarget;
        [HideInInspector]
        public bool TargetInView = false;
        [HideInInspector]
        public bool CriticalHit = false;

        [HideInInspector]
        public Yes_No UseRandomizedTrajectory = Yes_No.No;
        [HideInInspector]
        public Yes_No UseGravity = Yes_No.No;
        Vector3 m_TrajectoryOffset;
        [HideInInspector]
        public float TrajectoryXOffsetMin = 0;
        [HideInInspector]
        public float TrajectoryXOffsetMax = 0;
        [HideInInspector]
        public float TrajectoryYOffsetMin = 0;
        [HideInInspector]
        public float TrajectoryYOffsetMax = 0;
        [HideInInspector]
        public float TrajectoryZOffsetMin = 0;
        [HideInInspector]
        public float TrajectoryZOffsetMax = 0;

        public float AdjustedAngle;

        Vector3 AdjustTargetPosition;

        [HideInInspector]
        public GameObject ObjectToDisableOnCollision;
        public enum ArrowObject { No = 0, Yes = 1 };
        [HideInInspector]
        public ArrowObject ArrowProjectileRef = ArrowObject.No;
        [HideInInspector]
        public DamageTypeEnum DamageType;
        public enum DamageTypeEnum
        {
            Instant = 0,
            OverTime = 1
        }

        [HideInInspector]
        public AbilityTypeEnum AbilityType;
        public enum AbilityTypeEnum
        {
            Damage = 0,
            Support = 1
        }

        public enum Yes_No { No = 0, Yes = 1 };
        [HideInInspector]
        public Yes_No AbilityStacksRef = Yes_No.No;

        public enum TargetType { Player = 0, AI = 1, NonAITarget = 2 };
        [HideInInspector]
        public TargetType TargetTypeRef = TargetType.Player;

        [HideInInspector]
        public bool AngleTooBig = false;

        GameObject DamageOverTimeComponent;
        Vector3 m_PreviousPosition;
        Vector3 m_CurrentVelocity = new Vector3(4,4,4);
        bool ProjectileDirectionReceived = false;

        //Setup our AI's projectile once on Awake
        void Awake()
        {
            gameObject.layer = 2;
            gameObject.AddComponent<SphereCollider>();
            ProjectileCollider = GetComponent<SphereCollider>();
            ProjectileCollider.isTrigger = true;
            gameObject.AddComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            gameObject.isStatic = false;
            CollisionSoundObject = Resources.Load("Emerald Collision Sound") as GameObject;
        }

        void Start()
        {
            gameObject.AddComponent<AudioSource>();
            DamageOverTimeComponent = Resources.Load<GameObject>("Damage Over Time Component");
            ProjectileCollider.radius = ColliderRadius;
            InitailizeAudioSource();
        }

        void InitailizeAudioSource()
        {
            AudioSource m_AudioSource = GetComponent<AudioSource>();
            m_AudioSource.spatialBlend = EmeraldSystem.m_AudioSource.spatialBlend;
            m_AudioSource.minDistance = EmeraldSystem.m_AudioSource.minDistance;
            m_AudioSource.maxDistance = EmeraldSystem.m_AudioSource.maxDistance;
            m_AudioSource.rolloffMode = EmeraldSystem.m_AudioSource.rolloffMode;
        }

        void OnEnable()
        {
            ProjectileDirectionReceived = false;
            m_TargetDeadTimer = 0;
            ProjectileCollider.radius = ColliderRadius;
            if (ObjectToDisableOnCollision != null)
            {
                ObjectToDisableOnCollision.SetActive(true);
            }
        }

        //Get the angle when the projectile is created.
        public void GetHeatSeekingAngle()
        {
            if (HeatSeekingRef == HeatSeeking.Yes && ProjectileCurrentTarget != null && EmeraldSystem != null)
            {
                AdjustedAngle = EmeraldSystem.TargetAngle();

                if (AdjustedAngle <= (EmeraldSystem.MaxFiringAngle + 5))
                {
                    TargetInView = true;
                }
                else if (AdjustedAngle > (EmeraldSystem.MaxFiringAngle + 5))
                {
                    AngleTooBig = true;
                }
            }
        }

        public void GetAngle ()
        {
            if (ProjectileCurrentTarget != null && EmeraldSystem != null)
            {
                AdjustedAngle = EmeraldSystem.TargetAngle();

                if (AdjustedAngle <= (EmeraldSystem.MaxFiringAngle))
                {
                    TargetInView = true;
                }
                else if (AdjustedAngle > (EmeraldSystem.MaxFiringAngle))
                {
                    AngleTooBig = true;
                }
            }
        }

        void Update()
        {
            //If the target exceeds the AI's firing angle, fire the projectile towards the last detected destination.
            if (AngleTooBig && !Collided)
            {               
                if (TargetTypeRef == TargetType.AI && TargetEmeraldSystem != null && !ProjectileDirectionReceived)
                {
                    AdjustTargetPosition = TargetEmeraldSystem.HitPointTransform.position - transform.position;
                    ProjectileDirectionReceived = true;
                }
                else if (TargetTypeRef != TargetType.AI && !ProjectileDirectionReceived)
                {
                    AdjustTargetPosition = EmeraldSystem.m_InitialTargetPosition - new Vector3(EmeraldSystem.transform.position.x, transform.position.y, EmeraldSystem.transform.position.z);
                    ProjectileDirectionReceived = true;
                }

                transform.position = transform.position + AdjustTargetPosition.normalized * Time.deltaTime * ProjectileSpeed;

                if (AdjustTargetPosition != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(AdjustTargetPosition);
                }
            }

            if (!AngleTooBig)
            {
                //Continue to have our AI projectile follow the direction of its target until it collides with something
                if (!Collided && HeatSeekingRef == HeatSeeking.No && ProjectileDirection != Vector3.zero ||
                    !TargetInView && !Collided && ProjectileDirection != Vector3.zero)
                {
                    DeadTargetDetection();

                    if (TargetTypeRef == TargetType.AI && TargetEmeraldSystem != null && !ProjectileDirectionReceived)
                    {
                        AdjustTargetPosition = TargetEmeraldSystem.HitPointTransform.position - transform.position;
                        ProjectileDirectionReceived = true;
                    }
                    else if (TargetTypeRef == TargetType.Player && !ProjectileDirectionReceived)
                    {
                        AdjustTargetPosition = new Vector3(ProjectileDirection.x, ProjectileDirection.y + ProjectileCurrentTarget.localScale.y / 2 + EmeraldSystem.PlayerYOffset, ProjectileDirection.z);
                        ProjectileDirectionReceived = true;
                    }
                    else if (TargetTypeRef == TargetType.NonAITarget && !ProjectileDirectionReceived)
                    {
                        AdjustTargetPosition = new Vector3(ProjectileDirection.x, ProjectileDirection.y + ProjectileCurrentTarget.localScale.y / 2, ProjectileDirection.z);
                        ProjectileDirectionReceived = true;
                    }

                    transform.position += AdjustTargetPosition.normalized * Time.deltaTime * ProjectileSpeed;
                    transform.rotation = Quaternion.LookRotation(AdjustTargetPosition);  
                }

                if (!Collided && HeatSeekingRef == HeatSeeking.Yes && TargetInView)
                {
                    if (!HeatSeekingFinished)
                    {
                        if (TargetTypeRef == TargetType.AI && TargetEmeraldSystem != null)
                        {
                            AdjustTargetPosition = TargetEmeraldSystem.HitPointTransform.position;
                        }
                        else if (TargetTypeRef == TargetType.Player)
                        {
                            AdjustTargetPosition = new Vector3(ProjectileCurrentTarget.position.x, ProjectileCurrentTarget.position.y + ProjectileCurrentTarget.localScale.y / 2 + EmeraldSystem.PlayerYOffset, ProjectileCurrentTarget.position.z);
                        }
                        else if (TargetTypeRef == TargetType.NonAITarget)
                        {
                            AdjustTargetPosition = new Vector3(ProjectileCurrentTarget.position.x, ProjectileCurrentTarget.position.y + ProjectileCurrentTarget.localScale.y / 2, ProjectileCurrentTarget.position.z);
                        }

                        if (ProjectileCurrentTarget != null) 
                        {
                            DeadTargetDetection();

                            transform.position = Vector3.MoveTowards(transform.position, AdjustTargetPosition, Time.deltaTime * ProjectileSpeed);
                            transform.LookAt(AdjustTargetPosition);
                            HeatSeekingTimer += Time.deltaTime;

                            if (HeatSeekingTimer >= HeatSeekingSeconds || TargetEmeraldSystem != null && TargetEmeraldSystem.CurrentHealth <= 0)
                            {
                                LastDirection = ProjectileCurrentTarget.position - transform.position;
                                HeatSeekingFinished = true;                               
                            }
                        }
                    }
                    else if (HeatSeekingFinished && LastDirection != Vector3.zero || TargetEmeraldSystem != null && TargetEmeraldSystem.CurrentHealth <= 0)
                    {
                        DeadTargetDetection();                       

                        if (TargetTypeRef == TargetType.AI && TargetEmeraldSystem != null && !ProjectileDirectionReceived)
                        {
                            AdjustTargetPosition = TargetEmeraldSystem.HitPointTransform.position - transform.position;
                            ProjectileDirectionReceived = true;
                        }
                        else if (TargetTypeRef != TargetType.AI && !ProjectileDirectionReceived)
                        {
                            AdjustTargetPosition = new Vector3(LastDirection.x, LastDirection.y, LastDirection.z);
                            ProjectileDirectionReceived = true;
                        }

                        transform.position = transform.position + AdjustTargetPosition.normalized * Time.deltaTime * ProjectileSpeed;

                        if (AdjustTargetPosition != Vector3.zero)
                        {
                            transform.rotation = Quaternion.LookRotation(AdjustTargetPosition);
                        }                      
                    }
                }
            }

            if (Collided)
            {
                CollisionTimer += Time.deltaTime;
                if (CollisionTimer >= CollisionTime)
                {
                    EmeraldAIObjectPool.Despawn(gameObject);
                }
            }
        }

        void DeadTargetDetection()
        {
            if (m_PreviousPosition != Vector3.zero)
            {
                m_CurrentVelocity = (m_PreviousPosition - transform.position) / Time.deltaTime;
            }
            m_PreviousPosition = transform.position;

            if (!Collided && ProjectileCurrentTarget != null && Vector3.Distance(ProjectileCurrentTarget.position, transform.position) <= 2 || m_CurrentVelocity.magnitude < 1)
            {               
                if (ObjectToDisableOnCollision != null)
                {
                    ObjectToDisableOnCollision.SetActive(true);
                }

                m_TargetDeadTimer += Time.deltaTime;

                if (m_TargetDeadTimer >= 0.1f)
                {
                    if (ObjectToDisableOnCollision != null)
                    {
                        ObjectToDisableOnCollision.SetActive(false);
                    }
                    if (ImpactSound != null && SoundOnCollisionRef == EffectOnCollision.Yes)
                    {
                        CollisionSound = EmeraldAIObjectPool.SpawnEffect(CollisionSoundObject, transform.position, Quaternion.identity, 2);
                        CollisionSound.transform.SetParent(EmeraldAISystem.ObjectPool.transform);
                        AudioSource CollisionAudioSource = CollisionSound.GetComponent<AudioSource>();
                        CollisionAudioSource.PlayOneShot(ImpactSound);
                    }
                    if (EffectOnCollisionRef == EffectOnCollision.Yes)
                    {
                        if (CollisionEffect != null)
                        {
                            SpawnedEffect = EmeraldAIObjectPool.SpawnEffect(CollisionEffect, transform.position, Quaternion.identity, CollisionTimeout);
                            SpawnedEffect.transform.SetParent(EmeraldAISystem.ObjectPool.transform);
                        }
                    }
                    Collided = true;
                    ProjectileCollider.enabled = false;
                }
            }
        }

        //Handle all of our collision related calculations here. When this happens, effects and sound can be played before the object is despawned.
        void OnTriggerEnter(Collider C)
        {
            if (EmeraldSystem.EnableDebugging == EmeraldAISystem.YesOrNo.Yes && EmeraldSystem.DebugLogProjectileCollisionsEnabled == EmeraldAISystem.YesOrNo.Yes && C.gameObject != EmeraldSystem.gameObject)
            {
                Debug.Log("<b>" + "<color=green>" + EmeraldSystem.name + "'s Projectile Hit: " + "</color>" + "<color=red>" + C.gameObject.name + "</color>" + "</b>");
            }

            if (!Collided && EmeraldSystem != null && ProjectileCurrentTarget != null && C.transform.IsChildOf(ProjectileCurrentTarget.transform) && C.gameObject.layer != 2)
            {
                if (EffectOnCollisionRef == EffectOnCollision.Yes)
                {
                    if (CollisionEffect != null)
                    {
                        SpawnedEffect = EmeraldAIObjectPool.SpawnEffect(CollisionEffect, transform.position, Quaternion.identity, CollisionTimeout);
                        SpawnedEffect.transform.SetParent(EmeraldAISystem.ObjectPool.transform);
                    }
                }
                if (ImpactSound != null && SoundOnCollisionRef == EffectOnCollision.Yes)
                {
                    CollisionSound = EmeraldAIObjectPool.SpawnEffect(CollisionSoundObject, transform.position, Quaternion.identity, 2);
                    CollisionSound.transform.SetParent(EmeraldAISystem.ObjectPool.transform);
                    AudioSource CollisionAudioSource = CollisionSound.GetComponent<AudioSource>();
                    CollisionAudioSource.PlayOneShot(ImpactSound);
                }

                //Damage AI target with damage based off of currently applied ability
                if (TargetTypeRef == TargetType.AI && TargetEmeraldSystem != null && TargetEmeraldSystem.LocationBasedDamageComp == null)
                {
                    if (AbilityType == AbilityTypeEnum.Damage && DamageType == DamageTypeEnum.Instant)
                    {
                        TargetEmeraldSystem.Damage(Damage, EmeraldAISystem.TargetType.AI, EmeraldSystem.transform, EmeraldSystem.SentRagdollForceAmount, CriticalHit);
                        EmeraldSystem.OnDoDamageEvent.Invoke();
                        if (CriticalHit)
                            EmeraldSystem.OnCriticalHitEvent.Invoke();
                    }
                    else if (AbilityType == AbilityTypeEnum.Damage && DamageType == DamageTypeEnum.OverTime)
                    {
                        //Apply the initial damage to our target
                        TargetEmeraldSystem.Damage(AbilityImpactDamage, EmeraldAISystem.TargetType.AI, EmeraldSystem.transform, EmeraldSystem.SentRagdollForceAmount);
                        EmeraldSystem.OnDoDamageEvent.Invoke();
                        if (AbilityStacksRef == Yes_No.No && !TargetEmeraldSystem.ActiveEffects.Contains(EmeraldSystem.CurrentlyCreatedAbility.AbilityName) && AbilityName != string.Empty || AbilityStacksRef == Yes_No.Yes)
                        {
                            //Initialize the damage over time component
                            GameObject SpawnedDamageOverTimeComponent = EmeraldAIObjectPool.Spawn(DamageOverTimeComponent, ProjectileCurrentTarget.position, Quaternion.identity);
                            SpawnedDamageOverTimeComponent.transform.SetParent(EmeraldAISystem.ObjectPool.transform);
                            SpawnedDamageOverTimeComponent.GetComponent<EmeraldAIDamageOverTime>().Initialize(AbilityName, AbilityDamagePerIncrement, AbilityDamageIncrement, AbilityLength, 
                                DamageOverTimeEffect, DamageOvertimeTimeout, DamageOverTimeSound, null, m_EmeraldAIPlayerDamage, TargetEmeraldSystem, EmeraldSystem, EmeraldSystem.CurrentTarget, (EmeraldAISystem.TargetType)TargetTypeRef);
                            TargetEmeraldSystem.ActiveEffects.Add(AbilityName);
                        }
                    }
                }
                else if (TargetTypeRef == TargetType.AI && TargetEmeraldSystem != null && TargetEmeraldSystem.LocationBasedDamageComp != null && C.GetComponent<LocationBasedDamageArea>())
                {
                    if (AbilityType == AbilityTypeEnum.Damage && DamageType == DamageTypeEnum.Instant)
                    {
                        C.GetComponent<LocationBasedDamageArea>().DamageArea(Damage, EmeraldAISystem.TargetType.AI, EmeraldSystem.transform, EmeraldSystem.SentRagdollForceAmount, CriticalHit);
                        EmeraldSystem.OnDoDamageEvent.Invoke();
                        if (CriticalHit)
                            EmeraldSystem.OnCriticalHitEvent.Invoke();
                    }
                    else if (AbilityType == AbilityTypeEnum.Damage && DamageType == DamageTypeEnum.OverTime)
                    {
                        //Apply the initial damage to our target
                        C.GetComponent<LocationBasedDamageArea>().DamageArea(AbilityImpactDamage, EmeraldAISystem.TargetType.AI, EmeraldSystem.transform, EmeraldSystem.SentRagdollForceAmount);
                        EmeraldSystem.OnDoDamageEvent.Invoke();
                        if (AbilityStacksRef == Yes_No.No && !TargetEmeraldSystem.ActiveEffects.Contains(EmeraldSystem.CurrentlyCreatedAbility.AbilityName) && AbilityName != string.Empty || AbilityStacksRef == Yes_No.Yes)
                        {
                            //Initialize the damage over time component
                            GameObject SpawnedDamageOverTimeComponent = EmeraldAIObjectPool.Spawn(DamageOverTimeComponent, ProjectileCurrentTarget.position, Quaternion.identity);
                            SpawnedDamageOverTimeComponent.transform.SetParent(EmeraldAISystem.ObjectPool.transform);
                            SpawnedDamageOverTimeComponent.GetComponent<EmeraldAIDamageOverTime>().Initialize(AbilityName, AbilityDamagePerIncrement, AbilityDamageIncrement, AbilityLength,
                                DamageOverTimeEffect, DamageOvertimeTimeout, DamageOverTimeSound, null, m_EmeraldAIPlayerDamage, TargetEmeraldSystem, EmeraldSystem, EmeraldSystem.CurrentTarget, (EmeraldAISystem.TargetType)TargetTypeRef);
                            SpawnedDamageOverTimeComponent.GetComponent<EmeraldAIDamageOverTime>().m_LocationBasedDamageArea = C.GetComponent<LocationBasedDamageArea>();
                            TargetEmeraldSystem.ActiveEffects.Add(AbilityName);
                        }
                    }
                }
                else if (TargetTypeRef == TargetType.Player) //Damage the Player with damage based off of currently applied ability
                {
                    if (AbilityType == AbilityTypeEnum.Damage && DamageType == DamageTypeEnum.Instant)
                    {
                        DamagePlayer(Damage);
                    }
                    else if (AbilityType == AbilityTypeEnum.Damage && DamageType == DamageTypeEnum.OverTime)
                    {
                        //Apply the initial damage to our target
                        DamagePlayer(AbilityImpactDamage);
                        if (AbilityStacksRef == Yes_No.No && !EmeraldSystem.CurrentTarget.GetComponent<EmeraldAIPlayerDamage>().ActiveEffects.Contains(EmeraldSystem.CurrentlyCreatedAbility.AbilityName) 
                            && AbilityName != string.Empty || AbilityStacksRef == Yes_No.Yes)
                        {
                            //Initialize the damage over time component
                            GameObject SpawnedDamageOverTimeComponent = EmeraldAIObjectPool.Spawn(DamageOverTimeComponent, ProjectileCurrentTarget.position, Quaternion.identity);
                            SpawnedDamageOverTimeComponent.transform.SetParent(EmeraldAISystem.ObjectPool.transform);
                            SpawnedDamageOverTimeComponent.GetComponent<EmeraldAIDamageOverTime>().Initialize(AbilityName, AbilityDamagePerIncrement, AbilityDamageIncrement, AbilityLength,
                                DamageOverTimeEffect, DamageOvertimeTimeout, DamageOverTimeSound, null, m_EmeraldAIPlayerDamage, TargetEmeraldSystem, EmeraldSystem, EmeraldSystem.CurrentTarget, (EmeraldAISystem.TargetType)TargetTypeRef);
                            EmeraldSystem.CurrentTarget.GetComponent<EmeraldAIPlayerDamage>().ActiveEffects.Add(AbilityName);
                        }
                    }
                }
                else if (TargetTypeRef == TargetType.NonAITarget) //Damage a non-AI target with damage based off of currently applied ability
                {
                    if (AbilityType == AbilityTypeEnum.Damage && DamageType == DamageTypeEnum.Instant)
                    {
                        DamageNonAITarget(Damage);
                    }
                    else if (AbilityType == AbilityTypeEnum.Damage && DamageType == DamageTypeEnum.OverTime)
                    {
                        //Apply the initial damage to our target
                        DamageNonAITarget(AbilityImpactDamage);

                        //Get a reference to the Non-AI Target component
                        EmeraldAINonAIDamage m_EmeraldAINonAIDamage = StartingTarget.GetComponent<EmeraldAINonAIDamage>();

                        if (AbilityStacksRef == Yes_No.No && !m_EmeraldAINonAIDamage.ActiveEffects.Contains(EmeraldSystem.CurrentlyCreatedAbility.AbilityName) && AbilityName != string.Empty || AbilityStacksRef == Yes_No.Yes)
                        {
                            //Initialize the damage over time component
                            GameObject SpawnedDamageOverTimeComponent = EmeraldAIObjectPool.Spawn(DamageOverTimeComponent, ProjectileCurrentTarget.position, Quaternion.identity);
                            SpawnedDamageOverTimeComponent.transform.SetParent(EmeraldAISystem.ObjectPool.transform);
                            SpawnedDamageOverTimeComponent.GetComponent<EmeraldAIDamageOverTime>().Initialize(AbilityName, AbilityDamagePerIncrement, AbilityDamageIncrement, AbilityLength,
                                DamageOverTimeEffect, DamageOvertimeTimeout, DamageOverTimeSound, m_EmeraldAINonAIDamage, m_EmeraldAIPlayerDamage, TargetEmeraldSystem, EmeraldSystem, EmeraldSystem.CurrentTarget, (EmeraldAISystem.TargetType)TargetTypeRef);
                            m_EmeraldAINonAIDamage.ActiveEffects.Add(AbilityName);
                        }
                    }
                }

                if (ObjectToDisableOnCollision != null)
                {
                    ObjectToDisableOnCollision.SetActive(false);
                }

                if (ArrowProjectileRef == ArrowObject.Yes)
                {
                    CollisionTime = 0;
                }
                Collided = true;
                ProjectileCollider.enabled = false;
            }
            else if (!Collided && EmeraldSystem != null && ProjectileCurrentTarget != null && C.gameObject != ProjectileCurrentTarget.gameObject && C.gameObject != EmeraldSystem.gameObject && C.gameObject.layer != 2)
            {
                Collided = true;
                ProjectileCollider.enabled = false;

                if (ObjectToDisableOnCollision != null)
                {
                    ObjectToDisableOnCollision.SetActive(false);
                }

                if (EffectOnCollisionRef == EffectOnCollision.Yes)
                {
                    if (CollisionEffect != null)
                    {
                        SpawnedEffect = EmeraldAIObjectPool.SpawnEffect(CollisionEffect, transform.position, Quaternion.identity, 2);
                        SpawnedEffect.transform.SetParent(EmeraldAISystem.ObjectPool.transform);
                    }
                }
                if (ImpactSound != null && SoundOnCollisionRef == EffectOnCollision.Yes)
                {
                    CollisionSound = EmeraldAIObjectPool.SpawnEffect(CollisionSoundObject, transform.position, Quaternion.identity, 2);
                    CollisionSound.transform.SetParent(EmeraldAISystem.ObjectPool.transform);
                    AudioSource CollisionAudioSource = CollisionSound.GetComponent<AudioSource>();
                    CollisionAudioSource.PlayOneShot(ImpactSound);
                }

                if (ArrowProjectileRef == ArrowObject.Yes && !C.CompareTag(EmeraldSystem.EmeraldTag))
                {
                    CollisionTime = 10;
                }
                else if (ArrowProjectileRef == ArrowObject.Yes && C.CompareTag(EmeraldSystem.EmeraldTag))
                {
                    CollisionTime = 0;
                }
            }
        }

        void DamagePlayer(int SentDamage)
        {
            if (StartingTarget.GetComponent<EmeraldAIPlayerDamage>() != null)
            {
                StartingTarget.GetComponent<EmeraldAIPlayerDamage>().SendPlayerDamage(SentDamage, EmeraldSystem.transform, EmeraldSystem, CriticalHit);                              
            }
            else
            {
                StartingTarget.gameObject.AddComponent<EmeraldAIPlayerDamage>();
                StartingTarget.GetComponent<EmeraldAIPlayerDamage>().SendPlayerDamage(SentDamage, EmeraldSystem.transform, EmeraldSystem, CriticalHit);
            }

            if (CriticalHit)
                EmeraldSystem.OnCriticalHitEvent.Invoke();

            EmeraldSystem.OnDoDamageEvent.Invoke();
            m_EmeraldAIPlayerDamage = StartingTarget.GetComponent<EmeraldAIPlayerDamage>();
        }

        void DamageNonAITarget(int SentDamage)
        {
            if (StartingTarget.GetComponent<EmeraldAINonAIDamage>() != null)
            {
                StartingTarget.GetComponent<EmeraldAINonAIDamage>().SendNonAIDamage(SentDamage, EmeraldSystem.transform);
            }
            else
            {
                StartingTarget.gameObject.AddComponent<EmeraldAINonAIDamage>();
                StartingTarget.GetComponent<EmeraldAINonAIDamage>().SendNonAIDamage(SentDamage, EmeraldSystem.transform);
            }

            if (CriticalHit)
                EmeraldSystem.OnCriticalHitEvent.Invoke();

            EmeraldSystem.OnDoDamageEvent.Invoke();
        }
    }
}
