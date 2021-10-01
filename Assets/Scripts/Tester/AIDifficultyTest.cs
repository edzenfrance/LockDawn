using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EmeraldAI;
using System.Linq;

public class AIDifficultyTest : MonoBehaviour
{
    public List<EmeraldAISystem> NPC = new List<EmeraldAISystem>();
    EmeraldAIEventsManager EventsManager;
    public TMP_Dropdown difficultyDropdown;

    public void DifficultySelector()
    {
        if(difficultyDropdown.value == 0)
        {
            PlayerPrefs.SetInt("GameDifficulty", 0);
            foreach (EmeraldAISystem behavior in NPC)
            {
                EventsManager = behavior.GetComponent<EmeraldAISystem>().EmeraldEventsManagerComponent;
                EventsManager.ChangeBehavior(EmeraldAISystem.CurrentBehavior.Passive);
                behavior.DetectionRadius = 1;
                behavior.MaxChaseDistance = 7;
             // behavior.ExpandedChaseDistance = 7;
            }
        }
        if(difficultyDropdown.value == 1)
        {
            PlayerPrefs.SetInt("GameDifficulty", 1);
            foreach (EmeraldAISystem behavior in NPC)
            {
                EventsManager = behavior.GetComponent<EmeraldAISystem>().EmeraldEventsManagerComponent;
                EventsManager.ChangeBehavior(EmeraldAISystem.CurrentBehavior.Aggressive);
                behavior.DetectionRadius = 2;
                behavior.MaxChaseDistance = 7;
             // behavior.ExpandedChaseDistance = 7;
            }
        }
        if (difficultyDropdown.value == 2)
        {
            PlayerPrefs.SetInt("GameDifficulty", 2);
            foreach (EmeraldAISystem behavior in NPC)
            {
                EventsManager = behavior.GetComponent<EmeraldAISystem>().EmeraldEventsManagerComponent;
                EventsManager.ChangeBehavior(EmeraldAISystem.CurrentBehavior.Aggressive);
                behavior.DetectionRadius = 4;
                behavior.MaxChaseDistance = 8;
             // behavior.ExpandedChaseDistance = 8;
            }
        }
        if (difficultyDropdown.value == 3)
        {
            PlayerPrefs.SetInt("GameDifficulty", 3);
            foreach (EmeraldAISystem behavior in NPC)
            {
                EventsManager = behavior.GetComponent<EmeraldAISystem>().EmeraldEventsManagerComponent;
                EventsManager.ChangeBehavior(EmeraldAISystem.CurrentBehavior.Aggressive);
                behavior.DetectionRadius = 6;
                behavior.MaxChaseDistance = 10;
             // behavior.ExpandedChaseDistance = 10;
            }
        }
    }
}
