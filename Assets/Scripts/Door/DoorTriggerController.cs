using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorTriggerController : MonoBehaviour, IInteractable
{
    // www.youtube.com/watch?v=tJiO4cvsHAo

    [SerializeField] public Animator myDoor = null;

    [Header("Trigger Value")]
    [SerializeField] private bool openOutsideT = false;
    [SerializeField] private bool closeOutsideT = false;
    [SerializeField] private bool openInsideT = false;
    [SerializeField] private bool closeInsideT = false;

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
    [SerializeField] private Sprite handImage;
    [SerializeField] private Sprite keyImage;
    

    [Header("Key")]
    [SerializeField] private bool requireKeyA = false;
    [SerializeField] private GameObject keyNoteObject;
    [SerializeField] private TextMeshProUGUI keyNote;
    [SerializeField] private string keyWarning;

    int enDoorAccess;

    void Awake()
    {
        PlayerPrefs.DeleteKey("EnableDoorAccess");
    }

    void Start()
    {
        myDoor = transform.parent.GetComponent<Animator>();
        keyNote = GameObject.Find("Canvas UI/KeyNote").GetComponent<TextMeshProUGUI>();
        keyNoteObject = GameObject.Find("Canvas UI/KeyNote");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Save Door Name and Object Name
            PlayerPrefs.SetString("SaveDoorName", myDoor.name + "/" + gameObject.name);
            enDoorAccess = PlayerPrefs.GetInt("EnableDoorAccess", 1);
            Debug.Log("<color=white>TriggerDoorController</color> - DOOR CHECK: " + gameObject.name + " - <color=white>EnableDoorAccess:</color>  " + enDoorAccess);
            if (enDoorAccess == 1)
            {
                doorAccessButton.SetActive(true);
            }
            int keyLock = PlayerPrefs.GetInt("KeyLock");
            {
                //doorAccessButton.sprite
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        enDoorAccess = PlayerPrefs.GetInt("EnableDoorAccess", 1);
        if (enDoorAccess == 1)
        {
            doorAccessButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.DeleteKey("SaveDoorName");
            doorAccessButton.SetActive(false);
            keyNote.text = "";
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
            Debug.Log("<color=white>TriggerDoorController</color> - Requiring Key: " + requireKeyA);
            if (requireKeyA)
            {
                int keyLock = PlayerPrefs.GetInt("KeyLock");
                Debug.Log("<color=white>TriggerDoorController</color> - Key A: " + keyLock);
                if (keyLock == 1)
                {
                    justFcknOpenTheDoor();
                }
                else if (keyLock == 0)
                {
                    Debug.Log("<color=white>TriggerDoorController</color> - Key Required");
                    keyNote.text = keyWarning;
                    keyNoteObject.SetActive(true);
                    doorAccessButton.SetActive(false);
                }
            }
            else
            {
                justFcknOpenTheDoor();
            }
        }
        else if (closeOutsideT)
        {
            Debug.Log("<color=white>TriggerDoorController</color> - Close Outside");
            doorAccessButton.SetActive(false);
            gameObject.SetActive(false);

            openOutside.SetActive(true);
            openInside.SetActive(true);
            closeInside.SetActive(false);
            myDoor.Play("Door_Close");
        }
        else if (openInsideT)
        {
            Debug.Log("<color=white>TriggerDoorController</color> - Open Inside");
            doorAccessButton.SetActive(false);
            gameObject.SetActive(false);
            closeOutside.SetActive(true);
            closeInside.SetActive(true);
            openOutside.SetActive(false);
            myDoor.Play("Door_Open");
        }
        else if (closeInsideT)
        {
            Debug.Log("<color=white>TriggerDoorController</color> - Close Inside");
            doorAccessButton.SetActive(false);
            gameObject.SetActive(false);
            openOutside.SetActive(true);
            openInside.SetActive(true);
            closeOutside.SetActive(false);
            myDoor.Play("Door_Close");
        }
    }

    void justFcknOpenTheDoor()
    {
        Debug.Log("<color=white>TriggerDoorController</color> - Open Outside");
        doorAccessButton.SetActive(false);
        gameObject.SetActive(false);
        closeOutside.SetActive(true);
        closeInside.SetActive(true);
        openInside.SetActive(false);
        myDoor.Play("Door_Open");
    }

    public void OpenableDoor()
    {
        OpenDoor();
    }
}
