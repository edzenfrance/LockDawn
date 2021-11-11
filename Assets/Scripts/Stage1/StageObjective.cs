using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjective : MonoBehaviour
{

    [SerializeField] private AudioManager audioManager;

    void Start()
    {
        Time.timeScale = 0;
        Debug.Log("<color=white>StageMission</color> - Game Paused");
        audioManager.PlayAudioObjective();
    }

    public void OnResume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("<color=white>StageMission</color> - Game Unpaused");
        audioManager.StopAudio();
    }
}
