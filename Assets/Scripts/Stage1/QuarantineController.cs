using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuarantineController : MonoBehaviour
{
    [SerializeField] private GameObject quarantinePosition;
    [SerializeField] private GameObject character;

    [SerializeField] private Vector3 quarantineVectorPosition;
    [SerializeField] private Vector3 characterVectorPosition;

    [SerializeField] private GameObject canvasMap;
    [SerializeField] private GameObject canvasMap3D;
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject health;
    [SerializeField] private GameObject immunity;
    [SerializeField] private GameObject stageNumber;
    [SerializeField] private GameObject quarantineSkip;
    [SerializeField] private GameObject objectives;
    [SerializeField] private GameObject messageToInfected;
    [SerializeField] private GameObject cubeTV;

    [SerializeField] private Button okayButton;
    [SerializeField] private TextMeshProUGUI okayButtonText;

    int messageWaitCount = 10;

    private void Start()
    {
        canvasMap.SetActive(false);
        canvasMap3D.SetActive(false);
        inventoryButton.SetActive(false);
        settingsButton.SetActive(false);
        health.SetActive(false);
        immunity.SetActive(false);
        stageNumber.SetActive(false);
        quarantineSkip.SetActive(false);
        objectives.SetActive(false);
        character = GameObject.FindWithTag("Player");
        quarantineVectorPosition = quarantinePosition.transform.position;
        characterVectorPosition = character.transform.position;
        character.transform.position = quarantineVectorPosition;
        character.SetActive(false);
        character.SetActive(true);
        okayButton.GetComponent<Button>().interactable = false;
        StartCoroutine(RemoveQuarantineText());
    }

    IEnumerator RemoveQuarantineText()
    {
        while (true)
        {
            messageWaitCount--;
            yield return new WaitForSeconds(1);
            okayButtonText.text = "OKAY (" + messageWaitCount + ")";

            if (messageWaitCount == 0)
            {
                okayButtonText.text = "OKAY";
                okayButton.GetComponent<Button>().interactable = true;
                yield break;
            }

        }
    }

    public void ExitQuarantineMessage()
    {
        messageToInfected.SetActive(false);
        settingsButton.SetActive(true);
        cubeTV.SetActive(true);
    }
}
