using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [Header("Credits Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float animationSpeed;

    [Header("Credits Audio")]
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private float BGMVolume;


    void Start()
    {
        animationSpeed = 0.6f;
        animator.speed = animationSpeed;
        BGMVolume = PlayerPrefs.GetFloat("mVolume", 1);
        AudioSource.volume = BGMVolume;
    }

    public void FastForwardAnimation()
    {
        if (animationSpeed == 1f)
        {
            Debug.Log("Fast Forward");
            animationSpeed = 2f;
            animator.speed = animationSpeed;
        }
        else
        {
            Debug.Log("Normal");
            animationSpeed = 1f;
            animator.speed = animationSpeed;
        }
    }

    public void PauseAnimation()
    {
        Debug.Log("Pause");
        animationSpeed = 0f;
        animator.speed = animationSpeed;
    }
    
    public void CreditsBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
