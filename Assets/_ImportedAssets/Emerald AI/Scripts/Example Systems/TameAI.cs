using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EmeraldAI.Example
{
    public class TameAI : MonoBehaviour
    {
        public string AITag = "Respawn";
        public int TameDistance = 15;

        private RaycastHit hit;


        void Update()
        {
            Debug.DrawRay(transform.position, transform.forward * 6);
            //Draw a ray foward from our player at a distance according to the TameDistance
            if (Physics.Raycast(transform.position, transform.forward, out hit, TameDistance))
            {
                if (hit.collider.CompareTag(AITag))
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        //Check to see if the object we have hit contains an Emerald AI component
                        if (hit.collider.gameObject.GetComponent<EmeraldAISystem>() != null)
                        {
                            //Get a reference to the Emerald AI object that was hit
                            EmeraldAISystem EmeraldComponent = hit.collider.gameObject.GetComponent<EmeraldAISystem>();

                            //Check to see if our hit Emerald AI's behavior is not aggressive or companion so we can tame it
                            if (EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Aggressive && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion)
                            {
                                //Calls the TameAI function that allows an AI to be tamed.
                                EmeraldComponent.EmeraldEventsManagerComponent.TameAI(transform);
                                //Update the AI's UI colors to blue, as well as update the AI's name, to indicate the AI has been tamed.
                                EmeraldComponent.EmeraldEventsManagerComponent.UpdateUIHealthBarColor(new Color(0.1f, 0.1f, 1, 1));
                                EmeraldComponent.EmeraldEventsManagerComponent.UpdateUINameColor(new Color(0.1f, 0.25f, 1, 1));
                                EmeraldComponent.EmeraldEventsManagerComponent.UpdateUINameText("Tamed " + EmeraldComponent.AIName);
                            }
                        }
                    }
                }
            }
        }
    }
}
