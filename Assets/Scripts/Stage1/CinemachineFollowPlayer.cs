using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CinemachineFollowPlayer : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform playerTransform;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindWithTag("CinemachineTarget").GetComponent<Transform>().transform;
            virtualCamera.Follow = playerTransform;
            // vcam.LookAt = playerTransform;
        }
    }
}