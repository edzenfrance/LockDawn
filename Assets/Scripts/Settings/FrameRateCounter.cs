using System.Collections;
using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour
{
    public TMP_Text FPSText;
    private float refreshTime = 0.5f;
    private float fpsCounter;
    private bool refresh;

    void Start()
    {
        Debug.Log("<color=white>FrameRateCounter</color> - Start");
        refresh = true;
    }

    void GetFrameRate()
    {
        fpsCounter = 1.0f / Time.unscaledDeltaTime;
        FPSText.text = "FPS: " + fpsCounter.ToString("f0");
        refresh = false;
        StartCoroutine(RefreshDelay());
    }

    IEnumerator RefreshDelay()
    {
        yield return new WaitForSeconds(refreshTime);
        refresh = true;
    }

    void Update()
    {
        if (refresh)
        {
            GetFrameRate();
        }
    }
}
