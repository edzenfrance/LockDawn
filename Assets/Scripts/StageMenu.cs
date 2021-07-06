using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMenu : MonoBehaviour
{
    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void BackToPreviousScene()
    {
        SceneManager.LoadScene("MainMenu");
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

}
