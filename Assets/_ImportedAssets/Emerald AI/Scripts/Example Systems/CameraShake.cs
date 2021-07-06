using UnityEngine;
using System.Collections;

namespace EmeraldAI
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance;
        public Transform CameraTransform;

        Vector3 m_OriginalPosition;
        Quaternion m_OriginalRotation;

        void Awake()
        {
            Instance = this;
        }

        public void ShakeCamera(float ShakeTime, float ShakeAmount)
        {
            StartCoroutine(CameraShakeSequence(ShakeTime, ShakeAmount));
        }

        IEnumerator CameraShakeSequence(float ShakeTime, float ShakeAmount)
        {
            float t = 0;
            float TransitionIn = 0;
            float TransitionOut = 0;

            m_OriginalRotation = CameraTransform.localRotation;
            m_OriginalPosition = CameraTransform.localPosition;

            while ((t / ShakeTime) < 1)
            {
                t += Time.deltaTime;
                Quaternion m_RandomShakeAmount = m_OriginalRotation * Quaternion.Euler(Random.insideUnitSphere.x * ShakeAmount, Random.insideUnitSphere.y * ShakeAmount, Random.insideUnitSphere.z * ShakeAmount);
                Vector3 m_RandomShakeAmountPos = m_OriginalPosition + new Vector3(0, Random.insideUnitSphere.y * ShakeAmount / 2, 0);

                if ((t / ShakeTime) <= 0.5f)
                {
                    TransitionIn += Time.deltaTime;
                    CameraTransform.localRotation = Quaternion.Lerp(m_OriginalRotation, m_RandomShakeAmount, (TransitionIn / ShakeTime) * 2);
                    CameraTransform.localPosition = Vector3.Lerp(m_OriginalPosition, m_RandomShakeAmountPos, (TransitionIn / ShakeTime) * 2);
                }
                else if ((t / ShakeTime) > 0.5f)
                {
                    TransitionOut += Time.deltaTime;
                    CameraTransform.localRotation = Quaternion.Lerp(m_RandomShakeAmount, m_OriginalRotation, (TransitionOut / ShakeTime) * 2);
                    CameraTransform.localPosition = Vector3.Lerp(m_RandomShakeAmountPos, m_OriginalPosition, (TransitionOut / ShakeTime) * 2);
                }

                yield return null;
            }

            CameraTransform.localRotation = m_OriginalRotation;
            CameraTransform.localPosition = m_OriginalPosition;
        }
    }
}