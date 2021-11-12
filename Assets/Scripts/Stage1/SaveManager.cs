using UnityEngine;

public class SaveManager : MonoBehaviour
{

    public int characterLife;
    public int characterCoin;
    public int keyCount;

    private void Awake()
    {
        keyCount = PlayerPrefs.GetInt("Key Count");
        characterCoin = PlayerPrefs.GetInt("Coin");
    }

    public void SetFailedAchievementOne()
    {
        Debug.Log("No Achievement!");
        PlayerPrefs.SetInt("Achievement: Show Off", 1);
    }

    public void GetCharacterLife()
    {
        characterLife = PlayerPrefs.GetInt("CharacterLife", 3);
    }

    public void SetVitamins()
    {
        PlayerPrefs.SetInt("Vitamin", 1);
    }

    public void SetSpecialSyrup()
    {
        PlayerPrefs.SetInt("Special Syrup", 1);
    }

    public void UseSpecialSyrup()
    {
        PlayerPrefs.SetInt("Special Syrup", 0);
    }

    public void SetCoin()
    {
        characterCoin = PlayerPrefs.GetInt("Coin");
        characterCoin += 10;
        PlayerPrefs.SetInt("Coin", characterCoin);
    }

    public void SetKey(string KeyName)
    {
        PlayerPrefs.SetInt(KeyName, 1);
        keyCount = PlayerPrefs.GetInt("Key Count");
        keyCount += 1;
        PlayerPrefs.SetInt("Key Count", keyCount);
    }
}
