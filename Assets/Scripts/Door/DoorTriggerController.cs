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

    [Header("Scripts")]
    [SerializeField] private NoteController noteController;
    [SerializeField] private Inventory inventory;

    int currentStage;
    bool requireKeyFail = false;

    void Awake()
    {
        PlayerPrefs.DeleteKey("Door Access");
        keyImage = Resources.Load<Sprite>("Art/Icons/KeyTwo");
        handImage = Resources.Load<Sprite>("Art/Icons/hand_icon_white");
    }

    void Start()
    {
        myDoor = transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Save Door Name and Object Name
            PlayerPrefs.SetString("Door Name", myDoor.name + "/" + gameObject.name);

            // if door animation finished enable the door button
            SaveManager.GetDoorAccess();
            if (SaveManager.doorAccess == 1)
                doorAccessObject.SetActive(true);

            Debug.Log("<color=green>DoorTriggerController</color> - <color=white>Door Name:</color> " + gameObject.name + " - <color=white>EnableDoorButton:</color> " + SaveManager.doorAccess);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveManager.GetDoorAccess();
            if (SaveManager.doorAccess == 1)
                doorAccessObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.DeleteKey("Door Name");
            doorAccessObject.SetActive(false);
        }
    }

    public void OpenDoor()
    {
        if (isQuarantined)
        {
            noteController.ShowNote(TextManager.quarantineArea, 5.0f);
            doorAccessObject.SetActive(false);
            audioManager.PlayAudioDoorLockedNoKey();
            return;
        }

        if (isLocked)
        {
            noteController.ShowNote(TextManager.doorIsLocked, 1.2f);
            doorAccessObject.SetActive(false);
            audioManager.PlayAudioDoorLockedNoKey();
            return;
        }

        SaveManager.GetCurrentStage();
        currentStage = SaveManager.currentStage;

        if (currentStage == 1)
        {
            if (requireKeyA) RequiresKey(TextManager.S1_KeyWarn_A, "S1 Key A", TextManager.S1_DoorKey_A);
            if (requireKeyB) RequiresKey(TextManager.S1_KeyWarn_B, "S1 Key B", TextManager.S1_DoorKey_B);
            if (requireKeyC) RequiresKey(TextManager.S1_KeyWarn_C, "S1 Key C", TextManager.S1_DoorKey_C);
            if (requireKeyD) RequiresKey(TextManager.S1_KeyWarn_D, "S1 Key D", TextManager.S1_DoorKey_D);
            if (requireKeyE) RequiresKey(TextManager.S1_KeyWarn_E, "S1 Key E", TextManager.S1_DoorKey_E);
            if (requireKeyF) RequiresKey(TextManager.S1_KeyWarn_F, "S1 Key F", TextManager.S1_DoorKey_F);
            if (requireKeyFail)
                return;
        }
        if (currentStage == 2)
        {
            if (requireKeyA) RequiresKey(TextManager.S2_KeyWarn_A, "S2 Key A", TextManager.S2_DoorKey_A);
            if (requireKeyB) RequiresKey(TextManager.S2_KeyWarn_B, "S2 Key B", TextManager.S2_DoorKey_B);
            if (requireKeyC) RequiresKey(TextManager.S2_KeyWarn_C, "S2 Key C", TextManager.S2_DoorKey_C);
            if (requireKeyD) RequiresKey(TextManager.S2_KeyWarn_D, "S2 Key D", TextManager.S2_DoorKey_D);
            if (requireKeyE) RequiresKey(TextManager.S2_KeyWarn_E, "S2 Key E", TextManager.S2_DoorKey_E);
            if (requireKeyF) RequiresKey(TextManager.S2_KeyWarn_F, "S2 Key F", TextManager.S2_DoorKey_F);
            if (requireKeyFail)
                return;
        }

        if (playOpenAudio)
            audioManager.PlayAudioDoorOpen();
        else if (playCloseAudio)
            audioManager.PlayAudioDoorClose();

        if (animeColliderEnabled)
        {
            animCollider.SetActive(true);
            myDoor.enabled = true;
            Debug.Log("<color=white>DoorTriggerController</color> - Collider Enabled");
        }
        else
        {
            animCollider.SetActive(false);
            myDoor.enabled = true;
            Debug.Log("<color=white>DoorTriggerController</color> - Collider Disabled");
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
        SaveManager.GetKeyName(PrefsName);
        if (SaveManager.keyName == 0)
        {
            requireKeyFail = true;
            audioManager.PlayAudioDoorLockedKey();
            noteController.ShowNote(KeyWarning, 2.3f);
            doorAccessObject.SetActive(false);
            Debug.Log("<color=white>DoorTriggerController</color> - Key Required");
        }
        if (SaveManager.keyName == 1)
        {
            requireKeyFail = false;
            noteController.ShowNote(TextManager.unlockingDoor + KeyName, 2.0f);
            SaveManager.SetKeyName(PrefsName, 2);
            inventory.ReloadInventory();
        }
        if (SaveManager.keyName == 2)
        {
            requireKeyFail = false;
            noteController.ShowNote(TextManager.openingDoor + KeyName, 2.0f);
        }
    }

    public void OpenableDoor()
    {
        OpenDoor();
    }
}
