using UnityEngine;

public class EnemyModule : MonoBehaviour
{
    [SerializeField]
    private GameObject normalModule;
    [SerializeField]
    private GameObject brokenModule;
    [SerializeField]
    private GameObject burnParticles;

    private EnemyManager manager;
    private bool moduleBroke;
    private Vector2Int moduleId;
    private int shipId;

    private void Start()
    {
        normalModule.SetActive(true);
        brokenModule.SetActive(false);
        moduleBroke = false;
    }

    public void BreakModule()
    {
        moduleBroke = true;
        normalModule.SetActive(false);
        brokenModule.SetActive(true);
    }

    public void SetManager(EnemyManager _manager)
    {
        manager = _manager;
    }

    public bool IsModuleBroke()
    {
        return moduleBroke;
    }

    public Vector2Int GetModuleId()
    {
        return moduleId;
    }
    public void SetModuleId(int _x, int _y)
    {
        moduleId = new Vector2Int(_x, _y);
    }

    public int GetShipId()
    {
        return shipId;
    }
    public void SetShipId(int _id)
    {
        shipId = _id;
    }

    public void ActivateFireParticles()
    {
        burnParticles.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) 
        {
            manager.ModuleHited(this);
            collision.gameObject.GetComponent<Bullet>().doExplosion = true;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("FireBullet"))
        {
            manager.ModuleHited(this, true);
            manager.ModuleHited(this, true);
            collision.gameObject.GetComponent<Bullet>().doExplosion = true;
            Destroy(collision.gameObject);
        }

    }
}
