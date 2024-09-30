using UnityEngine;

public class Furniture : BaseFurniture
{
    public override void Interact(Player player)
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
}
