using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Utility
{
    public class EmeraldAIProjectileTimeout : MonoBehaviour
    {
        public float TimeoutSeconds = 3;
        float Timer;

        void Update()
        {
            Timer += Time.deltaTime;
            if (Timer >= TimeoutSeconds)
            {
                EmeraldAIObjectPool.Despawn(gameObject);
            }
        }

        void OnDisable()
        {
            Timer = 0;
        }
    }
}