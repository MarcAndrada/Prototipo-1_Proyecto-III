using UnityEngine;

public class BrokenModule : BaseFurniture
{
    [SerializeField] private InteractableObjectScriptable acceptedObject;
    [HideInInspector]
    public ModulesManager manager;
    public override void FinishRepair(PlayerController player)
    {
        base.FinishRepair(player);
        manager.health++;
    }
    protected override void InteractFixedForniture(PlayerController player)
    {
        //Este objeto como solo podremos interactuar con el cuando este roto no hace falta definir esta funcion
    }

    protected override void InteractBrokenForniture(PlayerController player)
    {
        if (player.GetInteractableObject() && acceptedObject == player.GetInteractableObject().GetInteractableObjectScriptable())
        {
            player.SetCanMove(false);
            ProgressBarManager.instance.AddPlayer(player, this);
            player.hintController.isInteracting = true;
            repairAudioSource = AudioManager.instance.Play2dLoop(repairClip, "Master", 0.7f, 0.95f, 1.05f);
        }

        ShowNeededInputHint(player, player.GetPlayerHintController());
    }
    
    public override void Release(PlayerController player)
    {
        ProgressBarManager.instance.RemovePlayer(player, this);
        player.hintController.isInteracting = false;
        player.SetCanMove(true);
        
        ShowNeededInputHint(player, player.GetPlayerHintController());
        
        AudioManager.instance.StopLoopSound(repairAudioSource);
        repairAudioSource = null;
    }

    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {
        if (_hintController.isInteracting)
        {
            _hintController.SetProgressBar(repairDuration, currentRepairTime);
            _hintController.UpdateActionType(PlayerHintController.ActionType.HOLDING);
        }
        else if (_player.GetInteractableObject() && acceptedObject == _player.GetInteractableObject().GetInteractableObjectScriptable())
        {
            _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
        }
        else
        {
            _hintController.UpdateActionType(PlayerHintController.ActionType.NONE);
        }
    }
}
