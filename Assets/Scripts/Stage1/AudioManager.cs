using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] audioSource;
    public AudioSource sounds;
    public AudioSource heart;

    [Header("Audio Clip")]
    public AudioClip footstep;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip doorLockedKey;
    public AudioClip doorLockedNoKey;
    public AudioClip zombieAttack;
    public AudioClip heartBeat;
    public AudioClip dead;
    public AudioClip objectiveVoice;
    public AudioClip pickUpItem;
    public AudioClip pickUpKey;
    public AudioClip pickUpBottle;
    public AudioClip pickUpPaper;
    public AudioClip pickUpCoin;
    public AudioClip woodBreak;
    void Awake()
    {
        audioSource = GetComponents<AudioSource>();
        sounds = audioSource[0];
        heart = audioSource[1];

        float isMusicMuted = PlayerPrefs.GetInt("Music Mute", 0);
        float isSoundMuted = PlayerPrefs.GetInt("Sound Mute", 0);
        float musicVolume = PlayerPrefs.GetFloat("Music Volume", 1);
        float soundVolume = PlayerPrefs.GetFloat("Music Volume", 1);

        if (isMusicMuted == 1)
            sounds.mute = true;
        if (isSoundMuted == 1)
            heart.mute = true;

        sounds.volume = soundVolume;
        heart.volume = musicVolume;
    }

    public void PlayAudioFootstep()
    {
        PlayOneShot(footstep);
    }

    public void PlayAudioDoorOpen()
    {
        PlaySounds(doorOpen);
    }

    public void PlayAudioDoorClose()
    {
        PlaySounds(doorClose);
    }

    public void PlayAudioDoorLockedKey()
    {
        PlaySounds(doorLockedKey);
    }

    public void PlayAudioDoorLockedNoKey()
    {
        PlaySounds(doorLockedNoKey);
    }

    public void PlayAudioZombieAttack()
    {
        PlaySounds(zombieAttack);
    }


    public void PlayAudioHeartBeat()
    {
        PlaySoundsLoop(heartBeat);
    }

    public void PlayAudioDeadCharacter()
    {
        PlaySoundsLoop(dead);
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

    public void PlayAudioPickUpCoin()
    {
        PlaySounds(pickUpCoin);
    }

    public void PlayAudioWoodBreak()
    {
        PlaySounds(woodBreak);
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

    public void PlayOneShot(AudioClip aClip)
    {
        sounds.PlayOneShot(aClip);
    }

    public void PlaySoundsLoop(AudioClip aClip)
    {
        heart.loop = true;
        heart.clip = aClip;
        heart.Play();
    }

    public void StopAudioLoop()
    {
        heart.Stop();
    }

    public void PauseAudio()
    {
        sounds.Pause();
    }

    public void UnpauseAudio()
    {
        sounds.UnPause();
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
