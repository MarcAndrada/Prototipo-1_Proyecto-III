using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BrokenModule : BaseFurniture
{
    [SerializeField] private InteractableObjectScriptable acceptedObject;
    [SerializeField] private float repairDuration = 5f;
    
    private Module currentModule;
    private bool isInteracting;
    private float currentRepairTime;

    private PlayerController player;
    
    private void Awake()
    {
        currentModule = GetComponentInParent<Module>();
        currentRepairTime = 0f;
    }

    private void Update()
    {
        if (isInteracting)
        {
            currentRepairTime += Time.deltaTime;

            player.GetPlayerHintController().SetProgressBar(repairDuration, currentRepairTime);
            ShowNeededInputHint(player, player.GetPlayerHintController());

            if (currentRepairTime >= repairDuration)
            {
                FinishRepair(player);
            }
        }

    }

    public override void Interact(PlayerController player)
    {
        
        if (player.GetInteractableObject() && acceptedObject == player.GetInteractableObject().GetInteractableObjectScriptable() && !isInteracting)
        {

            this.player = player;

            currentRepairTime = 0f;
            isInteracting = true;
            player.SetCanMove(false);
        }
    }
    public override void Release(PlayerController player)
    {
        if (isInteracting)
        {
            isInteracting = false;
            player.SetCanMove(true);
            ShowNeededInputHint(player, player.GetPlayerHintController());
        }
    }
    private void FinishRepair(PlayerController player)
    {
        currentModule.RepairModule();
        Destroy(player.GetInteractableObject().gameObject);
        player.ClearInteractableObject();

        isInteracting = false;
        player.SetCanMove(true);
    }
    
    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {
        if (isInteracting)
        {
            _hintController.SetProgressBar(repairDuration, currentRepairTime);
            _hintController.UpdateActionType(PlayerHintController.ActionType.HOLDING);
        }
        else if (_player.GetInteractableObject() && acceptedObject == _player.GetInteractableObject().GetInteractableObjectScriptable())
        {
            _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
        }
    }
}
