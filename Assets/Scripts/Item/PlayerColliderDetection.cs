using UnityEngine;

// www.youtube.com/watch?v=39L3GL1ZvFI&t=92s

public class PlayerColliderDetection : MonoBehaviour
{
    // public float speed;
    // Rigidbody rigidBody;

    void Start()
    {
       // rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // float f = Input.GetAxis("Horizontal");
        //rigidBody.velocity = Vector3.up * rigidBody.velocity.y + Vector3.right * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("<color=blue>PlayerColliderDetection</color> - OnTriggerEnter");
    }
}
