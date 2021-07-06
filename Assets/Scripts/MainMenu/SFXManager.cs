using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SFXManager : MonoBehaviour
{

    public static SFXManager sfxInstance;
    [Header("SFX Manager")]
    public AudioSource Audio;
    [Header("Sound Click")]
    public AudioClip UIClick;

    private void Awake()
    {
        if(sfxInstance != null && sfxInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        sfxInstance = this;
        DontDestroyOnLoad(this);
    }
}
