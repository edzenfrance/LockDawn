using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class QuarantineController : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject quarantinePosition;
    [SerializeField] private GameObject playFollowCamera;

    [Header("Objects")]
    [SerializeField] private GameObject canvasMap;
    [SerializeField] private GameObject canvasMap3D;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject health;
    [SerializeField] private GameObject immunity;
    [SerializeField] private GameObject stamina;
    [SerializeField] private GameObject stageNumber;
    [SerializeField] private GameObject life;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject stageTask;
    [SerializeField] private GameObject canvasTouchZone;
    [SerializeField] private GameObject canvasTouchZoneMoveNew;
    [SerializeField] private GameObject canvasTouchZoneSprint;

    [Header("Scripts")]
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private HealthController healthController;

    [Header("Watch Advertisement")]
    [SerializeField] private GameObject cameraTimeline;
    [SerializeField] private PlayableDirector cameraTimelineDirector;
    [SerializeField] private GameObject cubeTV;
    [SerializeField] private GameObject remoteButton;
    [SerializeField] private bool isWatching = false;

    [Header("Message")]
    [SerializeField] private GameObject playerMessage;
    [SerializeField] private GameObject quarantineStart;
    [SerializeField] private GameObject quarantineFinish;
    [SerializeField] private GameObject okayButton;
    [SerializeField] private TextMeshProUGUI okayButtonText;
    [Range(1, 10)]
    [SerializeField] private int okayButtonCountdown = 10;

    [Header("Countdown")]
    [Range(1.0f, 1200.0f)]
    [SerializeField] private float timeRemainingSeconds;
    [SerializeField] private TextMeshProUGUI quarantineTimeText;
    [SerializeField] private bool timerIsRunning = false;

    [SerializeField] private Transform[] stageSpawnPoint;

    int timerSpeed = 1;
    bool isQuarantine = true;

    void Awake()
    {
        okayButtonText.text = "OKAY (" + okayButtonCountdown + ")";
    }

    void Start()
    {
        playerMessage.SetActive(true);
        quarantineStart.SetActive(true);

        character = GameObject.FindWithTag("Player");
        character.transform.position = quarantinePosition.transform.position;
        character.SetActive(false);
        character.SetActive(true);

        canvasMap.SetActive(false);
        canvasMap3D.SetActive(false);
        health.SetActive(false);
        immunity.SetActive(false);
        stamina.SetActive(false);
        stageNumber.SetActive(false);
        life.SetActive(false);
        inventory.SetActive(false);
        stageTask.SetActive(false);
        canvasTouchZone.SetActive(false);
        canvasTouchZoneMoveNew.SetActive(false);
        canvasTouchZoneSprint.SetActive(false);

        isQuarantine = true;

        okayButton.GetComponent<Button>().interactable = false;
        StartCoroutine(ChangeQuarantineButtonCount());
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemainingSeconds > 0)
            {
                timeRemainingSeconds -= Time.deltaTime * timerSpeed;
                DisplayTime(timeRemainingSeconds);
            }
            else
            {
                timeRemainingSeconds = 0;
                timerIsRunning = false;
                QuarantineFinish();
            }
        }
    }

    public void CloseQuarantineMessage()
    {
        if (isQuarantine)
        {
            playerMessage.SetActive(false);
            quarantineStart.SetActive(false);
            okayButton.SetActive(false);
            settingsButton.SetActive(true);
            playFollowCamera.SetActive(true);
            remoteButton.SetActive(true);
            canvasTouchZone.SetActive(true);
            canvasTouchZoneMoveNew.SetActive(true);
            timerIsRunning = true;
        }
        else
        {
            saveManager.SetCurrentLife(3);
            saveManager.GetCurrentStage();
            int currentStagePosition = SaveManager.currentStage;
            currentStagePosition -= 1;
            character.transform.position = stageSpawnPoint[currentStagePosition].transform.position;
            character.SetActive(false);
            character.SetActive(true);
            healthController.RespawnCharacter();

            canvasMap.SetActive(true);
            canvasMap3D.SetActive(true);
            health.SetActive(true);
            immunity.SetActive(true);
            stamina.SetActive(true);
            stageNumber.SetActive(true);
            life.SetActive(true);
            inventory.SetActive(true);
            stageTask.SetActive(true);

            okayButton.SetActive(false);

            StartCoroutine(WaitCamera());
        }
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
            timerSpeed = 1;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        quarantineTimeText.text = "Time Remaning: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (cameraTimelineDirector == aDirector)
        {
            isWatching = true;
            remoteButton.SetActive(true);
            cubeTV.SetActive(true);
            timerSpeed = 4;
            // Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
        }
    }

    void QuarantineFinish()
    {
        playerMessage.SetActive(true);
        quarantineFinish.SetActive(true);
        okayButton.SetActive(true);
        remoteButton.SetActive(false);
        cubeTV.SetActive(false);
        isQuarantine = false;
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

    IEnumerator WaitCamera()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        canvasTouchZone.SetActive(true);
        canvasTouchZoneMoveNew.SetActive(true);
        canvasTouchZoneSprint.SetActive(true);
    }
}
