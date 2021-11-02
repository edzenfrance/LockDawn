using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class QuarantineController : MonoBehaviour
{
    [SerializeField] private GameObject quarantinePosition;
    [SerializeField] private GameObject character;

    [SerializeField] private Vector3 quarantineVectorPosition;
    [SerializeField] private Vector3 characterVectorPosition;

    [SerializeField] private GameObject playFollowCamera;
    [SerializeField] private GameObject canvasMap;
    [SerializeField] private GameObject canvasMap3D;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject health;
    [SerializeField] private GameObject immunity;
    [SerializeField] private GameObject stamina;
    [SerializeField] private GameObject playerMessage;
    [SerializeField] private GameObject canvasTouchZone;
    [SerializeField] private GameObject canvasTouchZoneMoveNew;
    [SerializeField] private GameObject canvasTouchZoneSprint;

    [Header("Watch Advertisement")]
    [SerializeField] private GameObject cameraTimeline;
    [SerializeField] private PlayableDirector cameraTimelineDirector;
    [SerializeField] private GameObject cubeTV;
    [SerializeField] private GameObject remoteButton;
    [SerializeField] private bool isWatching = false;

    [Header("Message")]
    [SerializeField] private Button okayButton;
    [SerializeField] private TextMeshProUGUI okayButtonText;
    [Range(1, 10)]
    [SerializeField] private int okayButtonCountdown = 5;

    [Header("Countdown")]
    [Range(1.0f, 600.0f)]
    [SerializeField] private float timeRemainingSeconds;
    [SerializeField] private TextMeshProUGUI quarantineTimeText;
    [SerializeField] private bool timerIsRunning = false;

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemainingSeconds > 0)
            {
                timeRemainingSeconds -= Time.deltaTime;
                DisplayTime(timeRemainingSeconds);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemainingSeconds = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        quarantineTimeText.text = "Time Remaning: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    private void Start()
    {
        canvasMap.SetActive(false);
        canvasMap3D.SetActive(false);
        health.SetActive(false);
        immunity.SetActive(false);
        stamina.SetActive(false);
        character = GameObject.FindWithTag("Player");
        quarantineVectorPosition = quarantinePosition.transform.position;
        characterVectorPosition = character.transform.position;
        character.transform.position = quarantineVectorPosition;
        character.SetActive(false);
        character.SetActive(true);
        okayButton.GetComponent<Button>().interactable = false;
        StartCoroutine(ChangeQuarantineButtonCount());
    }

    IEnumerator ChangeQuarantineButtonCount()
    {
        while (true)
        {
            okayButtonCountdown--;
            yield return new WaitForSeconds(1);
            okayButtonText.text = "OKAY (" + okayButtonCountdown + ")";

            if (okayButtonCountdown == 0)
            {
                okayButtonText.text = "OKAY";
                okayButton.GetComponent<Button>().interactable = true;
                yield break;
            }
        }
    }

    public void CloseQuarantineMessage()
    {
        playerMessage.SetActive(false);
        settingsButton.SetActive(true);
        playFollowCamera.SetActive(true);
        canvasTouchZone.SetActive(true);
        canvasTouchZoneMoveNew.SetActive(true);
        canvasTouchZoneSprint.SetActive(false);
        timerIsRunning = true;
    }

    public void WatchAds()
    {
        if (!isWatching)
        {
            remoteButton.SetActive(false);
            cameraTimeline.SetActive(true);
            cameraTimelineDirector.stopped += OnPlayableDirectorStopped;
        }
        else
        {
            cubeTV.SetActive(false);
            isWatching = false;
            cameraTimeline.SetActive(false);
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (cameraTimelineDirector == aDirector)
        {
            isWatching = true;
            remoteButton.SetActive(true);
            cubeTV.SetActive(true);
            // Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
        }

    }
}
