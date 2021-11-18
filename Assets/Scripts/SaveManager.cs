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
    public static int keyName;
    public static int doorAccess;
    public static int framerateOn;
    public static float cameraDistance;
    public static float lookSensitivity;
    public static int continueGame;

    public static float musicVolume;
    public static float soundVolume;
    public static int musicMute;
    public static int soundMute;

    public static int achievementA;
    public static int achievementB;
    public static int achievementC;
    public static int achievementD;
    public static int achievementE;
    public static int achievementF;

    public static int obtainSkinA;
    public static int obtainSkinB;
    public static int obtainSkinC;


    private void Awake()
    {
        keyCount = PlayerPrefs.GetInt("Key Count");
        currentCoin = PlayerPrefs.GetInt("Coin");
    }

    public static void SetAchievement(int count, int num)
    {
        // 0 = Failed  1 = Show it  2 = Show
        // Achivement 1 = Show Off
        PlayerPrefs.SetInt("Achievement " + count, num);
    }

    public static void GetAchievement()
    {
        // 0 = Failed  1 = Show it  2 = Show
        achievementA = PlayerPrefs.GetInt("Achievement 1");
        achievementB = PlayerPrefs.GetInt("Achievement 2");
        achievementC = PlayerPrefs.GetInt("Achievement 3");
        achievementD = PlayerPrefs.GetInt("Achievement 4");
        achievementE = PlayerPrefs.GetInt("Achievement 5");
    }

    public static void SetCurrentLife(int num)
    {
        PlayerPrefs.SetInt("Life", num);
    }

    public static void GetCurrentLife()
    {
        currentLife = PlayerPrefs.GetInt("Life", 3);
    }

    public static void ObtainMainItem(string mainitem)
    {
        PlayerPrefs.SetInt("Obtain " + mainitem, 1);
    }

    public static void SetRiddle(int num, int set)
    {
        PlayerPrefs.SetInt("Riddle " + num, set);
    }

    public static void SetCurrentStage(int num)
    {
        PlayerPrefs.SetInt("Current Stage", num);
    }

    public static void SetCurrentCharacter(int num)
    {
        PlayerPrefs.SetInt("Current Character", num);
    }

    public static void GetCurrentCharacter()
    {
        currentCharacter = PlayerPrefs.GetInt("Current Character", 0);
    }

    public static void GetCurrentStage()
    {
        currentStage = PlayerPrefs.GetInt("Current Stage", 1);
    }

    public static void SetSpecialSyrup()
    {
        int addsyrup = PlayerPrefs.GetInt("Special Syrup") + 1;
        PlayerPrefs.SetInt("Special Syrup", addsyrup);
    }

    public static void UseSpecialSyrup()
    {
        int usesyrup = PlayerPrefs.GetInt("Special Syrup") - 1;
        PlayerPrefs.SetInt("Special Syrup", usesyrup);
    }

    public static void SetCoin()
    {
        currentCoin = PlayerPrefs.GetInt("Coin") + 10;
        PlayerPrefs.SetInt("Coin", currentCoin);
    }

    public static void GetKeyCount()
    {
        keyCount = PlayerPrefs.GetInt("Key Count", 0);
    }
    public static void SetKeyCount()
    {
        keyCount = PlayerPrefs.GetInt("Key Count") + 1;
        PlayerPrefs.SetInt("Key Count", keyCount);
    }
    public static void ResetKeyCount()
    {
        PlayerPrefs.SetInt("Key Count", 0);
    }

    public static void SetKeyName(string keyname, int num)
    {
        PlayerPrefs.SetInt(keyname, num);
    }

    public static void GetKeyName(string keyname)
    {
        keyName = PlayerPrefs.GetInt(keyname, 0);
    }

    public static void SetQuarantine(int num)
    {
        PlayerPrefs.SetInt("Quarantine", num);
    }
    
    public static void SetDoorAccess(int num)
    {
        PlayerPrefs.SetInt("Door Access", num);
    }

    public static void GetDoorAccess()
    {
        doorAccess = PlayerPrefs.GetInt("Door Access", 1);
    }

    public static void SetShowFramerate(int num)
    {
        PlayerPrefs.SetInt("Framerate", num);
    }

    public static void GetShowFramerate()
    {
        PlayerPrefs.SetInt("Framerate", 0);
    }

    public static void SetCurrrentImmunity(int immunity)
    {
        PlayerPrefs.SetInt("Current Immunity", immunity);
    }

    public static void GetCurrentImmunity()
    {
        currentImmunity = PlayerPrefs.GetInt("Current Immunity", 0);
    }

    public static void SetGameDifficulty(int diff)
    {
        PlayerPrefs.SetInt("Game Difficulty", diff);
    }

    public static void GetGameDifficulty()
    {
        gameDifficulty = PlayerPrefs.GetInt("Game Difficulty", 2);
    }

    public static void GetCameraDistance()
    {
        cameraDistance = PlayerPrefs.GetFloat("Camera Distance", 1.75f);
    }

    public static void GetLookSensitivity()
    {
        lookSensitivity = PlayerPrefs.GetFloat("Look Sensitivity", 60f);
    }

    public static void SetCompleteStage(int stagenumber)
    {
        PlayerPrefs.SetInt("Stage " + stagenumber + " Complete", 1);
    }

    public static void DeleteKeyStage(int num)
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

    public static void NewGamePlayerPrefs()
    {
		PlayerPrefs.DeleteKey("S1 Key A");
		PlayerPrefs.DeleteKey("S1 Key B");
		PlayerPrefs.DeleteKey("S1 Key C");
		PlayerPrefs.DeleteKey("S1 Key D");
		PlayerPrefs.DeleteKey("S1 Key E");
		PlayerPrefs.DeleteKey("S1 Key F");
        PlayerPrefs.DeleteKey("S2 Key A");
        PlayerPrefs.DeleteKey("S2 Key B");
        PlayerPrefs.DeleteKey("S2 Key C");
        PlayerPrefs.DeleteKey("S2 Key D");
        PlayerPrefs.DeleteKey("S2 Key E");
        PlayerPrefs.DeleteKey("S2 Key F");
        PlayerPrefs.DeleteKey("Key Count");
        PlayerPrefs.DeleteKey("Obtain Vitamin");
        PlayerPrefs.DeleteKey("Obtain Alcohol");
        PlayerPrefs.DeleteKey("Obtain Face Mask");
        PlayerPrefs.DeleteKey("Obtain Face Shield");
        PlayerPrefs.DeleteKey("Obtain Vaccine");
        PlayerPrefs.DeleteKey("Special Syrup");
		PlayerPrefs.DeleteKey("Achievement 1");
        PlayerPrefs.DeleteKey("Achievement 2");
        PlayerPrefs.DeleteKey("Achievement 3");
        PlayerPrefs.DeleteKey("Achievement 4");
        PlayerPrefs.DeleteKey("Achievement 5");
        PlayerPrefs.DeleteKey("Achievement 6");
        PlayerPrefs.SetInt("Current Stage", 1);
		PlayerPrefs.SetInt("Current Immunity", 0);
        PlayerPrefs.SetInt("Life", 3);
        PlayerPrefs.SetInt("Coin", 0);
        PlayerPrefs.SetInt("Quarantine", 0);
        PlayerPrefs.SetInt("Continue Game", 0);
    }

    public static void SetContinueGame()
    {
        PlayerPrefs.SetInt("Continue Game", 1);
    }

    public static void GetContinueGame()
    {
        continueGame = PlayerPrefs.GetInt("Continue Game", 0);
    }

    public static void GetObtainSkin()
    {
        obtainSkinA = PlayerPrefs.GetInt("Obtain Skin 0");
        obtainSkinB = PlayerPrefs.GetInt("Obtain Skin 1");
        obtainSkinC = PlayerPrefs.GetInt("Obtain Skin 2");
    }

    public static void SetSoundMute(int num)
    {
        PlayerPrefs.SetInt("Sound Mute", num);
    }

    public static void SetMusicMute(int num)
    {
        PlayerPrefs.SetInt("Music Mute", num);
    }

    public static void SetSoundVolume(float num)
    {
        PlayerPrefs.SetFloat("Sound Volume", num);
    }

    public static void SetMusicVolume(float num)
    {
        PlayerPrefs.SetFloat("Music Volume", num);
    }

    public static void GetSoundMusic()
    {
        soundVolume = PlayerPrefs.GetFloat("Sound Volume", 1);
        musicVolume = PlayerPrefs.GetFloat("Music Volume", 1);
        soundMute = PlayerPrefs.GetInt("Sound Mute", 0);
        musicMute = PlayerPrefs.GetInt("Music Mute", 0);
    }
}
