using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryView;
    [SerializeField] private GameObject objectives;
    [SerializeField] private TextMeshProUGUI inventoryText;
    [SerializeField] private GameObject useButton;
    [SerializeField] private Sprite[] spriteList;
    [SerializeField] private Image[] image;
    [SerializeField] private Button[] button;
    [SerializeField] private GameObject[] buttonObject;

    int num;

    public void AccessInventory()
    {
        Debug.Log(inventoryView.activeSelf ? "Inventory is Active" : " Inventory is Inactive");
        if (inventoryView.activeSelf == false)
        {
            LoadItemImage();
            inventoryView.SetActive(true);
            objectives.SetActive(false);
        }
        else if (inventoryView.activeSelf == true)
        {
            inventoryView.SetActive(false);
            objectives.SetActive(true);
            inventoryText.text = "";
            useButton.SetActive(false);
        }
    }

    void LoadItemImage()
    {
        //PlayerPrefs.DeleteAll();
        int keyA = PlayerPrefs.GetInt("Key A");
        int vitamin = PlayerPrefs.GetInt("Vitamin");
        int syrup = PlayerPrefs.GetInt("Special Syrup");
        int iObj = 0;
        if (keyA == 1)
        {
            buttonObject[iObj].SetActive(true);
            button[iObj].image.sprite = spriteList[0];
            iObj++;
        }
        if (vitamin == 1)
        {
            buttonObject[iObj].SetActive(true);
            button[iObj].image.sprite = spriteList[1];
            iObj++;
        }
        if (syrup == 1)
        {
            buttonObject[iObj].SetActive(true);
            button[iObj].image.sprite = spriteList[2];
            iObj++;
        }
    }

    public void ReLoadItemImage()
    {
        LoadItemImage();
    }

    public void InventoryButtonA()
    {
        num = 0;
        LoadText();
    }

    public void InventoryButtonB()
    {
        num = 1;
        LoadText();
    }

    public void InventoryButtonC()
    {
        num = 2;
        LoadText();
    }

    public void InventoryButtonD()
    {
        num = 3;
        LoadText();
    }

    void LoadText()
    {
        Debug.Log("Sprite Name: " + image[num].sprite.name);
        string imageName = image[num].sprite.name;
        inventoryText.text = "";
        useButton.SetActive(false);
        if (imageName == "Inventory Key")
            inventoryText.text = "This key can unlock door.";
        if (imageName == "Inventory Syrup")
        {
            inventoryText.text = "Special syrup can stop your health from draining.";
            useButton.SetActive(true);
        }
    }
}
