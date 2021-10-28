using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temp_PlayFabVerifyEmail : MonoBehaviour
{
    void Start()
    {
        AddContactEmailToPlayer();
    }


    void AddContactEmailToPlayer()
    {
        var loginReq = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        var emailAddress = "mingmingandborange@gmail.com";

        PlayFabClientAPI.LoginWithCustomID(loginReq, loginRes =>
        {
            Debug.Log("Successfully logged in player with PlayFabId: " + loginRes.PlayFabId);
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

















}