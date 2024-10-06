using UnityEngine;

public class BrokenModule : BaseFurniture
{
    [SerializeField]
    private InteractableObjectScriptable acceptedObject;
    private Module currentModule;

    private void Awake()
    {
        currentModule = GetComponentInParent<Module>();    
    }

    public override void Interact(PlayerController player)
    {
        if (player.GetInteractableObject() && acceptedObject == player.GetInteractableObject().GetInteractableObjectScriptable())
        {
            currentModule.RepairModule();
            Destroy(player.GetInteractableObject().gameObject);
            player.ClearInteractableObject();
        }
    }

    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {
        if (_player.GetInteractableObject() && acceptedObject == _player.GetInteractableObject().GetInteractableObjectScriptable())
        {
            _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
        }
    }
}
