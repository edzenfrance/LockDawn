using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EmeraldAI.CharacterController;

namespace EmeraldAI.Example
{
    /// <summary>
    /// An example health script that the EmeraldAIPlayerDamage script calls.
    /// Various events can be created and used to cause damage to a 3rd party character controllers via the inspector.
    /// You can also edit the EmeraldAIPlayerDamage script directly and add custom functions.
    /// </summary>
    public class EmeraldAIPlayerHealth : MonoBehaviour
    {
        public int CurrentHealth = 100; [Space]
        public UnityEvent DamageEvent;
        public UnityEvent DeathEvent;

        [HideInInspector]
        public int StartingHealth;

        private void Start()
        {
            StartingHealth = CurrentHealth;
        }

        public void DamagePlayer (int DamageAmount)
        {
            CurrentHealth -= DamageAmount;
            DamageEvent.Invoke();

            if (GetComponent<EmeraldAICharacterControllerTopDown>() != null)
            {
                GetComponent<EmeraldAICharacterControllerTopDown>().DamagePlayer(DamageAmount);
            }

            if (CurrentHealth <= 0)
            {
                PlayerDeath();
            }
        }

        public void PlayerDeath ()
        {
            DeathEvent.Invoke();
        }
    }
}
