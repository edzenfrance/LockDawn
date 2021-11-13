using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Temp_StageMenu : MonoBehaviour
{
    public void BackToPreviousScene()
    {
        Debug.Log("Stage Selection: Back");
        SceneManager.LoadScene(PlayerPrefs.GetString("Scene"));
    }

    public void GotoStage1()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void GotoStage2()
    {
        //SceneManager.LoadScene("Stage2");
    }

    public void GotoStage3()
    {
        //SceneManager.LoadScene("Stage3");
    }

    public void GotoStage4()
    {
        //SceneManager.LoadScene("Stage4");
    }

    public void GotoStage5()
    {
        //SceneManager.LoadScene("Stage5");
    }

    public void GotoMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
