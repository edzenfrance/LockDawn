using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public float refreshTime;
    public TMP_Text FPSText;

    float fpsCounter;
    bool refresh;

    void Start()
    {
        refresh = true;
    }

    void REFRESH()
    {
        fpsCounter = 1.0f / Time.unscaledDeltaTime;
        refresh = false;
        StartCoroutine(refreshdelay());
    }

    IEnumerator refreshdelay()
    {
        yield return new WaitForSeconds(refreshTime);
        refresh = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (refresh)
        {
            REFRESH();
        }
        FPSText.text = "FPS: " + fpsCounter.ToString("f0");
    }
}
