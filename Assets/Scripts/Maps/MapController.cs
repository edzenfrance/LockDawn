using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject mapView;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private GameObject mapButton;

    public void openMinimap()
    {
        Time.timeScale = 0;
        mapView.SetActive(true);
        pauseButton.SetActive(false);
        inventoryButton.SetActive(false);
        mapButton.SetActive(false);
    }

    public void closeMinimap()
    {
        Time.timeScale = 1;
        mapView.SetActive(false);
        pauseButton.SetActive(true);
        inventoryButton.SetActive(true);
        mapButton.SetActive(true);
    }
}
