using UnityEngine;

public abstract class BaseFurniture : MonoBehaviour, IInteractableObjectParent
{
    [Header("Base Furniture")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private SelectedVisual selectedFurnitureVisual;

    private InteractableObject obj;

    [SerializeField]
    protected GameObject baseModel;
    [SerializeField]
    protected GameObject brokenModel;
    protected bool isFornitureBroke;

    public float repairDuration = 5f;

    [HideInInspector]
    public float currentRepairTime;
    [SerializeField]
    protected ParticleSystem repairParticles;

    protected virtual void Start()
    {
        RepairForniture();
    }
    public void Interact(PlayerController player)
    {
        if (!isFornitureBroke)
            InteractFixedForniture(player);
        else
            InteractBrokenForniture(player);
    }
    protected abstract void InteractFixedForniture(PlayerController player);
    protected abstract void InteractBrokenForniture(PlayerController player);
    
    public abstract void Release(PlayerController player);
    public abstract void ShowNeededInputHint(PlayerController _player, PlayerHintController _hintController);
    public virtual void BreakForniture()
    {
        ProgressBarManager.instance.AddFurniture(this);
        
        baseModel.SetActive(false);
        brokenModel.SetActive(true);
        isFornitureBroke = true;
    }
    public virtual void RepairForniture()
    {
        ProgressBarManager.instance.RemoveFurniture(this);
        
        baseModel.SetActive(true);
        brokenModel.SetActive(false);
        isFornitureBroke = false;

        ToggleRepairParticles(false);
        
    }
    public virtual void FinishRepair(PlayerController player)
    {
        if (player.GetInteractableObject() != null)
            Destroy(player.GetInteractableObject().gameObject);
        
        player.ClearInteractableObject();
        
        Release(player);

        //player.hintController.UpdateActionType(PlayerHintController.ActionType.NONE);
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

    public SelectedVisual GetSelectedFurnitureVisual()
    {
        return selectedFurnitureVisual;
    }
    public bool IsBroke()
    {
        return isFornitureBroke;
    }

    public void ToggleRepairParticles(bool _enabled)
    {
        if (_enabled)
            repairParticles.Play(true);
        else
            repairParticles.Stop(true);
    }
}
