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

    public void AccessInventory()
    {
        Debug.Log(inventoryView.activeSelf ? "Inventory is Active" : " Inventory is Inactive");
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
        int keyA = PlayerPrefs.GetInt("Key A");
        int keyB = PlayerPrefs.GetInt("Key B");
        int keyC = PlayerPrefs.GetInt("Key C");
        int keyD = PlayerPrefs.GetInt("Key D");
        int keyE = PlayerPrefs.GetInt("Key E");
        int keyF = PlayerPrefs.GetInt("Key F");
        int vitamin = PlayerPrefs.GetInt("Vitamin");
        int syrup = PlayerPrefs.GetInt("Special Syrup");
        int coin = PlayerPrefs.GetInt("Coin");
        iObj = 0;
        if (keyA == 1) LoadSprite(0);
        if (keyB == 1) LoadSprite(1);
        if (keyC == 1) LoadSprite(2);
        if (keyD == 1) LoadSprite(3);
        if (keyE == 1) LoadSprite(4);
        if (keyF == 1) LoadSprite(5);
        if (syrup >= 1) LoadSprite(6);
        if (vitamin == 1) LoadSprite(7);
        if (coin >= 10) LoadSprite(8);
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
        if (imageName == "Inventory Key A")
            inventoryText.text = "<color=green>Door Key: Upper Floor</color> - Can be use to open a locked door.";
        if (imageName == "Inventory Key B")
            inventoryText.text = "<color=green>Door Key: Stock Room</color> - Can be use to open a locked door.";
        if (imageName == "Inventory Key C")
            inventoryText.text = "<color=green>Door Key: Small Room</color> - Can be use to open a locked door.";
        if (imageName == "Inventory Key D")
            inventoryText.text = "<color=green>Door Key: Large Room</color> - Can be use to open a locked door.";
        if (imageName == "Inventory Key E")
            inventoryText.text = "<color=green>Door Key: Bath Room</color> - Can be use to open a locked door.";
        if (imageName == "Inventory Key F")
            inventoryText.text = "<color=green>Door Key: Kitchen</color> - Can be use to open a locked door.";
        if (imageName == "Inventory Syrup")
        {
            inventoryText.text = "<color=green>Special Syrup</color> - Can be use to stop your health from draining.";
            useSyrupButton.SetActive(true);
        }
        if (imageName == "Inventory Vitamin Bottle")
            inventoryText.text = "<color=green>Vitamins</color> - This item increased your immunity.";
        if (imageName == "Inventory Coin")
        {
            int coinCount = PlayerPrefs.GetInt("Coin");
            inventoryText.text = "<color=yellow>Coin (" + coinCount + ")</color> - Can be used to buy new skin in shop.";
        }
    }
}
