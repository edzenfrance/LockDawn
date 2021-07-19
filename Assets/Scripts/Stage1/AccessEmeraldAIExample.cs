using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI;

public class AccessEmeraldAIExample : MonoBehaviour
{
    public EmeraldAISystem EmeraldComponent;
    public EmeraldAIEventsManager EventsManager;

    //Get the EmeraldAISystem component and EmeraldAIEventsManager and store them as variables.
    void Start()
    {
        EmeraldComponent = GameObject.Find("Zombie1").GetComponent<EmeraldAISystem>();
        EventsManager = EmeraldComponent.EmeraldEventsManagerComponent;
    }

    //Changes the AI's current behavior to Aggressive
    public void ChangeBehaviorTest()
    {
        EventsManager.ChangeBehavior(EmeraldAISystem.CurrentBehavior.Aggressive);
        Debug.Log("AI Becomes aggressive");
    }

}