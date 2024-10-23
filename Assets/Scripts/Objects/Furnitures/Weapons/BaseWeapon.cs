﻿using UnityEngine;

public abstract class BaseWeapon : BaseFurniture
{    
    [Header("Positions"), Space]
    [SerializeField] private Transform bulletSpawner;
    [SerializeField] private Transform pilotPosition;
    
    [Header("Projectiles"), Space]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private InteractableObjectScriptable acceptedObject;
    [SerializeField] private float shootForce = 20f;
    [SerializeField] protected ParticleSystem projectileParticles;
    
    [Header("Reload Bullet"), Space]
    protected bool isReloading = false;

    [Header("Projectiles"), Space, SerializeField]
    protected GameObject shootParticles;

    [Space, Header("Audio"), SerializeField]
    protected AudioClip cannonShootClip;
    [SerializeField]
    protected AudioClip reloadCannonClip;

    private Transform originalParent;
    private bool hasBullet;
    private bool hasPilot;

    private void Awake()
    {
        hasBullet = false;
        projectileParticles.Stop(true);
    }

    public abstract void Activate(PlayerController player);

    public void EnterPilot(Transform pilot)
    {
        if (!hasPilot)
        {
            hasPilot = true;
            pilot.position = pilotPosition.position;
            pilot.rotation = Quaternion.LookRotation(pilotPosition.forward);
            Animator playerAnim = pilot.GetComponent<PlayerController>().animator;
            playerAnim.SetBool("OnCannon", true);
            playerAnim.SetBool("Pick", true);
        }
    }

    public void ExitPilot()
    {
        if (hasPilot)
        {
            Animator playerAnim = GetComponentInParent<PlayerController>().animator;
            playerAnim.SetBool("OnCannon", false);
            playerAnim.SetBool("Pick", false);

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
