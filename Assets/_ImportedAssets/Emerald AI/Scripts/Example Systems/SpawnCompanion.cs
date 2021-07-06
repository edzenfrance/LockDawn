using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI.Utility;

namespace EmeraldAI.Example
{
    public class SpawnCompanion : MonoBehaviour
    {
        public int TotalAllowedCompanions = 1;
        public GameObject CompanionAIObject;

        int CurrentCompanions;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (CurrentCompanions < TotalAllowedCompanions)
                {
                    //Spawn our AI using the Emerald Object Pool system
                    Vector3 SpawnPosition = transform.position + transform.forward * 5 + (Random.insideUnitSphere * 2);
                    SpawnPosition.y = transform.position.y;
                    GameObject SpawnedAI = EmeraldAIObjectPool.Spawn(CompanionAIObject, SpawnPosition, Quaternion.identity);

                    //Set an event on the created AI to remove the AI on death
                    SpawnedAI.GetComponent<EmeraldAISystem>().DeathEvent.AddListener(() => { RemoveAI(); });

                    //Add the spawned AI to the total amount of currently spawn AI
                    CurrentCompanions++;
                }
            }

            /*
            if (Input.GetKeyDown(KeyCode.H) && GameObject.Find("Fallen Guardian (Companion)") != null)
            {
                GameObject.Find("Fallen Guardian (Companion)").GetComponent<EmeraldAISystem>().EmeraldEventsManagerComponent.StopFollowing();
            }
            if (Input.GetKeyDown(KeyCode.J) && GameObject.Find("Fallen Guardian (Companion)") != null)
            {
                GameObject.Find("Fallen Guardian (Companion)").GetComponent<EmeraldAISystem>().EmeraldEventsManagerComponent.ResumeFollowing();
            }
            */
        }

        public void RemoveAI()
        {
            CurrentCompanions--;
        }
    }
}