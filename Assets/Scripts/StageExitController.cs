using UnityEngine;

public class StageExitController : MonoBehaviour
{
    [SerializeField] private SurveyManager surveyManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            surveyManager.ProcessSurvey("Stage 2 Survey");
        }
    }
}
