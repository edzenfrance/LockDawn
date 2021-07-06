using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.ComponentModel;

namespace EmeraldAI
{
    [System.Serializable]
    public class EmeraldAIFactionData : ScriptableObject
    {
        [SerializeField]
        public List<string> FactionNameList = new List<string>();
    }
}