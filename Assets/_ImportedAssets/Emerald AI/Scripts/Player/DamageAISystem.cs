using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI;

namespace EmeraldAI.CharacterController
{
    public class DamageAISystem : MonoBehaviour
    {
        public float AttackDistance = 4;
        public float AttackTime = 1;
        public int DamageAmountMin = 5;
        public int DamageAmountMax = 10;

        public enum PlayerType { FirstPerson, _3rdPerson };
        public PlayerType m_PlayerType = PlayerType.FirstPerson;

        public GameObject PlayerObject;
        public Camera PlayerCamera;

        Ray ray;
        RaycastHit hit;
        float m_Timer;
        Color LineColor = Color.green;

        void Update()
        {
            m_Timer += Time.deltaTime;

            //First Person
            if (m_PlayerType == PlayerType.FirstPerson)
            {
                Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.forward * 4, LineColor);
                //Only allow an attack to be triggered once per AttackTime interval
                if (m_Timer >= AttackTime)
                {
                    if (Input.GetMouseButton(0))
                    {
                        ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

                        //Fire a ray cast from the center of the assigned camera
                        if (Physics.Raycast(ray, out hit, AttackDistance))
                        {
                            //If a collider with an Emerald AI object is hit, reset the timer and apply damage to the hit AI
                            if (hit.collider != null && hit.collider.gameObject.GetComponent<EmeraldAISystem>() != null)
                            {
                                int DamageAmount = Random.Range(DamageAmountMin, DamageAmountMax + 1);
                                CombatTextSystem.Instance.CreateCombatText(DamageAmount, hit.transform.position - Vector3.up * 0.25f, false, false, false);
                                hit.collider.gameObject.GetComponent<EmeraldAISystem>().Damage(DamageAmount, EmeraldAISystem.TargetType.Player, transform.root, 400);
                                m_Timer = 0;
                            }
                        }
                    }
                }
            }

            //3rd Person
            if (m_PlayerType == PlayerType._3rdPerson)
            {
                Debug.DrawRay(PlayerObject.transform.position + new Vector3(0, PlayerObject.transform.localScale.y / 2, 0), PlayerObject.transform.forward * 4, LineColor);
                //Only allow an attack to be triggered once per AttackTime interval
                if (m_Timer >= AttackTime)
                {
                    if (Input.GetMouseButton(0))
                    {
                        //Fire a ray cast from the center of the assigned player
                        if (Physics.Raycast(PlayerObject.transform.position + new Vector3(0, PlayerObject.transform.localScale.y / 2, 0), PlayerObject.transform.forward, out hit, AttackDistance))
                        {
                            //If a collider with an Emerald AI object is hit, reset the timer and apply damage to the hit AI
                            if (hit.collider != null && hit.collider.gameObject.GetComponent<EmeraldAISystem>() != null)
                            {
                                EmeraldAISystem EmeraldComponent = hit.collider.gameObject.GetComponent<EmeraldAISystem>();
                                LineColor = Color.red;
                                int DamageAmount = Random.Range(DamageAmountMin, DamageAmountMax + 1);
                                CombatTextSystem.Instance.CreateCombatText(DamageAmount, EmeraldComponent.HitPointTransform.position, false, false, false);
                                EmeraldComponent.Damage(DamageAmount, EmeraldAISystem.TargetType.Player, transform.root, 400);
                                m_Timer = 0;
                            }
                        }
                        else
                        {
                            LineColor = Color.green;
                        }
                    }
                    else
                    {
                        LineColor = Color.green;
                    }
                }
            }
        }
    }
}