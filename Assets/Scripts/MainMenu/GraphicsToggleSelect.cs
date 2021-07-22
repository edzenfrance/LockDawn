using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsToggleSelect : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;

    private void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }

    public void LogSelectedToggle()
    {
        // May have several selected toggles
        foreach (Toggle toggle in toggleGroup.ActiveToggles())
        {
            if (toggle.name == "Low")
            {
                Debug.Log("GraphicsController - Quality: Low");
            }
            if (toggle.name == "Medium")
            {
                Debug.Log("GraphicsController - Quality: Medium");
            }
            if (toggle.name == "High")
            {
                Debug.Log("GraphicsController - Quality: High");
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
