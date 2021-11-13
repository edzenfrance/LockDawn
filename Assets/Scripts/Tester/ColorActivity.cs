// forum.unity.com/threads/changing-the-color-of-a-ui-button-with-scripting.267093/#post-2160892

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Test : MonoBehaviour
{
    //Reference to button to access its components
    private Button theButton;

    //this get the Transitions of the Button as its pressed
    private ColorBlock theColor;

    // Use this for initialization
    void Awake()
    {
        theButton = GetComponent<Button>();
        theColor = GetComponent<Button>().colors;

    }


    /// <summary>
    /// Example
    /// just add this to your Button component On Click()
    /// </summary>
    public void ButtonTransitionColors()
    {

        theColor.highlightedColor = Color.blue;
        theColor.normalColor = Color.cyan;
        theColor.pressedColor = Color.green;

        theButton.colors = theColor;
    }
}

