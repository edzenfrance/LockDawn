using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSceneController : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource soundAudio;

    private void Awake()
    {
        // Use FindGameObjectWithTag because Audio Source is automatically destroyed in MainMenuBGM.cs
        musicAudio = GameObject.FindGameObjectWithTag("MainMenuBGM").GetComponent<AudioSource>();
        soundAudio = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<AudioSource>();

        SaveManager.GetSoundMusic();

        if (SaveManager.musicMute == 1) musicAudio.mute = true;
        if (SaveManager.soundMute == 1) soundAudio.mute = true;
    }

    void OnEnable()
    {
        Debug.Log("<color=white>AudioSceneController</color> - On Enabled");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        musicAudio.volume = SaveManager.musicVolume;
        soundAudio.volume = SaveManager.soundVolume;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            if (!musicAudio.isPlaying)
            {
                musicAudio.Play();
                Debug.Log("<color=white>AudioSceneController</color> - Current Scene Index: " + scene.buildIndex + " - Audio Play");
            }
        }
        if (scene.buildIndex > 6)
        {
            //musicAudio.volume = 0f;
            //if (musicAudio.timeSamples > musicAudio.clip.samples - 300) musicAudio.Stop();  // Fix for pop/click artefact when stopping the music if not using Load Type: Streaming
            musicAudio.Stop();
            Debug.Log("<color=white>AudioSceneController</color> - Current Scene Index: " + scene.buildIndex + " - Audio Stop");
            Destroy(musicAudio);
            Destroy(soundAudio);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
