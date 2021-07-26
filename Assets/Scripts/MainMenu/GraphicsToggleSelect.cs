using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GraphicsToggleSelect : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Toggle toggleQualityLow, toggleQualityMedium, toggleQualityHigh;
    [SerializeField] private int setGraphicsQuality;

    private void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }

    void Start()
    {
        setGraphicsQuality = PlayerPrefs.GetInt("LDGraphicsQuality", 3);
        if (setGraphicsQuality == 1)
        {
            Debug.Log("<color=white>GraphicsToggleSelect</color> - Graphics Quality: Low");
            toggleQualityLow.isOn = true;
        }
        if (setGraphicsQuality == 2)
        {
            Debug.Log("<color=white>GraphicsToggleSelect</color> - Graphics Quality: Medium");
            toggleQualityMedium.isOn = true;
        }
        if (setGraphicsQuality == 3)
        {
            Debug.Log("<color=white>GraphicsToggleSelect</color> - Graphics Quality: High");
            toggleQualityHigh.isOn = true;
        }
    }

    public void LogSelectedToggle()
    {
        // May have several selected toggles
        foreach (Toggle toggle in toggleGroup.ActiveToggles())
        {
            if (toggle.name == "Low")
            {
                Debug.Log("<color=white>GraphicsToggleSelect</color> - Selected Quality: Low");
                PlayerPrefs.SetInt("LDGraphicsQuality", 1);
            }
            if (toggle.name == "Medium")
            {
                Debug.Log("<color=white>GraphicsToggleSelect</color> - Selected Quality: Medium");
                PlayerPrefs.SetInt("LDGraphicsQuality", 2);
            }
            if (toggle.name == "High")
            {
                Debug.Log("<color=white>GraphicsToggleSelect</color> - Selected Quality: High");
                PlayerPrefs.SetInt("LDGraphicsQuality", 3);
            }
        }

        // OR
        /*
        Toggle selectedToggle = ToggleGroup.ActiveToggles().FirstOrDefault();
        if (selectedToggle != null)
            Debug.Log(selectedToggle, selectedToggle);
        */
    }
}
