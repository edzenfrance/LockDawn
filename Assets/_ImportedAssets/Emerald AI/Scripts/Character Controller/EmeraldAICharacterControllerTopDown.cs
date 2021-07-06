using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using EmeraldAI;
using EmeraldAI.Example;

namespace EmeraldAI.CharacterController
{
    public class EmeraldAICharacterControllerTopDown : MonoBehaviour
    {
        [Header("Movement")]
        public LayerMask MovementMask = 2;              
        public Camera PlayerCamera;       
        public float StoppingDistanceMovement = 1;
        public float StoppingDistanceCombat = 2;
        public float RotationSpeedCombat = 10;

        [Header("Combat")]
        public float AttackSpeed = 1;
        public int MinDamage = 5;
        public int MaxDamage = 10;
        public int CritChance = 15;
        public float CritMultiplier = 2.5f;
        public string TargetTag = "";
        public int HealthRecoveryRate = 1;

        [Header("Effects & Indicators")]
        public GameObject ImpactEffect;
        public GameObject DestinationEffect;
        public enum TargetIndicatorEnum { ObjectRing, Shader };
        public TargetIndicatorEnum TargetIndicatorType = TargetIndicatorEnum.ObjectRing;
        public Color TargetIndicatorColor = Color.red;
        public enum CameraShakeEnum { Enabled, Disabled };
        public CameraShakeEnum CameraShakeState = CameraShakeEnum.Enabled;

        [Header("Sounds")]
        public List<AudioClip> FootstepSounds = new List<AudioClip>();
        public List<AudioClip> AttackSounds = new List<AudioClip>();
        public List<AudioClip> ImpactSounds = new List<AudioClip>();
        public List<AudioClip> CriticalHitSounds = new List<AudioClip>();
        public List<AudioClip> InjuredSounds = new List<AudioClip>();        

        EmeraldAISystem EmeraldComponent;
        Coroutine FadeHUDCoroutine;
        GameObject HUDObject;
        CanvasGroup m_CanvasGroup;
        Text AINameText;
        Image AIHealthBar;
        Vector3 AdjustTargetPosition;
        int Damage = 5;
        NavMeshAgent m_NavMeshAgent;
        Animator m_AnimatorController;
        Vector3 MousePosition;
        Transform CurrentTarget;
        float m_AttackTimer;
        float m_UpdateTargetPosition;
        AudioSource m_AudioSource;
        bool m_ClickEffectUsed;
        Vector3 m_LastMousePosition;
        float m_UpdatePositionTimer;
        float m_MouseUpClickEffectTimer;
        int CurrentHealth;
        float HealthRecoveryTimer;
        float UpdatePositionThreshold = 0.09f;
        GameObject TargetIndicator;
        Renderer PlayerRenderer;
        EmeraldAIPlayerHealth PlayerHealthComponent;

        //Initialize the player controller
        void Start()
        {
            PlayerRenderer = GetComponentInChildren<Renderer>();
            PlayerHealthComponent = GetComponent<EmeraldAIPlayerHealth>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_AnimatorController = GetComponent<Animator>();
            m_AudioSource = GetComponent<AudioSource>();
            UpdateAttackSpeed();

            HUDObject = Instantiate(Resources.Load("HUD Canvas") as GameObject, Vector3.zero, Quaternion.identity);
            m_CanvasGroup = HUDObject.GetComponent<CanvasGroup>();
            AINameText = GameObject.Find("HUD - AI Name").GetComponent<Text>();
            AIHealthBar = GameObject.Find("HUD - AI Health Bar").GetComponent<Image>();
            HUDObject.SetActive(false);
            TargetIndicator = Instantiate(Resources.Load("Target Indicator") as GameObject, Vector3.zero, Quaternion.AngleAxis(90, Vector3.right));
            TargetIndicator.GetComponent<Renderer>().material.color = TargetIndicatorColor;
            TargetIndicator.SetActive(false);
            Instantiate(Resources.Load("Player UI") as GameObject, Vector3.zero, Quaternion.identity);

            if (CameraShakeState == CameraShakeEnum.Enabled)
            {
                Instantiate(Resources.Load("Camera Shake System") as GameObject, Vector3.zero, Quaternion.identity);
                CameraShake.Instance.CameraTransform = PlayerCamera.transform;
                CameraShake.Instance.name = "Camera Shake System";
            }
        }


