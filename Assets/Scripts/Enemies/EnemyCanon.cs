using UnityEngine;

public class EnemyCanon : MonoBehaviour
{
    public enum CannonState { LOADING, SHOOTING  };

    [field: SerializeField]
    public Transform spawnBulletPos {  get; private set; }
    public CannonState currentState;

    [SerializeField]
    private GameObject bulletPrefab;

    [HideInInspector]
    public float shootProcess;
    [HideInInspector]
    public float shootCd;
    public Vector3 shipTargetPos {  get; private set; }

    [HideInInspector]
    public int shipId;

    public GameObject currentBullet {  get; private set; }

    public void InitializeShootCd()
    {
        shootProcess = Random.Range(0, shootCd / 2);
    }
    public void ShootBullet(Vector3 _shipHitPos)
    {
        currentState = CannonState.SHOOTING;
        currentBullet = Instantiate(bulletPrefab, spawnBulletPos.position, Quaternion.identity);
        shipTargetPos = _shipHitPos;
    }
}
