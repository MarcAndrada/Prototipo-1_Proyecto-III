using UnityEngine;

public class Furniture : BaseFurniture
{
    protected override void InteractFixedForniture(PlayerController player)
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
    protected override void InteractBrokenForniture(PlayerController player)
    {
        RepairForniture();
    }
    public override void Release(PlayerController player)
    {
        
    }
    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {
        //Comprobamos si la forniture esta rota y no tenemos ningun item en la mano el boton para interactuar
        if (isFornitureBroke && !_player.HasInteractableObject())
        {
            _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
            return;
        }


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
