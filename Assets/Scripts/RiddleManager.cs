using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RiddleManager : MonoBehaviour
{

    public GameObject mapButton;
    public GameObject inventoryButton;
    public GameObject hand;
    public GameObject answerButton;
    public GameObject showAnswer;
    public TextMeshProUGUI showAnswerText;
    public GameObject doneRiddleButton;

    [Header("Riddle")]
    public GameObject riddleCanvas;
    public TextMeshProUGUI questionText;
    public Toggle[] answerText;

    public ToggleGroup toggleGroup;
    public Toggle toggleA, toggleB, toggleC, toggleD;

    string[][] riddleTexts = new string[][] {
      new string[] {
        "I am neither a guest or a trespasser be, to this place I belong, it belongs also to me.",
        "A. Door",
        "B. Home",
        "C. Mother",
        "D. Land Owner",
        "B"},
      new string[] {
        "What begins but has no end and is the ending of all that begins?",
        "A. Death",
        "B. Reborn",
        "C. War",
        "D. Justice",
        "A"},
      new string[] {
        "Only one color, but not one size, Stuck at the bottom, yet easily flies. Present in sun, but not in rain, Doing no harm, and feeling no pain. What is it?",
        "A. Sky",
        "B. Light",
        "C. Darkness",
        "D. Shadow",
        "D"}
    };

    int num;
    string myAnswer;

    void OnEnable()
    {
        answerButton.SetActive(false);
    }

    public void ProcessRiddle(string getRiddleName)
    {
        riddleCanvas.SetActive(true);
        mapButton.SetActive(false);
        inventoryButton.SetActive(false);
        hand.SetActive(false);
        if (getRiddleName == "S1 Riddle A") num = 0;
        ChangeRiddleText();
    }

    void ChangeRiddleText()
    {
      questionText.text = riddleTexts[num][0];
      answerText[0].GetComponentInChildren<Text>().text = riddleTexts[num][1];
      answerText[1].GetComponentInChildren<Text>().text = riddleTexts[num][2];
      answerText[2].GetComponentInChildren<Text>().text = riddleTexts[num][3];
      answerText[3].GetComponentInChildren<Text>().text = riddleTexts[num][4];
    }

    public void SelectRiddleAnswer()
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

    public void CompareRiddleAnswer()
    {
        answerButton.SetActive(false);
        showAnswer.SetActive(true);
        if (myAnswer == riddleTexts[num][5])
        {
            showAnswerText.text = "Your riddle answer is correct! Congrats, you gained collectors item! You can view the item in collectors hall!";
            doneRiddleButton.SetActive(true);
        }
        else
        {
            showAnswerText.text = "Your riddle answer is wrong! Better luck next time!";
            doneRiddleButton.SetActive(true);
        }
    }
}
