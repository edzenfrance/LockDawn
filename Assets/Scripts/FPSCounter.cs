using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text FPSText;
    public float refreshTime;

    float fpsCounter;
    bool refresh;

    void Start()
    {
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
        yield return new WaitForSeconds(0.5f);
        refresh = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (refresh)
        {
            getFrameRate();
        }
    }
}
