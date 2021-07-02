using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectorsHallMenu : MonoBehaviour
{
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
