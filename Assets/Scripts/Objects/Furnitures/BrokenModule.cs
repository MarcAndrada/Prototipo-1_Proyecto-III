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
    private Coroutine repairCoroutine;
    
    private void Awake()
    {
        currentModule = GetComponentInParent<Module>();
        currentRepairTime = 0f;
    }
    public override void Interact(PlayerController player)
    {
        if (player.GetInteractableObject() && acceptedObject == player.GetInteractableObject().GetInteractableObjectScriptable())
        {
            if (!isInteracting)
            {
                currentRepairTime = 0f;
                isInteracting = true;
                player.SetCanMove(false);
                repairCoroutine = StartCoroutine(RepairModuleOverTime(player));
            }
        }
    }
    public override void Release(PlayerController player)
    {
        if (isInteracting)
        {
            isInteracting = false;
            player.SetCanMove(true);

            if (repairCoroutine != null)
            {
                StopCoroutine(repairCoroutine);
                repairCoroutine = null;
            }
        }
    }

    private IEnumerator RepairModuleOverTime(PlayerController player)
    {
        // Reparar el modulo
        while (currentRepairTime < repairDuration)
        {
            currentRepairTime += Time.deltaTime;
            
            // Aqui poner la UI de reparacion con el tiempo
            
            yield return null;
        }
        
        currentModule.RepairModule();
        Destroy(player.GetInteractableObject().gameObject);
        player.ClearInteractableObject();

        isInteracting = false;
        player.SetCanMove(true);
    }
    
    public override void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController)
    {
        if (_player.GetInteractableObject() && acceptedObject == _player.GetInteractableObject().GetInteractableObjectScriptable())
        {
            _hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
        }
    }
}
