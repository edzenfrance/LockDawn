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

    [Header("Player")]
    [SerializeField] private GameObject character;
    [SerializeField] private Animator animator;
    [SerializeField] private int currentHP = 100;
    [SerializeField] private TextMeshProUGUI healthCountText;
    [SerializeField] private GameObject healthBarFill;
    [SerializeField] private GameObject virtualTouchZone;

    [Header("NPC AI")]
    [SerializeField] private int damagePerSecond = 1;
    [SerializeField] private int damageTimeInSecond = 20;
    [SerializeField] private GameObject bloodSmearObject;

    [SerializeField] private RuntimeAnimatorController[] animatorControllers;

    private IEnumerator coroutine;
    private IEnumerator playDPS;

    bool achievementsA = true;
    int countDPS;

    void Awake()
    {
        healthBar = GetComponent<Slider>();
        healthBar.value = currentHP;
    }

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        animator = character.GetComponent<Animator>();
    }

    public void ChangeHealthPoint(int dHP)
    {
        Debug.Log("<color=white>HealthBarController</color> - Current Health: <color=red>" + currentHP + "</color>  Damage To Health: " + dHP);
        currentHP += dHP;
        healthCountText.text = ": " + currentHP;
        healthBar.value = currentHP;
        if (currentHP < 100)
        {
            if (achievementsA)
            {
                Debug.Log("Dead");
                PlayerPrefs.SetInt("Achievement Show Off", 1);
                achievementsA = false;
            }
        }
        if (currentHP <= 0)
        {
            PlayerDied();
        }
    }

    public void PlayerDPS(int dPS)
    {
        countDPS = 0;
        playDPS = DamageHealthPerSecond(-damagePerSecond);
        bloodSmearObject.SetActive(true);
        StartCoroutine(playDPS);
    }

    IEnumerator DamageHealthPerSecond(int dPS)
    {
        while (true)
        {
            countDPS++;
            yield return new WaitForSeconds(3);
            Debug.Log("Poison Count: " + countDPS + " Current HP: " + currentHP + " - DPS: " + dPS);
            currentHP += dPS;
            healthCountText.text = ": " + currentHP;
            healthBar.value = currentHP;
            if (currentHP <= 0)
            {
                PlayerDied();
                yield break;
            }
            /*
            if (countDPS == damageTimeInSecond)
            {
                bloodSmearObject.SetActive(false);
                yield break;
            }*/
        }
    }
    void PlayerDied()
    {
        virtualTouchZone.SetActive(false);
        objPauseButton.SetActive(false);
        objInventoryButton.SetActive(false);
        objMapButton.SetActive(false);
        animator.runtimeAnimatorController = animatorControllers[0];
        coroutine = WaitAndDeath(2f);
        healthBarFill.SetActive(false);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndDeath(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.enabled = false;
        deathUI.SetActive(true);
        virtualTouchZone.SetActive(true);
    }

    public void DeathRestart()
    {
        deathUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
