using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] audioSource;
    public AudioSource sounds;
    public AudioSource heart;

    [Header("Audio Clip")]
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip doorLocked;
    public AudioClip heartBeat;
    public AudioClip objectiveVoice;
    public AudioClip pickUpItem;
    public AudioClip pickUpKey;
    public AudioClip pickUpBottle;
    public AudioClip pickUpPaper;

    void Awake()
    {
        audioSource = GetComponents<AudioSource>();
        sounds = audioSource[0];
        heart = audioSource[1];
    }

    public void PlayAudioDoorOpen()
    {
        PlaySounds(doorOpen);
    }

    public void PlayAudioDoorClose()
    {
        PlaySounds(doorClose);
    }

    public void PlayAudioDoorLocked()
    {
        PlaySounds(doorLocked);
    }

    public void PlayAudioHeartBeat()
    {
        PlaySounds(heartBeat);
    }

    public void PlayAudioObjective()
    {
        PlaySounds(objectiveVoice);
    }

    public void PlayAudioPickUpItem()
    {
        PlaySounds(pickUpItem);
    }

    public void PlayAudioPickUpKey()
    {
        PlaySounds(pickUpKey);
    }

    public void PlayAudioPickUpBottle()
    {
        PlaySounds(pickUpBottle);
    }

    public void PlayAudioPickUpPaper()
    {
        PlaySounds(pickUpPaper);
    }

    public void Psounds(AudioClip aClip)
    {
      sounds.clip = aClip;
      sounds.Play();
    }

    public void PlaySounds(AudioClip aClip)
    {
        sounds.clip = aClip;
        sounds.Play();
    }

    public void PauseAudio()
    {
        sounds.Pause();
    }

    public void PlayAudio()
    {
        sounds.Play();
    }

    public void StopAudio()
    {
        sounds.Stop();
    }
}
