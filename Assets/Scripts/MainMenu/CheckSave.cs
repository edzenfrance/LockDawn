using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CheckSave : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private Button continueButton;

    public TextMeshProUGUI continueText;

    private ColorBlock buttonColor;

    void Awake()
    {
        buttonColor.highlightedColor = new Color32(255, 255, 255, 255);
        buttonColor.normalColor = new Color32(255, 255, 255, 80); // Alpha is 80 for mouse only
        buttonColor.pressedColor = new Color32(221, 5, 5, 60);
        buttonColor.selectedColor = new Color32(221, 5, 5, 60);
        buttonColor.disabledColor = new Color32(200, 200, 200, 0);
        continueButton.colors = buttonColor;
    }

    void OnEnable()
    {
        saveManager.GetContinueGame();
        if (SaveManager.continueGame == 1)
        {
            continueButton.interactable = true;
            continueText.color = new Color32(255, 255, 255, 128);
        }
        else if (SaveManager.continueGame == 0)
        {
            continueButton.interactable = false;
            continueText.color = new Color32(200, 200, 200, 128);
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Stage1", LoadSceneMode.Single);
    }
}
