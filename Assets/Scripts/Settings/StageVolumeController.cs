using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageVolumeController : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource[] audioSource;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sound;

    [Header("Volume Slider")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [Header("Volume Slider Fill")]
    [SerializeField] private Image musicSliderFill;
    [SerializeField] private Image soundSliderFill;

    [Header("Volume Slider Fill Color")]
    Color fillColorMute = new Color32(70, 19, 19, 255);
    Color fillColorUnmute = new Color32(91, 19, 19, 255);

    [Header("Audio Toggle")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;

    [Header("Text Toggle")]
    [SerializeField] private TextMeshProUGUI musicToggleText;
    [SerializeField] private TextMeshProUGUI soundToggleText;

    void Awake()
    {
        audioSource = GameObject.Find("Audio Manager").GetComponents<AudioSource>();
        sound = audioSource[0];
        music = audioSource[1];

        SaveManager.GetSoundMusic();

        musicSlider.value = SaveManager.musicVolume;
        soundSlider.value = SaveManager.soundVolume;

        if (SaveManager.musicMute == 1) musicToggle.isOn = false;
        if (SaveManager.soundMute == 1) soundToggle.isOn = false;
    }

    public void UpdateMusicVolume(float volume)
    {
        music.volume = volume;
        SaveManager.SetMusicVolume(volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        sound.volume = volume;
        SaveManager.SetSoundVolume(volume);
    }

    public void ToggleMusic()
    {
        bool musicToggleSwitch = musicToggle.isOn;
        if (musicToggleSwitch)
        {
            SaveManager.SetMusicMute(0);
            music.mute = false;
            musicSlider.enabled = true;
            musicSliderFill.color = fillColorUnmute;
            musicToggleText.text = "ON";
            Debug.Log("<color=white>StageVolumeController</color> - Toggle Music: ON");

        }
        else
        {
            SaveManager.SetMusicMute(1);
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
            SaveManager.SetSoundMute(0);
            sound.mute = false;
            soundSlider.enabled = true;
            soundSliderFill.color = fillColorUnmute;
            soundToggleText.text = "ON";
            Debug.Log("<color=white>StageVolumeController</color> - Toggle Sound: ON");
        }
        else
        {
            SaveManager.SetSoundMute(1);
            sound.mute = true;
            soundSlider.enabled = false;
            soundSliderFill.color = fillColorMute;
            soundToggleText.text = "OFF";
            Debug.Log("<color=white>StageVolumeController</color> - Toggle Sound: OFF");
        }
    }


}
