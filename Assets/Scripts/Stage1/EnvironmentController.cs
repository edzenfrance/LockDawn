using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private GameObject[] environmentHouse;

    public void EnvironmentEnabler()
    {
        Debug.Log("<color=white>EnvironmentEnabler</color> - Start");
        foreach (GameObject fObj in environmentHouse)
        {
            fObj.SetActive(false);
        }
        SaveManager.GetCurrentStage();
        SaveManager.GetQuarantine();
        if (SaveManager.isQuarantine == 1)
        {
            environmentHouse[0].SetActive(true);
            return;
        }
        if (SaveManager.currentStage == 1)
            environmentHouse[1].SetActive(true);
        if (SaveManager.currentStage == 2)
            environmentHouse[2].SetActive(true);
        if (SaveManager.currentStage == 4)
            environmentHouse[3].SetActive(true);
        if (SaveManager.currentStage == 5)
            environmentHouse[4].SetActive(true);
        Debug.Log("<color=white>EnvironmentEnabler</color> - Done");
    }
}
