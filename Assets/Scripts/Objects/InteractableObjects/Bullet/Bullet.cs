using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector3 direction;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);
    }

    void Update()
    {
       
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
        rb.velocity = (direction * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}