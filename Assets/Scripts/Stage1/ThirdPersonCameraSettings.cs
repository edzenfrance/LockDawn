using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ThirdPersonCameraSettings : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Cinemachine3rdPersonFollow vcamThirdPersonFollow;

    [Header("Slider Settings")]
    [SerializeField] private Slider cameraDistanceSlider;
    [SerializeField] private float cameraDistanceValue;
    [SerializeField] private float CameraDistance;
    [SerializeField] private int isFirstRunOnStage;

    private void Awake()
    {
        cameraDistanceSlider = GameObject.Find("CameraDistanceSlider").GetComponent<Slider>();
        vcam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        vcamThirdPersonFollow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        isFirstRunOnStage = PlayerPrefs.GetInt("IsFirstRunOnStage");
        if (isFirstRunOnStage == 0)
        {
            PlayerPrefs.SetInt("IsFirstRunOnStage", 1);
            PlayerPrefs.SetFloat("CameraDistance", 3);
            cameraDistanceSlider.value = 3f;
        }
        else
        {
            cameraDistanceValue = PlayerPrefs.GetFloat("CameraDistance");
            cameraDistanceSlider.value = cameraDistanceValue;
        }
    }

    public void TPFCameraDistance(float distance)
    {
        distance = Mathf.Round(distance * 10f) * 0.1f;
        vcamThirdPersonFollow.CameraDistance = distance;
        cameraDistanceSlider.value = distance;
        PlayerPrefs.SetFloat("CameraDistance", distance);
    }
}
