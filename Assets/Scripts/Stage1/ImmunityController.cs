using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImmunityController : MonoBehaviour
{
    [SerializeField] private Slider immunityBar;
    [SerializeField] private GameObject immunityFill;
    [SerializeField] private SaveManager saveManager;
    public int currentImmunity;

    public void CheckImmunity()
    {
        saveManager.GetCurrentImmunity();
        currentImmunity = SaveManager.currentImmunity;
        immunityBar.value = currentImmunity;
    }
}
