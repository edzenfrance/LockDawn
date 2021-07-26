using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioSceneController : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource soundAudio;

    [Header("Audio Source Volume")]
    [SerializeField] private float musicVolume;
    [SerializeField] private float soundVolume;

    [Header("Volume Mute Check")]
    [SerializeField] private int isMusicMuted;
    [SerializeField] private int isSoundMuted;

    private void Awake()
    {
        // Use FindGameObjectWithTag because Audio Source is automatically destroyed in MainMenuBGM.cs
        musicAudio = GameObject.FindGameObjectWithTag("MainMenuBGM").GetComponent<AudioSource>();
        soundAudio = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<AudioSource>();

        isMusicMuted = PlayerPrefs.GetInt("mVolumeMute", 0);
        isSoundMuted = PlayerPrefs.GetInt("sVolumeMute", 0);
        musicVolume = PlayerPrefs.GetFloat("mVolume", 1);
        soundVolume = PlayerPrefs.GetFloat("sVolume", 1);

        if (isMusicMuted == 1)
            musicAudio.mute = true;
        if (isSoundMuted == 1)
            soundAudio.mute = true;
    }

    void OnEnable()
    {
        Debug.Log("<color=white>AudioSceneController</color> - On Enabled");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        Debug.Log("<color=white>AudioSceneController</color> - On Disabled");
    }

    void Start()
    {
        musicAudio.volume = musicVolume;
        soundAudio.volume = soundVolume;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            musicAudio.Play();
            Debug.Log("<color=white>AudioSceneController</color> - Current Scene Index: " + scene.buildIndex + " - Audio Play");
        }
        // if ((scene.buildIndex != 0) && (scene.buildIndex != 1))
        if (scene.buildIndex > 4)
        {
            //musicAudio.volume = 0f;
            //if (musicAudio.timeSamples > musicAudio.clip.samples - 300) musicAudio.Stop();  // Fix for pop/click artefact when stopping the music if not using Load Type: Streaming
            musicAudio.Stop();
            Debug.Log("<color=white>AudioSceneController</color> - Current Scene Index: " + scene.buildIndex + " - Audio Stop");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
