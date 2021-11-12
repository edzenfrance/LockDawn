using UnityEngine;

public class Footstep : MonoBehaviour
{
    public AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    public void Step()
    {
        audioManager.PlayAudioFootstep();
    }
}
