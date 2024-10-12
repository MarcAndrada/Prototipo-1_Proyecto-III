using UnityEngine;

public class CannonBulletContainer : BaseFurniture
{    
    [SerializeField] private InteractableObjectScriptable interactableObjectSO;
    protected override void InteractFixedForniture(PlayerController player)
    {
        if (!player.HasInteractableObject())
        {
            Transform objTransform = Instantiate(interactableObjectSO.prefab, GetInteractableObjectFollowTransform());
            objTransform.GetComponent<InteractableObject>().SetInteractableObjectParent(player);
        }
    }
    protected override void InteractBrokenForniture(PlayerController player)
    {
        RepairForniture();
    }
    public override void Release(PlayerController player)
    {
        
    }
    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {
        if (!_player.HasInteractableObject())
        {
            _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
        }
    }
    
}
