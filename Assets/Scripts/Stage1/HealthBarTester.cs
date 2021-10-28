using System.Collections;
using UnityEngine;

public class HealthBarTester : MonoBehaviour
{
    [SerializeField] private HealthBarController healthBar;

    void Awake()
    {
        healthBar = GameObject.Find("Health Bar").GetComponent<HealthBarController>();
    }

    public void TestHP()
    {
        healthBar.ChangeHealthPoint(-1);
    }
}
