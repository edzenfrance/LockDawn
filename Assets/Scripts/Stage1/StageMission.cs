using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMission : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
        Debug.Log("<color=white>StageMission</color> - Game Paused");
    }

    public void OnResume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("<color=white>StageMission</color> - Game Unpaused");
    }
}
