using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    public AudioSource MusicAudio;
    public AudioSource SoundAudio;

    private float MusicVolume = 1f;
    private float SoundVolume = 1f;

    public GameObject ObjectMusic;
    public GameObject ObjectSound;

    public int IsFirstRun;
    bool restartMusic = false;

    private void Awake()
    {
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
            PlayerPrefs.SetFloat("mVolume", 1);
            PlayerPrefs.SetFloat("sVolume", 1);
            //Debug.Log("welcome again!");
        }
    }

    void Start()
    {
        ObjectMusic = GameObject.FindWithTag("MenuBackgroundMusic");
        MusicAudio = ObjectMusic.GetComponent<AudioSource>();
        MusicVolume = PlayerPrefs.GetFloat("mVolume");
        musicVolumeSlider.value = MusicVolume;
        MusicAudio.volume = MusicVolume;

        ObjectSound = GameObject.FindWithTag("MenuSFXManager");
        SoundAudio = ObjectSound.GetComponent<AudioSource>();
        SoundVolume = PlayerPrefs.GetFloat("sVolume");
        soundVolumeSlider.value = MusicVolume;
        SoundAudio.volume = SoundVolume;
    }

    void Update()
    {
        PlayerPrefs.SetFloat("mVolume", MusicVolume);
        PlayerPrefs.SetFloat("sVolume", SoundVolume);
        MusicAudio.volume = MusicVolume;
        SoundAudio.volume = SoundVolume;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.buildIndex + " Audio Volume: " + MusicAudio.volume);
        if (restartMusic)
        {
            Debug.Log("RESTART MUSIC");
            MusicAudio.Play();
            restartMusic = false;
        }
        if ((scene.buildIndex != 0) && (scene.buildIndex != 1))
        {
            Debug.Log("STOP MUSIC");
            restartMusic = true;
            // Fix for click artifact when stoping the music when not using streaming
            //Audio.volume = 0.0f;
            //if (Audio.timeSamples > Audio.clip.samples - 300) Audio.Stop();
            MusicAudio.Stop();
        }
    }

    public void UpdateSoundVolume(float volume)
    {
        SoundVolume = volume;
        Debug.Log("UPDATED VOLUME: " + SoundAudio.volume);
    }

    public void UpdateMusicVolume(float volume)
    {
        MusicVolume = volume;
        Debug.Log("UPDATED VOLUME: " + MusicAudio.volume);
    }
}
