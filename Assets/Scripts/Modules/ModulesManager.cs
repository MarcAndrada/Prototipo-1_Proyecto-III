using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ModulesManager : MonoBehaviour
{

    [Header("Configuration"), SerializeField]
    private ModulesConfiguration configuration;

    [Space, Header("Modules"), SerializeField]
    private GameObject moduleBasePrefab;
    [Space, Header("Modules Damage"), SerializeField]
    private float timeToHitModules;


    [Space, Header("Particles"), SerializeField]
    private GameObject particlesPrefab;
    [SerializeField]
    private Vector3 particleOffset;
    [SerializeField]
    private Quaternion particleRotation;

    private List<List<Module>> modules;

    public struct AttackProperties
    {
        public AttackProperties(Vector2Int _modulePos, float _damage)
        {
            modulePos = _modulePos;
            damage = _damage;
        }
        public Vector2Int modulePos;
        public float damage;
    }
    private List<AttackProperties> attackList;

    private GameObject shipParent;
    private Animator shipAnimator;
    [Space, Header("Animations"), SerializeField]
    private AnimatorController shimAnims;

    // Start is called before the first frame update
    void Awake()
    {
        attackList = new List<AttackProperties>();

        LoadModules();
        LoadModulesObjects();
        LoadColumnsWaterParticles();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ModuleAttacked(new AttackProperties(new Vector2Int(3, 10), 50f));
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ModuleAttacked(new AttackProperties(new Vector2Int(1, 1), 50f));
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ModuleAttacked(new AttackProperties(new Vector2Int(0, 0), 50f));
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            shipAnimator.SetTrigger("Damaged");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            shipAnimator.SetTrigger("Destroyed");
        }
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


    public void ModuleAttacked(AttackProperties _properties)
    {
        if ((_properties.modulePos.y < 0 || _properties.modulePos.y >= modules.Count) || (_properties.modulePos.x < 0 || _properties.modulePos.x >= modules[0].Count))
            return;


        modules[_properties.modulePos.y][_properties.modulePos.x].AddDamageZone();
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
        Invoke("DamageModule", timeToHitModules);

    }
    private void DamageModule()
    {
        //Recibir el daño maximo en la casilla golpeada
        modules[attackList[0].modulePos.y][attackList[0].modulePos.x].brokenModule.BreakForniture();
        modules[attackList[0].modulePos.y][attackList[0].modulePos.x].RemoveMainDamageZone();

        CheckObjectsToBreak();

        shipAnimator.SetTrigger("Damaged");

        /*ESO PARA ATACAR A LOS MODULOS DE ALREDEDOR
        * //Doble for
        * // 'i' sera para la coordenada 'y'
        * // 'j' sera para la coordenada 'x'
        * for (int i = -1; i <= 1; i++)
        * {
        *     //Comprobar si la columna existe, si no lo hace continuar la siguiente parte del bucle
        * 
        *     if (attackList[0].modulePos.y + i < 0 || attackList[0].modulePos.y + i >= modules.Count)
        *         continue;
        * 
        *     for (int j = -1; j <= 1; j++)
        *     {
        *         //Comprobar si la X esta dentro de la nave
        *         if (attackList[0].modulePos.x + j < 0 || attackList[0].modulePos.x + j >= modules[0].Count)
        *             continue;
        * 
        *         modules[attackList[0].modulePos.y + i][attackList[0].modulePos.x + j].GetDamage(attackList[0].damage * secondaryModulesHitDamage);
        *         modules[attackList[0].modulePos.y + i][attackList[0].modulePos.x + j].RemoveSecondaryDamageZone();
        *     }
        * }
        */


        attackList.RemoveAt(0);
    }
    private void CheckObjectsToBreak()
    {
        Vector3 cubePos = modules[attackList[0].modulePos.y][attackList[0].modulePos.x].transform.position + new Vector3(0, 1, 0);
        RaycastHit[] hits = Physics.BoxCastAll(cubePos, Vector3.one * (configuration.ModuleOffset / 2.5f), Vector3.up);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Module"))
                continue;

            if(hit.collider.TryGetComponent(out BaseFurniture _forniture))
            {
                Debug.Log("Hay una forniture con el nombre " + _forniture.gameObject.name);
                //Rompe la forniture
                _forniture.BreakForniture();
            }
            else if(hit.collider.TryGetComponent(out InteractableObject _object))
            {
                Debug.Log("Hay un objeto con el nombre " + _object.gameObject.name);
                //Borra el objeto
                Destroy(_object.gameObject);
            }
            else if (hit.collider.TryGetComponent(out PlayerController _player))
            {
                Debug.Log("Esta el player " + _player.gameObject.name);
                //Mata al player
                _player.KillPlayer();
            }
            else
            {
                Debug.Log("No sabemos lo que es, tiene el nombre de: " + hit.collider.gameObject.name);
            }


        }
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (modules != null && modules[0] != null && attackList.Count > 0)
        {
            Vector3 cubePos = modules[attackList[0].modulePos.y][attackList[0].modulePos.x].transform.position + new Vector3(0, 1, 0);
            Gizmos.DrawWireCube(cubePos, new Vector3(7, 2, 7));


        }
    }
}
