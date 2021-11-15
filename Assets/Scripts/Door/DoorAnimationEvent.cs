using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorAnimationEvent : MonoBehaviour
{
    [SerializeField] private GameObject animCollider;
    [SerializeField] private SaveManager saveManager;

    void EnableDoorHand()
    {
        saveManager.SetDoorAccess(1);
        animCollider.SetActive(false);
        Debug.Log("<color=yellow>DoorAnimationEvent</color> - Animation Ended");
    }

    void DisableDoorHand()
    {
        saveManager.SetDoorAccess(0);
        Debug.Log("<color=yellow>DoorAnimationEvent</color> - Animation Started");
    }
}
