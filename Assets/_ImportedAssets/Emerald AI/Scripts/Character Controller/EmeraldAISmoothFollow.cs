using UnityEngine;
using System.Collections;

namespace EmeraldAI.CharacterController
{
    public class EmeraldAISmoothFollow : MonoBehaviour
    {
        public Vector3 CameraOffset;
        public Transform target;
        public float smoothTime = 0.3F;
        private Vector3 velocity = Vector3.zero;

        void Update()
        {
            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, target.position + CameraOffset, ref velocity, smoothTime);
        }
    }
}