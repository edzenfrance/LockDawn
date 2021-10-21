using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerController : MonoBehaviour, IInteractable
{
    // www.youtube.com/watch?v=tJiO4cvsHAo

    [SerializeField] public Animator myDoor = null;

    [Header("Trigger Value")]
    [SerializeField] private bool openOutsideT = false;
    [SerializeField] private bool closeOutsideT = false;
    [SerializeField] private bool openInsideT = false;
    [SerializeField] private bool closeInsideT = false;
    [SerializeField] private bool closeInsideFarT = false;

    [Header("Door Collider Object")]
    [SerializeField] private GameObject openOutside;
    [SerializeField] private GameObject closeOutside;

    [SerializeField] private GameObject openInside;
    [SerializeField] private GameObject closeInside;

    [Header("Animation Collider")]
    [SerializeField] private GameObject animCollider;
    [SerializeField] private bool animeColliderEnabled = false;

    [Header("Door Button")]
    [SerializeField] private GameObject doorAccessButton;

    int enDoorAccess;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Save Door Name and Object Name
            PlayerPrefs.SetString("SaveDoorName", myDoor.name + "/" + gameObject.name);
            enDoorAccess = PlayerPrefs.GetInt("EnableDoorAccess", 1);
            Debug.Log("<color=white>TriggerDoorController</color> - DOOR CHECK: " + gameObject.name + " - EnableDoorAccess: " + enDoorAccess);
            if (enDoorAccess == 1)
            {
                doorAccessButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.DeleteKey("SaveDoorName");
            doorAccessButton.SetActive(false);
        }
    }

    public void OpenDoor()
    {
        if (animeColliderEnabled)
        {
            Debug.Log("Enabled Collider");
            animCollider.SetActive(true);
            myDoor.enabled = true;
        }
        else
        {
            Debug.Log("Disabled Collider");
            animCollider.SetActive(false);
            myDoor.enabled = true;
        }

        // Open the door
        if (openOutsideT)
        {
            Debug.Log("<color=white>TriggerDoorController</color> - Open Outside");
            doorAccessButton.SetActive(false);
            gameObject.SetActive(false);
            closeOutside.SetActive(true);
            closeInside.SetActive(true);
            openInside.SetActive(false);
            myDoor.Play("DoorOpen_RightKnob");
        }
        else if (closeOutsideT)
        {
            Debug.Log("<color=white>TriggerDoorController</color> - Close Outside");
            doorAccessButton.SetActive(false);
            gameObject.SetActive(false);
            openOutside.SetActive(true);
            openInside.SetActive(true);
            closeInside.SetActive(false);
            myDoor.Play("DoorClose_RightKnob");
        }
        else if (openInsideT)
        {
            Debug.Log("<color=white>TriggerDoorController</color> - Open Inside");
            doorAccessButton.SetActive(false);
            gameObject.SetActive(false);

            closeOutside.SetActive(true);
            closeInside.SetActive(true);
            openOutside.SetActive(false);

            myDoor.Play("DoorOpen_RightKnob");
        }
        else if (closeInsideT)
        {
            Debug.Log("<color=white>TriggerDoorController</color> - Close Inside");
            doorAccessButton.SetActive(false);
            gameObject.SetActive(false);
            openOutside.SetActive(true);
            openInside.SetActive(true);
            closeOutside.SetActive(false);
            myDoor.Play("DoorClose_RightKnob");
        }
    }

    public void OpenableDoor()
    {
        OpenDoor();
    }
}
