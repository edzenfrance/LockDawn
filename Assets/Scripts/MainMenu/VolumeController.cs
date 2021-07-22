using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] private AudioSource MusicAudio;
    [SerializeField] private AudioSource SoundAudio;

    [Header("Volume Slider")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundVolumeSlider;

    [SerializeField] private Image musicVolumeSliderFill;
    [SerializeField] private Image soundVolumeSliderFill;

    [Header("AudioSource Volume")]
    [SerializeField] private float MusicVolume = 1f;
    [SerializeField] private float SoundVolume = 1f;

    [Header("Audio Toggle Checkbox")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;

    [SerializeField] private TMPro.TextMeshProUGUI musicToggleText;
    [SerializeField] private TMPro.TextMeshProUGUI soundToggleText;

    [SerializeField] private int IsMusicMuted;
    [SerializeField] private int IsSoundMuted;

    Color colorMute = new Color32(130, 0, 0, 255);
    Color colorUnMute = new Color32(192, 10, 10, 255);

    void Awake()
    {
        //MusicAudio = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        //SoundAudio = GameObject.Find("SFXManager").GetComponent<AudioSource>();
        //musicVolumeSliderFill = musicVolumeSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        ///soundVolumeSliderFill = soundVolumeSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();

        Debug.Log("VolumeController - Started");
        IsMusicMuted = PlayerPrefs.GetInt("mVolumeMute");
        IsSoundMuted = PlayerPrefs.GetInt("sVolumeMute");
        MusicVolume = PlayerPrefs.GetFloat("mVolume");
        SoundVolume = PlayerPrefs.GetFloat("sVolume");

        if (IsMusicMuted == 1)
        {
            musicVolumeSlider.value = MusicVolume;
            musicToggle.isOn = false;
        }
        else
        {
            musicVolumeSlider.value = MusicVolume;
        }

        if (IsSoundMuted == 1)
        {
            soundVolumeSlider.value = SoundVolume;
            soundToggle.isOn = false;
        }
        else
        {
            soundVolumeSlider.value = SoundVolume;
        }
    }

    void Update()
    {
        MusicAudio.volume = MusicVolume;
        SoundAudio.volume = SoundVolume;
    }

    public void UpdateMusicVolume(float volume)
    {
        MusicVolume = volume;
        PlayerPrefs.SetFloat("mVolume", MusicVolume);
        Debug.Log("VolumeController - Slider Music Volume: " + MusicAudio.volume);
    }
    public void UpdateSoundVolume(float volume)
    {
        SoundVolume = volume;
        PlayerPrefs.SetFloat("sVolume", SoundVolume);
        Debug.Log("VolumeController - Slider Sound Volume: " + SoundAudio.volume);
    }

    public void ToggleMusic()
    {
        bool MusicToggleSwitch = musicToggle.isOn;
        if (MusicToggleSwitch)
        {
            PlayerPrefs.SetInt("mVolumeMute", 0);
            musicVolumeSlider.enabled = true;
            musicVolumeSliderFill.color = colorUnMute;
            musicToggleText.text = "ON";
            MusicVolume = PlayerPrefs.GetFloat("mVolume");
            Debug.Log("VolumeController - ToggleMusic: ON - Volume: " + MusicVolume);

        }
        else
        {
            PlayerPrefs.SetInt("mVolumeMute", 1);
            musicVolumeSlider.value = MusicVolume;
            musicVolumeSlider.enabled = false;
            musicVolumeSliderFill.color = colorMute;
            musicToggleText.text = "OFF";
            MusicVolume = 0f;
            Debug.Log("VolumeController - ToggleMusic: OFF - Volume: " + MusicVolume);
        }
    }

    public void ToggleSound()
    {
        bool soundToggleSwitch = soundToggle.isOn;
        if (soundToggleSwitch)
        {
            PlayerPrefs.SetInt("sVolumeMute", 0);
            soundVolumeSlider.enabled = true;
            soundVolumeSliderFill.color = colorUnMute;
            soundToggleText.text = "ON";
            SoundVolume = PlayerPrefs.GetFloat("sVolume");
            Debug.Log("VolumeController - ToggleSound: ON - Volume: " + SoundVolume);
        }
        else
        {
            PlayerPrefs.SetInt("sVolumeMute", 1);
            soundVolumeSlider.enabled = false;
            soundVolumeSliderFill.color = colorMute;
            soundToggleText.text = "OFF";
            SoundVolume = 0f;
            Debug.Log("VolumeController - ToggleSound: OFF - Volume: " + SoundVolume);
        }
    }


}
