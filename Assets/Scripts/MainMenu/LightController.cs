using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    public Animator lightAnimation;
    public CanvasGroup canvasGroup;
    public Toggle lightToggle;
    [SerializeField] private float lightAnimationEnabled = 1;

    void Start()
    {
        lightAnimationEnabled = PlayerPrefs.GetInt("animEnabled");

        if (lightAnimationEnabled == 0)
        {
            Debug.Log("AnimationController - PLAYERPREFS LIGHT: ENABLED [" + lightAnimationEnabled + "]");
            lightAnimation.enabled = true;
        }
        else
        {
            Debug.Log("AnimationController - PLAYERPREFS LIGHT: DISABLED [" + lightAnimationEnabled + "]");
            lightAnimation.enabled = false;
            canvasGroup.GetComponent<CanvasGroup>().alpha = 1.0f;
        }
    }

    public void lightSwitch()
    {
        bool onoffLight = lightToggle.GetComponent<Toggle>().isOn;
        if (onoffLight)
        {
            lightAnimation.enabled = false;
            canvasGroup.GetComponent<CanvasGroup>().alpha = 1.0f;
            PlayerPrefs.SetInt("animEnabled", 1);
            Debug.Log("LightController - PLAYERPREFS LIGHT: DISABLED");
        }
        else
        {
            lightAnimation.enabled = true;
            PlayerPrefs.SetInt("animEnabled", 0);
            Debug.Log("LightController - PLAYERPREFS LIGHT: ENABLED");
        }
    }
}
