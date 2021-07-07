using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] private AudioSource MusicAudio;
    [SerializeField] private AudioSource SoundAudio;

    [Header("AudioSource Volume Slider")]
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    [Header("AudioSource Volume")]
    [SerializeField] private float MusicVolume = 1f;
    [SerializeField] private float SoundVolume = 1f;

    [Header("First Run")]
    [SerializeField] private int IsFirstRun;

    bool restartMusic = false;

    private void Awake()
    {
        MusicAudio = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        SoundAudio = GameObject.Find("SFXManager").GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
        IsFirstRun = PlayerPrefs.GetInt("IsFirstRun");
        if (IsFirstRun == 0)
        {
            PlayerPrefs.SetInt("IsFirstRun", 1);
            PlayerPrefs.SetFloat("mVolume", 1);
            PlayerPrefs.SetFloat("sVolume", 1);
        }
        else
        {
            MusicVolume = PlayerPrefs.GetFloat("mVolume");
            SoundVolume = PlayerPrefs.GetFloat("sVolume");
        }
    }

    void Start()
    {
        musicVolumeSlider.value = MusicVolume;
        soundVolumeSlider.value = MusicVolume;
        MusicAudio.volume = MusicVolume;
        SoundAudio.volume = SoundVolume;
    }

    void Update()
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

    public void UpdateSoundVolume(float volume)
    {
        SoundVolume = volume;
        PlayerPrefs.SetFloat("sVolume", SoundVolume);
        Debug.Log("SoundController - SOUND VOLUME: " + SoundAudio.volume);
    }

    public void UpdateMusicVolume(float volume)
    {
        MusicVolume = volume;
        PlayerPrefs.SetFloat("mVolume", MusicVolume);
        Debug.Log("SoundController - MUSIC VOLUME: " + MusicAudio.volume);
    }
}
