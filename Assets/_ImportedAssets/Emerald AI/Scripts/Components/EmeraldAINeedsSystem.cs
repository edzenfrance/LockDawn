using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EmeraldAI
{
    public class EmeraldAINeedsSystem : MonoBehaviour
    {
        public UnityEvent GatherEvent;
        public LayerMask SearchLayerMask;
        public int SearchRadius = 50;
        public Color SearchRadiusColor = Color.yellow;
        public int WanderRadius = 50;
        public Color WanderRadiusColor = Color.green;
        public int MaxWaypoints = 5;
        public int UpdateResourcesFrequency = 5;
        public int ResourceUsage = 1;
        public int CurrentResourcesLevel = 20;
        public int ResourcesLowThreshold = 0;
        public int ResourcesFullThreshold = 30;
        public float ResourceRefillMultiplier = 1;
        public int SecondsNeededForDeath = 60;
        public enum DepletedResourcesKillsAIEnum { Yes, No };
        public DepletedResourcesKillsAIEnum DepletedResourcesKillsAI = DepletedResourcesKillsAIEnum.No;
        public int GatherResourceAnimationIndex = 1;
        public int IdleAnimationIndex = 1;

        EmeraldAISystem m_EmeraldAISystem;
        int m_TotalWaypoints;
        float m_UpdateNeedsTimer;
        float m_SecondsNeededForDeathTimer;
        Collider[] m_DetectedWaypointObjects;

        void Start()
        {
            m_EmeraldAISystem = GetComponent<EmeraldAISystem>();
            m_EmeraldAISystem.WanderRadius = WanderRadius;
            m_EmeraldAISystem.WanderTypeRef = EmeraldAISystem.WanderType.Stationary;
            m_EmeraldAISystem.WaypointsList.Clear();

            if (CurrentResourcesLevel <= ResourcesLowThreshold)
            {
                Invoke("UpdateWaypoints", 0.1f);
                m_EmeraldAISystem.EmeraldEventsManagerComponent.OverrideIdleAnimation(IdleAnimationIndex);
            }
            else
            {
                m_EmeraldAISystem.EmeraldEventsManagerComponent.UpdateStartingPosition();
                m_EmeraldAISystem.WanderTypeRef = EmeraldAISystem.WanderType.Dynamic;                
            }

            InvokeRepeating("UpdateNeeds", 0.1f, UpdateResourcesFrequency);
        }

        void UpdateWaypoints()
        {
            if (m_EmeraldAISystem.CombatStateRef == EmeraldAISystem.CombatState.NotActive)
            {
                if (m_TotalWaypoints < MaxWaypoints)
                {
                    m_DetectedWaypointObjects = Physics.OverlapSphere(transform.position, SearchRadius, SearchLayerMask);

                    if (m_DetectedWaypointObjects.Length == 0)
                    {
                        Invoke("UpdateWaypoints", 10);
                        m_EmeraldAISystem.WanderTypeRef = EmeraldAISystem.WanderType.Dynamic;
                        return;
                    }

                    foreach (Collider C in m_DetectedWaypointObjects)
                    {
                        if (!m_EmeraldAISystem.WaypointsList.Contains(C.transform.position))
                        {
                            m_EmeraldAISystem.WaypointsList.Add(C.transform.position);
                            m_TotalWaypoints++;
                        }
                    }
                }

                m_EmeraldAISystem.m_NavMeshAgent.autoBraking = false;
                m_EmeraldAISystem.m_NavMeshAgent.ResetPath();
                m_EmeraldAISystem.EmeraldEventsManagerComponent.OverrideIdleAnimation(GatherResourceAnimationIndex);
                if (m_DetectedWaypointObjects.Length > 1)
                {
                    m_EmeraldAISystem.WanderTypeRef = EmeraldAISystem.WanderType.Waypoints;
                    m_EmeraldAISystem.WaypointTypeRef = EmeraldAISystem.WaypointType.Random;
                    m_EmeraldAISystem.WaypointIndex = Random.Range(0, m_EmeraldAISystem.WaypointsList.Count);
                    m_EmeraldAISystem.m_NavMeshAgent.destination = m_EmeraldAISystem.WaypointsList[m_EmeraldAISystem.WaypointIndex];
                }
                else if (m_DetectedWaypointObjects.Length == 1)
                {
                    m_EmeraldAISystem.WanderTypeRef = EmeraldAISystem.WanderType.Waypoints;
                    m_EmeraldAISystem.WaypointTypeRef = EmeraldAISystem.WaypointType.Loop;
                    m_EmeraldAISystem.WaypointIndex = Random.Range(0, m_EmeraldAISystem.WaypointsList.Count);
                    m_EmeraldAISystem.m_NavMeshAgent.destination = m_EmeraldAISystem.WaypointsList[m_EmeraldAISystem.WaypointIndex];
                }
                m_EmeraldAISystem.WaypointTimer = 0;
            }
        }

        void UpdateNeeds()
        {
            if (m_EmeraldAISystem.CombatStateRef == EmeraldAISystem.CombatState.NotActive && m_EmeraldAISystem.WanderTypeRef != EmeraldAISystem.WanderType.Waypoints)
            {
                CurrentResourcesLevel -= ResourceUsage;

                if (CurrentResourcesLevel <= ResourcesLowThreshold)
                {
                    UpdateWaypoints();
                }
            }
        }

        void Update()
        {
            if (m_EmeraldAISystem.CombatStateRef == EmeraldAISystem.CombatState.NotActive && m_EmeraldAISystem.WanderTypeRef == EmeraldAISystem.WanderType.Waypoints)
            {
                if (m_EmeraldAISystem.m_NavMeshAgent.enabled && m_EmeraldAISystem.m_NavMeshAgent.remainingDistance <= m_EmeraldAISystem.StoppingDistance)
                {
                    m_UpdateNeedsTimer += Time.deltaTime * ResourceRefillMultiplier;

                    if (m_UpdateNeedsTimer >= 0.95f)
                    {
                        if (m_EmeraldAISystem.WaypointTypeRef == EmeraldAISystem.WaypointType.Loop)
                        {
                            m_EmeraldAISystem.AIAnimator.SetBool("Idle Active", true);
                        }
                        CurrentResourcesLevel++;
                        GatherEvent.Invoke();
                        m_UpdateNeedsTimer = 0;
                    }

                    if (CurrentResourcesLevel >= ResourcesFullThreshold)
                    {
                        m_EmeraldAISystem.EmeraldEventsManagerComponent.OverrideIdleAnimation(IdleAnimationIndex);
                        m_EmeraldAISystem.EmeraldEventsManagerComponent.UpdateStartingPosition();
                        m_EmeraldAISystem.WanderTypeRef = EmeraldAISystem.WanderType.Dynamic;
                    }
                }
            }

            if (DepletedResourcesKillsAI == DepletedResourcesKillsAIEnum.Yes && CurrentResourcesLevel <= 0)
            {
                m_SecondsNeededForDeathTimer += Time.deltaTime;

                if (m_SecondsNeededForDeathTimer >= SecondsNeededForDeath)
                {
                    m_EmeraldAISystem.EmeraldEventsManagerComponent.KillAI();
                    this.enabled = false;
                }
            }
        }
    }
}