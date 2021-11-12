using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// www.youtube.com/watch?v=39L3GL1ZvFI&t=92s

public class ItemDetection : MonoBehaviour
{
    [SerializeField] private GameObject grabItem;
    [SerializeField] private ItemGet itemGet;
    [SerializeField] private string itemNote;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("<color=white>ItemColliderDetection</color> - OnTriggerEnter");
        if (other.tag == "Player")
        {
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
