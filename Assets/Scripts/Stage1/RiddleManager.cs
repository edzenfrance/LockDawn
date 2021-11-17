using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RiddleManager : MonoBehaviour
{
    public SaveManager saveManager;
    public AudioManager audioManager;
    public GameObject getItemButton;

    [Header("Riddle")]
    public GameObject showAnswerButton;
    public GameObject answerText;
    public GameObject doneRiddleButton;
    public GameObject riddleCanvas;
    public TextMeshProUGUI questionText;

    [Header("Toggles")]
    public ToggleGroup toggleGroup;
    public Toggle[] toggleAnswer;

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
        audioManager.PlayAudioPickUpPaper();
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
      toggleAnswer[0].GetComponentInChildren<Text>().text = TextManager.riddleTexts[num][1];
      toggleAnswer[1].GetComponentInChildren<Text>().text = TextManager.riddleTexts[num][2];
      toggleAnswer[2].GetComponentInChildren<Text>().text = TextManager.riddleTexts[num][3];
      toggleAnswer[3].GetComponentInChildren<Text>().text = TextManager.riddleTexts[num][4];
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
        int ridnum = num + 1;
        if (myAnswer == TextManager.riddleTexts[num][5])
        {
            saveManager.SetRiddle(ridnum, 1);
            answerText.GetComponent<TextMeshProUGUI>().text = TextManager.riddleCorrect;
            doneRiddleButton.SetActive(true);
        }
        else
        {
            saveManager.SetRiddle(ridnum, 0);
            answerText.GetComponent<TextMeshProUGUI>().text = TextManager.riddleWrong;
            doneRiddleButton.SetActive(true);
        }
    }
}
