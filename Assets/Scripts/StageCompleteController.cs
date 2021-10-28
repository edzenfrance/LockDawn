using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCompleteController : MonoBehaviour
{
    [SerializeField] private GameObject getAchievements;
    [SerializeField] private StageTimer stageTimer;

    int achShowOff;

    void OnEnable()
    {
        stageTimer.OnFinishTimer();
        PlayerPrefs.SetInt("Achievement Show Off", 0);
        Debug.Log("Checking achievements");
        StartCoroutine(CheckAchievements());
    }

    IEnumerator CheckAchievements()
    {
        yield return new WaitForSeconds(2.5f);
        // HealthBarController script
        achShowOff = PlayerPrefs.GetInt("Achievement Show Off");
        if (achShowOff == 0)
        {
            getAchievements.SetActive(true);
        }
    }
}
