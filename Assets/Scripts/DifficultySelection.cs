using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultySelection : MonoBehaviour
{
    [Header("TextMeshPro Button")]
    [SerializeField] private Button buttonEasy;
    [SerializeField] private Button buttonNormal;
    [SerializeField] private Button buttonHard;
    [SerializeField] private TextMeshProUGUI buttonTextEasy;
    [SerializeField] private TextMeshProUGUI buttonTextNormal;
    [SerializeField] private TextMeshProUGUI buttonTextHard;
    [SerializeField] private Color buttonNormalColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Color buttonTextVertexColor = new Color32(192, 10, 10, 255);

    [Header("TextMeshPro Text")]
    [SerializeField] private GameObject textEasy;
    [SerializeField] private GameObject textNormal;
    [SerializeField] private GameObject textHard;

    public void DifficultyEasy()
    {
        DifficultyChange(1);
    }

    public void DifficultyNormal()
    {
        DifficultyChange(2);
    }

    public void DifficultyHard()
    {
        DifficultyChange(3);
    }

    public void DifficultyChange(int diff)
    {
        textEasy.SetActive(false);
        textNormal.SetActive(false);
        textHard.SetActive(false);

        buttonTextEasy.color = buttonNormalColor;
        buttonTextNormal.color = buttonNormalColor;
        buttonTextHard.color = buttonNormalColor;

        ColorBlock colorBlock = buttonEasy.GetComponent<Button>().colors; // To preserve other Color
        Color normalColorAlpha = buttonNormalColor;

        normalColorAlpha.a = 0f;
        colorBlock.normalColor = normalColorAlpha;
        buttonEasy.colors = colorBlock;
        buttonNormal.colors = colorBlock;
        buttonHard.colors = colorBlock;

        normalColorAlpha.a = 255f;
        colorBlock.normalColor = normalColorAlpha;
        if (diff == 1)
        {
            PlayerPrefs.SetInt("GameDifficulty", 1);
            textEasy.SetActive(true);
            buttonTextEasy.color = buttonTextVertexColor;
            buttonEasy.colors = colorBlock;
        }
        if (diff == 2)
        {
            PlayerPrefs.SetInt("GameDifficulty", 2);
            textNormal.SetActive(true);
            buttonTextNormal.color = buttonTextVertexColor;
            buttonNormal.colors = colorBlock;
        }
        if (diff == 3)
        {
            PlayerPrefs.SetInt("GameDifficulty", 3);
            textHard.SetActive(true);
            buttonTextHard.color = buttonTextVertexColor;
            buttonHard.colors = colorBlock;
        }
    }
}
