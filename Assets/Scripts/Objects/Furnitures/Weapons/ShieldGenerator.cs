using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : BaseWeapon
{
    private void Awake()
    {
        SetHasBullet(true);
    }
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
    }
    public override void Activate()
    {

    }
}
