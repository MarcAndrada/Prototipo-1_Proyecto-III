using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private InteractableObjectScriptable interactableObjectScriptable;
    [SerializeField] private SelectedObjectVisual selectedObjectVisual;

    private IInteractableObjectParent interactableObjectParent;
    
    public InteractableObjectScriptable GetInteractableObjectScriptable()
    {
        return interactableObjectScriptable;
    }

    public void SetInteractableObjectParent(IInteractableObjectParent interactableObjectParent)
    {
        if (this.interactableObjectParent != null)
        {
            this.interactableObjectParent.ClearInteractableObject();
        }

        this.interactableObjectParent = interactableObjectParent;
        
        if (interactableObjectParent.HasInteractableObject())
        {
            Debug.LogError("Already has an object");
        }
        interactableObjectParent.SetInteractableObject(this);
        
        transform.parent = interactableObjectParent.GetInteractableObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IInteractableObjectParent GetInteractableObjectParent()
    {
        return interactableObjectParent;
    }
    public SelectedObjectVisual GetSelectedObjectVisual()
    {
        return selectedObjectVisual;
    }
}
