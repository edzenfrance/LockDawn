using UnityEngine;

public class StageExitController : MonoBehaviour
{
    [SerializeField] private SurveyManager surveyManager;

    int num;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string name = gameObject.name;
            string getNum = name.Replace("StageExit", "");
            if (int.TryParse(getNum, out num))
            {
                surveyManager.ProcessSurvey("Stage " + num + " Survey");
                gameObject.SetActive(false);
            }
            else
                Debug.Log("Not a valid int");
        }
    }
}
