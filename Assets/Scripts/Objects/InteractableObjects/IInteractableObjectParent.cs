using UnityEngine;

public interface IInteractableObjectParent
{
    public Transform GetInteractableObjectFollowTransform();
    public void SetInteractableObject(InteractableObject interactableObject);
    public InteractableObject GetInteractableObject();
    public void ClearInteractableObject();
    public bool HasInteractableObject();
}
