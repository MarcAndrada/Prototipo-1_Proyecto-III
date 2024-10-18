using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class BaseWeapon : BaseFurniture
{    
    [Header("Positions"), Space]
    [SerializeField] private Transform bulletSpawner;
    [SerializeField] private Transform pilotPosition;
    
    [Header("Projectiles"), Space]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private InteractableObjectScriptable acceptedObject;
    [SerializeField] private float shootForce = 20f;
    
    [Header("Reload Bullet"), Space]
    protected bool isReloading = false;

    [Header("Projectiles"), Space, SerializeField]
    protected GameObject shootParticles;

    private Transform originalParent;
    private bool hasBullet;
    private bool hasPilot;

    private void Awake()
    {
        hasBullet = false;
    }

    public abstract void Activate(PlayerController player);

    public void EnterPilot(Transform pilot)
    {
        if (!hasPilot)
        {
            hasPilot = true;
            pilot.position = pilotPosition.position;
            pilot.rotation = Quaternion.LookRotation(pilotPosition.forward);
            pilot.GetComponent<PlayerController>().animator.SetBool("OnCannon", true);
        }
    }

    public void ExitPilot()
    {
        if (hasPilot)
        {
            GetComponentInParent<PlayerController>().animator.SetBool("OnCannon", false);
            transform.SetParent(GetOriginalParent());
            GetSelectedFurnitureVisual().SetCanSeeVisuals(true);
            hasPilot = false;
        }
    }
    public void SetHasBullet(bool bullet)
    {
        hasBullet = bullet;
    }

    public override void BreakForniture()
    {
        ExitPilot();
        SetHasBullet(false);
        base.BreakForniture();
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

    protected bool GetHasPilot()
    {
        return hasPilot;
    }
}
