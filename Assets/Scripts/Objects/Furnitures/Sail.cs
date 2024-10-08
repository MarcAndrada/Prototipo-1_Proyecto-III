using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sail : BaseFurniture
{
    private InteractableObjectScriptable acceptedObject;

    protected override void InteractFixedForniture(PlayerController player)
    {
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
    }

    
}
