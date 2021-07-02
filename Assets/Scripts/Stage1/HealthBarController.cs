using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    private Slider healthBar;
    private int currentHP = 100;

    void Awake()
    {
        healthBar = GetComponent<Slider>();
    }

    void Update()
    {
        healthBar.value = currentHP;
    }

    public void changeHP(int dHP)
    {
        currentHP += dHP;
    }

}
