using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchZoneButton : MonoBehaviour
{
    [SerializeField] private GameObject UIVirtualButtonJump;
    [SerializeField] private GameObject UIVirtualButtonSprint;
    [SerializeField] private Toggle enableJumpToggle;
    [SerializeField] private Toggle enableSprintToggle;
    [SerializeField] private int jumpEnabled;
    [SerializeField] private int sprintEnabled;

    void Start()
    {
        jumpEnabled = PlayerPrefs.GetInt("EnableJumpButton", 1);
        sprintEnabled = PlayerPrefs.GetInt("EnableSprintButton", 1);

        if (jumpEnabled == 0)
        {
            this.Invoke(() => JumpToggleIsOn(true), 0f);
        }
        if (jumpEnabled == 1)
        {
            this.Invoke(() => JumpToggleIsOn(false), 0f);
        }
        if (sprintEnabled == 0)
        {
            this.Invoke(() => SprintToggleIsOn(true), 0f);
        }
        if (sprintEnabled == 1)
        {
            this.Invoke(() => SprintToggleIsOn(false), 0f);
        }
    }

    void JumpToggleIsOn(bool value)
    {
        Debug.Log("<color=white>TouchZoneButton</color> - (Start) Jump Button: " + value);
        UIVirtualButtonJump.SetActive(value);
        enableJumpToggle.isOn = value;
    }

    void SprintToggleIsOn(bool value)
    {
        Debug.Log("<color=white>TouchZoneButton</color> - (Start) Sprint Button: " + value);
        UIVirtualButtonSprint.SetActive(value);
        enableSprintToggle.isOn = value;
    }


    public void ToggleJumpButton()
    {
        bool onoffJumpButton = enableJumpToggle.GetComponent<Toggle>().isOn;
        if (onoffJumpButton)
        {
            UIVirtualButtonJump.SetActive(true);
            PlayerPrefs.SetInt("EnableJumpButton", 0);
            Debug.Log("<color=white>TouchZoneButton</color> - (Toggle) Jump Button Enabled");
        }
        else
        {
            UIVirtualButtonJump.SetActive(false);
            PlayerPrefs.SetInt("EnableJumpButton", 1);
            Debug.Log("<color=white>TouchZoneButton</color> - (Toggle) Jump Button Disabled");
        }
    }

    public void ToggleSprintButton()
    {
        bool onoffSprintButton = enableSprintToggle.GetComponent<Toggle>().isOn;
        if (onoffSprintButton)
        {
            UIVirtualButtonSprint.SetActive(true);
            PlayerPrefs.SetInt("EnableSprintButton", 0);
            Debug.Log("<color=white>TouchZoneButton</color> - (Toggle) Sprint Button Enabled");
        }
        else
        {
            UIVirtualButtonSprint.SetActive(false);
            PlayerPrefs.SetInt("EnableSprintButton", 1);
            Debug.Log("<color=white>TouchZoneButton</color> - (Toggle) Sprint Button Disabled");
        }
    }
}

public static class Utility
{
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}
