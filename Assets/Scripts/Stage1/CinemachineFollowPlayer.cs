using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CinemachineFollowPlayer : MonoBehaviour
{
    public GameObject tPlayer;
    public Transform tFollowTarget;
    [SerializeField]
    private CinemachineVirtualCamera vcam;
    [SerializeField]
    private Cinemachine3rdPersonFollow vcamThirdPersonFollow;

    private void Awake()
    {
        vcam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        vcamThirdPersonFollow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        if (tPlayer == null)
        {
            tPlayer = GameObject.FindWithTag("CinemachineTarget");
        }
        tFollowTarget = tPlayer.transform;
        vcam.Follow = tFollowTarget;
        //vcam.LookAt = tFollowTarget;
    }
}