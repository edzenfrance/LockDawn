using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemGet : MonoBehaviour
{
    [SerializeField] private Toggle[] toggleObjectives;
    [SerializeField] private TextMeshProUGUI grabItem;
    [SerializeField] private Inventory inventory;
    [SerializeField] private TextMeshProUGUI taskKeyText;

    [Header("Exit")]
    [SerializeField] private GameObject[] stageExit;

    [Header("Types")]
    [SerializeField] private string objectName;
    [SerializeField] private bool getCoin;

    [Header("Scripts")]
    [SerializeField] private RiddleManager riddleManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private NoteController noteController;
    [SerializeField] private ImmunityController immunityController;

    int keyCount;

    public void getItem()
    {
        if (objectName == "S1 Key A") KeyCount("S1 Key A", "Door Key: Upper Floor");
        if (objectName == "S1 Key B") KeyCount("S1 Key B", "Door Key: Stock Room");
        if (objectName == "S1 Key C") KeyCount("S1 Key C", "Door Key: Small Room");
        if (objectName == "S1 Key D") KeyCount("S1 Key D", "Door Key: Large Room");
        if (objectName == "S1 Key E") KeyCount("S1 Key E", "Door Key: Bathroom");
        if (objectName == "S1 Key F") KeyCount("S1 Key F", "Door Key: Kitchen");
        if (objectName == "S1 Special Syrup")
        {
            saveManager.SetSpecialSyrup();
            toggleObjectives[1].isOn = true;
            audioManager.PlayAudioPickUpBottle();
        }
        if (objectName == "S1 Vitamins")
        {
            saveManager.ObtainMainItemImmunity("Vitamins", 5);
            immunityController.CheckImmunity();
            stageExit[0].SetActive(true);
            noteController.ShowNote("You got the main item vitamins!\nExit the house to finish the stage!", 3.0f);
            toggleObjectives[2].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S2 Alcohol")
        {
            saveManager.ObtainMainItemImmunity("Alcohol", 10);
            immunityController.CheckImmunity();
            stageExit[1].SetActive(true);
            noteController.ShowNote("You got the main item alcohol!\nExit the house to finish the stage!", 3.0f);
            toggleObjectives[2].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S3 Face Mask")
        {
            saveManager.ObtainMainItemImmunity("Face Mask", 30);
            immunityController.CheckImmunity();
            stageExit[2].SetActive(true);
            noteController.ShowNote("You got the main item face mask!\nGoto the end of the road to finish the staeg", 3.0f);
            toggleObjectives[2].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S4 Face Shield")
        {
            saveManager.ObtainMainItemImmunity("Face Shield", 50);
            immunityController.CheckImmunity();
            stageExit[3].SetActive(true);
            noteController.ShowNote("You got the main item face shield!\nExit the mall to finish the stage", 3.0f);
            toggleObjectives[2].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S5 Vaccine")
        {
            saveManager.ObtainMainItemImmunity("Vaccine", 100);
            immunityController.CheckImmunity();
            stageExit[4].SetActive(true);
            noteController.ShowNote("You got the main item vitamins!\nExit the hospital to finish the staeg!", 3.0f);
            toggleObjectives[2].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S1 Riddle A")
        {
            toggleObjectives[3].isOn = true;
            riddleManager.ProcessRiddle(objectName);
            audioManager.PlayAudioPickUpItem();
            return;
        }
        if (getCoin)
        {
            noteController.ShowNote("<color=yellow>+10 coins</color> added to inventory.", 1.5f);
            saveManager.SetCoin();
            audioManager.PlayAudioPickUpCoin();
        }
        inventory.ReloadInventory ();
        GameObject detectedObject = GameObject.Find("Item/"+ objectName);
        detectedObject.SetActive(false);
        gameObject.SetActive(false);
        Debug.Log("<color=white>ItemGet</color> - Added to inventory: " + objectName);
    }

    void KeyCount(string KeyName, string KeyNote)
    {
        saveManager.SetKey(KeyName);
        keyCount =  PlayerPrefs.GetInt("Key Count");
        taskKeyText.text = "Keys (" + keyCount + " of 6)";
        noteController.ShowNote("<color=green>" + KeyNote + "</color> added to inventory.", 2.0f);
        audioManager.PlayAudioPickUpKey();
        if (keyCount == 6)
            toggleObjectives[0].isOn = true;
    }

    public void ItemInfo(string itemNote, string objName)
    {
        if (itemNote == "Get the <color=yellow>Coin")
            getCoin = true;
        else
            getCoin = false;
        objectName = objName;
        grabItem.text = itemNote;
        Debug.Log("<color=white>ItemGet</color> - Detected Item: " + objectName);
    }
}
