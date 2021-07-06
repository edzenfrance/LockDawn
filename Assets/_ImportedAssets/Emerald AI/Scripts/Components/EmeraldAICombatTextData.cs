using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI
{
    [System.Serializable]
    public class EmeraldAICombatTextData : ScriptableObject
    {
        public enum CombatTextStateEnum { Enabled, Disabled };
        public CombatTextStateEnum CombatTextState = CombatTextStateEnum.Disabled;
        public Color PlayerTextColor = Color.white;
        public Color PlayerCritTextColor = Color.red;
        public Color PlayerTakeDamageTextColor = Color.red;
        public Color AITextColor = Color.white;
        public Color AICritTextColor = Color.red;
        public Color HealingTextColor = Color.green;      
        public Font TextFont;
        public int FontSize = 20;
        public int MaxFontSize = 6;
        public enum AnimationTypeEnum { Bounce, Upwards, OutwardsV1, OutwardsV2, Stationary };
        public AnimationTypeEnum AnimationType = AnimationTypeEnum.Bounce;
        public enum CombatTextTargetEnum { PlayerAndAI, PlayerOnly, AIOnly};
        public CombatTextTargetEnum CombatTextTargets = CombatTextTargetEnum.PlayerAndAI;
        public enum OutlineEffectEnum { Enabled, Disabled };
        public OutlineEffectEnum OutlineEffect = OutlineEffectEnum.Enabled;
        public enum UseAnimateFontSizeEnum { Enabled, Disabled };
        public UseAnimateFontSizeEnum UseAnimateFontSize = UseAnimateFontSizeEnum.Disabled;
        public float DefaultHeight = 1.75f;
    }
}