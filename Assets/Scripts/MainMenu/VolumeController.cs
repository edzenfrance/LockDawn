using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource soundAudio;

    [Header("Audio Source Volume")]
    [SerializeField] private float musicVolume;
    [SerializeField] private float soundVolume;

    [Header("Volume Slider")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [Header("Volume Slider Fill")]
    [SerializeField] private Image musicSliderFill;
    [SerializeField] private Image soundSliderFill;

    [Header("Volume Slider Fill Color")]
    [SerializeField] private Color fillColorMute = new Color32(130, 0, 0, 255);
    [SerializeField] private Color fillColorUnmute = new Color32(192, 10, 10, 255);

    [Header("Audio Toggle")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;

    [Header("Text Toggle")]
    [SerializeField] private TextMeshProUGUI musicToggleText;
    [SerializeField] private TextMeshProUGUI soundToggleText;

    [Header("Volume Mute")]
    [SerializeField] private int isMusicMuted;
    [SerializeField] private int isSoundMuted;

    void Awake()
    {
        // Use FindGameObjectWithTag because Audio Source is automatically destroyed in MainMenuBGM.cs
        musicAudio = GameObject.FindGameObjectWithTag("MainMenuBGM").GetComponent<AudioSource>();
        soundAudio = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<AudioSource>();
        //musicSliderFill = musicSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        //soundSliderFill = soundSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();

        isMusicMuted = PlayerPrefs.GetInt("mVolumeMute", 0);
        isSoundMuted = PlayerPrefs.GetInt("sVolumeMute", 0);
        musicVolume = PlayerPrefs.GetFloat("mVolume", 1);
        soundVolume = PlayerPrefs.GetFloat("sVolume", 1);

        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;

        if (isMusicMuted == 1)
            musicToggle.isOn = false;
        if (isSoundMuted == 1)
            soundToggle.isOn = false;
    }

    public void UpdateMusicVolume(float volume)
    {
        musicAudio.volume = volume;
        PlayerPrefs.SetFloat("mVolume", volume);
        Debug.Log("<color=white>VolumeController</color> - Slider Music - Volume: " + musicAudio.volume);
    }
    public void UpdateSoundVolume(float volume)
    {
        soundAudio.volume = volume;
        PlayerPrefs.SetFloat("sVolume", volume);
        Debug.Log("<color=white>VolumeController</color> - Slider Sound - Volume: " + soundAudio.volume);
    }

    public void ToggleMusic()
    {
        bool MusicToggleSwitch = musicToggle.isOn;
        if (MusicToggleSwitch)
        {
            PlayerPrefs.SetInt("mVolumeMute", 0);
            musicAudio.mute = false;
            musicSlider.enabled = true;
            musicSliderFill.color = fillColorUnmute;
            musicToggleText.text = "ON";
            Debug.Log("<color=white>VolumeController</color> - Toggle Music: ON");

        }
        else
        {
            PlayerPrefs.SetInt("mVolumeMute", 1);
            musicAudio.mute = true;
            musicSlider.enabled = false;
            musicSliderFill.color = fillColorMute;
            musicToggleText.text = "OFF";
            Debug.Log("<color=white>VolumeController</color> - Toggle Music: OFF");
        }
    }

    public void ToggleSound()
    {
        bool soundToggleSwitch = soundToggle.isOn;
        if (soundToggleSwitch)
        {
            PlayerPrefs.SetInt("sVolumeMute", 0);
            soundAudio.mute = false;
            soundSlider.enabled = true;
            soundSliderFill.color = fillColorUnmute;
            soundToggleText.text = "ON";
            Debug.Log("<color=white>VolumeController</color> - Toggle Sound: ON");
        }
        else
        {
            PlayerPrefs.SetInt("sVolumeMute", 1);
            soundAudio.mute = true;
            soundSlider.enabled = false;
            soundSliderFill.color = fillColorMute;
            soundToggleText.text = "OFF";
            Debug.Log("<color=white>VolumeController</color> - Toggle Sound: OFF");
        }
    }


}
