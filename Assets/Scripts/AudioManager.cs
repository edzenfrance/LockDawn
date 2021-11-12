using UnityEngine;

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

    void Awake()
    {
        audioSource = GetComponents<AudioSource>();
        sounds = audioSource[0];
        heart = audioSource[1];
    }

    public void PlayAudioFootstep()
    {
        PlaySounds(footstep);
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

    public void PlayAudio()
    {
        sounds.Play();
    }

    public void StopAudio()
    {
        sounds.Stop();
    }

}
