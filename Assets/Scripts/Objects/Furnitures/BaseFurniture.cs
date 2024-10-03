using UnityEngine;

public class BaseFurniture : MonoBehaviour, IInteractableObjectParent
{
    [Header("Base Furniture")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private SelectedFurnitureVisual selectedFurnitureVisual;

    private InteractableObject obj;
    public virtual void Interact(PlayerController player)
    {
        
    }
    public Transform GetInteractableObjectFollowTransform()
    {
        return spawnPoint;
    }
    public void SetInteractableObject(InteractableObject interactableObject)
    {
        obj = interactableObject;
    }

    public InteractableObject GetInteractableObject()
    {
        return obj;
    }

    public void ClearInteractableObject()
    {
        obj = null;
    }

    public bool HasInteractableObject()
    {
        return obj != null;
    }

    public SelectedFurnitureVisual GetSelectedFurnitureVisual()
    {
        return selectedFurnitureVisual;
    }
}
