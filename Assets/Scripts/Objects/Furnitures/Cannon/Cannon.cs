using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : BaseFurniture
{
    [SerializeField] private InteractableObjectScriptable acceptedObject;
    private bool hasBullet;

    private void Awake()
    {
        hasBullet = false;
    }
    public override void Interact(Player player)
    {

        // Comprobar si el jugador tiene una bala y insertarla dentro del cañon
        if (player.HasInteractableObject())
        {
            if (player.GetInteractableObject().GetInteractableObjectScriptable() == acceptedObject)
            {
                player.GetInteractableObject().SetInteractableObjectParent(this);
                hasBullet = true;
            }
        }
        else
        {
            if (hasBullet)
            {

            }
        }
    }
}
