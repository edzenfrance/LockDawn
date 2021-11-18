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
        SaveManager.GetAchievement();
        if (SaveManager.achievementA == 1)
        {
            SaveManager.SetAchievement(1, 2);
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
        SaveManager.ResetKeyCount();
        tasksText[3].text = "Keys (0 of 6)";
        inventoryView.SetActive(false);

        SaveManager.GetCurrentStage();
        int delCurrentStage = SaveManager.currentStage;
        int texCurrentStage = SaveManager.currentStage += 1;
        int posCurrentStage = SaveManager.currentStage -= 1;

        SaveManager.DeleteKeyStage(delCurrentStage);
        SaveManager.SetCurrentStage(texCurrentStage);

        if (texCurrentStage == 2) tasksText[0].text = "Alcohol";
        if (texCurrentStage == 3) tasksText[0].text = "Face Mask";
        if (texCurrentStage == 4) tasksText[0].text = "Face Shield";
        if (texCurrentStage == 5) tasksText[0].text = "Vaccine";

        character = GameObject.FindWithTag("Player");
        character.transform.position = stageVectorPosition[posCurrentStage].transform.position;
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
