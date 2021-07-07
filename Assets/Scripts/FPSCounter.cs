using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text FPSText;
    public float refreshTime;
    public float refreshTimeNew;
    public Slider fpsCounterSlider;

    float fpsCounter;
    bool refresh;

    void Start()
    {
        refreshTime = PlayerPrefs.GetFloat("FPSCounterRefreshTime");
        fpsCounterSlider.value = refreshTime;
        refresh = true;
    }

    void getFrameRate()
    {
        fpsCounter = 1.0f / Time.unscaledDeltaTime;
        FPSText.text = "FPS: " + fpsCounter.ToString("f0");
        refresh = false;
        StartCoroutine(refreshDelay());
    }

    IEnumerator refreshDelay()
    {
        refreshTime = refreshTimeNew;
        yield return new WaitForSeconds(refreshTime);
        refresh = true;
    }

    void Update()
    {
        if (refresh)
        {
            getFrameRate();
        }
    }

    public void refreshTimeChangeValue(float refreshTime)
    {
        refreshTime = Mathf.Round(refreshTime * 10f) * 0.1f;
        fpsCounterSlider.value = refreshTime;
        refreshTimeNew = refreshTime;
        PlayerPrefs.SetFloat("FPSCounterRefreshTime", refreshTime);
    }
}
