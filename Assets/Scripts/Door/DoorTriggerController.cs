using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] private GameObject doorAccessObject;
    [SerializeField] private Button doorAccessButton;
    public Sprite handImage;
    public Sprite keyImage;

    [Header("Door Audio Clip")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private bool playOpenAudio = false;
    [SerializeField] private bool playCloseAudio = false;

    [Header("Key")]
    [SerializeField] private bool isLocked = false;
    [SerializeField] private bool isQuarantined = false;
    [SerializeField] private bool requireKeyA = false;
    [SerializeField] private GameObject keyNoteObject;
    [SerializeField] private TextMeshProUGUI keyNote;

    string keyWarning = "This door requires <color=green>key.</color> The key must be around here, find it";
    string quarantineWarning = "You are in quarantine area! Watch some information about COVID-19 to reduce your quarantine time.";
    string lockedWarning = "This door is locked.";

    int enableDoorButton;
    int needKeyLock;

    void Awake()
    {
        PlayerPrefs.DeleteKey("EnableDoorAccess");
        PlayerPrefs.DeleteKey("KeyLock");
        keyImage = Resources.Load<Sprite>("Art/Icons/KeyTwo");
        handImage = Resources.Load<Sprite>("Art/Icons/hand_icon_white");
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

            // if Door Animation finish set the enDoorAccess to 1
            enableDoorButton = PlayerPrefs.GetInt("EnableDoorAccess");
            needKeyLock = PlayerPrefs.GetInt("Key A");

            Debug.Log("<color=green>DoorTriggerController</color> - <color=white>Door Name:</color> " + gameObject.name);
            Debug.Log("<color=green>DoorTriggerController</color> - <color=white>EnableDoorButton:</color>  " + enableDoorButton);

            if (enableDoorButton == 1)
            {
                doorAccessObject.SetActive(true);
            }
            if (needKeyLock == 1)
            {
                //doorAccessObject.sprite
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enableDoorButton = PlayerPrefs.GetInt("EnableDoorAccess", 1);
            if (enableDoorButton == 1)
                doorAccessObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.DeleteKey("SaveDoorName");
            doorAccessObject.SetActive(false);
            keyNote.text = "";
        }
    }

    public void OpenDoor()
    {
        if (isQuarantined)
        {
            keyNote.text = quarantineWarning;
            keyNoteObject.SetActive(true);
            doorAccessObject.SetActive(false);
            return;
        }

        if (isLocked)
        {
            keyNote.text = lockedWarning;
            keyNoteObject.SetActive(true);
            doorAccessObject.SetActive(false);
            audioManager.PlayAudioDoorLocked();
            return;
        }

        if (requireKeyA)
        {
            int checkKeyA = PlayerPrefs.GetInt("Key A");
            if (checkKeyA == 0)
            {
                Debug.Log("<color=white>DoorTriggerController</color> - Key Required");
                audioManager.PlayAudioDoorLocked();
                keyNote.text = keyWarning;
                keyNoteObject.SetActive(true);
                doorAccessObject.SetActive(false);
                return;
            }
        }

        if (playOpenAudio)
            audioManager.PlayAudioDoorOpen();
        else if (playCloseAudio)
            audioManager.PlayAudioDoorClose();

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

        if (openOutsideT)
        {
            Debug.Log("<color=white>DoorTriggerController</color> - Open Outside");
            doorAccessObject.SetActive(false);
            gameObject.SetActive(false);
            closeOutside.SetActive(true);
            closeInside.SetActive(true);
            openInside.SetActive(false);
            myDoor.Play("Door_Open");
        }
        else if (closeOutsideT)
        {
            Debug.Log("<color=white>DoorTriggerController</color> - Close Outside");
            doorAccessObject.SetActive(false);
            gameObject.SetActive(false);

            openOutside.SetActive(true);
            openInside.SetActive(true);
            closeInside.SetActive(false);
            myDoor.Play("Door_Close");
        }
        else if (openInsideT)
        {
            Debug.Log("<color=white>DoorTriggerController</color> - Open Inside");
            doorAccessObject.SetActive(false);
            gameObject.SetActive(false);
            closeOutside.SetActive(true);
            closeInside.SetActive(true);
            openOutside.SetActive(false);
            myDoor.Play("Door_Open");
        }
        else if (closeInsideT)
        {
            Debug.Log("<color=white>DoorTriggerController</color> - Close Inside");
            doorAccessObject.SetActive(false);
            gameObject.SetActive(false);
            openOutside.SetActive(true);
            openInside.SetActive(true);
            closeOutside.SetActive(false);
            myDoor.Play("Door_Close");
        }
    }

    public void OpenableDoor()
    {
        OpenDoor();
    }
}
