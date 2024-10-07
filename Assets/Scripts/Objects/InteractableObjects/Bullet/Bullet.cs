using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;  // Adjust this value to control the bullet's speed
    private Vector3 direction;

    void Update()
    {
        // Move the bullet forward at a constant speed
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy the bullet on collision
        Destroy(gameObject);
    }
}