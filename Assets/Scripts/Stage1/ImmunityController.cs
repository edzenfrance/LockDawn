using UnityEngine;
using UnityEngine.UI;

public class ImmunityController : MonoBehaviour
{
    [SerializeField] private Slider immunityBar;
    [SerializeField] private GameObject immunityFill;

    public void CheckImmunity()
    {
        SaveManager.GetCurrentImmunity();
        immunityBar.value = SaveManager.currentImmunity;
    }
}
