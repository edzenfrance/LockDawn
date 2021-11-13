using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImmunityController : MonoBehaviour
{
    public Slider immunityBar;
    public GameObject immunityFill;
    public int currentImmunity;
    void Awake()
    {
        Debug.Log("<color=white>ChangeImmunity</color> - Zero");
        immunityBar = GetComponent<Slider>();
        currentImmunity = PlayerPrefs.GetInt("Current Immunity", 0);

    }
    void Start()
    {
        immunityFill.SetActive(false);
        immunityBar.value = currentImmunity;
    }

    public void CheckImmunity()
    {
        currentImmunity = PlayerPrefs.GetInt("Current Immunity", 0);
        immunityBar.value = currentImmunity;
    }
}
