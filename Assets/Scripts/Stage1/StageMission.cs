using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMission : MonoBehaviour
{
    bool GamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        GamePaused = true;
        Debug.Log("<color=white>StageMission</color> - GamePaused = " + GamePaused);
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePaused)
        {
            Time.timeScale = 0;
            //Debug.Log("PauseScreen - GAME PAUSED: " + GamePaused);
        }
        else
        {
            Time.timeScale = 1;
            //Debug.Log("PauseScreen - GAME PAUSED: " + GamePaused);
        }
    }

    public void OnResume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("<color=white>StageMission</color> - GamePaused = " + GamePaused);
    }
}
