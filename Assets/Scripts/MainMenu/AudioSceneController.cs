using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioSceneController : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] private AudioSource MusicAudio;
    [SerializeField] private AudioSource SoundAudio;

    [Header("AudioSource Volume")]
    [SerializeField] private float MusicVolume;
    [SerializeField] private float SoundVolume;

    [Header("First Run")]
    [SerializeField] private int IsFirstRun;
    [SerializeField] private int IsMusicMuted;
    [SerializeField] private int IsSoundMuted;

    bool restartMusic = false;

    private void Awake()
    {
        MusicAudio = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        SoundAudio = GameObject.Find("SFXManager").GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
        IsFirstRun = PlayerPrefs.GetInt("IsFirstRun");
        IsMusicMuted = PlayerPrefs.GetInt("mVolumeMute");
        IsSoundMuted = PlayerPrefs.GetInt("sVolumeMute");

        if (IsFirstRun == 0)
        {
            PlayerPrefs.SetInt("IsFirstRun", 1);
            PlayerPrefs.SetFloat("mVolume", 1);
            PlayerPrefs.SetFloat("sVolume", 1);
            PlayerPrefs.SetFloat("mVolumeMute", 0);
            PlayerPrefs.SetFloat("sVolumeMute", 0);
        }
        else
        {
            MusicVolume = PlayerPrefs.GetFloat("mVolume");
            SoundVolume = PlayerPrefs.GetFloat("sVolume");
        }
        if (IsMusicMuted == 1)
        {
            MusicVolume = 0f;
        }
        if (IsSoundMuted == 1)
        {
            SoundVolume = 0f;
        }
    }

    void Start()
    {
        MusicAudio.volume = MusicVolume;
        SoundAudio.volume = SoundVolume;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.buildIndex);
        if (restartMusic)
        {
            MusicAudio.Play();
            restartMusic = false;
            Debug.Log("SoundController - RESTART MUSIC");
        }
        // if ((scene.buildIndex != 0) && (scene.buildIndex != 1))
        if (scene.buildIndex > 4)
        {
            restartMusic = true;
            //MusicAudio.volume = 0.0f;
            //if (MusicAudio.timeSamples > MusicAudio.clip.samples - 300) MusicAudio.Stop();  // Fix for click artifact when stoping the music when not using streaming
            MusicAudio.Stop();
            Debug.Log("SoundController - STOP MUSIC");
        }
    }
}
