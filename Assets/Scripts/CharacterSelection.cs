using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
	public GameObject[] characters;
	public int selectedCharacter = 0;
	public TextMeshProUGUI characterName;


	public void NextCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter = (selectedCharacter + 1) % characters.Length;
		characters[selectedCharacter].SetActive(true);

		GameObject charName = characters[selectedCharacter];
		characterName.text = "Select " + charName.name;
	}

	public void PreviousCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter--;
		if (selectedCharacter < 0)
		{
			selectedCharacter += characters.Length;
		}
		characters[selectedCharacter].SetActive(true);

		GameObject charName = characters[selectedCharacter];
		characterName.text = "Select " + charName.name;

	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
		SceneManager.LoadScene("Stage1", LoadSceneMode.Single);
	}

	public void BackToMenu()
    {
		SceneManager.LoadScene("MainMenu");
    }
}
