using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryView;
    [SerializeField] private GameObject objectives;
    [SerializeField] private TextMeshProUGUI inventoryText;
    [SerializeField] private GameObject useSyrupButton;
    [SerializeField] private Sprite[] spriteList;
    [SerializeField] private Image[] image;
    [SerializeField] private GameObject[] buttonObject;

    int num;
    int iObj = 0;
    int stageNum;

    public void AccessInventory()
    {
        Debug.Log(inventoryView.activeSelf ? "Inventory is Active" : "Inventory is Inactive");
        if (inventoryView.activeSelf == false)
        {
            ClearInventory();
            LoadItemImage();
            inventoryView.SetActive(true);
            objectives.SetActive(false);
        }
        else if (inventoryView.activeSelf == true)
        {
            inventoryView.SetActive(false);
            objectives.SetActive(true);
            inventoryText.text = "";
            useSyrupButton.SetActive(false);
        }
    }

    void ClearInventory()
    {
        inventoryText.text = "";
        useSyrupButton.SetActive(false);
        foreach (GameObject fObj in buttonObject)
        {
            fObj.GetComponent<Button>().image.sprite = null;
            fObj.SetActive(false);
        }
    }

    void LoadItemImage()
    {
        stageNum = PlayerPrefs.GetInt("Current Stage", 1);
        if (stageNum == 1)
        {
            int keyA = PlayerPrefs.GetInt("S1 Key A");
            int keyB = PlayerPrefs.GetInt("S1 Key B");
            int keyC = PlayerPrefs.GetInt("S1 Key C");
            int keyD = PlayerPrefs.GetInt("S1 Key D");
            int keyE = PlayerPrefs.GetInt("S1 Key E");
            int keyF = PlayerPrefs.GetInt("S1 Key F");
            int vitamin = PlayerPrefs.GetInt("S1 Vitamin");
            iObj = 0;
            if (keyA == 1) LoadSprite(0);
            if (keyB == 1) LoadSprite(1);
            if (keyC == 1) LoadSprite(2);
            if (keyD == 1) LoadSprite(3);
            if (keyE == 1) LoadSprite(4);
            if (keyF == 1) LoadSprite(5);
            if (vitamin == 1) LoadSprite(8);
        }
        if (stageNum == 2)
        {
            int keyA = PlayerPrefs.GetInt("S2 Key A");
            int keyB = PlayerPrefs.GetInt("S2 Key B");
            int keyC = PlayerPrefs.GetInt("S2 Key C");
            int keyD = PlayerPrefs.GetInt("S2 Key D");
            int keyE = PlayerPrefs.GetInt("S2 Key E");
            int keyF = PlayerPrefs.GetInt("S2 Key F");
            int alcohol = PlayerPrefs.GetInt("S2 Alcohol");
            iObj = 0;
            if (keyA == 1) LoadSprite(0);
            if (keyB == 1) LoadSprite(1);
            if (keyC == 1) LoadSprite(2);
            if (keyD == 1) LoadSprite(3);
            if (keyE == 1) LoadSprite(4);
            if (keyF == 1) LoadSprite(5);
            if (alcohol == 1) LoadSprite(9);
        }
        if (stageNum == 3)
        {
            int facemask = PlayerPrefs.GetInt("S3 Face Mask");
            iObj = 0;
            if (facemask == 1) LoadSprite(10);
        }
        if (stageNum == 4)
        {
            int keyA = PlayerPrefs.GetInt("S4 Key A");
            int keyB = PlayerPrefs.GetInt("S4 Key B");
            int keyC = PlayerPrefs.GetInt("S4 Key C");
            int keyD = PlayerPrefs.GetInt("S4 Key D");
            int keyE = PlayerPrefs.GetInt("S4 Key E");
            int keyF = PlayerPrefs.GetInt("S4 Key F");
            int vitamin = PlayerPrefs.GetInt("S4 Face Shield");
            iObj = 0;
            if (keyA == 1) LoadSprite(0);
            if (keyB == 1) LoadSprite(1);
            if (keyC == 1) LoadSprite(2);
            if (keyD == 1) LoadSprite(3);
            if (keyE == 1) LoadSprite(4);
            if (keyF == 1) LoadSprite(5);
            if (vitamin == 1) LoadSprite(11);
        }
        if (stageNum == 5)
        {
            int vitamin = PlayerPrefs.GetInt("S5 Vaccine");
            iObj = 0;
            if (vitamin == 1) LoadSprite(12);
        }
        int syrup = PlayerPrefs.GetInt("Special Syrup");
        int coin = PlayerPrefs.GetInt("Coin");
        if (syrup >= 1) LoadSprite(6);
        if (coin >= 10) LoadSprite(7);
    }

    void LoadSprite(int num)
    {
        buttonObject[iObj].SetActive(true);
        buttonObject[iObj].GetComponent<Button>().image.sprite = spriteList[num];
        iObj++;
    }

    public void ReloadInventory()
    {
        ClearInventory();
        LoadItemImage();
    }

    public void GetInventoryText()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        string getNum = name.Replace("InvButton", "");
        if (int.TryParse(getNum, out num))
            LoadText();
        else
            Debug.Log("Not a valid int");
    }

    void LoadText()
    {
        Debug.Log("Sprite Name: " + buttonObject[num].GetComponent<Image>().sprite.name + " Num: " + num);
        string imageName = buttonObject[num].GetComponent<Image>().sprite.name;
        inventoryText.text = "";
        useSyrupButton.SetActive(false);
        if (stageNum == 1)
        {
            if (imageName == "S1 Inventory Key A") inventoryText.text = TextManager.S1_Inventory_A;
            if (imageName == "S1 Inventory Key B") inventoryText.text = TextManager.S1_Inventory_B;
            if (imageName == "S1 Inventory Key C") inventoryText.text = TextManager.S1_Inventory_C;
            if (imageName == "S1 Inventory Key D") inventoryText.text = TextManager.S1_Inventory_D;
            if (imageName == "S1 Inventory Key E") inventoryText.text = TextManager.S1_Inventory_E;
            if (imageName == "S1 Inventory Key F") inventoryText.text = TextManager.S1_Inventory_F;
            if (imageName == "S1 Inventory Vitamin") inventoryText.text = TextManager.inventoryVitamin;
        }
        if (stageNum == 2)
        {
            if (imageName == "S2 Inventory Key A") inventoryText.text = TextManager.S1_Inventory_A;
            if (imageName == "S2 Inventory Key B") inventoryText.text = TextManager.S2_Inventory_A;
            if (imageName == "S2 Inventory Key C") inventoryText.text = TextManager.S2_Inventory_A;
            if (imageName == "S2 Inventory Key D") inventoryText.text = TextManager.S2_Inventory_A;
            if (imageName == "S2 Inventory Key E") inventoryText.text = TextManager.S2_Inventory_A;
            if (imageName == "S2 Inventory Key F") inventoryText.text = TextManager.S2_Inventory_A;
            if (imageName == "S2 Inventory Alcohol") inventoryText.text = TextManager.inventoryAlcohol;
        }
        if (stageNum == 3)
        {
            if (imageName == "S1 Inventory Face Mask") inventoryText.text = TextManager.inventoryFaceMask;
        }
        if (stageNum == 4)
        {
            if (imageName == "S1 Inventory Face Shield") inventoryText.text = TextManager.inventoryFaceShield;
        }
        if (stageNum == 5)
        {
            if (imageName == "S1 Inventory Vaccine") inventoryText.text = TextManager.inventoryVaccine;
        }
        if (imageName == "Inventory Syrup")
        {
            int syrupCount = PlayerPrefs.GetInt("Special Syrup");
            inventoryText.text = "[" + syrupCount + "] " + TextManager.inventorySyrup;
            useSyrupButton.SetActive(true);
        }

        if (imageName == "Inventory Coin")
        {
            int coinCount = PlayerPrefs.GetInt("Coin");
            inventoryText.text = "[" + coinCount + "] " + TextManager.inventoryCoin;
        }
    }
}
