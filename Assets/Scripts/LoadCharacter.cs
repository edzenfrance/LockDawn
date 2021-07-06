using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
	public GameObject[] characters;
	public int selectedCharacter = 0;

    void Awake()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        characters[selectedCharacter].SetActive(true);
        Debug.Log("LoadCharacter - ENABLED CHARACTER: " + characters[selectedCharacter]);
    }
}
