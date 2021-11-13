using UnityEngine;

public class StageExitController : MonoBehaviour
{
    [SerializeField] private SurveyManager surveyManager;

    int num;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            string name = gameObject.name;
            string getNum = name.Replace("StageExit", "");
            if (int.TryParse(getNum, out num))
                surveyManager.ProcessSurvey("Stage " + num + " Survey");
            else
                Debug.Log("Not a valid int");
        }
    }
}
