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

    [Header("Types")]
    [SerializeField] private string objectName;
    [SerializeField] private bool getCoin;

    [Header("Scripts")]
    [SerializeField] private RiddleManager riddleManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private NoteController noteController;
    [SerializeField] private ImmunityController immunityController;

    [Header("Exit")]
    [SerializeField] private GameObject[] stageExit;

    public void getItem()
    {
        if (objectName == "S1 Key A") SaveKey("S1 Key A", TextManager.S1_DoorKey_A_Add);
        if (objectName == "S1 Key B") SaveKey("S1 Key B", TextManager.S1_DoorKey_B_Add);
        if (objectName == "S1 Key C") SaveKey("S1 Key C", TextManager.S1_DoorKey_C_Add);
        if (objectName == "S1 Key D") SaveKey("S1 Key D", TextManager.S1_DoorKey_D_Add);
        if (objectName == "S1 Key E") SaveKey("S1 Key E", TextManager.S1_DoorKey_E_Add);
        if (objectName == "S1 Key F") SaveKey("S1 Key F", TextManager.S1_DoorKey_F_Add);

        if (objectName == "S2 Key A") SaveKey("S2 Key A", TextManager.S1_DoorKey_A_Add);
        if (objectName == "S2 Key B") SaveKey("S2 Key B", TextManager.S1_DoorKey_B_Add);
        if (objectName == "S2 Key C") SaveKey("S2 Key C", TextManager.S1_DoorKey_C_Add);
        if (objectName == "S2 Key D") SaveKey("S2 Key D", TextManager.S1_DoorKey_D_Add);
        if (objectName == "S2 Key E") SaveKey("S2 Key E", TextManager.S1_DoorKey_E_Add);
        if (objectName == "S2 Key F") SaveKey("S2 Key F", TextManager.S1_DoorKey_F_Add);

        if (objectName == "S1 Vitamins")
        {
            saveManager.ObtainMainItemImmunity("Vitamins", 5);
            immunityController.CheckImmunity();
            stageExit[0].SetActive(true);
            noteController.ShowNote(TextManager.gotVitamin, 3.0f);
            toggleObjectives[0].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S2 Alcohol")
        {
            saveManager.ObtainMainItemImmunity("Alcohol", 10);
            immunityController.CheckImmunity();
            stageExit[1].SetActive(true);
            noteController.ShowNote(TextManager.gotAlcohol, 3.0f);
            toggleObjectives[0].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S3 Face Mask")
        {
            saveManager.ObtainMainItemImmunity("Face Mask", 30);
            immunityController.CheckImmunity();
            stageExit[2].SetActive(true);
            noteController.ShowNote(TextManager.gotFaceMask, 3.0f);
            toggleObjectives[0].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S4 Face Shield")
        {
            saveManager.ObtainMainItemImmunity("Face Shield", 50);
            immunityController.CheckImmunity();
            stageExit[3].SetActive(true);
            noteController.ShowNote(TextManager.gotFaceShield, 3.0f);
            toggleObjectives[0].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S5 Vaccine")
        {
            saveManager.ObtainMainItemImmunity("Vaccine", 100);
            immunityController.CheckImmunity();
            stageExit[4].SetActive(true);
            noteController.ShowNote(TextManager.gotVaccine, 3.0f);
            toggleObjectives[0].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName.Contains("Riddle"))
        {
            toggleObjectives[2].isOn = true;
            riddleManager.ProcessRiddle(objectName);
            audioManager.PlayAudioPickUpPaper();
        }

        if (objectName.Contains("Special Syrup"))
        {
            saveManager.SetSpecialSyrup();
            toggleObjectives[1].isOn = true;
            audioManager.PlayAudioPickUpBottle();
        }

        if (objectName.Contains("Coin"))
        {
            noteController.ShowNote(TextManager.coinAdded, 1.5f);
            saveManager.SetCoin();
            audioManager.PlayAudioPickUpCoin();
        }

        inventory.ReloadInventory();
        saveManager.GetCurrentStage();
        int currentStage = SaveManager.currentStage;
        GameObject detectedObject = GameObject.Find("Item/Stage" + currentStage + "/" + objectName);
        detectedObject.SetActive(false);
        gameObject.SetActive(false);
        Debug.Log("<color=white>ItemGet</color> - Added to inventory: " + objectName);
    }

    public void ItemInfo(string itemNote, string objName)
    {
        objectName = objName;
        grabItem.text = itemNote;
        Debug.Log("<color=white>ItemGet</color> - Detected Item: " + objectName);
    }

    void SaveKey(string KeyName, string KeyNote)
    {
        saveManager.SetKeyName(KeyName);
        saveManager.SetKeyCount();
        saveManager.GetKeyCount();
        int keyCount = SaveManager.keyCount;
        taskKeyText.text = "Keys (" + keyCount + " of 6)";
        noteController.ShowNote(KeyNote + "</color> " + TextManager.addedToInventory, 3.0f);
        audioManager.PlayAudioPickUpKey();
        if (keyCount == 6)
            toggleObjectives[0].isOn = true;
    }
}
