using System;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : InteractableObject
{
    [Header("Shield")] 
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float cooldown = 20f;

    private bool canShieldActivate = false;

    private void Awake()
    {
        canShieldActivate = true;
        shieldObject.SetActive(false);
    }

    public override void OnPickUp()
    {
        if (canShieldActivate)
        {
            shieldObject.SetActive(true);
        }
    }

    public override void Interact(PlayerController player)
    {
        // Dejamos el objeto en el suelo
        transform.SetParent(null);
        transform.GetComponent<Collider>().enabled = true;
        
        if (TryGetComponent(out Rigidbody objectRigidbody))
        {
            objectRigidbody.isKinematic = false;
        }
        
        shieldObject.SetActive(false);
        player.ClearInteractableObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.GameObject());
            shieldObject.SetActive(false);
            canShieldActivate = false;
            Invoke("ShieldCD", cooldown);
        }
    }
    
    private void ShieldCD()
    {
        canShieldActivate = true;
    }
}