        void Update()
        {
            m_AnimatorController.SetFloat("Speed", m_NavMeshAgent.velocity.magnitude / m_NavMeshAgent.speed - (0.4f), 0.05f, Time.deltaTime);

            if (CurrentTarget != null)
            {
                m_UpdateTargetPosition += Time.deltaTime;
                if (m_UpdateTargetPosition >= 1)
                {
                    m_NavMeshAgent.SetDestination(CurrentTarget.position);
                    m_UpdateTargetPosition = 0;
                }

                if (m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance && !m_NavMeshAgent.pathPending)
                {
                    m_NavMeshAgent.updateRotation = false;
                    float step = RotationSpeedCombat * Time.deltaTime;
                    Vector3 m_TargetDirection = CurrentTarget.position - transform.position;
                    m_TargetDirection.y = 0;
                    Vector3 m_NewDirection = Vector3.RotateTowards(transform.forward, m_TargetDirection, step, 0.0f);
                    transform.rotation = Quaternion.LookRotation(m_NewDirection);

                    m_AttackTimer += Time.deltaTime;
                    if (m_AttackTimer >= AttackSpeed)
                    {
                        PlayAttackSound();
                        m_AnimatorController.SetTrigger("Attack");
                        m_AttackTimer = 0;
                    }
                }
                else
                {
                    m_NavMeshAgent.updateRotation = true;
                }
            }

            if (Input.GetMouseButtonDown(1) && m_AnimatorController.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Attack")
            {
                m_AttackTimer = AttackSpeed;
            }

            if (Input.GetMouseButton(0))
            {
                m_UpdatePositionTimer += Time.deltaTime;
                m_MouseUpClickEffectTimer += Time.deltaTime;

                if (m_UpdatePositionTimer >= UpdatePositionThreshold)
                {
                    if (m_AnimatorController.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Attack")
                    {
                        Ray m_Ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit m_hit;
                        if (Physics.Raycast(m_Ray, out m_hit, 1000.0f, MovementMask))
                        {
                            if (CurrentTarget == null)
                            {
                                if (!m_hit.collider.CompareTag(TargetTag))
                                {
                                    if (!m_ClickEffectUsed && DestinationEffect != null)
                                    {
                                        EmeraldAI.Utility.EmeraldAIObjectPool.SpawnEffect(DestinationEffect, m_hit.point + Vector3.up * 0.5f, Quaternion.identity, 2);
                                        m_ClickEffectUsed = true;
                                    }

                                    m_NavMeshAgent.updateRotation = true;
                                    m_NavMeshAgent.stoppingDistance = StoppingDistanceMovement;
                                    m_LastMousePosition = m_hit.point;

                                    if (Vector3.Distance(transform.position, m_hit.point) > StoppingDistanceMovement + 1)
                                    {
                                        m_NavMeshAgent.SetDestination(m_hit.point);
                                    }
                                }
                                else
                                {
                                    m_AnimatorController.ResetTrigger("Hit");
                                    m_NavMeshAgent.updateRotation = false;
                                    m_NavMeshAgent.stoppingDistance = StoppingDistanceCombat;
                                    CurrentTarget = m_hit.transform;
                                    if (TargetIndicatorType == TargetIndicatorEnum.Shader)
                                    {
                                        EmeraldAISystem m_EmeraldComponent = CurrentTarget.GetComponent<EmeraldAISystem>();
                                        m_EmeraldComponent.AIRenderer.material.SetColor("_OutlineColor", TargetIndicatorColor);
                                        m_EmeraldComponent.AIRenderer.material.SetInt("_OutlineEnabled", 1);
                                        m_EmeraldComponent.AIRenderer.material.SetFloat("_OutlineWidth", 0.1f);
                                    }
                                    m_NavMeshAgent.SetDestination(m_hit.point);
                                    m_AttackTimer = AttackSpeed;
                                    m_UpdateTargetPosition = 1f;
                                    if (TargetIndicatorType == TargetIndicatorEnum.ObjectRing)
                                    {
                                        TargetIndicator.transform.SetParent(CurrentTarget);
                                        TargetIndicator.transform.localPosition = new Vector3(0, 0.25f, 0);
                                        TargetIndicator.SetActive(true);
                                    }
                                    EmeraldComponent = CurrentTarget.GetComponent<EmeraldAISystem>();
                                    UpdateHUD();
                                    StopHUDFade();
                                }
                            }
                            else if (m_hit.collider.CompareTag(TargetTag) && m_hit.transform != CurrentTarget)
                            {
                                m_AnimatorController.ResetTrigger("Hit");
                                m_NavMeshAgent.updateRotation = false;
                                m_NavMeshAgent.stoppingDistance = StoppingDistanceCombat;
                                if (TargetIndicatorType == TargetIndicatorEnum.Shader)
                                {
                                    EmeraldAISystem m_EmeraldComponent = CurrentTarget.GetComponent<EmeraldAISystem>();
                                    m_EmeraldComponent.AIRenderer.material.SetInt("_OutlineEnabled", 0);
                                }
                                CurrentTarget = m_hit.transform;
                                if (TargetIndicatorType == TargetIndicatorEnum.Shader)
                                {
                                    EmeraldAISystem m_EmeraldComponent = CurrentTarget.GetComponent<EmeraldAISystem>();
                                    m_EmeraldComponent.AIRenderer.material.SetColor("_OutlineColor", TargetIndicatorColor);
                                    m_EmeraldComponent.AIRenderer.material.SetInt("_OutlineEnabled", 1);
                                    m_EmeraldComponent.AIRenderer.material.SetFloat("_OutlineWidth", 0.1f);
                                }
                                m_NavMeshAgent.SetDestination(m_hit.point);
                                m_AttackTimer = AttackSpeed;
                                m_UpdateTargetPosition = 1f;
                                if (TargetIndicatorType == TargetIndicatorEnum.ObjectRing)
                                {
                                    TargetIndicator.transform.SetParent(CurrentTarget);
                                    TargetIndicator.transform.localPosition = new Vector3(0, 0.25f, 0);
                                    TargetIndicator.SetActive(true);
                                }
                                EmeraldComponent = CurrentTarget.GetComponent<EmeraldAISystem>();
                                UpdateHUD();
                                StopHUDFade();
                            }
                            else if (!m_hit.collider.CompareTag(TargetTag) && CurrentTarget != null && Vector3.Distance(CurrentTarget.position, m_hit.point) > 3.5f)
                            {
                                if (!m_ClickEffectUsed && DestinationEffect != null)
                                {
                                    EmeraldAI.Utility.EmeraldAIObjectPool.SpawnEffect(DestinationEffect, m_hit.point + Vector3.up * 0.5f, Quaternion.identity, 2);
                                    m_ClickEffectUsed = true;
                                }

                                if (m_CanvasGroup.alpha == 1)
                                {
                                    FadeHUDCoroutine = StartCoroutine(FadeHUD(2, 1));
                                }

                                m_AnimatorController.ResetTrigger("Hit");
                                EmeraldAISystem m_EmeraldComponent = CurrentTarget.GetComponent<EmeraldAISystem>();
                                if (TargetIndicatorType == TargetIndicatorEnum.Shader)
                                {
                                    m_EmeraldComponent.AIRenderer.material.SetInt("_OutlineEnabled", 0);
                                }
                                CurrentTarget = null;
                                m_NavMeshAgent.updateRotation = true;
                                m_NavMeshAgent.stoppingDistance = StoppingDistanceMovement;
                                m_LastMousePosition = m_hit.point;
                                m_NavMeshAgent.SetDestination(m_hit.point);
                            }
                        }

                        m_UpdatePositionTimer = 0;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (TargetIndicatorType == TargetIndicatorEnum.Shader && CurrentTarget != null)
                {
                    EmeraldAISystem m_EmeraldComponent = CurrentTarget.GetComponent<EmeraldAISystem>();
                    m_EmeraldComponent.AIRenderer.material.SetInt("_OutlineEnabled", 0);
                    StopHUDFade();
                    FadeHUDCoroutine = StartCoroutine(FadeHUD(2, 1));
                    m_NavMeshAgent.SetDestination(transform.position);
                    CurrentTarget = null;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (DestinationEffect != null && m_MouseUpClickEffectTimer > 1)
                {
                    EmeraldAI.Utility.EmeraldAIObjectPool.SpawnEffect(DestinationEffect, m_LastMousePosition + Vector3.up * 0.5f, Quaternion.identity, 1);
                }

                m_NavMeshAgent.updateRotation = true;
                m_ClickEffectUsed = false;
                m_MouseUpClickEffectTimer = 0;

                if (CurrentTarget == null && TargetIndicatorType == TargetIndicatorEnum.ObjectRing)
                {
                    TargetIndicator.SetActive(false);
                }
            }

            HealthRecoveryTimer += Time.deltaTime;

            if (HealthRecoveryTimer >= 1 && PlayerHealthComponent.CurrentHealth < PlayerHealthComponent.StartingHealth)
            {
                PlayerHealthComponent.CurrentHealth += HealthRecoveryRate;
                UpdatePlayerHealthOrbUI.Instance.UpdateHealthUI();
                HealthRecoveryTimer = 0;
            }
        }

        void UpdateHUD()
        {
            m_CanvasGroup.alpha = 1;
            HUDObject.SetActive(true);
            AINameText.text = EmeraldComponent.AIName;
            AIHealthBar.fillAmount = (float)EmeraldComponent.CurrentHealth / EmeraldComponent.StartingHealth;
        }

        void StopHUDFade()
        {
            if (FadeHUDCoroutine != null)
            {
                StopCoroutine(FadeHUDCoroutine);
            }
        }

        IEnumerator FadeHUD(float Delay, float TransitionTime)
        {
            yield return new WaitForSeconds(Delay);
            float LerpValue = 1;
            float t = 0;

            while ((t / TransitionTime) < 1)
            {
                t += Time.deltaTime;
                LerpValue = Mathf.Lerp(1, 0, t / TransitionTime);
                m_CanvasGroup.alpha = LerpValue;

                yield return null;
            }

            HUDObject.SetActive(false);
            m_CanvasGroup.alpha = 1;
        }

        bool GenerateCritOdds()
        {
            bool CriticalHit = false;
            float m_GeneratedOdds = Random.Range(0.0f, 1.0f);
            m_GeneratedOdds = Mathf.RoundToInt(m_GeneratedOdds * 100);

            if (m_GeneratedOdds <= CritChance)
            {
                CriticalHit = true;
            }

            return CriticalHit;
        }

        public void DamagePlayer(int DamageAmount)
        {
            PlayInjuredSound();
            PlayHitAnimation();
            PlayerRenderer.material.SetFloat("_DamageFlash", 0.75f);
            StartCoroutine(DamageFlash(PlayerRenderer));

            CurrentHealth -= DamageAmount;
            UpdatePlayerHealthOrbUI.Instance.UpdateHealthUI();
        }

        public void PlayHitAnimation()
        {
            if (m_NavMeshAgent.velocity.magnitude == 0)
            {
                m_AnimatorController.SetTrigger("Hit");
            }
        }

        public void UpdateAttackSpeed()
        {
            float AdjustedAttackSpeed = Mathf.Round(1 / AttackSpeed * 100) / 100;
            m_AnimatorController.SetFloat("Attack Speed", AdjustedAttackSpeed);
        }

        public void PlayImpactSound()
        {
            if (ImpactSounds.Count > 0)
            {
                AudioClip RandomImpactSound = ImpactSounds[Random.Range(0, ImpactSounds.Count)];

                if (RandomImpactSound != null)
                {
                    m_AudioSource.PlayOneShot(RandomImpactSound);
                }
            }
        }

        public void PlayCriticalHitSound()
        {
            if (CriticalHitSounds.Count > 0)
            {
                AudioClip RandomCriticalHitSound = CriticalHitSounds[Random.Range(0, CriticalHitSounds.Count)];

                if (RandomCriticalHitSound != null)
                {
                    m_AudioSource.PlayOneShot(RandomCriticalHitSound);
                }
            }
        }

        public void PlayAttackSound()
        {
            if (AttackSounds.Count > 0)
            {
                AudioClip RandomAttackSound = AttackSounds[Random.Range(0, AttackSounds.Count)];

                if (RandomAttackSound != null)
                {
                    m_AudioSource.PlayOneShot(RandomAttackSound);
                }
            }
        }

        public void PlayInjuredSound()
        {
            if (InjuredSounds.Count > 0)
            {
                AudioClip RandomInjuredSound = InjuredSounds[Random.Range(0, InjuredSounds.Count)];

                if (RandomInjuredSound != null)
                {
                    m_AudioSource.PlayOneShot(RandomInjuredSound);
                }
            }
        }

        public void PlayFootstepSound()
        {
            if (FootstepSounds.Count > 0)
            {
                AudioClip RandomFootstepSound = FootstepSounds[Random.Range(0, FootstepSounds.Count)];

                if (RandomFootstepSound != null)
                {
                    m_AudioSource.PlayOneShot(RandomFootstepSound);
                }
            }
        }

        IEnumerator DamageFlash(Renderer TargetRenderer)
        {
            float t = 0;
            float LerpValue = 0;

            while ((t / 0.1f) < 1)
            {
                t += Time.deltaTime;
                LerpValue = Mathf.Lerp(0.75f, 0, t / 0.1f);
                TargetRenderer.material.SetFloat("_DamageFlash", LerpValue);

                yield return null;
            }
        }

        public void AttackTarget()
        {
            if (CurrentTarget != null)
            {
                if (Vector3.Distance(transform.position, CurrentTarget.position) <= StoppingDistanceCombat)
                {
                    Damage = Random.Range(MinDamage, MaxDamage);
                    bool CriticalHit = GenerateCritOdds();
                    if (CriticalHit)
                    {
                        Damage = Mathf.RoundToInt(Damage * CritMultiplier);
                        if (CameraShake.Instance != null)
                        {
                            CameraShake.Instance.ShakeCamera(0.32f, 0.35f);
                        }
                        PlayCriticalHitSound();
                    }
                    else
                    {
                        PlayImpactSound();
                    }

                    CombatTextSystem.Instance.CreateCombatText(Damage, CurrentTarget.position, CriticalHit, false, false);
                    EmeraldAISystem m_EmeraldComponent = CurrentTarget.GetComponent<EmeraldAISystem>();
                    m_EmeraldComponent.Damage(Damage, EmeraldAISystem.TargetType.Player, transform, 300);

                    m_EmeraldComponent.AIRenderer.material.SetFloat("_DamageFlash", 0.75f);
                    StartCoroutine(DamageFlash(m_EmeraldComponent.AIRenderer));

                    UpdateHUD();

                    if (ImpactEffect != null)
                    {
                        EmeraldAI.Utility.EmeraldAIObjectPool.SpawnEffect(ImpactEffect, CurrentTarget.position + new Vector3(0, CurrentTarget.localScale.y * 0.5f, 0), Quaternion.identity, 1);
                    }

                    if (m_EmeraldComponent.IsDead)
                    {
                        StopHUDFade();
                        FadeHUDCoroutine = StartCoroutine(FadeHUD(2, 1));

                        if (TargetIndicatorType == TargetIndicatorEnum.Shader)
                        {
                            m_EmeraldComponent = CurrentTarget.GetComponent<EmeraldAISystem>();
                            m_EmeraldComponent.AIRenderer.material.SetInt("_OutlineEnabled", 0);
                        }
                        CurrentTarget = null;
                        if (TargetIndicatorType == TargetIndicatorEnum.ObjectRing)
                        {
                            TargetIndicator.SetActive(false);
                        }
                        return;
                    }

                    PlayImpactSound();
                }
            }
        }
    }
}