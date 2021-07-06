using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EmeraldAI
{
    public class CombatTextSystem : MonoBehaviour
    {
        public static CombatTextSystem Instance;
        [HideInInspector]
        public GameObject CombatTextObject;
        public enum AnimationTypeEnum { Bounce, Upwards, OutwardsV1, OutwardsV2, Stationary };
        [HideInInspector]
        public AnimationTypeEnum AnimationType = AnimationTypeEnum.Bounce;
        [HideInInspector]
        public GameObject CombatTextCanvas;
        [HideInInspector]
        public EmeraldAICombatTextData m_EmeraldAICombatTextData;
        Transform m_CombatTextParent;

        private void Awake()
        {
            Instance = this;
        }

        public void Initialize()
        {
            CombatTextObject = (GameObject)Resources.Load("Combat Text") as GameObject;
            m_CombatTextParent = CombatTextCanvas.transform.GetChild(0);
            m_EmeraldAICombatTextData = (EmeraldAICombatTextData)Resources.Load("Combat Text Data") as EmeraldAICombatTextData;
        }

        public void CreateCombatText(int amount, Vector3 TextPosition, bool CriticalHit, bool HealingText, bool PlayerTakingDamage)
        {
            if (m_EmeraldAICombatTextData.CombatTextState == EmeraldAICombatTextData.CombatTextStateEnum.Enabled)
            {
                if (m_EmeraldAICombatTextData.CombatTextTargets == EmeraldAICombatTextData.CombatTextTargetEnum.AIOnly && PlayerTakingDamage)
                {
                    return;
                }

                AnimationType = (AnimationTypeEnum)m_EmeraldAICombatTextData.AnimationType;
                var t = EmeraldAI.Utility.EmeraldAIObjectPool.Spawn(CombatTextObject, TextPosition + Vector3.up, Quaternion.identity);
                t.transform.SetParent(m_CombatTextParent);
                t.transform.position = Vector3.zero;

                Text m_Text = t.GetComponent<Text>();
                m_Text.text = amount.ToString();
                m_Text.font = m_EmeraldAICombatTextData.TextFont;
                m_Text.fontSize = m_EmeraldAICombatTextData.FontSize;

                Outline m_OutLine = t.GetComponent<Outline>();
                if (m_EmeraldAICombatTextData.OutlineEffect == EmeraldAICombatTextData.OutlineEffectEnum.Enabled)
                {
                    m_OutLine.enabled = true;
                }
                else if (m_EmeraldAICombatTextData.OutlineEffect == EmeraldAICombatTextData.OutlineEffectEnum.Disabled)
                {
                    m_OutLine.enabled = false;
                }

                if (AnimationType == AnimationTypeEnum.Bounce)
                {
                    StartCoroutine(AnimateBounceText(m_Text, m_EmeraldAICombatTextData.PlayerTextColor, m_EmeraldAICombatTextData.PlayerCritTextColor,TextPosition, CriticalHit, HealingText, PlayerTakingDamage));
                }
                else if (AnimationType == AnimationTypeEnum.Upwards)
                {
                    StartCoroutine(AnimateUpwardsText(m_Text, m_EmeraldAICombatTextData.PlayerTextColor, m_EmeraldAICombatTextData.PlayerCritTextColor, TextPosition, CriticalHit, HealingText, PlayerTakingDamage));
                }
                else if (AnimationType == AnimationTypeEnum.OutwardsV1 || AnimationType == AnimationTypeEnum.OutwardsV2)
                {
                    StartCoroutine(AnimateOutwardsText(m_Text, m_EmeraldAICombatTextData.PlayerTextColor, m_EmeraldAICombatTextData.PlayerCritTextColor, TextPosition, CriticalHit, HealingText, PlayerTakingDamage));
                }
                else if (AnimationType == AnimationTypeEnum.Stationary)
                {
                    StartCoroutine(AnimateStationaryText(m_Text, m_EmeraldAICombatTextData.PlayerTextColor, m_EmeraldAICombatTextData.PlayerCritTextColor, TextPosition, CriticalHit, HealingText, PlayerTakingDamage));
                }
            }
        }

        public void CreateCombatTextAI(int Amount, Vector3 TextPosition, bool CriticalHit, bool HealingText)
        {
            if (m_EmeraldAICombatTextData.CombatTextState == EmeraldAICombatTextData.CombatTextStateEnum.Enabled)
            {
                AnimationType = (AnimationTypeEnum)m_EmeraldAICombatTextData.AnimationType;
                var t = EmeraldAI.Utility.EmeraldAIObjectPool.Spawn(CombatTextObject, TextPosition + Vector3.up, Quaternion.identity);
                t.transform.SetParent(m_CombatTextParent);
                t.transform.position = Vector3.zero;

                Text m_Text = t.GetComponent<Text>();                
                m_Text.text = Amount.ToString();
                m_Text.font = m_EmeraldAICombatTextData.TextFont;
                m_Text.fontSize = m_EmeraldAICombatTextData.FontSize;

                Outline m_OutLine = t.GetComponent<Outline>();                
                if (m_EmeraldAICombatTextData.OutlineEffect == EmeraldAICombatTextData.OutlineEffectEnum.Enabled)
                {
                    m_OutLine.enabled = true;
                }
                else if (m_EmeraldAICombatTextData.OutlineEffect == EmeraldAICombatTextData.OutlineEffectEnum.Disabled)
                {
                    m_OutLine.enabled = false;
                }

                if (AnimationType == AnimationTypeEnum.Bounce)
                {
                    StartCoroutine(AnimateBounceText(m_Text, m_EmeraldAICombatTextData.AITextColor, m_EmeraldAICombatTextData.AICritTextColor, TextPosition, CriticalHit, HealingText, false));
                }
                else if (AnimationType == AnimationTypeEnum.Upwards)
                {
                    StartCoroutine(AnimateUpwardsText(m_Text, m_EmeraldAICombatTextData.AITextColor, m_EmeraldAICombatTextData.AICritTextColor, TextPosition, CriticalHit, HealingText, false));
                }
                else if (AnimationType == AnimationTypeEnum.OutwardsV1 || AnimationType == AnimationTypeEnum.OutwardsV2)
                {
                    StartCoroutine(AnimateOutwardsText(m_Text, m_EmeraldAICombatTextData.AITextColor, m_EmeraldAICombatTextData.AICritTextColor, TextPosition, CriticalHit, HealingText, false));
                }
                else if (AnimationType == AnimationTypeEnum.Stationary)
                {
                    StartCoroutine(AnimateStationaryText(m_Text, m_EmeraldAICombatTextData.AITextColor, m_EmeraldAICombatTextData.AICritTextColor, TextPosition, CriticalHit, HealingText, false));
                }
            }
        }

        IEnumerator AnimateBounceText(Text m_Text, Color RegularTextColor, Color CritTextColor, Vector3 TargetPosition, bool CriticalHit, bool HealingText, bool PlayerTakingDamage)
        {
            if (CriticalHit)
            {
                m_Text.color = CritTextColor;
            }
            else if (!CriticalHit)
            {
                m_Text.color = RegularTextColor;
            }

            if (HealingText)
            {
                m_Text.color = m_EmeraldAICombatTextData.HealingTextColor;
                m_Text.text = "+" + m_Text.text;
            }

            if (PlayerTakingDamage)
            {
                m_Text.color = m_EmeraldAICombatTextData.PlayerTakeDamageTextColor;
                m_Text.text = "-" + m_Text.text;

                if (CriticalHit)
                {
                    m_Text.color = CritTextColor;
                }
            }

            float t = 0;
            float m_TextFade = 0;
            float RandomXPosition = Random.Range(-0.8f, 0.6f);
            RandomXPosition = Mathf.Round(RandomXPosition * 10f) / 10;
            float AnimateSmaller = 0;
            float AnimateLarger = 0;

            while ((t / 1.5f) < 1)
            {
                t += Time.deltaTime;
                float r = 1.0f - (t / 1);
                Vector3 m_TextPos = TargetPosition + new Vector3(r - RandomXPosition, Mathf.Sin(r * Mathf.PI), 0);
                m_TextPos = Camera.main.WorldToScreenPoint(m_TextPos - Vector3.right + Vector3.up * m_EmeraldAICombatTextData.DefaultHeight / 2);
                m_TextPos.z = 0.0f;

                if (m_EmeraldAICombatTextData.UseAnimateFontSize == EmeraldAICombatTextData.UseAnimateFontSizeEnum.Enabled)
                {
                    if (t <= 0.15f)
                    {
                        AnimateLarger += Time.deltaTime * 8;
                        m_Text.fontSize = (int)Mathf.Lerp((float)m_EmeraldAICombatTextData.FontSize, (float)m_EmeraldAICombatTextData.FontSize + m_EmeraldAICombatTextData.MaxFontSize, AnimateLarger);
                    }
                    else if (t > 0.15f && t <= 0.3f)
                    {
                        AnimateSmaller += Time.deltaTime * 6;
                        m_Text.fontSize = (int)Mathf.Lerp((float)m_EmeraldAICombatTextData.FontSize + m_EmeraldAICombatTextData.MaxFontSize, (float)m_EmeraldAICombatTextData.FontSize, AnimateSmaller);
                    }
                }

                if (t > 0.5f)
                {
                    m_TextFade += Time.deltaTime;
                    m_Text.color = new Color(m_Text.color.r, m_Text.color.g, m_Text.color.b, 1 - (m_TextFade * 2));
                }

                m_Text.transform.position = m_TextPos;

                yield return null;
            }

            EmeraldAI.Utility.EmeraldAIObjectPool.Despawn(m_Text.gameObject);
        }

        IEnumerator AnimateUpwardsText(Text m_Text, Color RegularTextColor, Color CritTextColor, Vector3 TargetPosition, bool CriticalHit, bool HealingText, bool PlayerTakingDamage)
        {
            if (CriticalHit)
            {
                m_Text.color = CritTextColor;
            }
            else if (!CriticalHit)
            {
                m_Text.color = RegularTextColor;
            }

            if (HealingText)
            {
                m_Text.color = m_EmeraldAICombatTextData.HealingTextColor;
                m_Text.text = "+" + m_Text.text;
            }

            if (PlayerTakingDamage)
            {
                m_Text.color = m_EmeraldAICombatTextData.PlayerTakeDamageTextColor;
                m_Text.text = "-" + m_Text.text;

                if (CriticalHit)
                {
                    m_Text.color = CritTextColor;
                }
            }

            float t = 0;
            float m_TextFade = 0;
            float RandomXPosition = Random.Range(-0.8f, 0.9f);
            RandomXPosition = Mathf.Round(RandomXPosition * 10f) / 10;
            float AnimateSmaller = 0;
            float AnimateLarger = 0;

            while ((t / 1.5f) < 1)
            {
                t += Time.deltaTime*0.75f;
                float r = 1.0f - (t / 1);
                Vector3 m_TextPos = TargetPosition + new Vector3(RandomXPosition, -r * m_EmeraldAICombatTextData.DefaultHeight, 0);
                m_TextPos = Camera.main.WorldToScreenPoint(m_TextPos + Vector3.up * m_EmeraldAICombatTextData.DefaultHeight);
                m_TextPos.z = 0.0f;

                if (m_EmeraldAICombatTextData.UseAnimateFontSize == EmeraldAICombatTextData.UseAnimateFontSizeEnum.Enabled)
                {
                    if (t <= 0.15f)
                    {
                        AnimateLarger += Time.deltaTime * 8;
                        m_Text.fontSize = (int)Mathf.Lerp((float)m_EmeraldAICombatTextData.FontSize, (float)m_EmeraldAICombatTextData.FontSize + m_EmeraldAICombatTextData.MaxFontSize, AnimateLarger);
                    }
                    else if (t > 0.15f && t <= 0.3f)
                    {
                        AnimateSmaller += Time.deltaTime * 6;
                        m_Text.fontSize = (int)Mathf.Lerp((float)m_EmeraldAICombatTextData.FontSize + m_EmeraldAICombatTextData.MaxFontSize, (float)m_EmeraldAICombatTextData.FontSize, AnimateSmaller);
                    }
                }

                if (t > 0.5f)
                {
                    m_TextFade += Time.deltaTime;
                    m_Text.color = new Color(m_Text.color.r, m_Text.color.g, m_Text.color.b, 1 - (m_TextFade * 1.5f));
                }

                m_Text.transform.position = m_TextPos;

                yield return null;
            }

            EmeraldAI.Utility.EmeraldAIObjectPool.Despawn(m_Text.gameObject);
        }

        IEnumerator AnimateOutwardsText(Text m_Text, Color RegularTextColor, Color CritTextColor, Vector3 TargetPosition, bool CriticalHit, bool HealingText, bool PlayerTakingDamage)
        {
            if (CriticalHit)
            {
                m_Text.color = CritTextColor;
            }
            else if (!CriticalHit)
            {
                m_Text.color = RegularTextColor;
            }

            if (HealingText)
            {
                m_Text.color = m_EmeraldAICombatTextData.HealingTextColor;
                m_Text.text = "+" + m_Text.text;
            }

            if (PlayerTakingDamage)
            {
                m_Text.color = m_EmeraldAICombatTextData.PlayerTakeDamageTextColor;
                m_Text.text = "-" + m_Text.text;

                if (CriticalHit)
                {
                    m_Text.color = CritTextColor;
                }
            }

            float t = 0;
            float m_TextFade = 0;
            float RandomXPosition = Random.Range(-2f, 2.1f);
            RandomXPosition = Mathf.Round(RandomXPosition * 10f) / 10;
            float RandomYPosition = m_EmeraldAICombatTextData.DefaultHeight;
            RandomYPosition = Mathf.Round(RandomYPosition * 10f) / 10;
            Vector3 m_TextPos = Vector3.zero;
            float r = 1.0f;
            float AnimateSmaller = 0;
            float AnimateLarger = 0;

            while ((t / 1) < 1)
            {
                t += Time.deltaTime/2;

                if (r > 0.5f)
                {
                    r = 1.0f - (t*4 / 1);
                }

                if (AnimationType == AnimationTypeEnum.OutwardsV1)
                {
                    m_TextPos = TargetPosition + new Vector3(Mathf.Lerp(0, RandomXPosition, t), -r * m_EmeraldAICombatTextData.DefaultHeight + Mathf.Sin(t * Mathf.PI), 0);
                }
                else if (AnimationType == AnimationTypeEnum.OutwardsV2)
                {
                    m_TextPos = TargetPosition + new Vector3(Mathf.Lerp(0, RandomXPosition, 1 - r), -r * m_EmeraldAICombatTextData.DefaultHeight + Mathf.Sin(r + RandomYPosition * Mathf.PI), 0);
                }

                m_TextPos = Camera.main.WorldToScreenPoint(m_TextPos+Vector3.up*RandomYPosition);
                m_TextPos.z = 0.0f;
                m_Text.transform.position = m_TextPos;

                if (m_EmeraldAICombatTextData.UseAnimateFontSize == EmeraldAICombatTextData.UseAnimateFontSizeEnum.Enabled)
                {
                    if (t <= 0.15f)
                    {
                        AnimateLarger += Time.deltaTime * 8;
                        m_Text.fontSize = (int)Mathf.Lerp((float)m_EmeraldAICombatTextData.FontSize, (float)m_EmeraldAICombatTextData.FontSize + m_EmeraldAICombatTextData.MaxFontSize, AnimateLarger);
                    }
                    else if (t > 0.15f && t <= 0.3f)
                    {
                        AnimateSmaller += Time.deltaTime * 6;
                        m_Text.fontSize = (int)Mathf.Lerp((float)m_EmeraldAICombatTextData.FontSize + m_EmeraldAICombatTextData.MaxFontSize, (float)m_EmeraldAICombatTextData.FontSize, AnimateSmaller);
                    }
                }

                if (t > 0.5f)
                {
                    m_TextFade += Time.deltaTime;
                    m_Text.color = new Color(m_Text.color.r, m_Text.color.g, m_Text.color.b, 1 - (m_TextFade * 2));
                }

                yield return null;
            }

            EmeraldAI.Utility.EmeraldAIObjectPool.Despawn(m_Text.gameObject);
        }

        IEnumerator AnimateStationaryText(Text m_Text, Color RegularTextColor, Color CritTextColor, Vector3 TargetPosition, bool CriticalHit, bool HealingText, bool PlayerTakingDamage)
        {
            if (CriticalHit)
            {
                m_Text.color = CritTextColor;
            }
            else if (!CriticalHit)
            {
                m_Text.color = RegularTextColor;
            }

            if (HealingText)
            {
                m_Text.color = m_EmeraldAICombatTextData.HealingTextColor;
                m_Text.text = "+" + m_Text.text;
            }

            if (PlayerTakingDamage)
            {
                m_Text.color = m_EmeraldAICombatTextData.PlayerTakeDamageTextColor;
                m_Text.text = "-" + m_Text.text;

                if (CriticalHit)
                {
                    m_Text.color = CritTextColor;
                }
            }

            float t = 0;
            float m_TextFade = 0;
            float RandomXPosition = Random.Range(-1f, 1.1f);
            RandomXPosition = Mathf.Round(RandomXPosition * 10f) / 10;
            float RandomYPosition = m_EmeraldAICombatTextData.DefaultHeight;
            RandomYPosition = Mathf.Round(RandomYPosition * 10f) / 10;
            float AnimateSmaller = 0;
            float AnimateLarger = 0;

            while ((t / 1.5f) < 1)
            {
                t += Time.deltaTime;
                Vector3 m_TextPos = TargetPosition + new Vector3(RandomXPosition, RandomYPosition, 0);
                m_TextPos = Camera.main.WorldToScreenPoint(m_TextPos - Vector3.up * 0.5f);
                m_TextPos.z = 0.0f;

                if (m_EmeraldAICombatTextData.UseAnimateFontSize == EmeraldAICombatTextData.UseAnimateFontSizeEnum.Enabled)
                {
                    if (t <= 0.15f)
                    {
                        AnimateLarger += Time.deltaTime * 8;
                        m_Text.fontSize = (int)Mathf.Lerp((float)m_EmeraldAICombatTextData.FontSize, (float)m_EmeraldAICombatTextData.FontSize + m_EmeraldAICombatTextData.MaxFontSize, AnimateLarger);
                    }
                    else if (t > 0.15f && t <= 0.3f)
                    {
                        AnimateSmaller += Time.deltaTime * 6;
                        m_Text.fontSize = (int)Mathf.Lerp((float)m_EmeraldAICombatTextData.FontSize + m_EmeraldAICombatTextData.MaxFontSize, (float)m_EmeraldAICombatTextData.FontSize, AnimateSmaller);
                    }
                }

                if (t > 0.5f)
                {
                    m_TextFade += Time.deltaTime;
                    m_Text.color = new Color(m_Text.color.r, m_Text.color.g, m_Text.color.b, 1 - (m_TextFade * 2));
                }

                m_Text.transform.position = m_TextPos;

                yield return null;
            }

            EmeraldAI.Utility.EmeraldAIObjectPool.Despawn(m_Text.gameObject);
        }
    }
}