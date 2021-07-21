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

    [SerializeField] private Image musicVolumeSliderFill;
    [SerializeField] private Image soundVolumeSliderFill;

    [Header("AudioSource Volume")]
    [SerializeField] private float MusicVolume = 1f;
    [SerializeField] private float SoundVolume = 1f;

    [Header("Audio Toggle Checkbox")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;

    public TMPro.TextMeshProUGUI musicToggleText;
    public TMPro.TextMeshProUGUI soundToggleText;

    [Header("First Run")]
    [SerializeField] private int IsFirstRun;
    [SerializeField] private int IsMusicMuted;
    [SerializeField] private int IsSoundMuted;

    bool restartMusic = false;

    Color colorMute = new Color32(130, 0, 0, 255);
    Color colorUnMute = new Color32(192, 10, 10, 255);

    private void Awake()
    {
        MusicAudio = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        SoundAudio = GameObject.Find("SFXManager").GetComponent<AudioSource>();
        musicVolumeSliderFill = musicVolumeSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        soundVolumeSliderFill = soundVolumeSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();

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
            IsMusicMuted = PlayerPrefs.GetInt("mVolumeMute");
            IsSoundMuted = PlayerPrefs.GetInt("sVolumeMute");
            if (IsMusicMuted == 1)
            {
                musicVolumeSlider.enabled = false;
                musicVolumeSliderFill.color = colorMute;
                musicToggleText.text = "OFF";
                musicToggle.isOn = false;
                MusicVolume = 0f;
            }
            else
            {
                MusicVolume = PlayerPrefs.GetFloat("mVolume");
            }

            if (IsSoundMuted == 1)
            {
                soundVolumeSlider.enabled = false;
                soundVolumeSliderFill.color = colorMute;
                soundToggleText.text = "OFF";
                soundToggle.isOn = false;
                SoundVolume = 0f;
            }
            else
            {
                SoundVolume = PlayerPrefs.GetFloat("sVolume");
            }
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

    public void MusicToggleControl()
    {
        bool MusicToggleSwitch = musicToggle.isOn;
        if (MusicToggleSwitch)
        {
            PlayerPrefs.SetInt("mVolumeMute", 0);
            musicVolumeSlider.enabled = true;
            musicVolumeSliderFill.color = colorUnMute;
            musicToggleText.text = "ON";
            MusicVolume = PlayerPrefs.GetFloat("mVolume");
        }
        else
        {
            PlayerPrefs.SetInt("mVolumeMute", 1);
            musicVolumeSlider.enabled = false;
            musicVolumeSliderFill.color = colorMute;
            musicToggleText.text = "OFF";
            MusicVolume = 0f;
        }
    }

    public void soundToggleControl()
    {
        bool soundToggleSwitch = soundToggle.isOn;
        if (soundToggleSwitch)
        {
            PlayerPrefs.SetInt("sVolumeMute", 0);
            soundVolumeSlider.enabled = true;
            soundVolumeSliderFill.color = colorUnMute;
            soundToggleText.text = "ON";
            SoundVolume = PlayerPrefs.GetFloat("sVolume");

        }
        else
        {
            PlayerPrefs.SetInt("sVolumeMute", 1);
            soundVolumeSlider.enabled = false;
            soundVolumeSliderFill.color = colorMute;
            soundToggleText.text = "OFF";
            SoundVolume = 0f;
        }
    }
}
