using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorAnimationEvent : MonoBehaviour
{
    [SerializeField] private GameObject animCollider;

    void EnableDoorHand()
    {
        SaveManager.SetDoorAccess(1);
        animCollider.SetActive(false);
        Debug.Log("<color=yellow>DoorAnimationEvent</color> - Animation Ended");
    }

    void DisableDoorHand()
    {
        SaveManager.SetDoorAccess(0);
        Debug.Log("<color=yellow>DoorAnimationEvent</color> - Animation Started");
    }
}
