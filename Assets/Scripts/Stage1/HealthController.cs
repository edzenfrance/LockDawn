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
    [SerializeField] private float currentHP = 100f;
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

    [Header("Script")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Inventory inventory;
    [SerializeField] private NoteController noteController;

    [Header("Character Control")]
    [SerializeField] private GameObject touchZoneVirtualMove;
    [SerializeField] private GameObject touchZoneCanvas;
    [SerializeField] private GameObject playerFollowCamera;

    [Header("NPC")]
    [Range(1, 3)]
    [SerializeField] private float damageEverySecond = 3f;
    [SerializeField] private GameObject bloodSmear;


    [Header("Quarantine")]
    [SerializeField] private GameObject quarantineObject;
    [SerializeField] private GameObject lifeObject;
    [SerializeField] private TextMeshProUGUI lifeText;

    [Header("Animation and Prefab")]
    [SerializeField] private RuntimeAnimatorController[] animatorControllers;
    [SerializeField] private GameObject[] characterPrefabs;

    bool achievementA = true;
    bool isInfected = false;
    bool isAlive = true;
    bool stopInfection = false;

    void Awake()
    {
        healthBar.value = currentHP;
    }

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        animator = character.GetComponent<Animator>();
        SaveManager.SetAchievement(1, 1);
    }

    public void ChangeHealthPoint(int dmgAmount, bool dmgNpc)
    {
        if (isAlive)
        {
            currentHP += dmgAmount;
            healthCountText.text = currentHP.ToString("f0");
            healthBar.value = currentHP;
            Debug.Log("<color=white>HealthController</color> - Current HP: <color=red>" + currentHP + "</color>  Damage: <color=red>" + dmgAmount + "</color>  <color=green>[Npc]</color>");
            if (dmgNpc)  // Added because of Traps in Stage 2
            {
                audioManager.PlayAudioZombieAttack();
                if (!isInfected)
                    StartCoroutine(CharacterInfected());
            }
            if (achievementA)
            {
                if (currentHP < 100)
                {
                    SaveManager.SetAchievement(1, 0);
                    achievementA = false;
                }
            }
            if (currentHP <= 0)
                CharacterDied();
        }
    }

    IEnumerator CharacterInfected()
    {
        Debug.Log("<color=white>HealthController</color> - <color=red>Player is now infected</color>");
        isInfected = true;
        audioManager.PlayAudioHeartBeat();
        infectedNote.SetActive(true);
        bloodSmear.SetActive(true);
        SaveManager.GetCurrentImmunity();
        float immunityNum = (float)SaveManager.currentImmunity;
        float damagePerSecond = damageEverySecond - ((immunityNum / 100f) * 3f);
        while (true)
        {
            if (currentHP > 0f)
            {
                currentHP -= damagePerSecond;
                healthCountText.text = currentHP.ToString("f0");
                healthBar.value = currentHP;
                Debug.Log("<color=white>HealthController</color> - Current HP: <color=red>" + currentHP + "</color>  Damage: <color=red>" + damagePerSecond + "</color>  <color=green>[Infection]</color>");
                yield return new WaitForSeconds(damageEverySecond);
            }
            if (stopInfection)
            {
                stopInfection = false;
                yield break;
            }
            if (currentHP <= 0)
            {
                CharacterDied();
                yield break;
            }
        }
    }

    private void CharacterDied()
    {
        isAlive = false;
        isInfected = false;
        healthCountText.text = "0";
        infectedNote.SetActive(false);
        ObjectSetActive(false);
        SaveManager.GetCurrentLife();
        int currentLife = SaveManager.currentLife;
        if (currentLife > 0)
            currentLife -= 1;
        SaveManager.SetCurrentLife(currentLife);
        lifeText.text = "Life: " + currentLife;
        Debug.Log("<color=white>HealthController</color> - Life: " + currentLife);
        if (currentLife <= 0)
        {
            audioManager.StopAudioLoop();
            quarantineObject.SetActive(true);
            return;
        }
        audioManager.PlayAudioDeadCharacter();
        animator.runtimeAnimatorController = animatorControllers[0];
        StartCoroutine(CharacterDeath());
    }

    IEnumerator CharacterDeath()
    {
        yield return new WaitForSeconds(2f);
        deathUI.SetActive(true);
        touchZoneVirtualMove.SetActive(true);
        character.SetActive(false);
    }

    public void RespawnCharacter()
    {
        isAlive = true;
        isInfected = false;
        audioManager.StopAudioLoop();
        infectedNote.SetActive(false);
        ObjectSetActive(true);
        deathUI.SetActive(false);
        bloodSmear.SetActive(false);
        character.SetActive(true);
        animator.runtimeAnimatorController = animatorControllers[1];
        currentHP = 100f;
        healthBar.value = currentHP;
        healthCountText.text = currentHP.ToString();
    }

    public void RespawnFromQuarantine()
    {
        infectedNote.SetActive(false);
        bloodSmear.SetActive(false);
        currentHP = 100f;
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
            SaveManager.UseSpecialSyrup();
            inventory.ReloadInventory();
            audioManager.StopAudioLoop();
            noteController.ShowNote(TextManager.stopInfection, 2);
        }
        else
            noteController.ShowNote(TextManager.notInfected, 2);
    }

    void ObjectSetActive(bool setBool)
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
}
