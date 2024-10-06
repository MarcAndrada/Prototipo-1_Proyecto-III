using UnityEngine;

public abstract class BaseFurniture : MonoBehaviour, IInteractableObjectParent
{
    [Header("Base Furniture")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private SelectedVisual selectedFurnitureVisual;

    private InteractableObject obj;
    public abstract void Interact(PlayerController player);

    public abstract void Release(PlayerController player);
    public abstract void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController);
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

    public SelectedVisual GetSelectedFurnitureVisual()
    {
        return selectedFurnitureVisual;
    }
}
