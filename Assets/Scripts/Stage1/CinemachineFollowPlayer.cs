using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CinemachineFollowPlayer : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Cinemachine3rdPersonFollow vcamThirdPersonFollow;
    [SerializeField] private GameObject vcamPlayerCameraRoot;
    [SerializeField] private Transform vcamFollow;

    void Start()
    {
        vcam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        vcamThirdPersonFollow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        if (vcamPlayerCameraRoot == null)
        {
            vcamPlayerCameraRoot = GameObject.FindWithTag("CinemachineTarget");
            vcamFollow = vcamPlayerCameraRoot.transform;
            vcam.Follow = vcamFollow;
            //vcam.LookAt = vcamFollowTarget;
        }
    }
}