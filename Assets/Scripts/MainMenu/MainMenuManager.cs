using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

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
        PlayerPrefs.SetString("Scene", "MainMenu");
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

    public void OpenShop()
    {
        SceneManager.LoadScene("CharacterShop");
    }
}
