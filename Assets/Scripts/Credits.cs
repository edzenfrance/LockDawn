using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [Range(0, 5)]
    public float Speed = 1.0f;
    public Animator CreditsAnimation;

    public void CreditsFastForward()
    {
        if (Speed == 1.0f)
        {
            Debug.Log("Fast Forward");
            Speed = 2.0f;
            CreditsAnimation.speed = 2.0f;
        }
        else
        {
            Debug.Log("Normal");
            Speed = 1.0f;
            CreditsAnimation.speed = 1.0f;
        }
    }

    public void CreditsPause()
    {
        Debug.Log("Pause");
        Speed = 0.0f;
        CreditsAnimation.speed = 0.0f;
    }
    
    public void CreditsBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
