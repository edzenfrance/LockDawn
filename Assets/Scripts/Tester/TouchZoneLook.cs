using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TouchZoneLook : MonoBehaviour
{
    [SerializeField] UIVirtualTouchZone UIVirtualTouchZoneLook;
    [SerializeField] private TMP_Text cameraSensitivityText;
    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private float cameraSensitivityValue;

    // Start is called before the first frame update
    void Start()
    {
        cameraSensitivityValue = PlayerPrefs.GetFloat("TouchZoneLookMultiplier", 60);
        cameraSensitivityText.text = "Camera Sensitivity: " + cameraSensitivityValue;
        cameraSensitivitySlider.value = cameraSensitivityValue;
        UIVirtualTouchZoneLook.magnitudeMultiplier = cameraSensitivityValue;
    }

    public void UpdateLookSensitivty(float sensitivity)
    {
        UIVirtualTouchZoneLook.magnitudeMultiplier = sensitivity;
        PlayerPrefs.SetFloat("TouchZoneLookMultiplier", sensitivity);
        cameraSensitivityText.text = "Camera Sensitivity: " + sensitivity;
    }

}
