using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class CameraDistance : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Cinemachine3rdPersonFollow vcamThirdPersonFollow;

    [Header("Slider Settings")]
    [SerializeField] private TMP_Text cameraDistanceText;
    [SerializeField] private Slider cameraDistanceSlider;
    [SerializeField] private float cameraDistanceValue;

    private void Start()
    {
        //Commented because Camera Distance Slider is now disabled by default
        //cameraDistanceSlider = GameObject.Find("CameraDistanceSlider").GetComponent<Slider>();
        //vcam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        vcamThirdPersonFollow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

        cameraDistanceValue = PlayerPrefs.GetFloat("CameraDistance", 3.00f);
        cameraDistanceSlider.value = cameraDistanceValue;
        cameraDistanceText.text = cameraDistanceValue.ToString("f2");
        vcamThirdPersonFollow.CameraDistance = cameraDistanceValue;
    }

    public void TPFCameraDistance(float distance)
    {
        //distance = Mathf.Round(distance * 10f) * 0.1f;
        vcamThirdPersonFollow.CameraDistance = distance;
        PlayerPrefs.SetFloat("CameraDistance", distance);
        cameraDistanceText.text = distance.ToString("f2");
    }
}