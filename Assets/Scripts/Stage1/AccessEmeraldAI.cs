using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI;

public class AccessEmeraldAI : MonoBehaviour
{
    [SerializeField] private EmeraldAISystem EmeraldComponent;
    EmeraldAIEventsManager EventsManager;
    [SerializeField] private float distanceFromTarget;
    [SerializeField] private float maxChaseDistance;
    [SerializeField] private float detectionDistance;
    [SerializeField] private int NPCDifficulty;

    float fpsCounter;
    bool refresh;

    //Get the EmeraldAISystem component and EmeraldAIEventsManager and store them as variables.
    void Awake()
    {
        EmeraldComponent = GetComponent<EmeraldAISystem>();
        EventsManager = EmeraldComponent.EmeraldEventsManagerComponent;
        maxChaseDistance = EmeraldComponent.MaxChaseDistance;
        PlayerPrefs.SetInt("GameDifficulty", 2);
        NPCDifficulty = PlayerPrefs.GetInt("GameDifficulty", 2);
        refresh = true;
    }

    void Update()
    {
        if (refresh)
        {
            changeNPCSettings();
        }
    }

    void changeNPCSettings()
    {
        distanceFromTarget = EventsManager.GetDistanceFromTarget();
        if (distanceFromTarget > 0)
        {
            if (distanceFromTarget > maxChaseDistance)
            {
                EmeraldComponent.WalkFootstepVolume = 0f;
                EmeraldComponent.RunFootstepVolume = 0f;
                #if UNITY_EDITOR
                    Debug.Log("<color=white>AccessEmeraldAI</color> - WalkFootStepVolume - 0f | RunFootstepVolume - 0f");
                #endif
            }
            else
            {
                Debug.Log("NPC: " + NPCDifficulty);
                EmeraldComponent.WalkFootstepVolume = 0.015f;
                EmeraldComponent.RunFootstepVolume = 0.015f;
                if (NPCDifficulty == 1)
                {
                    EmeraldComponent.DetectionRadius = 2;   // Detection Distance
                    EmeraldComponent.MaxChaseDistance = 7;  // Chase Distance
                }
                if (NPCDifficulty == 2)
                {
                    EmeraldComponent.DetectionRadius = 4;
                    EmeraldComponent.MaxChaseDistance = 8;
                }
                if (NPCDifficulty == 3)
                {
                    EmeraldComponent.DetectionRadius = 6;
                    EmeraldComponent.MaxChaseDistance = 10;
                }
                #if UNITY_EDITOR
                    Debug.Log("<color=white>AccessEmeraldAI</color> - WalkFootStepVolume - 0.015f | RunFootstepVolume - 0.015f");
                #endif
            }
        }
        refresh = false;
        StartCoroutine(refreshDelay());
    }

    IEnumerator refreshDelay()
    {
        yield return new WaitForSeconds(1f);
        refresh = true;
    }

    //Changes the AI's current behavior to Aggressive
    public void ChangeBehaviorToAggressive()
    {
        EventsManager.ChangeBehavior(EmeraldAISystem.CurrentBehavior.Passive);
        EmeraldComponent.DetectionRadius = 6;           // Detection Distance
        EmeraldComponent.MaxChaseDistance = 16;         // Chase Distance
        EmeraldComponent.ExpandedChaseDistance = 15;    // Expanded Chase Distance
    }
}