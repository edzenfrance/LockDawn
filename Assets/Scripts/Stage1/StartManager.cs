using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class StartManager : MonoBehaviour
{
    //[SerializeField] private TMP_Text currentCharacterText;
    [SerializeField] private UIVirtualTouchZone UIVirtualTouchZoneLook;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private GameObject framerateCounter;
    [SerializeField] private Slider immunityBar;
    [SerializeField] private GameObject immunityFill;
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private GameObject[] NPCDifficulty;

    private int currentImmunity;

    void Awake()
    {
        // Character
        int currentCharacter = PlayerPrefs.GetInt("Current Character", 1);
        saveManager.GetCurrentStage();
        int currentStage = SaveManager.currentStage;
        GameObject prefab = characterPrefabs[currentCharacter];
        GameObject clone = Instantiate(prefab, spawnPoint[currentStage-1].position, Quaternion.identity);
        Debug.Log("<color=white>StartManager</color> - Character: " + prefab.name + " - SpawnPoint: " + currentStage);
        //currentCharacterText.text = "Player: " + prefab.name;

        // NPC
        int npcDiff = PlayerPrefs.GetInt("Game Difficulty", 2);
        NPCDifficulty[0].SetActive(true);
        NPCDifficulty[1].SetActive(false);
        NPCDifficulty[2].SetActive(false);
        if (npcDiff == 1) NPCDifficulty[0].SetActive(true);
        if (npcDiff == 2) NPCDifficulty[1].SetActive(true);
        if (npcDiff == 3) NPCDifficulty[2].SetActive(true);

        // Look Sensitivity
        float lookSens = PlayerPrefs.GetFloat("Look Sensitivity", 60);
        UIVirtualTouchZoneLook.magnitudeMultiplier = lookSens;

        // Framerate Counter
        int framerateOn = PlayerPrefs.GetInt("Show Framerate", 0);
        if (framerateOn == 1)
            framerateCounter.SetActive(true);
        else
            framerateCounter.SetActive(false);

        // Camera
        float cameraDis = PlayerPrefs.GetFloat("Camera Distance", 1.75f);
        virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = cameraDis;;

        // Immunity
        saveManager.GetCurrentImmunity();
        currentImmunity = SaveManager.currentImmunity;
        Debug.Log("<color=white>ChangeImmunity</color> - Current Immunity: " + currentImmunity);
        if (currentImmunity > 0)
            immunityFill.SetActive(true);
        else
            immunityFill.SetActive(false);
        immunityBar.value = currentImmunity;
    }
}
