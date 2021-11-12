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
    [SerializeField] private bool requireKeyB = false;
    [SerializeField] private bool requireKeyC = false;
    [SerializeField] private bool requireKeyD = false;
    [SerializeField] private bool requireKeyE = false;
    [SerializeField] private bool requireKeyF = false;
    [SerializeField] private GameObject noteObject;
    [SerializeField] private NoteController noteController;

    string keyWarningA = "This door requires <color=green>Key: Upper Floor</color> to open.\nThe key must be around here, find it!";
    string keyWarningB = "This door requires <color=green>Key: Stock Room</color> to open.\nThe key must be around here, find it!";
    string keyWarningC = "This door requires <color=green>Key: Small Room</color> to open.\nThe key must be around here, find it!";
    string keyWarningD = "This door requires <color=green>Key: Large Room</color> to open.\nThe key must be around here, find it!";
    string keyWarningE = "This door requires <color=green>Key: Bathroom</color> to open.\nThe key must be around here, find it!";
    string keyWarningF = "This door requires <color=green>Key: Kitchen</color> to open.\nThe key must be around here, find it!";
    string quarantineWarning = "You are in quarantine area! Watch some information to learn about COVID-19 and to reduce your quarantine time.";
    string budgeWarning = "The door doesnt budge.";

    int enableDoorButton;
    int needKeyLock;

    int checkKey;

    bool requireKeyFail = false;

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
        noteController = noteObject.GetComponent<NoteController>();
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
        }
    }

    public void OpenDoor()
    {
        if (isQuarantined)
        {
            noteController.ShowNote(quarantineWarning, 1.5f);
            doorAccessObject.SetActive(false);
            audioManager.PlayAudioDoorLockedNoKey();
            return;
        }

        if (isLocked)
        {
            noteController.ShowNote(budgeWarning, 1.2f);
            doorAccessObject.SetActive(false);
            audioManager.PlayAudioDoorLockedNoKey();
            return;
        }

        if (requireKeyA) RequiresKey(keyWarningA, "Key A", "Upper Floor");
        if (requireKeyB) RequiresKey(keyWarningB, "Key B", "Stock Room");
        if (requireKeyC) RequiresKey(keyWarningC, "Key C", "Small Room");
        if (requireKeyD) RequiresKey(keyWarningD, "Key D", "Large Room");
        if (requireKeyE) RequiresKey(keyWarningE, "Key E", "Bathroom");
        if (requireKeyF) RequiresKey(keyWarningF, "Key F", "Kitchen");
        if (requireKeyFail)
            return;

        Debug.Log("Opening Doors..");
        if (playOpenAudio)
            audioManager.PlayAudioDoorOpen();
        else if (playCloseAudio)
            audioManager.PlayAudioDoorClose();

        if (animeColliderEnabled)
        {
            Debug.Log("<color=white>DoorTriggerController</color> - Enabled Collider");
            animCollider.SetActive(true);
            myDoor.enabled = true;
        }
        else
        {
            Debug.Log("<color=white>DoorTriggerController</color> - Disabled Collider");
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

    void RequiresKey(string KeyWarning, string PrefsName, string KeyName)
    {
        checkKey = PlayerPrefs.GetInt(PrefsName);
        if (checkKey == 0)
        {
            requireKeyFail = true;
            audioManager.PlayAudioDoorLockedKey();
            noteController.ShowNote(KeyWarning, 2.3f);
            doorAccessObject.SetActive(false);
            Debug.Log("<color=white>DoorTriggerController</color> - Key Required");
        }
        else if (checkKey == 1)
            noteController.ShowNote("Opening the door using <color=green>Key: " + KeyName, 1.5f);
    }

    public void OpenableDoor()
    {
        OpenDoor();
    }
}
