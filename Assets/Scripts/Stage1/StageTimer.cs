using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class StageTimer : MonoBehaviour
{
    public PlayFabLeaderboard playfabLeaderboard;
    public TextMeshProUGUI timerText;

    float timer = 0.0f;
    bool timeEnable = true;
    bool timeEnded = false;

    void Update()
    {
        if (!timeEnded)
        {
            if (timeEnable)
            {
                timer += Time.deltaTime;
                timerText.text = "Timer: " + timer.ToString("f0");
            }
            else
                TimerEnd();
        }
    }

    void OnEnable()
    {
        timer = 0.0f;
    }

    public void OnFinishTimer()
    {
        timeEnable = false;
    }

    public void TimerEnd()
    {
        timeEnded = true;
        int timerCount = ((int)timer);
        Debug.Log("TimerEnd: " + timerCount);
        playfabLeaderboard.SendLeaderboard(timerCount);
    }
}
