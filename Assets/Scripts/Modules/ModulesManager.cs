using System.Collections.Generic;
using UnityEngine;

public class ModulesManager : MonoBehaviour
{

    [Header("Configuration"), SerializeField]
    private ModulesConfiguration configuration;
    [SerializeField]
    private GameObject loseCanvas;

    [Space, Header("Modules"), SerializeField]
    private GameObject moduleBasePrefab;
    public int health;
    private int minHealth;

    [Space, Header("Particles"), SerializeField]
    private GameObject particlesPrefab;
    [SerializeField]
    private Vector3 particleOffset;
    [SerializeField]
    private Quaternion particleRotation;

    private List<List<Module>> modules;

    private List<Vector2Int> attackList;

    private GameObject shipParent;
    private Animator shipAnimator;
    [Space, Header("Animations"), SerializeField]
    private RuntimeAnimatorController shimAnims;

    // Start is called before the first frame update
    void Awake()
    {
        attackList = new List<Vector2Int>();

        CalculateHealth();
        LoadModules();
        LoadModulesObjects();
        LoadColumnsWaterParticles();
    }

    #region Load Functions
    private void CalculateHealth()
    {
        health = configuration.Height * configuration.Width;
        minHealth = Mathf.FloorToInt(configuration.Height * configuration.Width / 2.5f);
    }
    private void LoadModules()
    {
        modules = new List<List<Module>>();

        Vector3 modulePosition = Vector3.zero;

        shipParent = new GameObject("ShipObject");
        shipAnimator = shipParent.AddComponent<Animator>();
        shipAnimator.runtimeAnimatorController = shimAnims;

        for (int i = 0; i < configuration.Height; i++)
        {
            
            modules.Add(new List<Module>());
            modulePosition.x = 0;
            for (int j = 0; j < configuration.Width; j++)
            {
                Module module = Instantiate(moduleBasePrefab, shipParent.transform).GetComponent<Module>();
                module.transform.position = modulePosition;
                modules[i].Add(module);
                modulePosition.x += configuration.ModuleOffset;
                module.GetComponentInChildren<BrokenModule>().manager = this;
            }

            modulePosition.z += configuration.ModuleOffset;
        }
    }
    private void LoadModulesObjects()
    {
        foreach (KeyValuePair<Vector2Int, GameObject> modulePos in configuration.ModulesPositions) 
        {
            GameObject moduleObject = Instantiate(modulePos.Value, shipParent.transform);
            moduleObject.transform.position = modules[modulePos.Key.y][modulePos.Key.x].transform.position;
            modules[modulePos.Key.y][modulePos.Key.x].starterObjectInModule = moduleObject;
        }
    }
    private void LoadColumnsWaterParticles()
    {
        for (int i = 0; i < modules[0].Count; i++)
        {
            GameObject currentParticles = Instantiate(particlesPrefab, shipParent.transform);
            Vector3 particlePosSpawn = modules[0][i].transform.position + particleOffset;

            currentParticles.transform.position = particlePosSpawn;
            currentParticles.transform.rotation = particleRotation;
            
        }
    }
    #endregion

    public void ModuleAttacked(Vector2Int _properties)
    {
        if ((_properties.y < 0 || _properties.y >= modules.Count) || (_properties.x < 0 || _properties.x >= modules[0].Count))
            return;


        modules[_properties.y][_properties.x].AddDamageZone();
        // ESTO PARA ATACAR A LOS MODULOS DE ALREDEDOR
        ////Doble for
        //// 'i' sera para la coordenada 'y'
        //// 'j' sera para la coordenada 'x'
        //for (int i = -1; i <= 1; i++)
        //{
        //    //Comprobar si la columna existe, si no lo hace continuar la siguiente parte del bucle

        //    if (_properties.modulePos.y + i < 0 || _properties.modulePos.y + i >= modules.Count)
        //        continue;

        //    for (int j = -1; j <= 1; j++)
        //    {
        //        //Comprobar si la X esta dentro de la nave
        //        if (_properties.modulePos.x + j < 0 || _properties.modulePos.x + j >= modules[0].Count)
        //            continue;

        //        modules[_properties.modulePos.y + i][_properties.modulePos.x + j].AddSecondaryDamageZone();
        //    }
        //}


        attackList.Add(_properties);
        
    }
    public void DamageModule()
    {
        //Recibir el daño maximo en la casilla golpeada
        modules[attackList[0].y][attackList[0].x].brokenModule.BreakForniture();
        modules[attackList[0].y][attackList[0].x].RemoveMainDamageZone();

        CheckObjectsToBreak();

        shipAnimator.SetTrigger("Damaged");


        attackList.RemoveAt(0);
        //Dañar el barco
        health--;
        //Comprobar si el barco esta roto
        if(health <= minHealth)
            BreakShip();
    }
    private void CheckObjectsToBreak()
    {
        Vector3 cubePos = modules[attackList[0].y][attackList[0].x].transform.position + new Vector3(0, 1, 0);
        RaycastHit[] hits = Physics.BoxCastAll(cubePos, Vector3.one * (configuration.ModuleOffset / 2.5f), Vector3.up);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Module"))
                continue;

            if(hit.collider.TryGetComponent(out BaseFurniture _forniture))
            {
                //Rompe la forniture
                _forniture.BreakForniture();
            }
            else if(hit.collider.TryGetComponent(out InteractableObject _object))
            {
                //Borra el objeto
                Destroy(_object.gameObject);
            }
            else if (hit.collider.TryGetComponent(out PlayerController _player))
            {
                //Mata al player
                _player.KillPlayer();
            }


        }
    }
    private void BreakShip()
    {
        shipAnimator.SetTrigger("Destroyed");
        Debug.Log("You Lose");
        loseCanvas.SetActive(true);

    }

    public Vector3 GetModulePositionAtSide(EnemyManager.Side _side)
    {
        switch (_side)
        {
            case EnemyManager.Side.LEFT:
                return modules[0][0].transform.position;
            case EnemyManager.Side.RIGHT:
                return modules[0][configuration.Width - 1].transform.position;
            default:
                return Vector3.zero;
        }
    }

    public (Vector3, Vector2Int) GetRandomFixedModulePosition(int _loops)
    {
        int i = Random.Range(0, configuration.Height);
        int j = Random.Range(0, configuration.Width);


        if(_loops >= 100)
        {
            return (modules[i][j].transform.position, new Vector2Int(j, i));
        }

        if (modules[i][j].isBroken)
        {
            return GetRandomFixedModulePosition(_loops + 1);
        }

        foreach (Vector2Int currentAttack in attackList)
        {
            if (currentAttack == new Vector2Int(j, i))
                return GetRandomFixedModulePosition(_loops + 1);
        }

        modules[i][j].isBroken = true;
        return (modules[i][j].transform.position, new Vector2Int(j, i));

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (modules != null && modules[0] != null && attackList.Count > 0)
        {
            Vector3 cubePos = modules[attackList[0].y][attackList[0].x].transform.position + new Vector3(0, 1, 0);
            Gizmos.DrawWireCube(cubePos, new Vector3(7, 2, 7));


        }
    }
}
