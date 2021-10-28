using System.Collections;
using UnityEngine;

public class ItemGrabDetection : MonoBehaviour
{
    [SerializeField] private string detectedItem;
    [SerializeField] private GameObject detectedObject;
    [SerializeField] private GameObject stageComplete;

    public void getItem()
    {
        detectedItem = PlayerPrefs.GetString("DetectedItem");
        Debug.Log("<color=white>ItemGrabDetection</color> - Detected Item: " + detectedItem);
        detectedObject = GameObject.Find(detectedItem);
        detectedObject.SetActive(false);
        gameObject.SetActive(false);
        if (detectedItem == "Key1")
        {
            Debug.Log("<color=white>ItemGrabDetection</color> - Stored Key Item: " + detectedItem);
            PlayerPrefs.SetInt("KeyLock", 1);
        }
        if (detectedItem == "SpecialSyrup")
        {
            Debug.Log("<color=white>ItemGrabDetection</color> - Stored Key Item: " + detectedItem);
            PlayerPrefs.SetInt("SpecialSyrup", 1);
        }
        if (detectedItem == "Vitamins")
        {
            stageComplete.SetActive(true);

        }
    }
}
