using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Utility
{
    public class SummonedAIComponent : MonoBehaviour
    {
        int m_DespawnSeconds = 10;

        public void IntitializeSummon (int DespawnSeconds)
        {
            m_DespawnSeconds = DespawnSeconds;
            StartCoroutine(DespawnSummonedAI());
        }

        IEnumerator DespawnSummonedAI ()
        {
            yield return new WaitForSeconds(m_DespawnSeconds);
            GetComponent<EmeraldAISystem>().EmeraldEventsManagerComponent.KillAI();
        }
    }
}