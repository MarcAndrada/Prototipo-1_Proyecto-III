using UnityEngine;

public class BaseWeapon : BaseFurniture
{    
    
    [SerializeField] private Transform bulletSpawner;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private InteractableObjectScriptable acceptedObject;
    [SerializeField] private float shootForce = 20f;

    private Transform originalParent;
    private bool hasBullet;
    private void Awake()
    {
        hasBullet = false;
    }

    public virtual void Shoot()
    {
        
    }

    public void SetHasBullet(bool bullet)
    {
        hasBullet = bullet;
    }

    public void SetOriginalParent(Transform parent)
    {
        originalParent = parent;
    }
    
    public Transform GetBulletSpawner()
    {
        return bulletSpawner;
    }
    public GameObject GetProjectilePrefab()
    {
        return projectilePrefab;
    }
    public InteractableObjectScriptable GetAcceptedObject()
    {
        return acceptedObject;
    }
    public float GetShootForce()
    {
        return shootForce;
    }
    public Transform GetOriginalParent()
    {
        return originalParent;
    }
    public bool GetHasBullet()
    {
        return hasBullet;
    }
}
