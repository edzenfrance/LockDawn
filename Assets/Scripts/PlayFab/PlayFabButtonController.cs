using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabButtonController : MonoBehaviour
{
    public Button accountButton;
    public TextMeshProUGUI accountButtonText;
    public GameObject noNetwork;

    public GameObject playFabLogin;
    public GameObject gameTitle;
    public GameObject mainMenuButton;
    public GameObject collectorsHallButton;
    public GameObject playFabManagerButton;
    public GameObject alreadyLoginPanel;
    public GameObject loginRegisterPanel;
    public GameObject playFabButtonController;

    string getUsername;
    bool usernameFound = false;

    void Start()
    {
        InvokeRepeating(nameof(CheckNetwork), 0f, 2.5f);
    }

    void OnEnable()
    {
        StartCoroutine(GetAccountInformation());
    }


    public void CheckNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            accountButton.interactable = false;
            if (playFabManagerButton.activeSelf)
            {
                noNetwork.SetActive(true);
            }
            accountButtonText.text = "No Internet";
            var newColorBlock = accountButton.colors;
            newColorBlock.disabledColor = new Color(1, 1, 1, 1);
            accountButton.colors = newColorBlock;
        }
        else
        {
            accountButton.interactable = true;
            noNetwork.SetActive(false);
            if (!usernameFound)
            {
                accountButtonText.text = "Account";
            }
        }
    }

    public void OpenPlayFabMenu()
    {
        StartCoroutine(GetAccountInformation());
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return;
        }
        else
        {
            if (usernameFound)
            {
                playFabLogin.SetActive(true);
                alreadyLoginPanel.SetActive(true);
                loginRegisterPanel.SetActive(false);
                noNetwork.SetActive(false);
                gameTitle.SetActive(false);
                mainMenuButton.SetActive(false);
                collectorsHallButton.SetActive(false);
                playFabManagerButton.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                playFabLogin.SetActive(true);
                alreadyLoginPanel.SetActive(false);
                loginRegisterPanel.SetActive(true);
                noNetwork.SetActive(false);
                gameTitle.SetActive(false);
                mainMenuButton.SetActive(false);
                collectorsHallButton.SetActive(false);
                playFabManagerButton.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator GetAccountInformation()
    {
        GetAccountInfo();
        yield return new WaitForSeconds(2);
    }

    void exitPlayFabMenuNoConnection()
    {
        playFabLogin.SetActive(false);
        noNetwork.SetActive(true);
        gameTitle.SetActive(true);
        mainMenuButton.SetActive(true);
        collectorsHallButton.SetActive(true);
        playFabManagerButton.SetActive(true);
    }

    void GetAccountInfo()
    {
        Debug.Log("----- GetAccountInfo ----");
        var request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnError);
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult resultData)
    {
        Debug.Log("RECIEVED ACCOUNT INFORMATION");
        Debug.Log("Username: " + resultData.AccountInfo.Username);
        Debug.Log("Created: " + resultData.AccountInfo.Created);
        Debug.Log("PlayFabId: " + resultData.AccountInfo.PlayFabId);
        getUsername = resultData.AccountInfo.Username;
        accountButtonText.text = resultData.AccountInfo.Username;
        usernameFound = true;
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Output: " + error);
    }


    private void OnPlayFabError(PlayFabError obj)
    {
        Debug.Log("Error: " + obj.Error);
        string output = "";

        switch (obj.Error)
        {
            case PlayFabErrorCode.AccountBanned:
                output = "Bye bye. You're account has been banned";
                break;
            case PlayFabErrorCode.EmailAddressNotAvailable:
                output = "Email address already in use";
                break;
            case PlayFabErrorCode.InvalidEmailAddress:
                output = "Invalid email address";
                break;
            case PlayFabErrorCode.InvalidParams:
                output = "Invalid email or password";
                break;
            case PlayFabErrorCode.InvalidUsernameOrPassword:
                output = "Invalid username or password";
                break;
            case PlayFabErrorCode.InvalidEmailOrPassword:
                output = "Invalid email or password";
                break;
            case PlayFabErrorCode.UsernameNotAvailable:
                output = "Username not available";
                break;
            case PlayFabErrorCode.AccountNotFound:
                output = "Account not found";
                break;
            case PlayFabErrorCode.SmtpServerLimitExceeded:
                output = "SMPT Server Limit Exceeded";
                break;
            case PlayFabErrorCode.ServiceUnavailable:
                output = "No internet connection";
                break;
            default:
                break;
        }
        Debug.Log("Output: " + output);
    }

    public void LogoutAndBack()
    {
        playFabButtonController.SetActive(true);
        StartCoroutine(WaitToLogout());
        StartCoroutine(GetAccountInformation());
    }

    IEnumerator WaitToLogout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        yield return new WaitForSeconds(2f);
        usernameFound = false;
        gameTitle.SetActive(true);
        mainMenuButton.SetActive(true);
        playFabLogin.SetActive(false);
        playFabManagerButton.SetActive(true);
        collectorsHallButton.SetActive(true);
        alreadyLoginPanel.SetActive(false);
        loginRegisterPanel.SetActive(false);

    }
}