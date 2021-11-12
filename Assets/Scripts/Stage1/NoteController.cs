using System.Collections;
using UnityEngine;
using TMPro;

public class NoteController : MonoBehaviour
{
    [SerializeField] private GameObject noteObject;
    [SerializeField] private TextMeshProUGUI noteText;

    public void ShowNote(string note, float second)
    {
        noteObject.SetActive(true);
        noteText.text = note;
        StartCoroutine(RemoveNoteWait(second));
    }

    IEnumerator RemoveNoteWait(float waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);
        noteText.text = "";
        noteObject.SetActive(false);
    }

    public void ShowNoteForever(string note)
    {
        noteObject.SetActive(true);
        noteText.text = note;
    }

    public void RemoveNoteInstant()
    {
        noteText.text = "";
        noteObject.SetActive(false);
    }
}
