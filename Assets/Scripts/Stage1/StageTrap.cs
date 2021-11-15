using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTrap : MonoBehaviour
{
    public BoxCollider Floor;
    public AudioManager audioManager;
    public NoteController noteController;
    public HealthController healthController;
    [Range(1, 50)]
    public int fallDamage = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Floor.isTrigger = true;
            audioManager.PlayAudioWoodBreak();
            StartCoroutine(TriggerOff());
        }
    }

    IEnumerator TriggerOff()
    {
        yield return new WaitForSeconds(0.5f);
        noteController.ShowNote("You fall from upper floor!\nBe careful what you step on!", 4);
        healthController.ChangeHealthPoint(-fallDamage, false);
        Floor.isTrigger = false;
    }
}

