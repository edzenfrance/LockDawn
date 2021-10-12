using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public GameObject buttonsDialog;
    public GameObject restartDialog;
    public GameObject settingDialog;
    public GameObject exitDialog;


    public Transform box;
    public CanvasGroup background;

    bool GamePaused = false;

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

    public void OnEnable()
    {
        Debug.Log("<color=white>PauseScreen</color> - OnEnable");
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);
        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().setOnComplete(OnEnableComplete).delay = 0.1f;
    }

    void OnEnableComplete()
    {
        GamePaused = true;
    }

    public void OnResume()
    {
        GamePaused = false;
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnResumeComplete);
    }

    void OnResumeComplete()
    {
        gameObject.SetActive(false);
    }

    public void OnStage()
    {
        GamePaused = false;
        Debug.Log("PauseScreen - Previous Scene: Stage 1");
        PlayerPrefs.SetString("Scene", "Stage1");
        SceneManager.LoadScene("StageSelect");
    }

    public void OnRestart()
    {
        buttonsDialog.SetActive(false);
        Debug.Log("PauseScreen - RESTART SCENE: " + SceneManager.GetActiveScene().buildIndex);
        restartDialog.SetActive(true);
    }

    public void OnRestartYes()
    {

        Debug.Log("PauseScreen - RESTART SCENE: " + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GamePaused = false;
    }

    public void OnRestartNo()
    {
        buttonsDialog.SetActive(true);
        restartDialog.SetActive(false);
    }

    public void OnSettings()
    {
        GamePaused = false;
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnSettingsComplete);
    }

    void OnSettingsComplete()
    {
        gameObject.SetActive(false);
        settingDialog.SetActive(true);
    }


    public void OnExit()
    {
        buttonsDialog.SetActive(false);
        exitDialog.SetActive(true);
    }

    public void OnExitYes()
    {
        GamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnExitNo()
    {
        buttonsDialog.SetActive(true);
        exitDialog.SetActive(false);
    }
}