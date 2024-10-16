using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector3 direction;
    private Rigidbody rb;

    [SerializeField]
    private GameObject explosionParticles;
    [HideInInspector]
    public bool doExplosion;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
        rb.velocity = (direction * speed);
    }


    private void OnDestroy()
    {
        if(doExplosion)
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
    }
}