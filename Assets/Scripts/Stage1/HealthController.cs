using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HealthController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject character;
    [SerializeField] private Animator animator;

    [Header("Health")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private int currentHP = 100;
    [SerializeField] private TextMeshProUGUI healthCountText;
    [SerializeField] private GameObject healthBarFill;


    [Header("Button")]
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private GameObject mapButton;

    [Header("Object")]
    [SerializeField] private GameObject infectedNote;
    [SerializeField] private GameObject stageNumber;
    [SerializeField] private GameObject stageTask;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject note;

    [Header("Script")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private Inventory inventory;

    [Header("Character Control")]
    [SerializeField] private GameObject touchZoneVirtualMove;
    [SerializeField] private GameObject touchZoneCanvas;
    [SerializeField] private GameObject playerFollowCamera;

    [Header("NPC")]
    [SerializeField] private int damagePerSecond = 1;
    [SerializeField] private GameObject bloodSmear;

    [Header("Quarantine")]
    [SerializeField] private GameObject quarantineObject;
    [SerializeField] private GameObject lifeObject;
    [SerializeField] private TextMeshProUGUI lifeText;

    [Header("Animation and Prefab")]
    [SerializeField] private RuntimeAnimatorController[] animatorControllers;
    [SerializeField] private GameObject[] characterPrefabs;

    int countDPS;

    bool achievementsA = true;
    bool isInfected = false;
    bool isAlive = true;
    bool isDead = false;
    bool stopInfection = false;

    void Awake()
    {
        healthBar.value = currentHP;
    }

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        animator = character.GetComponent<Animator>();
        saveManager.SetAchievement(1, 1);
    }

    public void ChangeHealthPoint(int dHP, bool zombieDmg)
    {
        if (isAlive)
        {
            Debug.Log("<color=white>HealthBarController</color> - Current Health: <color=red>" + currentHP + "</color>  Damage To Health: " + dHP);
            currentHP += dHP;
            healthCountText.text = ": " + currentHP;
            healthBar.value = currentHP;
            if (zombieDmg)
            {
                audioManager.PlayAudioZombieAttack();
                if (!isInfected)
                {
                    Debug.Log("<color=white>HealthBarController</color> - Player is now infected");
                    countDPS = 0;
                    isInfected = true;
                    audioManager.PlayAudioHeartBeat();
                    infectedNote.SetActive(true);
                    bloodSmear.SetActive(true);
                    StartCoroutine(DamageHealthPerSecond(-damagePerSecond));
                }
            }
            if (currentHP < 100)
            {
                if (achievementsA)
                {
                    saveManager.SetAchievement(1, 0);
                    achievementsA = false;
                }
            }
            if (currentHP <= 0)
            {
                healthCountText.text = ": 0";
                isAlive = false;
                if (!isDead)
                {
                    Debug.Log("<color=white>HealthBarController</color> - Player died because of hit");
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
            if (stopInfection)
                yield break;
            if (currentHP > 0)
            {
                healthCountText.text = currentHP.ToString();
                healthBar.value = currentHP;
            }
            if (currentHP <= 0)
            {
                healthCountText.text = "0";
                isAlive = false;
                if (!isDead)
                {
                    Debug.Log("<color=white>HealthBarController</color> - Player died because of infection");
                    PlayerDied();
                    yield break;
                }
                else
                    yield break;
            }
            Debug.Log("<color=white>HealthBarController</color> - Damage Count: " + countDPS + " Current HP: " + currentHP + " - DPS: " + dPS);
        }
    }

    private void PlayerDied()
    {
        isDead = true;
        isInfected = false;
        infectedNote.SetActive(false);
        EnableDisableObjects(false);
        saveManager.GetCurrentLife();
        int currentLife = SaveManager.currentLife;
        currentLife -= 1;
        saveManager.SetCurrentLife(currentLife);
        lifeText.text = "Life: " + currentLife;
        Debug.Log("<color=white>HealthBarController</color> - Life: " + currentLife);
        if (currentLife <= 0)
        {
            audioManager.StopAudioLoop();
            quarantineObject.SetActive(true);
            return;
        }
        audioManager.PlayAudioDeadCharacter();
        animator.runtimeAnimatorController = animatorControllers[0];
        StartCoroutine(WaitAndDeath(2f));
    }

    private IEnumerator WaitAndDeath(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // animator.enabled = false;
        deathUI.SetActive(true);
        touchZoneVirtualMove.SetActive(true);
        character.SetActive(false);
    }

    public void RespawnCharacter()
    {
        isAlive = true;
        isDead = false;
        audioManager.StopAudioLoop();
        infectedNote.SetActive(false);
        EnableDisableObjects(true);
        deathUI.SetActive(false);
        bloodSmear.SetActive(false);
        character.SetActive(true);
        animator.runtimeAnimatorController = animatorControllers[1];
        currentHP = 100;
        healthBar.value = currentHP;
        healthCountText.text = currentHP.ToString();
    }

    public void RespawnFromQuarantine()
    {
        infectedNote.SetActive(false);
        bloodSmear.SetActive(false);
        currentHP = 100;
        healthBar.value = currentHP;
        healthCountText.text = currentHP.ToString();
    }

    public void StopInfection()
    {
        if (isInfected)
        {
            stopInfection = true;
            isInfected = false;
            bloodSmear.SetActive(false);
            infectedNote.SetActive(false);
            note.SetActive(true);
            saveManager.UseSpecialSyrup();
            inventory.ReloadInventory();
            audioManager.StopAudioLoop();
            note.GetComponent<TextMeshProUGUI>().text = TextManager.stopInfection;
            StartCoroutine(StopInfectionDone());
        }
        else
        {
            note.SetActive(true);
            note.GetComponent<TextMeshProUGUI>().text = TextManager.notInfected;
            StartCoroutine(StopInfectionDone());
        }
    }

    IEnumerator StopInfectionDone()
    {
        yield return new WaitForSeconds(2);
        note.SetActive(false);
        note.GetComponent<TextMeshProUGUI>().text = "";
    }

    void EnableDisableObjects(bool setBool)
    {
        touchZoneVirtualMove.SetActive(setBool);
        settingsButton.SetActive(setBool);
        inventoryButton.SetActive(setBool);
        mapButton.SetActive(setBool);
        healthBarFill.SetActive(setBool);
        stageNumber.SetActive(setBool);
        stageTask.SetActive(setBool);
        lifeObject.SetActive(setBool);
        playerFollowCamera.SetActive(setBool);
        touchZoneCanvas.SetActive(setBool);
    }

    /*
    public void ChangeAnimA()
    {
        animator.runtimeAnimatorController = animatorControllers[0];
    }

    public void ChangeAnimB()
    {
        animator.runtimeAnimatorController = animatorControllers[1];
    }
    */
}
