using UnityEngine;

public class Module : MonoBehaviour
{
    [HideInInspector]
    public GameObject starterObjectInModule;

    [field: Space, SerializeField]
    public BrokenModule brokenModule {  get; private set; }
    [SerializeField]
    private GameObject damageHitZone;

    public bool isBroken;

    private int damageZoneCount;

    private void Start()
    {
        isBroken = false;
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
