using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource soundAudio;

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

    void Awake()
    {
        // Use FindGameObjectWithTag because Audio Source is automatically destroyed in MainMenuBGM.cs
        musicAudio = GameObject.FindGameObjectWithTag("MainMenuBGM").GetComponent<AudioSource>();
        soundAudio = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<AudioSource>();
        //musicSliderFill = musicSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        //soundSliderFill = soundSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();

        SaveManager.GetSoundMusic();

        musicSlider.value = SaveManager.musicVolume;
        soundSlider.value = SaveManager.soundVolume;

        if (SaveManager.musicMute == 1) musicToggle.isOn = false;
        if (SaveManager.soundMute == 1) soundToggle.isOn = false;
    }

    public void UpdateMusicVolume(float volume)
    {
        musicAudio.volume = volume;
        SaveManager.SetSoundVolume(volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        soundAudio.volume = volume;
        SaveManager.SetSoundVolume(volume);
    }

    public void ToggleMusic()
    {
        bool nusicToggleSwitch = musicToggle.isOn;
        if (nusicToggleSwitch)
        {
            SaveManager.SetMusicMute(0);
            musicAudio.mute = false;
            musicSlider.enabled = true;
            musicSliderFill.color = fillColorUnmute;
            musicToggleText.text = "ON";
            Debug.Log("<color=white>VolumeController</color> - Toggle Music: ON");

        }
        else
        {
            SaveManager.SetMusicMute(1);
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
            SaveManager.SetSoundMute(0);
            soundAudio.mute = false;
            soundSlider.enabled = true;
            soundSliderFill.color = fillColorUnmute;
            soundToggleText.text = "ON";
            Debug.Log("<color=white>VolumeController</color> - Toggle Sound: ON");
        }
        else
        {
            SaveManager.SetSoundMute(1);
            soundAudio.mute = true;
            soundSlider.enabled = false;
            soundSliderFill.color = fillColorMute;
            soundToggleText.text = "OFF";
            Debug.Log("<color=white>VolumeController</color> - Toggle Sound: OFF");
        }
    }


}
