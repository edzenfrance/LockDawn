using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorColliderController : MonoBehaviour
{
    [SerializeField] public Animator myDoor = null;

    void Start()
    {
        myDoor = transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("<color=white>DoorAnimationController</color> - Animation Paused");
            myDoor.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("<color=white>DoorAnimationController</color> - Animation Unpaused");
            myDoor.enabled = true;
        }
    }

}
