using UnityEngine;

public class RotatorCoin : MonoBehaviour
{
    public float speed = 100;

    private void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
