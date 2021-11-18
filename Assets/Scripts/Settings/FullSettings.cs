using UnityEngine;
using UnityEngine.UI;

public class FullSettings : MonoBehaviour
{
    public GameObject PauseScreen;
    public Transform box;
    public CanvasGroup background;
    public Canvas canvasSettings;

    [Header("FrameRate")]
    [SerializeField] private Toggle framerateToggle;
    [SerializeField] private GameObject framerateCounter;

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
        canvasSettings.sortingOrder = 8;
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);
        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().setOnComplete(OnEnableComplete).delay = 0f;
    }

    void OnEnableComplete()
    {
        GamePaused = true;
    }

    public void OnBack()
    {
        GamePaused = false;
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnBackComplete);
    }

    void OnBackComplete()
    {
        canvasSettings.sortingOrder = 5;
        gameObject.SetActive(false);
        PauseScreen.SetActive(true);
    }

    public void ToggleFrameRate()
    {
        bool MusicToggleSwitch = framerateToggle.isOn;
        if (MusicToggleSwitch)
        {
            SaveManager.SetShowFramerate(0);
            framerateCounter.SetActive(true);
            Debug.Log("<color=white>FullSettings</color> - Frame Rate: ON");

        }
        else
        {
            SaveManager.SetShowFramerate(1);
            framerateCounter.SetActive(false);
            Debug.Log("<color=white>FullSettings</color> - Frame Rate: OFF");
        }
    }
}