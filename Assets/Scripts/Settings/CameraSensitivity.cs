using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraSensitivity : MonoBehaviour
{
    [SerializeField] UIVirtualTouchZone UIVirtualTouchZoneLook;
    [SerializeField] private TMP_Text cameraSensitivityText;
    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private float cameraSensitivityValue;

    void Start()
    {
        cameraSensitivityValue = PlayerPrefs.GetFloat("Look Sensitivity", 60);
        cameraSensitivityText.text = cameraSensitivityValue.ToString();
        cameraSensitivitySlider.value = cameraSensitivityValue;
        UIVirtualTouchZoneLook.magnitudeMultiplier = cameraSensitivityValue;
    }

    public void UpdateLookSensitivty(float sensitivity)
    {
        UIVirtualTouchZoneLook.magnitudeMultiplier = sensitivity;
        PlayerPrefs.SetFloat("Look Sensitivity", sensitivity);
        cameraSensitivityText.text = sensitivity.ToString();
    }

}
