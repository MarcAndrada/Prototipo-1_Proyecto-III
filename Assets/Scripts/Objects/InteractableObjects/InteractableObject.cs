using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Interactable Object")]
    [SerializeField] private InteractableObjectScriptable interactableObjectScriptable;
    [SerializeField] private SelectedVisual selectedObjectVisual;

    private IInteractableObjectParent interactableObjectParent;
    
    public InteractableObjectScriptable GetInteractableObjectScriptable()
    {
        return interactableObjectScriptable;
    }

    public virtual void Interact(PlayerController player)
    {
        
    }

    public virtual void OnPickUp()
    {
        
    }
    
    public void SetInteractableObjectParent(IInteractableObjectParent interactableObjectParent)
    {
        if (this.interactableObjectParent != null)
        {
            this.interactableObjectParent.ClearInteractableObject();
        }

        this.interactableObjectParent = interactableObjectParent;
        
        interactableObjectParent.SetInteractableObject(this);
        
        transform.parent = interactableObjectParent.GetInteractableObjectFollowTransform();
        
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        transform.forward = interactableObjectParent.GetInteractableObjectFollowTransform().forward;
    }

    public IInteractableObjectParent GetInteractableObjectParent()
    {
        return interactableObjectParent;
    }
    public SelectedVisual GetSelectedObjectVisual()
    {
        return selectedObjectVisual;
    }
}
