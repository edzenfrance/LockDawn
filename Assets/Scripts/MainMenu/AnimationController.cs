using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator lightAnimation;
    public CanvasGroup canvasGroup;
    public GameObject buttonUncheck;
    public GameObject buttonCheck;

    private float LightAnimEnabled = 1;

    void Start()
    {
        LightAnimEnabled = PlayerPrefs.GetInt("animEnabled");
        Debug.Log("PLAYERPREFS LIGHTS: " + LightAnimEnabled);

        if (LightAnimEnabled == 0)
        {
            Debug.Log("LIGHTS IS ENABLED: " + LightAnimEnabled + " TRUE");
            lightAnimation.enabled = true;
            buttonCheck.SetActive(false);
            buttonUncheck.SetActive(true);
        }
        if (LightAnimEnabled == 1)
        {
            Debug.Log("LIGHTS ENABLED: " + LightAnimEnabled + " FALSE");
            lightAnimation.enabled = false;
            canvasGroup.GetComponent<CanvasGroup>().alpha = 1.0f;
            buttonCheck.SetActive(true);
            buttonUncheck.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
