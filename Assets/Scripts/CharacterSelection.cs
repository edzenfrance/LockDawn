using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
#if UNITY_EDITOR
    [Help("Instructions for adding new character\n\n"
    + "1. Add the new character to Hierarchy [TAB] > character [OBJECT] \n"
    + "2. Change the Tag and Layer\n"
	+ "     - Tag: Player\n"
	+ "     - Layer: Controlled Character (NOTE: Select 'no, this object only')\n"
    + "3. Change the Animator component\n"
	+ "      - Controller: StarterAssetsThirdPerson.controller\n"
    + "      - Apply Root Motion: Uncheck (NOTE: Character cannot walk if this is check)\n"
	+ "      - Update Mode: Normal\n"
	+ "      - Culling Mode: Cull Update Transforms\n"
    + "4. Create empty GameObject inside the new character\n"
	+ "5. Rename the empty GameObject to PlayerCameraRoot\n"
    + "6. Change the PlayerCameraRoot Tag\n"
	+ "     - Tag: CinemachineTarget\n"
    + "6. Open the Scene in 3D view\n"
	+ "7. Move the PlayerCameraRoot object to the chest of new character\n"
    + "8. Copy the Components from other character to the new character\n"
	+ "     - Character Controller\n"
	+ "     - Starter Assets Inputs\n"
	+ "     - Basic Rigid Body\n"
	+ "     - Third Person Controller\n"
	+ "     - Player Input\n"
    + "9. Modify the Controller component based on the size of new character\n"
	+ "     - Center Y\n"
	+ "     - Radius\n"
	+ "     - Height\n"
    + "10. Change the Player Input component\n"
	+ "     - Actions: StarterAssets.inputactions\n"
    + "11. Create new Prefab in Prefabs folder\n"
	+ "12. Drag the new character to the new Prefab and save\n"
	+ "13. Click Hierarchy [TAB] > character [OBJECT]\n"
	+ "14. Add new Element [+] in character List (NOTE: Below this Instructions)\n"
	+ "15. Drag the new character prefab in new Element of character List\n"
	+ "16. Change the Animator component of new character prefab\n"
	+ "     - Controller: Look Around-Nervous-A.controller (NOTE: This can be change to any controller)\n\n"
	+ "IMPORTANT NOTES:\n\n"
	+ "1. Change Rig > Animation Type to Humanoid before adding the character\n"
	+ "2. Default Animator > Controller of all character Prefab must be StarterAssetsThirdPerson\n"
    + "3. Character prefab must be added in Load Character Prefabs List in Stage 1-5 scene\n"
	+ "4. If character is floating, modify the 'Grounded Offset' and 'Grounded Radius' in ThirdPersonController Script component of character\n"
	+ "5. Disable the ThirdPersonController Script component in Character Selection scene", UnityEditor.MessageType.None)]
#endif
	[SerializeField] private int currentCharacter;
	[SerializeField] private GameObject buyText;
	[SerializeField] private GameObject useCharacter;
	[SerializeField] private TextMeshProUGUI characterNameText;
	[SerializeField] private GameObject[] character;
	[SerializeField] private string[] characterName;


	public void NextCharacter()
	{
		character[currentCharacter].SetActive(false);
		currentCharacter = (currentCharacter + 1) % character.Length;
		UseCharacter();
		ProcessPremiumSkin();
		character[currentCharacter].SetActive(true);
	}

	public void PreviousCharacter()
	{
		character[currentCharacter].SetActive(false);
		currentCharacter--;
		if (currentCharacter < 0)
			currentCharacter += character.Length;
		UseCharacter();
		ProcessPremiumSkin();
		character[currentCharacter].SetActive(true);
	}

	void ProcessPremiumSkin()
    {
		if (currentCharacter >= 2)
		{
			SaveManager.GetObtainSkin();
			if (currentCharacter == 2)
				if (SaveManager.obtainSkinA != 1)
					BuyCharacter();
			if (currentCharacter == 3)
				if (SaveManager.obtainSkinB != 1)
					BuyCharacter();

			if (currentCharacter == 4)
				if (SaveManager.obtainSkinC != 1)
					BuyCharacter();
		}
	}

	void UseCharacter()
	{
		buyText.SetActive(false);
		useCharacter.SetActive(true);
		characterNameText.text = "Select " + characterName[currentCharacter];
	}

	void BuyCharacter()
    {
		buyText.SetActive(true);
		buyText.GetComponent<TextMeshProUGUI>().text = "Buy this " + characterName[currentCharacter] + " skin at shop!";
		useCharacter.SetActive(false);
	}

	public void StartGame()
	{
		SaveManager.NewGamePlayerPrefs();
		SaveManager.SetCurrentCharacter(currentCharacter);
		SceneManager.LoadScene("Stage1", LoadSceneMode.Single);
	}

	public void GoToMenu()
    {
		SceneManager.LoadScene("MainMenu");
    }
}
