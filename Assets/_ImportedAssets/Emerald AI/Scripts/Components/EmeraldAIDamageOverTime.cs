using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Utility
{
    public class EmeraldAIDamageOverTime : MonoBehaviour
    {
        [HideInInspector]
        public LocationBasedDamageArea m_LocationBasedDamageArea;
        [HideInInspector]
        public int m_DamageAmount;
        [HideInInspector]
        public float m_DamageIncrement;
        [HideInInspector]
        public float m_AbilityLength;
        [HideInInspector]
        public string m_AbilityName;
        [HideInInspector]
        public GameObject m_DamageOverTimeEffect;
        [HideInInspector]
        public AudioClip m_DamageOverTimeSound;
        [HideInInspector]
        public EmeraldAISystem m_TargetEmeraldComponent;       
        bool EmeraldAITarget;
        float DamageTimer;
        float ActiveLengthTimer;
        float m_DamageOverTimeTimeout;
        AudioSource m_AudioSource;
        Vector3 m_EffectPosition;
        Transform m_TargetTransform;
        EmeraldAISystem.TargetType m_TargetType;
        EmeraldAINonAIDamage m_NonAIDamageComponent;
        EmeraldAIPlayerDamage m_EmeraldAIPlayerDamage;
        EmeraldAISystem m_AttackerEmeraldComponent;

        void Start()
        {
            m_AudioSource = gameObject.AddComponent<AudioSource>();
        }

        //Initialize our damage over time spell
        public void Initialize (string AbilityName, int DamageAmount, float DamageIncrement, float AbilityLength, GameObject DamageOverTimeEffect, float DamageOvertimeTimeout, AudioClip DamageOverTimeSound, EmeraldAINonAIDamage NonAIDamageComponent,
            EmeraldAIPlayerDamage PlayerDamageComponent, EmeraldAISystem TargetEmeraldComponent, EmeraldAISystem AttackerEmeraldComponent, Transform TargetTransform, EmeraldAISystem.TargetType TargetType)
        {
            m_DamageOverTimeEffect = DamageOverTimeEffect;
            m_DamageOverTimeTimeout = DamageOvertimeTimeout;
            m_DamageOverTimeSound = DamageOverTimeSound;
            m_AbilityName = AbilityName;
            m_DamageAmount = DamageAmount;
            m_DamageIncrement = DamageIncrement;
            m_AbilityLength = AbilityLength;
            m_TargetEmeraldComponent = TargetEmeraldComponent;
            m_AttackerEmeraldComponent = AttackerEmeraldComponent;
            m_TargetType = TargetType;
            m_TargetTransform = TargetTransform;
            m_EmeraldAIPlayerDamage = PlayerDamageComponent;
            m_NonAIDamageComponent = NonAIDamageComponent;
        }

        void OnEnable()
        {
            DamageTimer = 0;
            ActiveLengthTimer = 0;
        }

        void Update()
        {
            DamageTimer += Time.deltaTime;
            ActiveLengthTimer += Time.deltaTime;

            if (ActiveLengthTimer >= m_AbilityLength + 0.05f)
            {
                if (m_TargetType == EmeraldAISystem.TargetType.AI)
                {
                    if (m_TargetEmeraldComponent.ActiveEffects.Contains(m_AbilityName))
                    {
                        m_TargetEmeraldComponent.ActiveEffects.Remove(m_AbilityName);
                    }
                }
                else if (m_TargetType == EmeraldAISystem.TargetType.Player)
                {
                    if (m_EmeraldAIPlayerDamage.ActiveEffects.Contains(m_AbilityName))
                    {
                        m_EmeraldAIPlayerDamage.ActiveEffects.Remove(m_AbilityName);
                    }
                }

                if (!m_AudioSource.isPlaying)
                {
                    EmeraldAIObjectPool.Despawn(gameObject);
                }
            }

            if (DamageTimer >= m_DamageIncrement && ActiveLengthTimer <= m_AbilityLength + 0.05f)
            {
                if (m_DamageOverTimeEffect != null)
                {
                    if (m_TargetType == EmeraldAISystem.TargetType.AI)
                    {
                        m_EffectPosition = m_TargetEmeraldComponent.transform.position + new Vector3 (0, m_TargetEmeraldComponent.HitPointTransform.localPosition.y, 0);
                    }
                    else
                    {
                        m_EffectPosition = m_TargetTransform.position;
                    }

                    EmeraldAIObjectPool.SpawnEffect(m_DamageOverTimeEffect, m_EffectPosition, Quaternion.identity, m_DamageOverTimeTimeout);
                }

                if (m_DamageOverTimeSound != null)
                {
                    m_AudioSource.PlayOneShot(m_DamageOverTimeSound);
                }

                //Apply damage over time to another AI
                if (m_TargetType == EmeraldAISystem.TargetType.AI && !m_TargetEmeraldComponent.LocationBasedDamageComp)
                {
                    m_TargetEmeraldComponent.Damage(m_DamageAmount);
                    m_AttackerEmeraldComponent.OnDoDamageEvent.Invoke();
                }
                else if (m_TargetType == EmeraldAISystem.TargetType.AI && m_TargetEmeraldComponent.LocationBasedDamageComp)
                {
                    m_LocationBasedDamageArea.DamageArea(m_DamageAmount);
                    m_AttackerEmeraldComponent.OnDoDamageEvent.Invoke();
                }
                else if (m_TargetType == EmeraldAISystem.TargetType.Player) //Apply damage over time to the player
                {
                    if (m_TargetTransform.GetComponent<EmeraldAIPlayerDamage>() != null)
                    {
                        m_TargetTransform.GetComponent<EmeraldAIPlayerDamage>().SendPlayerDamage(m_DamageAmount, m_AttackerEmeraldComponent.transform, m_AttackerEmeraldComponent);
                        m_AttackerEmeraldComponent.OnDoDamageEvent.Invoke();
                    }
                    else
                    {
                        m_TargetTransform.gameObject.AddComponent<EmeraldAIPlayerDamage>();
                        m_TargetTransform.GetComponent<EmeraldAIPlayerDamage>().SendPlayerDamage(m_DamageAmount, m_AttackerEmeraldComponent.transform, m_AttackerEmeraldComponent);
                        m_AttackerEmeraldComponent.OnDoDamageEvent.Invoke();
                    }
                }
                else if (m_TargetType == EmeraldAISystem.TargetType.NonAITarget) //Apply the damage over time to a non-AI target
                {
                    if (m_TargetTransform.GetComponent<EmeraldAINonAIDamage>() != null)
                    {
                        m_TargetTransform.GetComponent<EmeraldAINonAIDamage>().SendNonAIDamage(m_DamageAmount, m_AttackerEmeraldComponent.transform);
                        m_AttackerEmeraldComponent.OnDoDamageEvent.Invoke();
                    }
                    else
                    {
                        m_TargetTransform.gameObject.AddComponent<EmeraldAINonAIDamage>();
                        m_TargetTransform.GetComponent<EmeraldAINonAIDamage>().SendNonAIDamage(m_DamageAmount, m_AttackerEmeraldComponent.transform);
                        m_AttackerEmeraldComponent.OnDoDamageEvent.Invoke();
                    }
                }

                DamageTimer = 0;
            }

            if (m_TargetType == EmeraldAISystem.TargetType.AI && m_TargetEmeraldComponent.IsDead)
            {
                if (m_TargetEmeraldComponent.ActiveEffects.Contains(m_AbilityName))
                {
                    m_TargetEmeraldComponent.ActiveEffects.Remove(m_AbilityName);
                }
                EmeraldAIObjectPool.Despawn(gameObject);
            }
            else if (m_TargetType == EmeraldAISystem.TargetType.NonAITarget && m_NonAIDamageComponent.Health <= 0)
            {
                if (m_NonAIDamageComponent.ActiveEffects.Contains(m_AbilityName))
                {
                    m_NonAIDamageComponent.ActiveEffects.Remove(m_AbilityName);
                }
                EmeraldAIObjectPool.Despawn(gameObject);
            }
        }
    }
}