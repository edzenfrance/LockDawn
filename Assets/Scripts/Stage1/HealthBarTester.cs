using System.Collections;
using UnityEngine;

public class HealthBarTester : MonoBehaviour
{
    private HealthBarController healthBar;

    void Awake()
    {
        healthBar = GameObject.Find("Health Bar").GetComponent<HealthBarController>();
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            healthBar.changeHP(1);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            healthBar.changeHP(-1);
        }
        */
    }
}
