using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    [SerializeField] private Animator lightAnimation;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Toggle lightToggle;
    [SerializeField] private float lightAnimationEnabled = 1;

    void Start()
    {
        lightAnimationEnabled = PlayerPrefs.GetInt("LightFixed");

        if (lightAnimationEnabled == 0)
        {
            Debug.Log("<color=white>LightController</color> - Light Animation: Enabled");
            lightAnimation.enabled = true;
            lightToggle.isOn = false;
        }
        else
        {
            Debug.Log("<color=white>LightController</color> - Light Animation: Disabled");
            lightAnimation.enabled = false;
            canvasGroup.GetComponent<CanvasGroup>().alpha = 1.0f;
            lightToggle.isOn = true;
        }
    }

    public void lightSwitch()
    {
        bool onoffLight = lightToggle.GetComponent<Toggle>().isOn;
        if (onoffLight)
        {
            lightAnimation.enabled = false;
            canvasGroup.GetComponent<CanvasGroup>().alpha = 1.0f;
            PlayerPrefs.SetInt("LightFixed", 1);
            Debug.Log("<color=white>LightController</color> - Set Light Animation: <color=black>Disabled</color>");
        }
        else
        {
            lightAnimation.enabled = true;
            PlayerPrefs.SetInt("LightFixed", 0);
            Debug.Log("<color=white>LightController</color> - Set Light Animation: <color=white>Enabled</color>");
        }
    }
}
