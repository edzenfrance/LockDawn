using UnityEngine;
using TMPro;

public class LoadCharacterNPC : MonoBehaviour
{
    [SerializeField] private TMP_Text selectedCharacterText;
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private GameObject[] NPCDifficulty;
    void Awake()
    {
        // Character
        int selectedCharacter = PlayerPrefs.GetInt("Current Character", 0);
        int currentStage = PlayerPrefs.GetInt("Current Stage", 1);
        GameObject prefab = characterPrefabs[selectedCharacter];
        GameObject clone = Instantiate(prefab, spawnPoint[currentStage].position, Quaternion.identity);
        Debug.Log("<color=white>LoadCharacter</color> - Enabled Character: " + prefab.name);
        selectedCharacterText.text = "Player: " + prefab.name;

        // NPC
        int npcDiff = PlayerPrefs.GetInt("Game Difficulty", 2);
        NPCDifficulty[0].SetActive(true);
        NPCDifficulty[1].SetActive(false);
        NPCDifficulty[2].SetActive(false);
        if (npcDiff == 1) NPCDifficulty[0].SetActive(true);
        if (npcDiff == 2) NPCDifficulty[1].SetActive(true);
        if (npcDiff == 3) NPCDifficulty[2].SetActive(true);
    }
}
