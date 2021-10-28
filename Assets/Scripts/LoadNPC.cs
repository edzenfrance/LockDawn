using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNPC : MonoBehaviour
{
    [SerializeField] private GameObject[] npcDifficulty;

    int npcDiff;

    private void Awake()
    {
        PlayerPrefs.SetInt("GameDifficulty", 2);
        npcDiff = PlayerPrefs.GetInt("GameDifficulty", 1);
        Debug.Log("GameDifficulty: " + npcDiff);
        if (npcDiff == 1)
        {
            Debug.Log("NPC Easy");
            npcDifficulty[0].SetActive(true);
            npcDifficulty[1].SetActive(false);
            npcDifficulty[2].SetActive(false);
        }
        if (npcDiff == 2)
        {
            npcDifficulty[0].SetActive(false);
            npcDifficulty[1].SetActive(true);
            npcDifficulty[2].SetActive(false);
        }
        if (npcDiff == 3)
        {
            npcDifficulty[0].SetActive(false);
            npcDifficulty[1].SetActive(false);
            npcDifficulty[2].SetActive(true);
        }
    }
}
