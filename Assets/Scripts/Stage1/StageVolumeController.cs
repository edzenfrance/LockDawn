using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageVolumeController : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource[] audioSource;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sound;

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
        audioSource = GameObject.Find("Audio Manager").GetComponents<AudioSource>();
        sound = audioSource[0];
        music = audioSource[1];

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
        music.volume = volume;
        PlayerPrefs.SetFloat("mVolume", volume);
        Debug.Log("<color=white>StageVolumeController</color> - Slider Music - Volume: " + music.volume);
    }
    public void UpdateSoundVolume(float volume)
    {
        sound.volume = volume;
        PlayerPrefs.SetFloat("sVolume", volume);
        Debug.Log("<color=white>StageVolumeController</color> - Slider Sound - Volume: " + sound.volume);
    }

    public void ToggleMusic()
    {
        bool MusicToggleSwitch = musicToggle.isOn;
        if (MusicToggleSwitch)
        {
            PlayerPrefs.SetInt("mVolumeMute", 0);
            music.mute = false;
            musicSlider.enabled = true;
            musicSliderFill.color = fillColorUnmute;
            musicToggleText.text = "ON";
            Debug.Log("<color=white>StageVolumeController</color> - Toggle Music: ON");

        }
        else
        {
            PlayerPrefs.SetInt("mVolumeMute", 1);
            music.mute = true;
            musicSlider.enabled = false;
            musicSliderFill.color = fillColorMute;
            musicToggleText.text = "OFF";
            Debug.Log("<color=white>StageVolumeController</color> - Toggle Music: OFF");
        }
    }

    public void ToggleSound()
    {
        bool soundToggleSwitch = soundToggle.isOn;
        if (soundToggleSwitch)
        {
            PlayerPrefs.SetInt("sVolumeMute", 0);
            sound.mute = false;
            soundSlider.enabled = true;
            soundSliderFill.color = fillColorUnmute;
            soundToggleText.text = "ON";
            Debug.Log("<color=white>StageVolumeController</color> - Toggle Sound: ON");
        }
        else
        {
            PlayerPrefs.SetInt("sVolumeMute", 1);
            sound.mute = true;
            soundSlider.enabled = false;
            soundSliderFill.color = fillColorMute;
            soundToggleText.text = "OFF";
            Debug.Log("<color=white>StageVolumeController</color> - Toggle Sound: OFF");
        }
    }


}
