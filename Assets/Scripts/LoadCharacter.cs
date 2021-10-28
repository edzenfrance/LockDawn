using UnityEngine;
using TMPro;

public class LoadCharacter : MonoBehaviour
{
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TMP_Text selectedCharacterText;

    void Awake()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter", 0);
        GameObject prefab = characterPrefabs[selectedCharacter];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("<color=white>LoadCharacter</color> - Enabled Character: " + prefab.name);
        selectedCharacterText.text = "Player: " + prefab.name;
    }
}
