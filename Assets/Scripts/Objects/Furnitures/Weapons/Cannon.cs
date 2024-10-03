using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : BaseWeapon
{
    public override void Interact(PlayerController player)
    {
        if (player.HasInteractableObject())
        {
            if (player.GetInteractableObject().GetInteractableObjectScriptable() == GetAcceptedObject() && !GetHasBullet())
            {
                player.GetInteractableObject().SetInteractableObjectParent(this);
                // Animacion de la bala
                Destroy(GetInteractableObject().gameObject);
                SetHasBullet(true);
            }
        }
        else
        {
            if (!player.GetIsAimingCannon())
            {
                EnterPilot(player.transform);
                SetOriginalParent(this.transform.parent);
                this.transform.SetParent(player.GetInteractableObjectFollowTransform());
                player.SetIsAimingCannon(true);
            }
            else if (player.GetIsAimingCannon())
            {
                ExitPilot();
                this.transform.SetParent(GetOriginalParent());
                player.SetIsAimingCannon(false);
            }
        }
    }
    public override void Shoot()
    {
        if (GetHasBullet())
        {
            GameObject projectile = Instantiate(GetProjectilePrefab(), GetBulletSpawner().position + GetBulletSpawner().forward, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().isKinematic = false;
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                projectileRb.AddForce(GetBulletSpawner().forward * GetShootForce(), ForceMode.Impulse);
            }
            SetHasBullet(false);
            SetInteractableObject(null);
        }
    }
}
