using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI
{
    public class LocationBasedDamageArea : MonoBehaviour
    {
        public float DamageMultiplier = 1;

        /// <summary>
        /// Damages an AI's location based damage component and applies a multiplier to the damage receieved. The parameters of this are the same as the EmeraldAISystem Damage function. 
        /// Call this function instead if you want to utilize the Location Based Damage feature. Ensure that the AI you are damaging has a Location Based Damage script applied to where 
        /// the EmeraldAISystem script is and that you have pressed the Get Colliders button on the Location Based Damage component.
        /// </summary>
        public void DamageArea(int DamageAmount, EmeraldAISystem.TargetType? TypeOfTarget = null, Transform AttackerTransform = null, int RagdollForce = 0, bool CriticalHit = false)
        {
            EmeraldAISystem EmeraldComponent = transform.root.GetComponentInChildren<EmeraldAISystem>(); 
            DamageAmount = Mathf.RoundToInt(DamageAmount * DamageMultiplier);
            EmeraldComponent.Damage(DamageAmount, TypeOfTarget, AttackerTransform, RagdollForce, CriticalHit);
        }

        /// <summary>
        /// Creates an impact effect at the ImpactPosition. The Impact Effect is based off of your AI's Hit Effects List (Located under AI's Settings>Combat>Hit Effect).
        /// </summary>
        public void CreateImpactEffect(Vector3 ImpactPosition)
        {
            EmeraldAISystem EmeraldComponent = transform.root.GetComponentInChildren<EmeraldAISystem>();
            if (EmeraldComponent.UseBloodEffectRef == EmeraldAISystem.UseBloodEffect.Yes && !EmeraldComponent.DamageEffectInhibitor && EmeraldComponent.BloodEffectsList.Count > 0)
            {
                GameObject RandomBloodEffect = EmeraldComponent.BloodEffectsList[Random.Range(0, EmeraldComponent.BloodEffectsList.Count)];
                if (RandomBloodEffect != null)
                {
                    GameObject SpawnedBlood = EmeraldAI.Utility.EmeraldAIObjectPool.SpawnEffect(RandomBloodEffect, ImpactPosition, transform.rotation, EmeraldComponent.BloodEffectTimeoutSeconds) as GameObject;
                    EmeraldComponent.LBDImpactPosition = ImpactPosition;
                    SpawnedBlood.transform.SetParent(transform);
                }
            }
        }
    }
}