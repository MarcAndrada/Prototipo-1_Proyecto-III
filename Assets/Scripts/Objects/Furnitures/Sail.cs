using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sail : BaseFurniture
{
    private InteractableObjectScriptable acceptedObject;
    private bool isDisabled;
    
    public override void Interact(PlayerController player)
    {
        if (isDisabled)
        {
            // Not move the raft
        }
        else
        {
            // Move the raft 
        }
    }

    public override void Release(PlayerController player)
    {
        
    }
    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {

    }

    public bool GetIsDisabled()
    {
        return isDisabled;
    }

    public void SetIsDisabled(bool _isDisabled)
    {
        isDisabled = _isDisabled;
    }
}
