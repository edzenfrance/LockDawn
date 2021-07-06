using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmeraldAI;

namespace EmeraldAI.CharacterController
{
    public class DamageAIByAngle : MonoBehaviour
    {
        public LayerMask AIMask;
        public int DamageAmount = 5;
        public int DamageRange = 6;
        [Range(40, 75)]
        public int DamageAngle = 45;
        public KeyCode DamageButton = KeyCode.Mouse0;

        void Update()
        {
            if (Input.GetKeyDown(DamageButton))
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, DamageRange, AIMask);

                for (int i = 0; i < hitColliders.Length; i++)
                {
                    Transform TempTarget = hitColliders[i].transform;

                    Vector3 targetDir = TempTarget.position - transform.position;
                    float angle = Vector3.Angle(targetDir, transform.forward);

                    if (TempTarget.GetComponent<EmeraldAISystem>() != null && angle <= DamageAngle)
                    {
                        TempTarget.GetComponent<EmeraldAISystem>().Damage(DamageAmount, EmeraldAISystem.TargetType.Player, transform, 500);
                    }
                }
            }
        }
    }
}