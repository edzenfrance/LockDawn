using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject deathUI;

    [Header("Death Disabled")]
    [SerializeField] private GameObject objPauseButton;
    [SerializeField] private GameObject objInventoryButton;
    [SerializeField] private GameObject objMapButton;
    [SerializeField] private GameObject infectedNote;
    [SerializeField] private GameObject stageNumber;
    [SerializeField] private GameObject quarantineSkip;
    [SerializeField] private GameObject taskMission;

    [Header("Player")]
    [SerializeField] private GameObject character;
    [SerializeField] private Animator animator;
    [SerializeField] private int currentHP = 100;
    [SerializeField] private TextMeshProUGUI healthCountText;
    [SerializeField] private GameObject healthBarFill;
    [SerializeField] private GameObject virtualTouchZone;
    [SerializeField] private GameObject mapView;
    [SerializeField] private GameObject playerFollowCamera;
    [SerializeField] private GameObject canvasTouchZone;

    [Header("NPC AI")]
    [SerializeField] private int damagePerSecond = 1;
    [SerializeField] private GameObject bloodSmearObject;

    [Header("Quarantine")]
    [SerializeField] private TextMeshProUGUI quarantineSkipText;
    [SerializeField] private GameObject quarantine;

    [SerializeField] private RuntimeAnimatorController[] animatorControllers;

    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Transform spawnPoint;

    int countDPS;
    int countQuarantineSkip;

    bool achievementsA = true;
    bool isInfected = false;
    bool isAlive = true;
    bool isDead = false;

    void Awake()
    {
        healthBar = GetComponent<Slider>();
        healthBar.value = currentHP;
    }

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        animator = character.GetComponent<Animator>();
        PlayerPrefs.DeleteKey("SkipQuarantine");
    }

    public void ChangeHealthPoint(int dHP)
    {
        if (isAlive)
        {
            Debug.Log("<color=white>HealthBarController</color> - Current Health: <color=red>" + currentHP + "</color>  Damage To Health: " + dHP);
            currentHP += dHP;
            healthCountText.text = ": " + currentHP;
            healthBar.value = currentHP;
            if (!isInfected)
            {
                Debug.Log("Player is now infected");
                countDPS = 0;
                isInfected = true;
                infectedNote.SetActive(true);
                bloodSmearObject.SetActive(true);
                StartCoroutine(DamageHealthPerSecond(-damagePerSecond));
            }
            if (currentHP < 100)
            {
                if (achievementsA)
                {
                    Debug.Log("No Achievement!");
                    PlayerPrefs.SetInt("Achievement: Show Off", 1);
                    achievementsA = false;
                }
            }
            if (currentHP <= 0)
            {
                healthCountText.text = ": 0";
                isAlive = false;
                if (!isDead)
                {
                    Debug.Log("Player died because of hit");
                    PlayerDied();
                }
            }
        }
    }

    private IEnumerator DamageHealthPerSecond(int dPS)
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            countDPS++;
            currentHP += dPS;
            if (currentHP > 0)
            {
                healthCountText.text = ": " + currentHP;
                healthBar.value = currentHP;
            }
            if (currentHP <= 0)
            {
                healthCountText.text = ": 0";
                isAlive = false;
                if (!isDead)
                {
                    Debug.Log("Player died because of infection");
                    PlayerDied();
                    yield break;
                }
                else
                    yield break;
            }
            Debug.Log("Damage Count: " + countDPS + " Current HP: " + currentHP + " - DPS: " + dPS);
        }
    }

    private void PlayerDied()
    {


        isDead = true;
        isInfected = false;
        infectedNote.SetActive(false);
        EnableDisableObjects(false);

        playerFollowCamera.SetActive(false);
        canvasTouchZone.SetActive(false);

        countQuarantineSkip = PlayerPrefs.GetInt("SkipQuarantine");
        countQuarantineSkip += 1;
        quarantineSkipText.text = "Quarantine: (" + countQuarantineSkip + "/3)";
        Debug.Log("Count Quarantine Skip: " + countQuarantineSkip);
        if (countQuarantineSkip == 4)
        {
            quarantine.SetActive(true);
            return;
        }
        PlayerPrefs.SetInt("SkipQuarantine", countQuarantineSkip);
        animator.runtimeAnimatorController = animatorControllers[0];
        StartCoroutine(WaitAndDeath(2f));
    }

    private IEnumerator WaitAndDeath(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // animator.enabled = false;
        deathUI.SetActive(true);
        virtualTouchZone.SetActive(true);
        character.SetActive(false);
        //Destroy(character);
    }

    public void DeathRestart()
    {
        isAlive = true;
        isDead = false;

        infectedNote.SetActive(false);
        deathUI.SetActive(false);

        EnableDisableObjects(true);

        mapView.SetActive(true);
        bloodSmearObject.SetActive(false);
        character.SetActive(true);
        animator.runtimeAnimatorController = animatorControllers[1];

        currentHP = 100;
        healthBar.value = currentHP;
        healthCountText.text = ": " + currentHP;

        mapView.SetActive(false);

        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void EnableDisableObjects(bool setBool)
    {
        virtualTouchZone.SetActive(setBool);
        objPauseButton.SetActive(setBool);
        objInventoryButton.SetActive(setBool);
        objMapButton.SetActive(setBool);
        healthBarFill.SetActive(setBool);
        stageNumber.SetActive(setBool);
        quarantineSkip.SetActive(setBool);
        taskMission.SetActive(setBool);
        playerFollowCamera.SetActive(setBool);
        canvasTouchZone.SetActive(setBool);
    }

    public void ChangeAnimA()
    {
        animator.runtimeAnimatorController = animatorControllers[0];
    }

    public void ChangeAnimB()
    {
        animator.runtimeAnimatorController = animatorControllers[1];
    }
}
