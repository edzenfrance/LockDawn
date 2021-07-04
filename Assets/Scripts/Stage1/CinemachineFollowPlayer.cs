using UnityEngine;
using Cinemachine;

public class CinemachineFollowPlayer : MonoBehaviour
{
    public GameObject tPlayer;
    public Transform tFollowTarget;
    private CinemachineVirtualCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();

        if (tPlayer == null)
        {
            tPlayer = GameObject.FindWithTag("CinemachineTarget");
        }
        tFollowTarget = tPlayer.transform;
        vcam.Follow = tFollowTarget;
        //vcam.LookAt = tFollowTarget;
    }
}