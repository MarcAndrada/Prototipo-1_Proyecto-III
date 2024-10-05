using UnityEngine;

public class BaseWeapon : BaseFurniture
{    
    [Header("Positions")]
    [SerializeField] private Transform bulletSpawner;
    [SerializeField] private Transform pilotPosition;
    
    [Header("Projectiles")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private InteractableObjectScriptable acceptedObject;
    [SerializeField] private float shootForce = 20f;
    
    private Transform originalParent;
    private bool hasBullet;
    private bool hasPilot;
    private void Awake()
    {
        hasBullet = false;
    }

    public override void Interact(PlayerController player)
    {

    }

    public virtual void Activate()
    {
        
    }
    public void EnterPilot(Transform pilot)
    {
        if (!hasPilot)
        {
            hasPilot = true;
            pilot.position = pilotPosition.position;
            pilot.rotation = Quaternion.LookRotation(pilotPosition.forward);
        }
    }

    public void ExitPilot()
    {
        if (hasPilot)
        {
            hasPilot = false;
        }
    }
    public void SetHasBullet(bool bullet)
    {
        hasBullet = bullet;
    }

    public void SetOriginalParent(Transform parent)
    {
        originalParent = parent;
    }
    
    protected Transform GetBulletSpawner()
    {
        return bulletSpawner;
    }
    protected GameObject GetProjectilePrefab()
    {
        return projectilePrefab;
    }
    protected InteractableObjectScriptable GetAcceptedObject()
    {
        return acceptedObject;
    }
    protected float GetShootForce()
    {
        return shootForce;
    }
    protected Transform GetOriginalParent()
    {
        return originalParent;
    }
    protected bool GetHasBullet()
    {
        return hasBullet;
    }

    public override void NeededInputHint()
    {
        throw new System.NotImplementedException();
    }
}
