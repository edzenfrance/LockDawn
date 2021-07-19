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

    //Get the EmeraldAISystem component and EmeraldAIEventsManager and store them as variables.
    void Start()
    {
        EmeraldComponent = GetComponent<EmeraldAISystem>();
        EventsManager = EmeraldComponent.EmeraldEventsManagerComponent;
        maxChaseDistance = EmeraldComponent.MaxChaseDistance;
    }

    void Update()
    {
        distanceFromTarget = EventsManager.GetDistanceFromTarget();
        if (distanceFromTarget > 0)
        {
            if (distanceFromTarget > maxChaseDistance)
            {
                EmeraldComponent.WalkFootstepVolume = 0f;
                EmeraldComponent.RunFootstepVolume = 0f;
            }
            else
            {
                EmeraldComponent.WalkFootstepVolume = 0.015f;
                EmeraldComponent.RunFootstepVolume = 0.015f;
            }
        }
    }

    //Changes the AI's current behavior to Aggressive
    public void ChangeBehaviorToAggressive()
    {
        
        EventsManager.ChangeBehavior(EmeraldAISystem.CurrentBehavior.Aggressive);
        Debug.Log("AI is now aggressive");
    }
}