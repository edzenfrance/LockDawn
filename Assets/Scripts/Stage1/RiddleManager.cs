using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RiddleManager : MonoBehaviour
{
    public GameObject getItemButton;
    public GameObject showAnswerButton;
    public GameObject answerText;
    public GameObject doneRiddleButton;

    [Header("Riddle")]
    public GameObject riddleCanvas;
    public TextMeshProUGUI questionText;
    public Toggle[] answerToggle;

    public ToggleGroup toggleGroup;
    public Toggle toggleA, toggleB, toggleC, toggleD;

    int num;
    string myAnswer;

    void OnEnable()
    {
        showAnswerButton.SetActive(false);
    }

    public void ProcessRiddle(string getRiddleName)
    {
        riddleCanvas.SetActive(true);
        getItemButton.SetActive(false);
        if (getRiddleName == "S1 Riddle A") num = 0;
        if (getRiddleName == "S2 Riddle A") num = 1;
        if (getRiddleName == "S3 Riddle A") num = 2;
        if (getRiddleName == "S4 Riddle A") num = 3;
        if (getRiddleName == "S5 Riddle A") num = 4;
        ChangeRiddleText();
    }

    void ChangeRiddleText()
    {
      questionText.text = TextManager.riddleTexts[num][0];
      answerToggle[0].GetComponentInChildren<Text>().text = TextManager.riddleTexts[num][1];
      answerToggle[1].GetComponentInChildren<Text>().text = TextManager.riddleTexts[num][2];
      answerToggle[2].GetComponentInChildren<Text>().text = TextManager.riddleTexts[num][3];
      answerToggle[3].GetComponentInChildren<Text>().text = TextManager.riddleTexts[num][4];
    }

    public void SelectRiddleAnswer()
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

    public void CompareRiddleAnswer()
    {
        showAnswerButton.SetActive(false);
        answerText.SetActive(true);
        if (myAnswer == TextManager.riddleTexts[num][5])
        {
            answerText.GetComponent<TextMeshProUGUI>().text = TextManager.riddleCorrect;
            doneRiddleButton.SetActive(true);
        }
        else
        {
            answerText.GetComponent<TextMeshProUGUI>().text = TextManager.riddleWrong;
            doneRiddleButton.SetActive(true);
        }
    }
}
