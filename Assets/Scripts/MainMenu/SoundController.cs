using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public GameObject ObjectMusic;

    public AudioSource AmbientAudio;
    private float MusicVolume = 1f;

    bool restartMusic = false;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        Debug.Log("MUSIC VOLUME: " + MusicVolume);
        ObjectMusic = GameObject.FindWithTag("MenuBackgroundMusic");
        AmbientAudio = ObjectMusic.GetComponent<AudioSource>();
        MusicVolume = PlayerPrefs.GetFloat("mVolume");
        Debug.Log("VOLUME CHECK START: " + MusicVolume);
        musicVolumeSlider.value = MusicVolume;
        AmbientAudio.volume = MusicVolume;
    }

    void Update()
    {
        PlayerPrefs.SetFloat("mVolume", MusicVolume);
        AmbientAudio.volume = MusicVolume;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.buildIndex + " Audio Volume: " + AmbientAudio.volume);
        if (restartMusic)
        {
            Debug.Log("RESTART MUSIC");
            AmbientAudio.Play();
            restartMusic = false;
        }
        if ((scene.buildIndex != 0) && (scene.buildIndex != 1))
        {
            Debug.Log("STOP MUSIC");
            restartMusic = true;
            // Fix for click artifact when stoping the music when not using streaming
            //Audio.volume = 0.0f;
            //if (Audio.timeSamples > Audio.clip.samples - 300) Audio.Stop();
            AmbientAudio.Stop();
        }
    }

    public void UpdateVolume(float volume)
    {
        MusicVolume = volume;
        Debug.Log("UPDATED VOLUME: " + AmbientAudio.volume);
    }
}
