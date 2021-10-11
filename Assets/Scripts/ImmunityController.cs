using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImmunityController : MonoBehaviour
{
    public Slider immunityBar;
    public GameObject immunityFill;
    public int currentImmunity = 0;


    private void Awake()
    {
        Debug.Log("<color=white>ChangeImmunity</color> - Zero");
        immunityBar = GetComponent<Slider>();

    }

    // Start is called before the first frame update
    void Start()
    {
        immunityFill.SetActive(false);
        immunityBar.value = currentImmunity;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void ChangeSlider(float value)
    {

    }
}
