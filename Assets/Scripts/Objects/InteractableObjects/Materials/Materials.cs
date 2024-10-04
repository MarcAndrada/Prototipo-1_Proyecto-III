using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : InteractableObject
{
    public override void Interact(PlayerController player)
    {
        transform.SetParent(null);
        transform.GetComponent<Collider>().enabled = true;
        
        if (TryGetComponent(out Rigidbody objectRigidbody))
        {
            objectRigidbody.isKinematic = false;
        }
        
        player.ClearInteractableObject();
        
        // Add live to platform
        
    }
}
