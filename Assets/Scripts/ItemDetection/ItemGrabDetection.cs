using UnityEngine;

public class ItemGrabDetection : MonoBehaviour
{

    [SerializeField] private string detectedItem;
    [SerializeField] private GameObject detectedObject;

    public void getItem()
    {
        detectedItem = PlayerPrefs.GetString("DetectedItem");
        Debug.Log("<color=white>ItemGrabDetection</color> - Stored Item Inventory: " + detectedItem);
        detectedObject = GameObject.Find(detectedItem);
        detectedObject.SetActive(false);
        gameObject.SetActive(false);
    }

}
