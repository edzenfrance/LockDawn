using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Example
{
    /// <summary>
    /// An example script that gets an AI with a raycast then moves said AI to the position of the mouse on the terrain.
    /// </summary>
    public class MoveToMousePosition : MonoBehaviour
    {
        //public List<string> FactionName = new List<string>();
        public int ReceivedFaction = 0;
        Camera CameraComponent;
        EmeraldAISystem EmeraldComponent;
        Vector3 MovePosition;
        GameObject ArrowObject;
        GameObject PositionObject;

        void Start()
        {
            CameraComponent = GetComponent<Camera>();
            ArrowObject = GameObject.Find("Arrow Object");
            ArrowObject.SetActive(false);
            PositionObject = GameObject.Find("Position Object");
            PositionObject.SetActive(false);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray;
                RaycastHit hit;
                ray = CameraComponent.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 45))
                {
                    if (hit.collider.GetComponent<EmeraldAISystem>() != null)
                    {
                        if (hit.collider.GetComponent<EmeraldAISystem>() != null)
                        {
                            EmeraldComponent = hit.collider.GetComponent<EmeraldAISystem>();
                            ArrowObject.SetActive(true);
                        }  
                    }

                    if (EmeraldComponent != null)
                    {
                        if (hit.collider.GetComponent<EmeraldAISystem>() == null)
                        {
                            EmeraldComponent.EmeraldEventsManagerComponent.SetDestinationPosition(hit.point);
                        }

                        if (hit.collider.GetComponent<EmeraldAISystem>() == null)
                        {
                            PositionObject.transform.position = hit.point;
                            PositionObject.SetActive(true);
                        }                        
                    }
                    else
                    {
                        ArrowObject.SetActive(false);
                        PositionObject.SetActive(false);
                    }
                }
            }

            if (EmeraldComponent != null)
            {
                ArrowObject.transform.position = EmeraldComponent.transform.position+new Vector3(0,3.5f,0);

                if (EmeraldComponent.CurrentHealth <= 0)
                {
                    EmeraldComponent = null;
                    ArrowObject.SetActive(false);
                    PositionObject.SetActive(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EmeraldComponent = null;
                ArrowObject.SetActive(false);
                PositionObject.SetActive(false);
            }
        }
    }
}