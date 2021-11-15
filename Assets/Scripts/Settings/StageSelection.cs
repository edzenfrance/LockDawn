using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelection : MonoBehaviour
{
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private CanvasGroup overlay;
    [SerializeField] private Transform overlayObject;

    bool GamePaused = false;

    void Update()
    {
        if (GamePaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void OnEnable()
    {
        overlay.alpha = 0;
        overlay.LeanAlpha(1, 0.5f);
        overlayObject.localPosition = new Vector2(0, -Screen.height);
        overlayObject.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().setOnComplete(OnEnableComplete).delay = 0f; // .delay = 0.1f
    }

    void OnEnableComplete()
    {
        GamePaused = true;
    }

    public void OnBack()
    {
        GamePaused = false;
        overlay.LeanAlpha(0, 0.5f);
        overlayObject.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnBackComplete);
    }

    void OnBackComplete()
    {
        gameObject.SetActive(false);
        PauseScreen.SetActive(true);
    }
}
