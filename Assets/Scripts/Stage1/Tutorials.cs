using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorials : MonoBehaviour
{
    [SerializeField] private GameObject touchZoneLook;
    [SerializeField] private GameObject touchZoneMove;
    [SerializeField] private GameObject tutorials;



    private void Start()
    {
        Time.timeScale = 0;
    }

    public void finishTutorial()
    {
        Time.timeScale = 1;
        tutorials.SetActive(false);
        touchZoneLook.SetActive(true);
        touchZoneMove.SetActive(true);
    }
}


