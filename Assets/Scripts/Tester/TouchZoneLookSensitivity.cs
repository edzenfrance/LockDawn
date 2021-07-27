using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TouchZoneLookSensitivity : MonoBehaviour
{
    [SerializeField] UIVirtualTouchZone updateMultiplier;
    [SerializeField] private TMP_Text lookSensitivityText;
    [SerializeField] private Slider lookSensitivitySlider;
    [SerializeField] private float lookSensitivity;

    // Start is called before the first frame update
    void Start()
    {
        lookSensitivity = PlayerPrefs.GetFloat("LookSensitivity", 60);
        lookSensitivityText.text = "Look Sensitivity: " + lookSensitivity;
        lookSensitivitySlider.value = lookSensitivity;
        updateMultiplier.magnitudeMultiplier = lookSensitivity;
    }

    public void UpdateLookSensitivty(float sensitivity)
    {
        updateMultiplier.magnitudeMultiplier = sensitivity;
        PlayerPrefs.SetFloat("LookSensitivity", sensitivity);
        lookSensitivityText.text = "Look Sensitivity: " + sensitivity;
    }

}
