using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBulletContainer : BaseFurniture
{    
    [SerializeField] private InteractableObjectScriptable interactableObjectSO;

    public override void Interact(Player player)
    {
        if (!player.HasInteractableObject())
        {
            Transform objTransform = Instantiate(interactableObjectSO.prefab, GetInteractableObjectFollowTransform());
            objTransform.GetComponent<InteractableObject>().SetInteractableObjectParent(player);
        }
    }
}
