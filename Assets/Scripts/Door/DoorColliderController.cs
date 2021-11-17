using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorColliderController : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private AudioManager audioManager;

    void Start()
    {
        myDoor = transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("<color=white>DoorAnimationController</color> - Enter - Animation Paused");
            myDoor.enabled = false;
            audioManager.PauseAudio();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myDoor.enabled = false;
            audioManager.PauseAudio();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("<color=white>DoorAnimationController</color> - Exit - Animation Unpaused");
            myDoor.enabled = true;
            audioManager.UnpauseAudio();

        }
    }

}
