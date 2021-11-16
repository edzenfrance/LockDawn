using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static int currentCharacter;
    public static int currentLife;
    public static int currentCoin;
    public static int currentImmunity;
    public static int currentStage;
    public static int gameDifficulty;
    public static int keyCount;
    public static int doorAccess;
    public static int framerateOn;
    public static float cameraDistance;
    public static float lookSensitivity;

    public static int ObtainSkinA;
    public static int ObtainSkinB;
    public static int ObtainSkinC;

    private void Awake()
    {
        keyCount = PlayerPrefs.GetInt("Key Count");
        currentCoin = PlayerPrefs.GetInt("Coin");
    }

    public void SetAchievement(int count, int num)
    {
        // 0 = Failed  1 = Show it  2 = Show
        // Achivement 1 = Show Off
        PlayerPrefs.SetInt("Achievement " + count, num);
    }    

    public void SetCurrentLife(int num)
    {
        PlayerPrefs.SetInt("Life", num);
    }

    public void GetCurrentLife()
    {
        currentLife = PlayerPrefs.GetInt("Life", 3);
    }

    public void ObtainMainItemImmunity(string mainItem, int immunity)
    {
        PlayerPrefs.SetInt("Obtain " + mainItem, 1);
        PlayerPrefs.SetInt("Current Immunity", immunity);
    }

    public void SetCurrentStage(int num)
    {
        PlayerPrefs.SetInt("Current Stage", num);
    }

    public void GetCurrentCharacter()
    {
        currentCharacter = PlayerPrefs.GetInt("Current Character", 0);
    }

    public void GetCurrentStage()
    {
        currentStage = PlayerPrefs.GetInt("Current Stage", 1);
    }

    public void SetSpecialSyrup()
    {
        int addsyrup = PlayerPrefs.GetInt("Special Syrup");
        addsyrup += 1;
        PlayerPrefs.SetInt("Special Syrup", addsyrup);
    }

    public void UseSpecialSyrup()
    {
        int addsyrup = PlayerPrefs.GetInt("Special Syrup");
        addsyrup -= 1;
        PlayerPrefs.SetInt("Special Syrup", addsyrup);
    }

    public void SetCoin()
    {
        currentCoin = PlayerPrefs.GetInt("Coin");
        currentCoin += 10;
        PlayerPrefs.SetInt("Coin", currentCoin);
    }

    public void GetKeyCount()
    {
        keyCount = PlayerPrefs.GetInt("Key Count", 0);
    }

    public void SetKeyName(string KeyName)
    {
        PlayerPrefs.SetInt(KeyName, 1);
    }

    public void SetKeyCount()
    {
        keyCount = PlayerPrefs.GetInt("Key Count");
        keyCount += 1;
        PlayerPrefs.SetInt("Key Count", keyCount);
    }

    public void ResetKeyCount()
    {
        PlayerPrefs.SetInt("Key Count", 0);
    }
    
    public void SetDoorAccess(int num)
    {
        PlayerPrefs.SetInt("Door Access", num);
    }

    public void GetDoorAccess()
    {
        doorAccess = PlayerPrefs.GetInt("Door Access", 1);
    }

    public void SetShowFramerate(int num)
    {
        PlayerPrefs.SetInt("Framerate", num);
    }

    public void GetShowFramerate()
    {
        PlayerPrefs.SetInt("Framerate", 0);
    }

    public void GetCurrentImmunity()
    {
        currentImmunity = PlayerPrefs.GetInt("Current Immunity", 0);
    }

    #region Character Selection

    public void SetGameDifficulty(int diff)
    {
        PlayerPrefs.SetInt("Game Difficulty", diff);
    }

    public void GetGameDifficulty()
    {
        gameDifficulty = PlayerPrefs.GetInt("Game Difficulty", 2);
    }

    public void GetCameraDistance()
    {
        cameraDistance = PlayerPrefs.GetFloat("Camera Distance", 1.75f);
    }

    public void GetLookSensitivity()
    {
        lookSensitivity = PlayerPrefs.GetFloat("Look Sensitivity", 60f);
    }

    public void SetCompleteStage(int stagenumber)
    {
        PlayerPrefs.SetInt("Stage " + stagenumber + " Complete", 1);
    }

    public void DeleteKeyStage(int num)
    {
        if (num == 1)
        {
            PlayerPrefs.DeleteKey("S1 Key A");
            PlayerPrefs.DeleteKey("S1 Key B");
            PlayerPrefs.DeleteKey("S1 Key C");
            PlayerPrefs.DeleteKey("S1 Key D");
            PlayerPrefs.DeleteKey("S1 Key E");
            PlayerPrefs.DeleteKey("S1 Key F");
        }
        if (num == 2)
        {
            PlayerPrefs.DeleteKey("S2 Key A");
            PlayerPrefs.DeleteKey("S2 Key B");
            PlayerPrefs.DeleteKey("S2 Key C");
            PlayerPrefs.DeleteKey("S2 Key D");
            PlayerPrefs.DeleteKey("S2 Key E");
            PlayerPrefs.DeleteKey("S2 Key F");
        }
        if (num == 3)
        {
            PlayerPrefs.DeleteKey("S1 Key A");
            PlayerPrefs.DeleteKey("S1 Key B");
            PlayerPrefs.DeleteKey("S1 Key C");
            PlayerPrefs.DeleteKey("S1 Key D");
            PlayerPrefs.DeleteKey("S1 Key E");
            PlayerPrefs.DeleteKey("S1 Key F");
        }
        if (num == 4)
        {
            PlayerPrefs.DeleteKey("S1 Key A");
            PlayerPrefs.DeleteKey("S1 Key B");
            PlayerPrefs.DeleteKey("S1 Key C");
            PlayerPrefs.DeleteKey("S1 Key D");
            PlayerPrefs.DeleteKey("S1 Key E");
            PlayerPrefs.DeleteKey("S1 Key F");
        }
        if (num == 5)
        {
            PlayerPrefs.DeleteKey("S1 Key A");
            PlayerPrefs.DeleteKey("S1 Key B");
            PlayerPrefs.DeleteKey("S1 Key C");
            PlayerPrefs.DeleteKey("S1 Key D");
            PlayerPrefs.DeleteKey("S1 Key E");
            PlayerPrefs.DeleteKey("S1 Key F");
        }
    }

    public void NewGamePlayerPrefs()
    {
		PlayerPrefs.DeleteKey("Key A");
		PlayerPrefs.DeleteKey("Key B");
		PlayerPrefs.DeleteKey("Key C");
		PlayerPrefs.DeleteKey("Key D");
		PlayerPrefs.DeleteKey("Key E");
		PlayerPrefs.DeleteKey("Key F");
		PlayerPrefs.DeleteKey("Vitamin");
		PlayerPrefs.DeleteKey("Special Syrup");
		PlayerPrefs.DeleteKey("Key Count");
		PlayerPrefs.DeleteKey("Coin");
		PlayerPrefs.DeleteKey("Achievement 1");
		PlayerPrefs.SetInt("Current Stage", 1);
		PlayerPrefs.SetInt("Current Immunity", 0);
   }

    public void GetObtainSkin()
    {
        ObtainSkinA = PlayerPrefs.GetInt("Obtain Skin 0");
        ObtainSkinB = PlayerPrefs.GetInt("Obtain Skin 1");
        ObtainSkinC = PlayerPrefs.GetInt("Obtain Skin 2");
    }

    #endregion
}
