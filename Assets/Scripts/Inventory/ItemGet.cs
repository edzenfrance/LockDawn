using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemGet : MonoBehaviour
{
    [SerializeField] private Toggle[] toggleObjectives;
    [SerializeField] private GameObject stageComplete;
    [SerializeField] private TextMeshProUGUI grabItem;
    [SerializeField] private string objectName;
    [SerializeField] private Inventory inventory;

    public RiddleManager riddleManager;

    public void getItem()
    {
        if (objectName == "S1 Key A")
            PlayerPrefs.SetInt("Key A", 1);

        if (objectName == "S1 Special Syrup")
        {
            PlayerPrefs.SetInt("Special Syrup", 1);
            toggleObjectives[1].isOn = true;
        }
        if (objectName == "S1 Vitamins")
        {
            PlayerPrefs.SetInt("Vitamin", 1);
            stageComplete.SetActive(true);
            toggleObjectives[2].isOn = true;
        }
        if (objectName == "S1 Riddle A")
        {
            toggleObjectives[3].isOn = true;
            riddleManager.ProcessRiddle(objectName);
            return;
        }
        inventory.ReLoadItemImage();
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
            case "S1 Riddle A":
                handtext = "Answer the riddle";
                break;
            case "S1 Riddle B":
                handtext = "Answer the riddle";
                break;
            default:
                handtext = "Get this thing";
                break;
        }
        grabItem.text = handtext;
        Debug.Log("<color=white>ItemGrabDetection</color> - Detected Item: " + objectName);
    }
}
