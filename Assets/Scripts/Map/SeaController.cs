using UnityEngine;

public class SeaController : MonoBehaviour
{

    [SerializeField]
    private GameObject waterSplashParticles;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Module"))
            return;

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().KillPlayer();
        }

        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
        }




        //Instanciar particulas
        Vector3 splashPosition = new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z);
        Instantiate(waterSplashParticles, splashPosition, Quaternion.identity);

    }
}
