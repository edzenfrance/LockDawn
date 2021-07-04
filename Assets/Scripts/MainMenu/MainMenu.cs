using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GotoAchievements()
    {
        SceneManager.LoadScene("Achievements");
    }

    public void GotoCharacterSelection()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

    public void GotoStageSelection()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GotoCollectorsHall()
    {
        SceneManager.LoadScene("CollectorsHall");
    }

    public void GotoCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MenuSoundClick()
    {
        SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.UIClick);
    }
}
