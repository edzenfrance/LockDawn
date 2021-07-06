using UnityEngine;
using TMPro;

public class LoadCharacter : MonoBehaviour
{
	public GameObject[] characters;
	public int selectedCharacter = 0;
    public TMP_Text charSelected;

    void Awake()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        characters[selectedCharacter].SetActive(true);
        Debug.Log("LoadCharacter - ENABLED CHARACTER: " + characters[selectedCharacter]);

        charSelected = GameObject.Find("SelectedPlayerName").GetComponent<TMP_Text>();
        charSelected.text = "Player: " + PlayerPrefs.GetString("CharSel");
        
    }
}
