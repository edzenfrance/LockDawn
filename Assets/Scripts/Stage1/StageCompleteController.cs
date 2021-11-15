using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageCompleteController : MonoBehaviour
{
    [SerializeField] private GameObject getAchievements;
    [SerializeField] private GameObject nextStageButton;
    [SerializeField] private GameObject inventoryView;
    [SerializeField] private StageTimer stageTimer;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject[] stageVectorPosition;
    [SerializeField] private TextMeshProUGUI[] tasksText;
    [SerializeField] private Toggle[] tasksToggle;


    void OnEnable()
    {
        stageTimer.OnFinishTimer();

        // 0 = Failed  1 = Show it  2 = Show
        int achievementA = PlayerPrefs.GetInt("Achievement 1");
        int achievementB = PlayerPrefs.GetInt("Achievement 2");
        int achievementC = PlayerPrefs.GetInt("Achievement 3");
        int achievementD = PlayerPrefs.GetInt("Achievement 4");
        int achievementE = PlayerPrefs.GetInt("Achievement 5");

        if (achievementA == 1)
        {
            saveManager.SetAchievement(1, 2);
            getAchievements.SetActive(true);
        }
        Time.timeScale = 0;
        StartCoroutine(OffAchievements());
    }

    public void MoveToNextStage()
    {
        Time.timeScale = 1;

        tasksToggle[0].isOn = false;
        tasksToggle[1].isOn = false;
        tasksToggle[2].isOn = false;
        tasksToggle[3].isOn = false;
        saveManager.ResetKeyCount();
        tasksText[3].text = "Keys (0 of 6)";
        inventoryView.SetActive(false);

        saveManager.GetCurrentStage();
        int currentStage = SaveManager.currentStage;
        saveManager.DeleteKeyStage(currentStage);

        currentStage += 1;
        saveManager.SetCurrentStage(currentStage);

        if (currentStage == 2) tasksText[0].text = "Alcohol";
        if (currentStage == 3) tasksText[0].text = "Face Mask";
        if (currentStage == 4) tasksText[0].text = "Face Shield";
        if (currentStage == 5) tasksText[0].text = "Vaccine";

        int currentStagePosition = currentStage -= 1;
        character = GameObject.FindWithTag("Player");
        character.transform.position = stageVectorPosition[currentStagePosition].transform.position;
        character.SetActive(false);
        character.SetActive(true);

        StartCoroutine(DisableStageComplete());
    }

    IEnumerator DisableStageComplete()
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }

    IEnumerator OffAchievements()
    {
        yield return new WaitForSeconds(1.0f);
        getAchievements.SetActive(false);
        nextStageButton.SetActive(true);
    }
}
