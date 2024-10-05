using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    public enum ModuleState { REPAIRED, DAMAGED, FIXED}

    [SerializeField]
    private float maxHealth;
    private float health;

    [HideInInspector]
    public GameObject starterObjectInModule;

    [SerializeField]
    private GameObject damagedHitZone;
    


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void GetDamage(float _damage)
    {
        health -= _damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        damagedHitZone.SetActive(true);
        Invoke("DisableDamageZone", 2);
        CheckIfModuleBreak();
    }
    private void CheckIfModuleBreak()
    {
        if (health > 0)
            return;
    }

    private void DisableDamageZone()
    {
        damagedHitZone.SetActive(false);
    }

}
