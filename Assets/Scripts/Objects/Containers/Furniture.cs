using UnityEngine;

public class Furniture : MonoBehaviour, IInteractableObjectParent
{
    [SerializeField] private InteractableObjectScriptable interactableObjectSO;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Furniture secondCounter;

    private InteractableObject obj;

    private void Update()
    {
        // Testing 
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (obj != null)
            {
                obj.SetInteractableObjectParent(secondCounter);
            }
        }
    }

    public void Interact(Player player)
    {
        if (obj == null)
        {
            Transform objTransform = Instantiate(interactableObjectSO.prefab, spawnPoint);
            objTransform.GetComponent<InteractableObject>().SetInteractableObjectParent(this);
        }
        else
        {
            obj.SetInteractableObjectParent(player);
        }
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
}
