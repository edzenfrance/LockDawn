using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class StartManager : MonoBehaviour
{
    [SerializeField] private UIVirtualTouchZone UIVirtualTouchZoneLook;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private GameObject framerateCounter;
    [SerializeField] private Slider immunityBar;
    [SerializeField] private GameObject immunityFill;
    [SerializeField] private TextMeshProUGUI immunityText;
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private GameObject[] NPCDifficulty;

    private int currentImmunity;

    void Awake()
    {
        // Character
        saveManager.GetCurrentCharacter();
        saveManager.GetCurrentStage();
        int currentCharacter = SaveManager.currentCharacter;
        int currentStage = SaveManager.currentStage;
        GameObject prefab = characterPrefabs[currentCharacter];
        GameObject clone = Instantiate(prefab, spawnPoint[currentStage-1].position, Quaternion.identity);

        // NPC
        saveManager.GetGameDifficulty();
        int gameDiff = SaveManager.gameDifficulty;
        NPCDifficulty[0].SetActive(true);
        NPCDifficulty[1].SetActive(false);
        NPCDifficulty[2].SetActive(false);
        if (gameDiff == 1) NPCDifficulty[0].SetActive(true);
        if (gameDiff == 2) NPCDifficulty[1].SetActive(true);
        if (gameDiff == 3) NPCDifficulty[2].SetActive(true);

        // Look Sensitivity
        saveManager.GetLookSensitivity();
        float lookSensitivity = SaveManager.lookSensitivity;
        UIVirtualTouchZoneLook.magnitudeMultiplier = lookSensitivity;

        // Framerate Counter
        saveManager.GetShowFramerate();
        int framerateOn = SaveManager.framerateOn;
        if (framerateOn == 1)
            framerateCounter.SetActive(true);
        else
            framerateCounter.SetActive(false);

        // Camera
        saveManager.GetCameraDistance();
        float cameraDistance = SaveManager.cameraDistance;
        virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = cameraDistance;;

        // Immunity
        saveManager.GetCurrentImmunity();
        currentImmunity = SaveManager.currentImmunity;
        if (currentImmunity > 0)
            immunityFill.SetActive(true);
        else
            immunityFill.SetActive(false);
        immunityBar.value = currentImmunity;
        immunityText.text = currentImmunity.ToString();

        Debug.Log("<color=white>StartManager</color> - Prefab: " + prefab.name + " - SpawnPoint: " + currentStage);
        Debug.Log("<color=white>StartManager</color> - Game Difficulty: " + gameDiff);
        Debug.Log("<color=white>StartManager</color> - Framerate: " + framerateOn);
        Debug.Log("<color=white>StartManager</color> - Camera Distance: " + cameraDistance);
        Debug.Log("<color=white>StartManager</color> - Current Immunity: " + currentImmunity);
        Debug.Log("<color=white>StartManager</color> - Look Sensitivity: " + lookSensitivity);
    }
}
