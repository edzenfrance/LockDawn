using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemGet : MonoBehaviour
{

    [SerializeField] private Toggle[] toggleObjectives;
    [SerializeField] private TextMeshProUGUI grabItem;
    [SerializeField] private TextMeshProUGUI taskKeyText;

    [Header("Immunity")]
    [SerializeField] private Slider immunityBar;
    [SerializeField] private GameObject immunityFill;
    [SerializeField] private TextMeshProUGUI immunityText;

    [Header("Types")]
    [SerializeField] private string objectName;

    [Header("Scripts")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private NoteController noteController;
    [SerializeField] private RiddleManager riddleManager;
    [SerializeField] private ImmunityController immunityController;
    [SerializeField] private Inventory inventory;

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

        if (objectName == "S1 Vitamin")
        {
            SaveManager.ObtainMainItem("Vitamin");
            SaveManager.SetCurrrentImmunity(5);
            immunityController.CheckImmunity();
            stageExit[0].SetActive(true);
            noteController.ShowNote(TextManager.gotVitamin, 5.0f);
            toggleObjectives[0].isOn = true;
            SaveManager.GetCurrentImmunity();
            immunityFill.SetActive(true);
            immunityBar.value = SaveManager.currentImmunity;
            immunityText.text = SaveManager.currentImmunity.ToString();
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S2 Alcohol")
        {
            SaveManager.ObtainMainItem("Alcohol");
            SaveManager.SetCurrrentImmunity(10);
            immunityController.CheckImmunity();
            stageExit[1].SetActive(true);
            noteController.ShowNote(TextManager.gotAlcohol, 5.0f);
            toggleObjectives[0].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S3 Face Mask")
        {
            SaveManager.ObtainMainItem("Face Mask");
            SaveManager.SetCurrrentImmunity(30);

            immunityController.CheckImmunity();
            stageExit[2].SetActive(true);
            noteController.ShowNote(TextManager.gotFaceMask, 5.0f);
            toggleObjectives[0].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S4 Face Shield")
        {
            SaveManager.ObtainMainItem("Face Shield");
            SaveManager.SetCurrrentImmunity(50);
            immunityController.CheckImmunity();
            stageExit[3].SetActive(true);
            noteController.ShowNote(TextManager.gotFaceShield, 5.0f);
            toggleObjectives[0].isOn = true;
            audioManager.PlayAudioPickUpItem();
        }

        if (objectName == "S5 Vaccine")
        {
            SaveManager.ObtainMainItem("Vaccine");
            SaveManager.SetCurrrentImmunity(100);
            immunityController.CheckImmunity();
            stageExit[4].SetActive(true);
            noteController.ShowNote(TextManager.gotVaccine, 5.0f);
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
            SaveManager.SetSpecialSyrup();
            toggleObjectives[1].isOn = true;
            audioManager.PlayAudioPickUpBottle();
        }

        if (objectName.Contains("Coin"))
        {
            noteController.ShowNote(TextManager.coinAdded, 1.5f);
            SaveManager.SetCoin();
            audioManager.PlayAudioPickUpCoin();
        }

        inventory.ReloadInventory();
        SaveManager.GetCurrentStage();
        GameObject detectedObject = GameObject.Find("Item/Stage" + SaveManager.currentStage.ToString() + "/" + objectName);
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
        SaveManager.SetKeyName(KeyName, 1);
        SaveManager.SetKeyCount();
        SaveManager.GetKeyCount();
        taskKeyText.text = "Keys (" + SaveManager.keyCount + " of 6)";
        noteController.ShowNote(KeyNote + "</color> " + TextManager.addedToInventory, 3.0f);
        audioManager.PlayAudioPickUpKey();
        if (SaveManager.keyCount == 6)
            toggleObjectives[0].isOn = true;
    }
}
