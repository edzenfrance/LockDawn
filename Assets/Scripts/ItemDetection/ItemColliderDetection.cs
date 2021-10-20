using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// www.youtube.com/watch?v=39L3GL1ZvFI&t=92s

public class ItemColliderDetection : MonoBehaviour
{
    // public AudioSource audioSource;
    [SerializeField] private GameObject grabItem;
    [SerializeField] private string item;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("<color=white>ItemColliderDetection</color> - OnTriggerEnter");
        if (other.tag == "Player")
        {
                grabItem.SetActive(true);
                Debug.Log("<color=white>ItemColliderDetection</color> - Get Item: Enabled");
                // audioSource.enabled = true;;
                item = gameObject.name;
                grabItem.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Get the " + item;
                PlayerPrefs.SetString("DetectedItem", item);
                Debug.Log("<color=white>ItemColliderDetection</color> - Detected Object Name: " + item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("<color=white>ItemColliderDetection</color> - OnTriggerExit");
        if (other.tag == "Player")
        {
            grabItem.SetActive(false);
            PlayerPrefs.DeleteKey("DetectedItem");
            Debug.Log("<color=white>ItemColliderDetection</color> - Get Item: Disabled");
        }
    }
}
