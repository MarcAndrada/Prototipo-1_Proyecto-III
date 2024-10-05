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

    public override void NeededInputHint()
    {
        throw new System.NotImplementedException();
    }
}
