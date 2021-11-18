using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class StartManager : MonoBehaviour
{
    [SerializeField] private UIVirtualTouchZone UIVirtualTouchZoneLook;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private EnvironmentController environmentController;
    [SerializeField] private GameObject framerateCounter;
    [SerializeField] private Slider immunityBar;
    [SerializeField] private GameObject immunityFill;
    [SerializeField] private TextMeshProUGUI immunityText;
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private GameObject[] NPCDifficulty;

    void Awake()
    {
        // Character
        SaveManager.GetCurrentCharacter();
        SaveManager.GetCurrentStage();
        SaveManager.GetContinueGame();

        GameObject prefab = characterPrefabs[SaveManager.currentCharacter];
        if (SaveManager.continueGame == 0)
        {
            GameObject clone = Instantiate(prefab, spawnPoint[SaveManager.currentStage - 1].position, Quaternion.identity);
        }
        if (SaveManager.continueGame == 1)
        {
            // Immunity
            SaveManager.GetCurrentImmunity();
            if (SaveManager.currentImmunity > 0)
                immunityFill.SetActive(true);
            else
                immunityFill.SetActive(false);
            immunityBar.value = SaveManager.currentImmunity;
            immunityText.text = SaveManager.currentImmunity.ToString();

            Vector3 pos = PlayerPrefsExtra.GetVector3("Pos", spawnPoint[SaveManager.currentStage - 1].position);
            GameObject clone = Instantiate(prefab, pos, Quaternion.identity);
        }

        // NPC
        SaveManager.GetGameDifficulty();
        NPCDifficulty[0].SetActive(true);
        NPCDifficulty[1].SetActive(false);
        NPCDifficulty[2].SetActive(false);
        if (SaveManager.gameDifficulty == 1) NPCDifficulty[0].SetActive(true);
        if (SaveManager.gameDifficulty == 2) NPCDifficulty[1].SetActive(true);
        if (SaveManager.gameDifficulty == 3) NPCDifficulty[2].SetActive(true);

        // Look Sensitivity
        SaveManager.GetLookSensitivity();
        UIVirtualTouchZoneLook.magnitudeMultiplier = SaveManager.lookSensitivity;

        // Framerate Counter
        SaveManager.GetShowFramerate();
        if (SaveManager.framerateOn == 1)
            framerateCounter.SetActive(true);
        else
            framerateCounter.SetActive(false);

        // Camera
        SaveManager.GetCameraDistance();
        virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = SaveManager.cameraDistance;;

        // Debug
        Debug.Log("<color=white>StartManager</color> - Prefab: " + prefab.name + " - SpawnPoint: " + SaveManager.currentStage);
        Debug.Log("<color=white>StartManager</color> - Game Difficulty: " + SaveManager.gameDifficulty);
        Debug.Log("<color=white>StartManager</color> - Framerate: " + SaveManager.framerateOn);
        Debug.Log("<color=white>StartManager</color> - Camera Distance: " + SaveManager.cameraDistance);
        Debug.Log("<color=white>StartManager</color> - Current Immunity: " + SaveManager.currentImmunity);
        Debug.Log("<color=white>StartManager</color> - Look Sensitivity: " + SaveManager.lookSensitivity);

        // Destroy Object From Main Menu
        GameObject objMusic = GameObject.FindGameObjectWithTag("MainMenuBGM");
        GameObject objSound = GameObject.FindGameObjectWithTag("SFXManager");
        Destroy(objMusic);
        Destroy(objSound);
    }
}
