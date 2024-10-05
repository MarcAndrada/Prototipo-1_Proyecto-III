using UnityEngine;

public class Furniture : BaseFurniture
{
    public override void Interact(PlayerController player)
    {
        if (!HasInteractableObject())
        {
            if (player.HasInteractableObject())
            {
                player.GetInteractableObject().SetInteractableObjectParent(this);
            }
        }
        else
        {
            if (!player.HasInteractableObject())
            {
                GetInteractableObject().SetInteractableObjectParent(player);
            }
        }
    }

    public override void NeededInputHint()
    {

    }
}
