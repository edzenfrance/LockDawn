using UnityEngine;

public class MainMenuBGM : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] BGMObject = GameObject.FindGameObjectsWithTag("MainMenuBGM");
        if (BGMObject.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

}
