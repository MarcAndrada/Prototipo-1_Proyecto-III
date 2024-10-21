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
        if (!player.GetInteractableObject())
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
        if (_hintController.isInteracting && isFornitureBroke)
        {
            _hintController.SetProgressBar(repairDuration, currentRepairTime);
            _hintController.UpdateActionType(PlayerHintController.ActionType.HOLDING);
        }
        else if(!_player.HasInteractableObject())
        {
            _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
        }
    }
    
}
