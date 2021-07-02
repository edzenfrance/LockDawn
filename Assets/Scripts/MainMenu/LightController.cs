using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Animator lightAnimation;
    public CanvasGroup canvasGroup;
    public GameObject buttonUncheck;
    public GameObject buttonCheck;

    public void LightsOn()
    {
        lightAnimation.enabled = true;
        buttonCheck.SetActive(false);
        buttonUncheck.SetActive(true);
        PlayerPrefs.SetInt("animEnabled", 0);
        Debug.Log("PLAYERPREFS LIGHT: 0");
    }

    public void LightsOff()
    {
        lightAnimation.enabled = false;
        canvasGroup.GetComponent<CanvasGroup>().alpha = 1.0f;
        buttonCheck.SetActive(true);
        buttonUncheck.SetActive(false);
        PlayerPrefs.SetInt("animEnabled", 1);
        Debug.Log("PLAYERPREFS LIGHT: 1");
    }

}
