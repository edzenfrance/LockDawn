using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Utility
{
    public class VisibilityCheck : MonoBehaviour
    {
        public EmeraldAISystem EmeraldComponent;
        bool SystemActivated = false;

        void Start()
        {
            EmeraldComponent.Deactivate();
            Invoke("InitializeDelay", 1);
        }

        void InitializeDelay()
        {
            SystemActivated = true;
        }

        void OnBecameInvisible()
        {
            EmeraldComponent.Deactivate();
        }

        void OnWillRenderObject()
        {
            if (EmeraldComponent.OptimizedStateRef == EmeraldAISystem.OptimizedState.Active)
            {
                EmeraldComponent.Activate();
            }
        }

        void OnBecameVisible()
        {
            if (SystemActivated)
            {
                EmeraldComponent.Activate();
            }
        }
    }
}
