using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckSave : MonoBehaviour
{
    [Header("Continue Button")]
    public TextMeshProUGUI continueText;
    public Button continueButton;

    private ColorBlock buttonColor;

    private void Awake()
    {
        // Add TextMeshPro to attached object
        //continueTextMesh = GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();

        buttonColor.highlightedColor = new Color32(255, 255, 255, 0);
        buttonColor.normalColor = new Color32(255, 255, 255, 80); // Alpha is 80 for mouse only
        buttonColor.pressedColor = new Color32(221, 5, 5, 60);
        buttonColor.selectedColor = new Color32(221, 5, 5, 60);
        buttonColor.disabledColor = new Color32(200, 200, 200, 0);

        continueButton.colors = buttonColor;

        continueText.color = new Color32(200, 200, 200, 128);


    }

}
