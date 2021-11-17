using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SurveyManager : MonoBehaviour
{
    public SaveManager saveManager;
    public AudioManager audioManager;

    [Header("Survey")]
    public GameObject showAnswerButton;
    public GameObject answerText;
    public GameObject doneSurveyButton;
    public GameObject surveyCanvas;
    public TextMeshProUGUI questionText;

    [Header("Toggles")]
    public ToggleGroup toggleGroup;
    public GameObject[] toggleObject;

    int num;
    string myAnswer;
    int toggleLimit;

    void OnEnable()
    {
        showAnswerButton.SetActive(false);
    }

    public void ProcessSurvey(string StageSurvey)
    {
        surveyCanvas.SetActive(true);
        audioManager.PlayAudioPickUpPaper();
        if (StageSurvey == "Stage 1 Survey") SetStageNumber(1, 0);
        if (StageSurvey == "Stage 2 Survey") SetStageNumber(2, 1);
        if (StageSurvey == "Stage 3 Survey") SetStageNumber(3, 2);
        if (StageSurvey == "Stage 4 Survey") SetStageNumber(4, 3);
        if (StageSurvey == "Stage 5 Survey 2") SetStageNumber(5, 4);
        if (StageSurvey == "Stage 5 Survey 1") SetStageNumber(5, 5);
    }

    void SetStageNumber(int stagenumber, int arraycount)
    {
        if (arraycount < 5)
            saveManager.SetCompleteStage(stagenumber);
        num = arraycount;
        ChangeSurveyText();
    }

    void ChangeSurveyText()
    {
        string getToggleLimit = TextManager.surveyTexts[num][0];
        if (int.TryParse(getToggleLimit, out toggleLimit))
        {
            toggleObject[0].SetActive(false);
            toggleObject[1].SetActive(false);
            toggleObject[2].SetActive(false);
            toggleObject[3].SetActive(false);
            questionText.text = TextManager.surveyTexts[num][3];
            int g = 4;
            for (int i = 0; i < toggleLimit; i++)
            {
                toggleObject[i].SetActive(true);
                toggleObject[i].GetComponent<Toggle>().GetComponentInChildren<Text>().text = TextManager.surveyTexts[num][g];
                g += 1;
            }
        }
        else
        {
            Debug.Log("<color=white>SurveyManager</color> - Not a valid int");
            return;
        }
    }

    public void SelectSurveyAnswer()
    {
        foreach (Toggle toggle in toggleGroup.ActiveToggles())
        {
            showAnswerButton.SetActive(true);
            if (toggle.name == "Toggle A") myAnswer = "A";
            if (toggle.name == "Toggle B") myAnswer = "B";
            if (toggle.name == "Toggle C") myAnswer = "C";
            if (toggle.name == "Toggle D") myAnswer = "D";
        }
    }

    public void CompareSurveyAnswer()
    {
        showAnswerButton.SetActive(false);
        answerText.SetActive(true);
        if (myAnswer == TextManager.surveyTexts[num][1])
        {
            answerText.GetComponent<TextMeshProUGUI>().text = TextManager.surveyCorrect;
            doneSurveyButton.SetActive(true);
        }
        else
        {
            answerText.GetComponent<TextMeshProUGUI>().text = TextManager.surveyWrong + TextManager.surveyTexts[num][2];
            doneSurveyButton.SetActive(true);
        }
    }


}
