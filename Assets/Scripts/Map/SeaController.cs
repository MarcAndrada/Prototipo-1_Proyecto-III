using UnityEngine;

public class SeaController : MonoBehaviour
{

    [SerializeField]
    private GameObject waterSplashParticles;

    [SerializeField]
    private AudioClip seaLoopClip;
    private AudioSource seaLoopSource;
    [SerializeField]
    private AudioClip fallWaterClip;


    private void OnEnable()
    {
        seaLoopSource = AudioManager.instance.Play2dLoop(seaLoopClip, "Master", 1.2f, 1, 1);
    }

    private void OnDisable()
    {
        AudioManager.instance.StopLoopSound(seaLoopSource);
        seaLoopSource = null;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Module"))
            return;

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().KillPlayer();
        }
        else if (collision.CompareTag("Bullet") || collision.CompareTag("FireBullet") || collision.CompareTag("Weapon"))
        {
            Destroy(collision.gameObject);
        }




        //Instanciar particulas
        Vector3 splashPosition = new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z);
        Instantiate(waterSplashParticles, splashPosition, Quaternion.identity);
        AudioManager.instance.Play2dOneShotSound(fallWaterClip, "Master", 0.8f);
    }
}
