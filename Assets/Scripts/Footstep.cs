using UnityEngine;

public class Footstep : MonoBehaviour
{
    public AudioClip[] audioClip;
    public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Step()
    {
        AudioClip clip = audioClip[2];
        audioSource.PlayOneShot(clip);
    }
}
