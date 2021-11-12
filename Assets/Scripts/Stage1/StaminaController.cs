using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using StarterAssets;

public class StaminaController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{

    [Range(0.1f, 5.0f)]
    public float stamina = 2.5f;
    private float maximumStamina;
    [Range(2.0f, 8.0f)]
    public int regenStamina = 4;
    public bool exhausted = false;

    public GameObject character;
    public StarterAssetsInputs starterAssetsInputs;
    public Button touchZoneSprintButton;

    [SerializeField] private Slider staminaBar;
    [SerializeField] private GameObject staminaBarFill;

    [Header("Output")]
    public UnityEvent<bool> buttonStateOutputEvent;
    public UnityEvent buttonClickOutputEvent;

    public bool enableDebugLog = false;

    void Start()
    {
        staminaBar.maxValue = stamina;
        maximumStamina = stamina;
        staminaBar.value = stamina;
        touchZoneSprintButton.interactable = false;
        character = GameObject.FindGameObjectWithTag("Player");
        starterAssetsInputs = character.GetComponent<StarterAssetsInputs>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (touchZoneSprintButton.interactable)
        {
            if (!exhausted)
            {
                OutputButtonStateValue(true);
                StopAllCoroutines();
                StartCoroutine(DepleteStamina());
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OutputButtonStateValue(false);
        StopAllCoroutines();
        if (stamina < maximumStamina)
            StartCoroutine(ReplenishStamina());
    }

    private IEnumerator ReplenishStamina()
    {
        staminaBarFill.SetActive(true);
        while (stamina < maximumStamina)
        {
            stamina += Time.deltaTime/regenStamina;
            staminaBar.value = stamina;
            if (enableDebugLog) Debug.Log("Replenishing stamina: " + stamina);
            yield return null;
        }
        exhausted = false;
        starterAssetsInputs.getMoveInput();
        if (enableDebugLog) Debug.Log("New Character is replenish! ");
    }

    private IEnumerator DepleteStamina()
    {
        while (stamina > 0f)
        {
            stamina -= Time.deltaTime;
            if (stamina < 0f)
            {
                staminaBarFill.SetActive(false);
            }
            staminaBar.value = stamina;
            if (enableDebugLog) Debug.Log("Depleting stamina: " + stamina);
            yield return null;
        }
        touchZoneSprintButton.interactable = false;
        starterAssetsInputs.SprintInput(false);
        exhausted = true;
        yield return new WaitForSeconds(0.5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OutputButtonClickEvent();
    }

    void OutputButtonStateValue(bool buttonState)
    {
        buttonStateOutputEvent.Invoke(buttonState);
    }

    void OutputButtonClickEvent()
    {
        buttonClickOutputEvent.Invoke();
    }


}