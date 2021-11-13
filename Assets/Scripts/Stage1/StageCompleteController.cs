using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCompleteController : MonoBehaviour
{
    [SerializeField] private GameObject getAchievements;
    [SerializeField] private SurveyManager surveyManager;
    [SerializeField] private StageTimer stageTimer;

    int achShowOff;

    void OnEnable()
    {
        stageTimer.OnFinishTimer();
        PlayerPrefs.SetInt("Achievement Show Off", 0);
        // Get the stage number first
        surveyManager.ProcessSurvey("Stage 1 Survey");
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
