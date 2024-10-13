using UnityEngine;

public class EnemyCanon : MonoBehaviour
{
    [field: SerializeField]
    public Transform spawnBulletPos {  get; private set; }

    [SerializeField]
    private GameObject bulletPrefab;

    [HideInInspector]
    public float shootTimeWait;

}
