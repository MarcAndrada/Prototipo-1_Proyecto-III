using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBulletContainer : BaseFurniture
{    
    [SerializeField] private InteractableObjectScriptable interactableObjectSO;

    public override void Interact(PlayerController player)
    {
        if (!player.HasInteractableObject())
        {
            Transform objTransform = Instantiate(interactableObjectSO.prefab, GetInteractableObjectFollowTransform());
            objTransform.GetComponent<InteractableObject>().SetInteractableObjectParent(player);
        }
    }
    public override void NeededInputHint()
    {
        PlayerManager.instance.hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
    }
}
