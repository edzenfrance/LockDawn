using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace EmeraldAI.Utility
{
    [InitializeOnLoad]
    public class EmeraldAIDefines
    {
        const string EmeraldAIDefinesString = "EMERALD_AI_PRESENT";

        static EmeraldAIDefines()
        {
            InitializeEmeraldAIDefines();
        }

        static void InitializeEmeraldAIDefines()
        {
            var BTG = EditorUserBuildSettings.selectedBuildTargetGroup;
            string EmeraldAIDef = PlayerSettings.GetScriptingDefineSymbolsForGroup(BTG);

            if (!EmeraldAIDef.Contains(EmeraldAIDefinesString))
            {
                if (string.IsNullOrEmpty(EmeraldAIDef))
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(BTG, EmeraldAIDefinesString);
                }
                else
                {
                    if (EmeraldAIDef[EmeraldAIDef.Length - 1] != ';')
                    {
                        EmeraldAIDef += ';';
                    }

                    EmeraldAIDef += EmeraldAIDefinesString;
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(BTG, EmeraldAIDef);
                }
            }
        }
    }
}
