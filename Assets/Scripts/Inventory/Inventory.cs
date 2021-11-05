using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryView;
    [SerializeField] private Sprite[] spriteList;
    [SerializeField] private Image[] imageObject;


    public void AccessInventory()
    {
        Debug.Log(inventoryView.activeSelf ? "Inventory is Active" : " Inventory is Inactive");
        if (inventoryView.activeSelf == false)
        {
            LoadItemImage();
            inventoryView.SetActive(true); 
        }
        else if (inventoryView.activeSelf == true)
        {
            inventoryView.SetActive(false);
        }
    }

    void LoadItemImage()
    {
        PlayerPrefs.SetInt("Key A", 1);
        PlayerPrefs.SetInt("Vitamin", 1);
        PlayerPrefs.SetInt("Special Syrup", 1);
        int KeyA = PlayerPrefs.GetInt("Key A");
        int vitamin = PlayerPrefs.GetInt("Vitamin");
        int syrup = PlayerPrefs.GetInt("Special Syrup");
        int iObj = 0;
        if (KeyA == 1)
        {
            imageObject[iObj].sprite = spriteList[0];
            iObj++;
        }
        if (vitamin == 1)
        {
            imageObject[iObj].sprite = spriteList[1];
            iObj++;
        }
        if (syrup == 1)
        {
            imageObject[iObj].sprite = spriteList[2];
            iObj++;
        }
    }
}
