using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Utility
{
    /// <summary>
    /// Can be used to test Emerald AI's Events by debug logging whether or not an Event was triggered correctly. 
    /// To use, attach this script to an AI and call the DebugEventTest function from one of the Emerald AI Events.
    /// </summary>
    public class DebugEvent : MonoBehaviour
    {
        private const string Message = "This is a Debug Log test to confirm this event is working properly.";

        public void DebugEventTest ()
        {
            Debug.Log(Message);
        }
    }
}