using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class Temp_PlayFabManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI infoUsername;
    public TextMeshProUGUI infoCreatedAt;

    [Header("Login")]
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;

    [Header("Register")]
    public TMP_InputField registerUsername;
    public TMP_InputField registerEmail;
    public TMP_InputField registerPassword;

    [Header("Reset")]
    public TMP_InputField resetEmail;

    void Start()
    {
        loginPassword.contentType = TMP_InputField.ContentType.Password;
        registerPassword.contentType = TMP_InputField.ContentType.Password;
    }



    #region Register Account Then Send Verification Email

    public void RegisterButton()
    {
        if (registerPassword.text.Length < 6)
        {
            messageText.text = "Password too short!";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            TitleId = PlayFabSettings.TitleId,
            Username = registerUsername.text,
            Email = registerEmail.text,
            Password = registerPassword.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnPlayFabError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
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
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginResultA =>
        {
            Debug.Log("Successfully logged in player with PlayFabId: " + OnLoginResultA.PlayFabId);
            AddOrUpdateContactEmailA(emailAddress);

        }, FailureCallback);
    }

    void AddOrUpdateContactEmailA(string emailAddress)
    {
        var request = new AddOrUpdateContactEmailRequest
        {
            //PlayFabId = playFabId,
            EmailAddress = emailAddress
        };
        PlayFabClientAPI.AddOrUpdateContactEmail(request, result =>
        {
            messageText.text = "Verification sent to email!";
            Debug.Log("The player's account has been updated with a contact email");
        }, FailureCallback);
    }

    #endregion





    void AddContactEmailToPlayer()
    {
        var loginReq = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = false
        };

        var emailAddress = registerEmail.text;
        Debug.Log("Email Address Use: " + emailAddress);

        PlayFabClientAPI.LoginWithCustomID(loginReq, loginResY =>
        {
            Debug.Log("Successfully logged in player with PlayFabId: " + loginResY.PlayFabId);
            AddOrUpdateContactEmail(emailAddress);
        }, FailureCallback);
    }

    void AddOrUpdateContactEmail(string emailAddress)
    {
        var request = new AddOrUpdateContactEmailRequest
        {
            //PlayFabId = playFabId,
            EmailAddress = emailAddress
        };
        PlayFabClientAPI.AddOrUpdateContactEmail(request, result =>
        {
            Debug.Log("The player's account has been updated with a contact email");
        }, FailureCallback);
    }

    void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }




    #region Login Account

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = loginEmail.text,
            Password = loginPassword.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginResult, OnPlayFabError);
    }

    void OnLoginResult(LoginResult result)
    {
        loginEmail.text = "";
        loginPassword.text = "";
        messageText.text = "You are now logged in!";
        GetAccountInfoTwo();
    }

    #endregion



    #region Get Account Info

    public void GetAccountInfo()
    {
        var request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnPlayFabError);
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult resultData)
    {
        Debug.Log("Recieved account data");
        // Let data output
        Debug.Log("Username: " + resultData.AccountInfo.Username);
        // Show result on text field
        //infoUsername.text = resultData.AccountInfo.Username.ToString();
        //infoCreatedAt.text = resultData.AccountInfo.Created.ToString();
        //infoCreatedAt.text = resultData.AccountInfo.PrivateInfo.Email.ToString();
        Debug.Log("Created: " + resultData.AccountInfo.Created);
        Debug.Log("PlayFabId: " + resultData.AccountInfo.PlayFabId);
    }

    #endregion



    #region Login

    void CustomLogin()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnCustomLoginSuccess, OnCustomLoginError);
    }

    private void OnCustomLoginSuccess(LoginResult result)
    {
        Debug.Log("Successful login/Create account");
    }

    private void OnCustomLoginError(PlayFabError error)
    {
        Debug.Log("API error!");
    }

    #endregion


    public void GetAccountInfoTwo()
    {
        var request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccessTwo, OnPlayFabError);
    }

    private void OnGetAccountInfoSuccessTwo(GetAccountInfoResult resultData)
    {
        GetPlayerProfileT(resultData.AccountInfo.PlayFabId);
        Debug.Log("PlayFabId: " + resultData.AccountInfo.PlayFabId);
    }


    void GetPlayerProfileT(string playFabId)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowContactEmailAddresses = true,
                ShowDisplayName = true
            }
        },
        result => Debug.Log("The player's DisplayName profile data is: " + result.PlayerProfile.ContactEmailAddresses[0].VerificationStatus),
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void GetPlayerProfile(string playFabId)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowContactEmailAddresses = true
            }
        },
        result => Debug.Log("The player's DisplayName profile data is: " + result.PlayerProfile.ContactEmailAddresses[0].VerificationStatus),
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void GetPlayerProfileTwo(string playFabId)
    {
        var request = new GetPlayerProfileRequest
        {
            PlayFabId = playFabId,

            //Added this to fix the problem
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true,
                ShowLinkedAccounts = true,
            }
        };
        PlayFabClientAPI.GetPlayerProfile(request,
        (GetPlayerProfileResult obj) => {

            Debug.Log(obj.PlayerProfile.DisplayName);
            Debug.Log(obj.PlayerProfile.LinkedAccounts);
        },
        (PlayFabError obj) => {
            Debug.Log("Playfab : unable to get player profile ");
        });
    }


    #region Reset Password

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = resetEmail.text,
            TitleId = "D5450"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnPlayFabError);
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset email sent!";
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
            case PlayFabErrorCode.InvalidParams:
                output = "Please fill out all fields";
                break;
            case PlayFabErrorCode.InvalidUsernameOrPassword:
                output = "Wrong username or password";
                break;
            case PlayFabErrorCode.UsernameNotAvailable:
                output = "Please choose username";
                break;
            case PlayFabErrorCode.AccountNotFound:
                output = "Account not found";
                break;
            case PlayFabErrorCode.SmtpServerLimitExceeded:
                output = "SMPT Server Limit Exceeded";
                break;
            default:
                break;
        }
        // Assign the text of the error message 
        messageText.text = output;
    }


    public void BackButton()
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
