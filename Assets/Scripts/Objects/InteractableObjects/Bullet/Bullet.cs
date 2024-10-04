using UnityEngine;

public class Bullet : InteractableObject
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
    }
}