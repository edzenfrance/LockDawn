using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Utility
{
    public class VisibilityCheckDelay : MonoBehaviour
    {
        public EmeraldAISystem EmeraldComponent;
        float DeactivateSeconds;
        public enum CurrentBehavior { Passive = 1, Cautious = 2, Companion = 3, Aggresive = 4 };
        bool SystemActivated = false;

        void Start()
        {
            DeactivateSeconds = EmeraldComponent.DeactivateDelay;
            EmeraldComponent.Deactivate();
            Invoke("InitializeDelay", 1);
        }

        void InitializeDelay ()
        {
            SystemActivated = true;
        }

        void OnWillRenderObject()
        {
            if (EmeraldComponent.OptimizedStateRef == EmeraldAISystem.OptimizedState.Active)
            {
                EmeraldComponent.Activate();
            }
        }

        void OnBecameInvisible()
        {
            Invoke("DeactivateDelay", DeactivateSeconds);
        }

        void DeactivateDelay ()
        {
            if (EmeraldComponent.CurrentTarget == null && !EmeraldComponent.ReturningToStartInProgress && EmeraldComponent.BehaviorRef != EmeraldAISystem.CurrentBehavior.Companion)
            {
                EmeraldComponent.Deactivate();
            }
        }

        void OnBecameVisible()
        {
            if (SystemActivated)
            {
                CancelInvoke();
                EmeraldComponent.Activate();
            }
        }
    }
}
