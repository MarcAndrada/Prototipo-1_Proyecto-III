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

    [Space, Header("Audio"), SerializeField]
    private AudioClip bulletHitClip;

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
        if (doExplosion)
        {
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            AudioManager.instance.Play2dOneShotSound(bulletHitClip, "Master", 0.3f);
        }
    }
}