using UnityEngine;

public class Furniture : BaseFurniture
{
    public override void Interact(PlayerController player)
    {
        if (!HasInteractableObject())
        {
            if (player.HasInteractableObject())
            {
                player.GetInteractableObject().SetInteractableObjectParent(this);
            }
        }
        else
        {
            if (!player.HasInteractableObject())
            {
                GetInteractableObject().SetInteractableObjectParent(player);
            }
        }
    }
    public override void Release(PlayerController player)
    {
        
    }
    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {

        if (!HasInteractableObject())
        {
            if (_player.HasInteractableObject())
            {
               _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
            }
        }
        else
        {
            if (!_player.HasInteractableObject())
            {
                _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
            }
        }
    }
}
