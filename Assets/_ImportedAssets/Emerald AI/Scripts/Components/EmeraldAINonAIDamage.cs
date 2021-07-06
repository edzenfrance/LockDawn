using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI;

namespace EmeraldAI
{
    public class EmeraldAINonAIDamage : MonoBehaviour
    {
        public int Health = 50;
        public List<string> ActiveEffects = new List<string>();
        EmeraldAISystem EmeraldComponent;

        /// <summary>
        /// Manages Non-AI damage with an external script that can be customized as needed.
        /// </summary>
        public void SendNonAIDamage(int DamageAmount, Transform Target, bool CriticalHit = false)
        {
            DefaultDamage(DamageAmount, Target);

            //Creates damage text on the player's position, if enabled.
            CombatTextSystem.Instance.CreateCombatText(DamageAmount, transform.position + new Vector3(0, transform.localScale.y / 2, 0), CriticalHit, false, false);
        }

        void DefaultDamage(int DamageAmount, Transform Target)
        {
            Health -= DamageAmount;

            if (Health <= 0)
            {
                Debug.Log("The Non-AI Target has died.");
                gameObject.layer = 0;
                gameObject.tag = "Untagged";
                EmeraldComponent = Target.GetComponent<EmeraldAISystem>();
                EmeraldComponent.EmeraldDetectionComponent.PreviousTarget = EmeraldComponent.CurrentTarget;
                Invoke("DelaySearchForTarget", 0.75f);             
            }
        }

        void DelaySearchForTarget ()
        {
            EmeraldComponent.EmeraldDetectionComponent.SearchForTarget();
        }
    }
}
