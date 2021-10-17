using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryView;

    public void OpenInventory()
    {
        inventoryView.SetActive(true);
    }

    public void CloseInventory()
    {
        inventoryView.SetActive(false);
    }

}
