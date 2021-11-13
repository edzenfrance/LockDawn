using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_MoveObject : MonoBehaviour
{
    [SerializeField] private GameObject quarantinePosition;
    [SerializeField] private GameObject character;

    [SerializeField] private Vector3 quarantineVectorPosition;
    [SerializeField] private Vector3 characterVectorPosition;

    private void Start()
    {
        character = GameObject.FindWithTag("Player");
        quarantineVectorPosition = quarantinePosition.transform.position;
        characterVectorPosition = character.transform.position;
    }

    public void MoveToQuarantine()
    {
        character.transform.position = quarantineVectorPosition;
        character.SetActive(false);
        character.SetActive(true);
    }
}
