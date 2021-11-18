using System.Collections;
using UnityEngine;

public class StageObjective : MonoBehaviour
{

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private EnvironmentController environmentController;
    [SerializeField] private Canvas canvasSettings;
    [SerializeField] private GameObject loadingUI;

    void Start()
    {
        Time.timeScale = 0;
        canvasSettings.sortingOrder = 8;
        Debug.Log("<color=white>StageObjective</color> - Game Paused");
        StartCoroutine(playVoiceOver());
    }

    public void OnResume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("<color=white>StageObjective</color> - Game Unpaused");
        audioManager.StopAudio();
    }

    IEnumerator playVoiceOver()
    {
        environmentController.EnvironmentEnabler();
        yield return new WaitForSecondsRealtime(2);
        loadingUI.SetActive(false);
        audioManager.PlayAudioObjective();
    }
}
