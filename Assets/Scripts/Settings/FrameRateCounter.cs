using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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

    void getFrameRate()
    {
        fpsCounter = 1.0f / Time.unscaledDeltaTime;
        FPSText.text = "FPS: " + fpsCounter.ToString("f0");
        refresh = false;
        StartCoroutine(refreshDelay());
    }

    IEnumerator refreshDelay()
    {
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
}
