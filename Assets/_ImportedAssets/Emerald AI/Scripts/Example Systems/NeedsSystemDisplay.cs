using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EmeraldAI.Example
{
    public class NeedsSystemDisplay : MonoBehaviour
    {
        public Text TextDisplay;
        public EmeraldAINeedsSystem m_EmeraldAINeedsSystem;

        private void Start()
        {
            InvokeRepeating("UpdateTextDisplay", 0.1f, 0.5f);
        }

        void UpdateTextDisplay ()
        {
            TextDisplay.text = "AI's Current Resources: " + m_EmeraldAINeedsSystem.CurrentResourcesLevel.ToString();
        }
    }
}