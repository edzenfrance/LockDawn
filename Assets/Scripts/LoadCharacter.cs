using UnityEngine;
using TMPro;

public class LoadCharacter : MonoBehaviour
{
    [SerializeField]
#if UNITY_EDITOR
    [Help("Instructions for adding new character\n\n"
    + "Note: Be sure to change the Rig > Animation Type to Humanoid before adding the character\n\n"
    + "1. Add the new character to Hierarchy > Characters\n"
    + "2. Change character Tag to Player\n"
    + "3. Change character Layer to Controlled Character (Select 'no, this object only')\n"
    + "4. In Animator component change the Controller to StarterAssetsThirdPerson.controller\n"
    + "5. Uncheck Apply Root Motion (Character cannot walk if this is check)\n"
    + "6. Create empty object in your character and name it PlayerCameraRoot\n"
    + "7. Change PlayerCameraRoot Tag to CinemachineTarget\n"
    + "8. Open Scene in 3D view and move the PlayerCameraRoot to the chest of the character\n"
    + "9. Copy the Components from other character to new character (Character Controller, Starter Assets Inputs, Basic Rigid Body, Third Person Controller, Player Input)\n"
    + "10. Modify Character Controller component Center Y, Radius, Height based on character\n"
    + "11. Change Player Input component Actions to StarterAssets.inputactions\n"
    + "12. Disable the character and add to Characters List\n"
    + "13. Now add the character to Character Selection scene" , UnityEditor.MessageType.None)]
#endif
    private int selectedCharacter;
    public TMP_Text selectedCharacterText;
    public GameObject[] characters;


    void Awake()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter", 0);
        characters[selectedCharacter].SetActive(true);
        Debug.Log("<color=white>LoadCharacter</color> - Enabled Character: " + characters[selectedCharacter]);

        selectedCharacterText = GameObject.Find("SelectedPlayerName").GetComponent<TMP_Text>();
        selectedCharacterText.text = "Player: " + PlayerPrefs.GetString("CharSel");
        
    }
}
