using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public GameObject ButtonsDialog;
    public GameObject RestartDialog;
    public GameObject SettingDialog;
    public GameObject ExitDialog;

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
        Debug.Log("PauseScreen - OnEnable");
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
        ButtonsDialog.SetActive(false);
        Debug.Log("PauseScreen - RESTART SCENE: " + SceneManager.GetActiveScene().buildIndex);
        RestartDialog.SetActive(true);
    }

    public void OnRestartYes()
    {

        Debug.Log("PauseScreen - RESTART SCENE: " + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GamePaused = false;
    }

    public void OnRestartNo()
    {
        ButtonsDialog.SetActive(true);
        RestartDialog.SetActive(false);
    }

    public void OnSettings()
    {
        ButtonsDialog.SetActive(false);
        SettingDialog.SetActive(true);
    }

    public void OnSettingsBack()
    {
        ButtonsDialog.SetActive(true);
        SettingDialog.SetActive(false);
    }

    public void OnExit()
    {
        ButtonsDialog.SetActive(false);
        ExitDialog.SetActive(true);
    }

    public void OnExitYes()
    {
        GamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnExitNo()
    {
        ButtonsDialog.SetActive(true);
        ExitDialog.SetActive(false);
    }
}