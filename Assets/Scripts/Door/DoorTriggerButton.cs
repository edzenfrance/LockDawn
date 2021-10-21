using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerButton : MonoBehaviour
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private string doorName;

    public void OpenCloseDoor()
    {
        doorName = PlayerPrefs.GetString("SaveDoorName");
        Debug.Log("<color=green>TriggerDoorButton</color> - " + doorName);
        doorTransform = GameObject.Find(doorName).GetComponent<Transform>();
        doorTransform.gameObject.GetComponent<IInteractable>().OpenableDoor();
    }
}   