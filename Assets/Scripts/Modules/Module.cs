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

    [Space, SerializeField]
    private GameObject workingModule;
    [SerializeField]
    private GameObject brokeModule;

    [Space, SerializeField]
    private GameObject damageHitZone;

    private int damageZoneCount;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        workingModule.SetActive(true);
        brokeModule.SetActive(false);
    }

    public void GetDamage(float _damage)
    {
        health -= _damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        CheckIfModuleBreak();
    }
    private void CheckIfModuleBreak()
    {
        if (health > 0)
            return;

        workingModule.SetActive(false);
        brokeModule.SetActive(true);
    }

    public void RepairModule()
    {
        health = maxHealth;
        workingModule.SetActive(true);
        brokeModule.SetActive(false);
    }

    public void AddDamageZone()
    {
        damageZoneCount++;
        damageHitZone.SetActive(true);
    }
    public void RemoveMainDamageZone()
    {
        damageZoneCount--;

        if (damageZoneCount <= 0)
            damageHitZone.SetActive(false);

    }



}
