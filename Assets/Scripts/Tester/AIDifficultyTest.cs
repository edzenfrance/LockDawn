using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EmeraldAI;

public class AIDifficultyTest : MonoBehaviour
{
    public List<EmeraldAISystem> NPC = new List<EmeraldAISystem>();
    EmeraldAIEventsManager EventsManager;
    public TMP_Dropdown difficultyDropdown;

    void Start()
    {
        // = GetComponent<EmeraldAISystem>();
        //EventsManager = EmeraldComponent.EmeraldEventsManagerComponent;
    }

    public void DifficultySelector()
    {
        if(difficultyDropdown.value == 0)
        {
            foreach (EmeraldAISystem behavior in NPC)
            {
                EventsManager = behavior.GetComponent<EmeraldAISystem>().EmeraldEventsManagerComponent;
                EventsManager.ChangeBehavior(EmeraldAISystem.CurrentBehavior.Aggressive);
            }
        }
        if(difficultyDropdown.value == 1)
        {
            foreach (EmeraldAISystem behavior in NPC)
            {
                EventsManager = behavior.GetComponent<EmeraldAISystem>().EmeraldEventsManagerComponent;
                EventsManager.ChangeBehavior(EmeraldAISystem.CurrentBehavior.Passive);
            }
        }
    }
}
