using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private GameObject inventoryItem;
    [SerializeField] private GameObject inventoryBelt;

    [Header("Buttons")]
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private GameObject beltButtonLeft;
    [SerializeField] private GameObject beltButtonRight;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject mapButton;

    [Header("Images")]
    [SerializeField] private Sprite[] spriteList;

    int checkKeyLock;

    public void OpenInventory()
    {
        // PlayerPrefs.SetInt("KeyLock", 0);
        checkKeyLock = PlayerPrefs.GetInt("KeyLock");
        if (checkKeyLock == 1)
        {
            Debug.Log("Enabling Inventory Item " + checkKeyLock);
            inventoryItem.SetActive(true);
            beltButtonLeft.SetActive(true);
            beltButtonRight.SetActive(true);
        }
        else
        {
            Debug.Log("Disabling Inventory Item " + checkKeyLock);
            inventoryItem.SetActive(false);
            beltButtonLeft.SetActive(false);
            beltButtonRight.SetActive(false);
        }
        inventoryBelt.SetActive(true);
        inventoryButton.SetActive(false);
        pauseButton.SetActive(false);
        mapButton.SetActive(false);
    }

    public void viewLeftInventory()
    {
        inventoryItem.GetComponent<Image>().sprite = spriteList[0];
    }

    public void viewRightInventory()
    {
        inventoryItem.GetComponent<Image>().sprite = spriteList[1];
    }

    public void CloseInventory()
    {
        inventoryBelt.SetActive(false);
        inventoryButton.SetActive(true);
        pauseButton.SetActive(true);
        mapButton.SetActive(true);
    }

}
