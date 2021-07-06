using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Example
{
    /// <summary>
    /// A script that damages AI based on collisions. Can be used for dynamic damaging objects such as rocks, logs, 
    /// and other falling objects or collision based weapons.
    /// </summary>
    public class DamageAIByCollision : MonoBehaviour
    {
        public int DamageAmount = 10;

        private void OnCollisionEnter(Collision collision)
        {
            //Damages an AI to the collided object
            if (collision.gameObject.GetComponent<EmeraldAI.EmeraldAISystem>() != null)
            {
                collision.gameObject.GetComponent<EmeraldAI.EmeraldAISystem>().Damage(DamageAmount, EmeraldAI.EmeraldAISystem.TargetType.Player, transform, 400);
            }
        }
    }
}