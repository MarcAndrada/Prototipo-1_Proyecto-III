using UnityEngine;

public class EnemyCanon : MonoBehaviour
{
    public enum CannonState { LOADING, SHOOTING, BREAK };

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
    public Vector2Int shipTargetPosId {  get; private set; }

    [HideInInspector]
    public int shipId;

    public GameObject currentBullet {  get; private set; }

    private void Start()
    {
        GetComponentInChildren<Animator>().SetBool("Pick", true);
    }

    
    public void InitializeShootCd()
    {
        shootProcess = Random.Range(0, shootCd / 2);
    }
    public void ShootBullet(Vector3 _shipHitPos, Vector2Int _hitPosId)
    {
        if (currentBullet)
        {
            Destroy(currentBullet);
        }

        currentState = CannonState.SHOOTING;
        currentBullet = Instantiate(bulletPrefab, spawnBulletPos.position, Quaternion.identity);
        shipTargetPos = _shipHitPos;
        shipTargetPosId = _hitPosId;
    }

    public Vector3 BreakCannon()
    {
        currentState = CannonState.BREAK;
        if(currentBullet)
            Destroy(currentBullet);

        return shipTargetPos;
    }
}
