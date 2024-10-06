using Unity.VisualScripting;
using UnityEngine;

public class Shield : InteractableObject
{
    [Header("Shield")] 
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float cooldown = 20f;

    private Collider shieldCollision;
    private bool isOnCooldown = false;

    private void Awake()
    {
        shieldCollision = shieldObject.GetComponent<Collider>();
        shieldCollision.isTrigger = true;
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.GameObject());
            shieldObject.SetActive(false);
            isOnCooldown = true;
            Invoke("ShieldCD", cooldown);
        }
    }
    
    private void ShieldCD()
    {
        shieldObject.SetActive(true);
        isOnCooldown = false;
    }
}