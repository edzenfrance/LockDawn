using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorAnimationEvent : MonoBehaviour
{
    [SerializeField] private GameObject animCollider;

    void EnableDoorHand()
    {
        PlayerPrefs.SetInt("EnableDoorAccess", 1);
        Debug.Log("<color=yellow>DoorAnimationController</color> - Animation Ended");
        animCollider.SetActive(false);
    }

    void DisableDoorHand()
    {
        PlayerPrefs.SetInt("EnableDoorAccess", 0);
        Debug.Log("<color=yellow>DoorAnimationController</color> - Animation Started");
    }
}
