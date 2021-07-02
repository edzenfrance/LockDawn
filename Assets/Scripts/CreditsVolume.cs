using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsVolume : MonoBehaviour
{
    private float creditsVolume = 0f;
    public AudioSource creditsAudio;

    void Start()
    {
        creditsVolume = PlayerPrefs.GetFloat("mVolume");
        creditsAudio.volume = creditsVolume;
    }

}
