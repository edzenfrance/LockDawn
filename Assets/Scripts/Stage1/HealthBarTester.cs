using System.Collections;
using UnityEngine;

public class HealthBarTester : MonoBehaviour
{
    [SerializeField] private HealthController healthBar;

    void Awake()
    {
        healthBar = GameObject.Find("Health Bar").GetComponent<HealthController>();
    }

    public void TestHP()
    {
        healthBar.ChangeHealthPoint(-1);
    }
}
