using System.Collections;
using UnityEngine;
using TMPro;

public class ItemGrabDetection : MonoBehaviour
{
    [SerializeField] private GameObject stageComplete;
    [SerializeField] private TextMeshProUGUI grabItem;
    [SerializeField] private string objectName;

    public void getItem()
    {
        if (objectName == "S1 Key A")
            PlayerPrefs.SetInt("Keys A", 1);

        if (objectName == "S1 Special Syrup")
            PlayerPrefs.SetInt("Special Syrup", 1);

        if (objectName == "S1 Vitamins")
        {
            PlayerPrefs.SetInt("Vitamins", 1);
            stageComplete.SetActive(true);
        }

        GameObject detectedObject = GameObject.Find("Item/"+ objectName);
        detectedObject.SetActive(false);
        gameObject.SetActive(false);
        Debug.Log("<color=white>ItemGrabDetection</color> - Added to inventory: " + objectName);
    }

    public void itemName(string objName)
    {
        string handtext = "";
        objectName = objName;

        switch(objName)
        {
            case "S1 Key A":
                handtext = "Get the key";
                break;
            case "S1 Special Syrup":
                handtext = "Get the special syrup";
                break;
            case "S1 Vitamins":
                handtext = "Get the vitamins!";
                break;
            default:
                handtext = "Get this thing";
                break;
        }
        grabItem.text = handtext;
        Debug.Log("<color=white>ItemGrabDetection</color> - Detected Item: " + objectName);
    }
}
