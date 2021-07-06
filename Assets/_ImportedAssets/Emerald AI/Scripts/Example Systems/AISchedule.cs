using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI;

namespace EmeraldAI.Example
{
    public class AISchedule : MonoBehaviour
    {
        public WanderType MorningWanderType = WanderType.Stationary;
        public Transform MorningLocation;
        public int IdleAnimationMorning = 1; //Idle animations are 1 through 3

        public WanderType DayWanderType = WanderType.Dynamic;
        public Transform DayLocation;
        public int IdleAnimationDay = 1; //Idle animations are 1 through 3

        public WanderType EveningWanderType = WanderType.Stationary;
        public Transform EveningLocation;
        public int IdleAnimationEvening = 1; //Idle animations are 1 through 3

        public WanderType NightWanderType = WanderType.Stationary;
        public Transform NightLocation;
        public int IdleAnimationNight = 1; //Idle animations are 1 through 3

        public enum WanderType { Dynamic = 0, Stationary = 2 };

        public CurrentScheduleEneum CurrentSchedule = CurrentScheduleEneum.Morning;
        public enum CurrentScheduleEneum { Morning = 0, Day = 1, Evening = 2, Night = 3 };

        EmeraldAISystem m_EmeraldAISystem;
        EmeraldAIEventsManager m_EmeraldAIEventsManager;

        void Start()
        {
            m_EmeraldAISystem = GetComponent<EmeraldAISystem>();
            m_EmeraldAIEventsManager = GetComponent<EmeraldAIEventsManager>();
            InvokeRepeating("UpdateAISchedule", 0.1f, 1);
        }

        void UpdateAISchedule()
        {
            if (!m_EmeraldAISystem.IsDead)
            {
                if (CurrentSchedule == CurrentScheduleEneum.Morning && m_EmeraldAISystem.StartingDestination.x != MorningLocation.position.x)
                {
                    m_EmeraldAIEventsManager.ChangeWanderType((EmeraldAISystem.WanderType)MorningWanderType); //Set our AI's Wandering Type for this time of day
                    m_EmeraldAISystem.m_NavMeshAgent.ResetPath(); //Reset our path so the previous destination doesn't interfere with the new one
                    m_EmeraldAIEventsManager.SetDestination(MorningLocation); //Set our destination for this time of day
                    m_EmeraldAISystem.StartingDestination = MorningLocation.position; //Set our dynamic wandering destination for this time of day
                    m_EmeraldAIEventsManager.OverrideIdleAnimation(IdleAnimationMorning); //Set our Idle Animation Index for this time of day
                    m_EmeraldAISystem.WaitTime = 2; //Dynamic - Set our Wait Time to 2 so the AI starts its animation 2 seconds after arrive to its new destination
                    m_EmeraldAISystem.StationaryIdleSeconds = 2; //Stationary - Set our Wait Time to 2 so the AI starts its animation 2 seconds after arrive to its new destination
                }
                else if (CurrentSchedule == CurrentScheduleEneum.Day && m_EmeraldAISystem.StartingDestination.x != DayLocation.position.x)
                {
                    m_EmeraldAIEventsManager.ChangeWanderType((EmeraldAISystem.WanderType)DayWanderType);
                    m_EmeraldAISystem.m_NavMeshAgent.ResetPath();
                    m_EmeraldAIEventsManager.SetDestination(DayLocation);
                    m_EmeraldAISystem.StartingDestination = DayLocation.position;
                    m_EmeraldAIEventsManager.OverrideIdleAnimation(IdleAnimationDay);
                    m_EmeraldAISystem.WaitTime = 2;
                    m_EmeraldAISystem.StationaryIdleSeconds = 2;
                }
                else if (CurrentSchedule == CurrentScheduleEneum.Evening && m_EmeraldAISystem.StartingDestination.x != EveningLocation.position.x)
                {
                    m_EmeraldAIEventsManager.ChangeWanderType((EmeraldAISystem.WanderType)EveningWanderType);
                    m_EmeraldAISystem.m_NavMeshAgent.ResetPath();
                    m_EmeraldAIEventsManager.SetDestination(EveningLocation);
                    m_EmeraldAISystem.StartingDestination = EveningLocation.position;
                    m_EmeraldAIEventsManager.OverrideIdleAnimation(IdleAnimationEvening);
                    m_EmeraldAISystem.WaitTime = 2;
                    m_EmeraldAISystem.StationaryIdleSeconds = 2;
                }
                else if (CurrentSchedule == CurrentScheduleEneum.Night && m_EmeraldAISystem.StartingDestination.x != NightLocation.position.x)
                {
                    m_EmeraldAIEventsManager.ChangeWanderType((EmeraldAISystem.WanderType)NightWanderType);
                    m_EmeraldAISystem.m_NavMeshAgent.ResetPath();
                    m_EmeraldAIEventsManager.SetDestination(NightLocation);
                    m_EmeraldAISystem.StartingDestination = NightLocation.position;
                    m_EmeraldAIEventsManager.OverrideIdleAnimation(IdleAnimationNight);
                    m_EmeraldAISystem.WaitTime = 2;
                    m_EmeraldAISystem.StationaryIdleSeconds = 2;
                }
            }
        }
    }
}