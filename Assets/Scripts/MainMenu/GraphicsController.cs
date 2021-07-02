using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsController : MonoBehaviour
{
    public GameObject lowButtonCheck;
    public GameObject lowButtonUncheck;

    public GameObject mediumButtonCheck;
    public GameObject mediumButtonUncheck;

    public GameObject highButtonUncheck;
    public GameObject highButtonCheck;

    public void lowButtonClick()
    {
        lowButtonCheck.SetActive(true);
        lowButtonUncheck.SetActive(false);

        mediumButtonCheck.SetActive(false);
        mediumButtonUncheck.SetActive(true);

        highButtonCheck.SetActive(false);
        highButtonUncheck.SetActive(true);
    }

    public void mediumButtonClick()
    {
        mediumButtonCheck.SetActive(true);
        mediumButtonUncheck.SetActive(false);

        lowButtonCheck.SetActive(false);
        lowButtonUncheck.SetActive(true);

        highButtonCheck.SetActive(false);
        highButtonUncheck.SetActive(true);

    }

    public void highButtonClick()
    {
        highButtonCheck.SetActive(true);
        highButtonUncheck.SetActive(false);

        lowButtonCheck.SetActive(false);
        lowButtonUncheck.SetActive(true);

        mediumButtonCheck.SetActive(false);
        mediumButtonUncheck.SetActive(true);

    }


}
