using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManagerX : MonoBehaviour
{
    [Header("UI TextMesh")]
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI infoUsername;
    public TextMeshProUGUI infoCreatedAt;

    [Header("Login InputField")]
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;

    [Header("Register InputField")]
    public TMP_InputField registerUsername;
    public TMP_InputField registerEmail;
    public TMP_InputField registerPassword;
    public TMP_InputField registerAge;

    [Header("Reset InputField")]
    public TMP_InputField resetEmail;

    [Header("GameObject")]
    public GameObject gameTitle;
    public GameObject mainMenuButton;
    public GameObject collectorsHallButton;
    public GameObject playFabLogin;
    public GameObject loginPanel;
    public GameObject resetPanel;
    public GameObject forgetPasswordButton;
    public GameObject playFabManagerButton;
    public GameObject noNetwork;
    public GameObject playFabButtonController;

    public ToggleGroup toggleGroup;

    string emailStatus = "";

    string genderToggle = "Male";
    int ageInput = 12;

    void Start()
    {
        loginPassword.contentType = TMP_InputField.ContentType.Password;
        registerPassword.contentType = TMP_InputField.ContentType.Password;
        registerAge.contentType = TMP_InputField.ContentType.IntegerNumber;
    }


    void CheckNetworkConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            messageText.text = "No internet connection";
            return;
        }
    }


    #region Register Account Then Send Verification Email

    public void RegisterButton()
    {
        ageInput = int.Parse(registerAge.text);
        if (ageInput < 12)
        {
            messageText.text = "Your age must be 12 and up to play this game!";
            StartCoroutine(ClearMessageText(3f));
            return;
        }
        if (registerPassword.text.Length < 6)
        {
            messageText.text = "Password too short!";
            StartCoroutine(ClearMessageText(2.5f));
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            TitleId = PlayFabSettings.TitleId,
            Username = registerUsername.text,
            Email = registerEmail.text,
            Password = registerPassword.text,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnPlayFabError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("OnRegisterSuccess");
        LoginAfterRegister();
    }

    void LoginAfterRegister()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = registerEmail.text,
            Password = registerPassword.text
        };
        var emailAddress = registerEmail.text;
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginResult =>
        {
            Debug.Log("Successfully logged in player with PlayFabId: " + OnLoginResult.PlayFabId);
            SaveGenderAndAge();
            AddOrUpdateContactEmail(emailAddress);
        }, OnPlayFabError);
    }

    void SaveGenderAndAge()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Gender",  genderToggle},
                {"Age", ageInput.ToString()}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend =>
        {
            Debug.Log("Successful User Data Send: Gender: " + genderToggle + " Age: " + ageInput.ToString());
        }, OnPlayFabError);
    }


    // This will send verification email to the registered account
    void AddOrUpdateContactEmail(string emailAddress)
    {
        var request = new AddOrUpdateContactEmailRequest
        {
            //PlayFabId = playFabId,
            EmailAddress = emailAddress
        };
        PlayFabClientAPI.AddOrUpdateContactEmail(request, result =>
        {
            messageText.text = "Verification sent to email!";
            StartCoroutine(ClearMessageText(2.5f));
            registerUsername.text = "";
            registerEmail.text = "";
            registerPassword.text = "";
            registerAge.text = "";

            Debug.Log("The player's account has been updated with a contact email");
        }, OnPlayFabError);
    }

    public void ToggleGender()
    {
        foreach (Toggle toggle in toggleGroup.ActiveToggles())
        {
            if (toggle.name == "Male")
            {
                genderToggle = "Male";
            }
            if (toggle.name == "Female")
            {
                genderToggle = "Female";
            }
        }
    }

    #endregion



    #region Login Account

    public void LoginButton()
    {
        if (loginEmail.text.Length == 0)
        {
            messageText.text = "Invalid email address";
            StartCoroutine(ClearMessageText(2.0f));
            return;
        }
        if (loginPassword.text.Length == 0)
        {
            messageText.text = "Invalid password";
            StartCoroutine(ClearMessageText(2.0f));
            return;
        }
        CheckNetworkConnection();
        var request = new LoginWithEmailAddressRequest
        {
            Email = loginEmail.text,
            Password = loginPassword.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginResult, OnLoginError);
    }

    void OnLoginError(PlayFabError error)
    {
        messageText.text = "Invalid email or password";
        forgetPasswordButton.SetActive(true);
        StartCoroutine(ClearMessageText(1.5f));
    }

    void OnLoginResult(LoginResult result)
    {
        GetAccountInfo();
    }

    // Check if account is verified
    public void GetAccountInfo()
    {
        var request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnPlayFabError);
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult resultData)
    {
        Debug.Log("ACCOUNT INFORMATION");
        //infoUsername.text = resultData.AccountInfo.Username.ToString();
        //infoCreatedAt.text = resultData.AccountInfo.Created.ToString();
        //infoCreatedAt.text = resultData.AccountInfo.PrivateInfo.Email.ToString();
        Debug.Log("Username: " + resultData.AccountInfo.Username);
        Debug.Log("Created: " + resultData.AccountInfo.Created);
        Debug.Log("PlayFabId: " + resultData.AccountInfo.PlayFabId);
        Debug.Log("Email: " + resultData.AccountInfo.PrivateInfo.Email);
        GetPlayerProfile(resultData.AccountInfo.PlayFabId);
    }

    void GetPlayerProfile(string playFabId)
    {
        var request = new GetPlayerProfileRequest
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowContactEmailAddresses = true,
                ShowDisplayName = true
            }
        };
        PlayFabClientAPI.GetPlayerProfile(request, result =>
        {
            Debug.Log("Verification Status: " + result.PlayerProfile.ContactEmailAddresses[0].VerificationStatus);
            emailStatus = "" + result.PlayerProfile.ContactEmailAddresses[0].VerificationStatus;
            if (emailStatus == "Pending")
            {
                PlayFabClientAPI.ForgetAllCredentials();
                messageText.text = "Your account is not yet verified!";
            }
            else
            {
                loginEmail.text = "";
                loginPassword.text = "";
                messageText.text = "You are now logged in!";
                StartCoroutine(BackToMainMenu(1f));
            }
        }, FailureCallback);
    }

    #endregion



    #region Back to Main Menu

    IEnumerator BackToMainMenu(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        messageText.text = "";
        playFabLogin.SetActive(false);
        gameTitle.SetActive(true);
        mainMenuButton.SetActive(true);
        collectorsHallButton.SetActive(true);
        loginPanel.SetActive(false);
        playFabManagerButton.SetActive(true);
        playFabButtonController.SetActive(true);
    }

    #endregion



    #region Change Password

    public void ChangePasswordButton()
    {
        GetAccountInfoEmail();
    }

    public void GetAccountInfoEmail()
    {
        var request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoEmailSuccess, OnPlayFabError);
    }
    private void OnGetAccountInfoEmailSuccess(GetAccountInfoResult resultData)
    {
        Debug.Log("Email: " + resultData.AccountInfo.PrivateInfo.Email);
        GetPlayerProfileEmail(resultData.AccountInfo.PrivateInfo.Email);
    }

    void GetPlayerProfileEmail(string email)
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = "D5450"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordChange, OnPlayFabError);
    }

    private void OnPasswordChange(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Change password reset email sent!";
        StartCoroutine(BackToMainMenu(1f));
    }

    #endregion



    #region Reset Password

    public void ForgotPasswordButton()
    {
        messageText.text = "";
        loginPanel.SetActive(false);
        resetPanel.SetActive(true);
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = loginEmail.text,
            TitleId = "D5450"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnPasswordResetError);
    }

    void OnPasswordResetError(PlayFabError error)
    {
        Debug.Log(error);
        messageText.text = "Invalid email address";
        StartCoroutine(ClearMessageText(1.5f));
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset email sent!";
        StartCoroutine(ClearMessageText(1.5f));
    }

    #endregion



    #region Logout

    public void LogoutButton()
    {
        PlayFabClientAPI.ForgetAllCredentials();
    }

    #endregion


    #region Leaderboards

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "LockDawnScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnPlayFabError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful Leaderboard Sent");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "LockDawnScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnPlayFabError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }

    #endregion


    private void OnPlayFabError(PlayFabError obj)
    {
        Debug.Log("Error: " + obj.Error);
        //errorText.text = obj.Error.ToString();

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
        // Assign the text of the error message 
        messageText.text = output;
        StartCoroutine(ClearMessageText(1.5f));
    }

    void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    IEnumerator ClearMessageText(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        messageText.text = "";
    }


    public void BackButton()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            messageText.text = "";
            loginEmail.text = "";
            loginPassword.text = "";
            registerUsername.text = "";
            registerEmail.text = "";
            registerPassword.text = "";
            resetEmail.text = "";

            playFabLogin.SetActive(false);
            noNetwork.SetActive(true);
            gameTitle.SetActive(true);
            mainMenuButton.SetActive(true);
            collectorsHallButton.SetActive(true);
            playFabManagerButton.SetActive(true);
        }
        else
        {
            messageText.text = "";
            loginEmail.text = "";
            loginPassword.text = "";
            registerUsername.text = "";
            registerEmail.text = "";
            registerPassword.text = "";
            resetEmail.text = "";
        }
    }
}
