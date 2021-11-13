using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SurveyManager : MonoBehaviour
{

    public GameObject mapButton;
    public GameObject inventoryButton;
    public GameObject hand;
    public GameObject answerButton;
    public GameObject showAnswer;
    public TextMeshProUGUI showAnswerText;
    public GameObject doneSurveyButton;

    [Header("Survey")]
    public GameObject surveyCanvas;
    public TextMeshProUGUI questionText;

    [Header("Toggles")]
    public GameObject[] toggleObject;
    public ToggleGroup toggleGroup;
    //public Toggle toggleA, toggleB, toggleC, toggleD;

    string[][] surveyTexts = new string[][] {
      new string[] {
        "2",
        "B",
        "While playing the game for the first time, what have you learned about the game that you can do in real life to avoid getting infected or spread the virus?",
        "A. Hide from infected",
        "B. Physical distancing",
      },
      new string[] {
        "3",
        "A",
        "What is the most essential way to make your family safe from COVID-19 while inside your house?",
        "A. Use disinfectant regularly",
        "B. Wash hands using water only",
        "C. Use gloves inside the house everytime",
       },
       new string[] {
        "3",
        "B",
        "When do you need to where your facemask?",
        "A. When sleeping",
        "B. When outside",
        "C. When an authority is looking",
       },
       new string[] {
        "3",
        "C",
        "How do you wear your face shield properly?",
        "A. Over the head",
        "B. On the back of your head",
        "C. Aligned to your face",
      },
      new string[] {
        "2",
        "A",
        "_______ is a biological preparation that provides active acquired immunity to a particular infectious disease.",
        "A. Vaccine",
        "B. Vitamin",
      },
      new string[] {
        "2",
        "A",
        "You are now on the final stage of the game, does the game help you to understand how safety protocols work in a pandemic situation?",
        "A. Yes",
        "B. No",
      }
    };

    int num;
    int limit;
    string myAnswer;

    void OnEnable()
    {
        answerButton.SetActive(false);
    }

    public void ProcessSurvey(string StageSurvey)
    {
        mapButton.SetActive(false);
        inventoryButton.SetActive(false);
        hand.SetActive(false);
        surveyCanvas.SetActive(true);
        if (StageSurvey == "Stage 1 Survey") SetStageNumber(0, 1);
        if (StageSurvey == "Stage 2 Survey") SetStageNumber(1, 2);
        if (StageSurvey == "Stage 3 Survey") SetStageNumber(2, 3);
        if (StageSurvey == "Stage 4 Survey") SetStageNumber(3, 4);
        if (StageSurvey == "Stage 5 Survey") SetStageNumber(4, 5);
        if (StageSurvey == "Stage 5 First Survey")
        {
            num = 5;
            ChangeSurveyText();
        }
    }

    void SetStageNumber(int count, int stagenumber)
    {
        num = count;
        PlayerPrefs.SetInt("Stage " + stagenumber + " Complete", 1);
        ChangeSurveyText();
    }

    void ChangeSurveyText()
    {
        string getLimit = surveyTexts[num][0];
        if (int.TryParse(getLimit, out limit))
        {
            toggleObject[0].SetActive(false);
            toggleObject[1].SetActive(false);
            toggleObject[2].SetActive(false);
            toggleObject[3].SetActive(false);

            questionText.text = surveyTexts[num][2];
            int g = 3;
            for (int i = 0; i < limit; i++)
            {
                toggleObject[i].SetActive(true);
                toggleObject[i].GetComponent<Toggle>().GetComponentInChildren<Text>().text = surveyTexts[num][g];
                g += 1;
            }
        }
        else
        {
            Debug.Log("Not a valid int");
            return;
        }
    }

    public void SelectSurveyAnswer()
    {
        foreach (Toggle toggle in toggleGroup.ActiveToggles())
        {
            answerButton.SetActive(true);
            if (toggle.name == "Toggle A") myAnswer = "A";
            if (toggle.name == "Toggle B") myAnswer = "B";
            if (toggle.name == "Toggle C") myAnswer = "C";
            if (toggle.name == "Toggle D") myAnswer = "D";
        }
    }

    public void CompareSurveyAnswer()
    {
        answerButton.SetActive(false);
        showAnswer.SetActive(true);
        int correctNum = 0;
        if (myAnswer == surveyTexts[num][1])
        {
            showAnswerText.text = "Your answer it correctly! You learn something!";
            doneSurveyButton.SetActive(true);
        }
        else
        {
            if (surveyTexts[num][1] == "A") correctNum = 3;
            if (surveyTexts[num][1] == "B") correctNum = 4;
            if (surveyTexts[num][1] == "C") correctNum = 5;
            showAnswerText.text = "Your answer is wrong!\n" + surveyTexts[num][correctNum] + " is the correct answer!";
            doneSurveyButton.SetActive(true);
        }
    }


}
