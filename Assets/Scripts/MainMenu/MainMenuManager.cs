using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject gameTitle;
    [SerializeField] private GameObject versionText;
    [SerializeField] private GameObject collectorsHall;
    [SerializeField] private GameObject checkSave;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject playFabManager;
    [SerializeField] private GameObject shop;


    public void OpenSettings()
    {
        gameTitle.SetActive(false);
        versionText.SetActive(false);
        mainMenuButton.SetActive(false);
        settingsMenu.SetActive(true);
        versionText.SetActive(false);
        collectorsHall.SetActive(false);
        playFabManager.SetActive(false);
        shop.SetActive(false);
        SFXManager.sfxInstance.audioSource.PlayOneShot(SFXManager.sfxInstance.UIClick);
    }

    public void ExitSettings()
    {
        gameTitle.SetActive(true);
        versionText.SetActive(true);
        mainMenuButton.SetActive(true);
        settingsMenu.SetActive(false);
        versionText.SetActive(true);
        collectorsHall.SetActive(true);
        playFabManager.SetActive(true);
        shop.SetActive(true);
        SFXManager.sfxInstance.audioSource.PlayOneShot(SFXManager.sfxInstance.UIClick);
    }

    public void OpenPlay()
    {
        mainMenuButton.SetActive(false);
        checkSave.SetActive(true);
        versionText.SetActive(false);
        collectorsHall.SetActive(false);
        playFabManager.SetActive(false);
        shop.SetActive(false);
        SFXManager.sfxInstance.audioSource.PlayOneShot(SFXManager.sfxInstance.UIClick);
    }

    public void ExitPlay()
    {
        mainMenuButton.SetActive(true);
        checkSave.SetActive(false);
        versionText.SetActive(true);
        collectorsHall.SetActive(true);
        playFabManager.SetActive(true);
        shop.SetActive(true);
        SFXManager.sfxInstance.audioSource.PlayOneShot(SFXManager.sfxInstance.UIClick);
    }

    public void OpenAchievements()
    {
        SFXManager.sfxInstance.audioSource.PlayOneShot(SFXManager.sfxInstance.UIClick);
        SceneManager.LoadScene("Achievements");
    }

    public void OpenCharacterSelection()
    {
        SFXManager.sfxInstance.audioSource.PlayOneShot(SFXManager.sfxInstance.UIClick);
        SceneManager.LoadScene("CharacterSelection");
    }

    public void OpenStageSelection()
    {
        SFXManager.sfxInstance.audioSource.PlayOneShot(SFXManager.sfxInstance.UIClick);
        SceneManager.LoadScene("StageSelect");
    }

    public void OpenCollectorsHall()
    {
        SFXManager.sfxInstance.audioSource.PlayOneShot(SFXManager.sfxInstance.UIClick);
        SceneManager.LoadScene("CollectorsHall");
    }

    public void OpenCredits()
    {
        SFXManager.sfxInstance.audioSource.PlayOneShot(SFXManager.sfxInstance.UIClick);
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame()
    {
        SFXManager.sfxInstance.audioSource.PlayOneShot(SFXManager.sfxInstance.UIClick);
        Application.Quit();
    }

    public void OpenShop()
    {
        SFXManager.sfxInstance.audioSource.PlayOneShot(SFXManager.sfxInstance.UIClick);
        SceneManager.LoadScene("CharacterShop");
    }
}
