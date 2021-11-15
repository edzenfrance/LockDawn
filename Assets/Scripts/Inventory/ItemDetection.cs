using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// www.youtube.com/watch?v=39L3GL1ZvFI&t=92s

public class ItemDetection : MonoBehaviour
{
    [SerializeField] private GameObject grabItem;
    [SerializeField] private ItemGet itemGet;
    [SerializeField] private string itemCheck;

    private string itemNote;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("<color=white>ItemColliderDetection</color> - OnTriggerEnter");
        if (other.tag == "Player")
        {
            string objectName = gameObject.name;
            if (itemCheck == "Key")
            {
                if (objectName == "S1 Key A") itemNote = TextManager.S1_ItemGet_A;
                if (objectName == "S1 Key B") itemNote = TextManager.S1_ItemGet_B;
                if (objectName == "S1 Key C") itemNote = TextManager.S1_ItemGet_C;
                if (objectName == "S1 Key D") itemNote = TextManager.S1_ItemGet_D;
                if (objectName == "S1 Key E") itemNote = TextManager.S1_ItemGet_E;
                if (objectName == "S1 Key F") itemNote = TextManager.S1_ItemGet_F;

                if (objectName == "S2 Key A") itemNote = TextManager.S2_ItemGet_A;
                if (objectName == "S2 Key B") itemNote = TextManager.S2_ItemGet_B;
                if (objectName == "S2 Key C") itemNote = TextManager.S2_ItemGet_C;
                if (objectName == "S2 Key D") itemNote = TextManager.S2_ItemGet_D;
                if (objectName == "S2 Key E") itemNote = TextManager.S2_ItemGet_E;
                if (objectName == "S2 Key F") itemNote = TextManager.S2_ItemGet_F;
            }
            if (itemCheck == "Main")
            {
                if (objectName == "S1 Vitamins") itemNote = TextManager.ItemGet_Vitamins;
                if (objectName == "S2 Alcohol") itemNote = TextManager.ItemGet_Alcohol;
                if (objectName == "S3 Face Mask") itemNote = TextManager.ItemGet_Facemask;
                if (objectName == "S4 Face Shield") itemNote = TextManager.ItemGet_Faceshield;
                if (objectName == "S5 Vaccine") itemNote = TextManager.ItemGet_Vaccine;
            }
            if (itemCheck == "Syrup") itemNote = TextManager.ItemGet_Syrup;
            if (itemCheck == "Coin") itemNote = TextManager.ItemGet_Coin;

            grabItem.SetActive(true);
            itemGet.ItemInfo(itemNote, gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("<color=white>ItemColliderDetection</color> - OnTriggerExit");
        if (other.tag == "Player")
        {
            grabItem.SetActive(false);
        }
    }
}
