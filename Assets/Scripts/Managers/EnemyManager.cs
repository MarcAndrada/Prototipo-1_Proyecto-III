using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public enum Side { LEFT, RIGHT }

    [Serializable]
    struct EnemyPreload
    {
        public Side side;
        public int height;
        public int width;
        public int totalCanons;
        public int health;
        public float shootCd;
    }
    struct Enemy
    {
        public List<List<EnemyModule>> ship;
        public GameObject shipParent;
        public Animator shipAnimator;
        public int totalModulesBroken;
        public bool enemyAlive;
        public void AddMolduleBroke()
        {
            totalModulesBroken++;
        }
    }

    [SerializeField]
    private ModulesManager modulesManager;

    [Space, Header("Modules"), SerializeField]
    private GameObject enemyModulePrefab;
    [SerializeField]
    private GameObject cannonPrefab;
    [SerializeField]
    private float shipOffset;
    [SerializeField]
    private float moduleOffset;
    [SerializeField]
    private float moduleHeight;

    [Space, Header("Enemies Preloads"), SerializeField]
    private List<EnemyPreload> preloadsList;
    private List<Enemy> enemies;
    private List<EnemyCanon> cannonList;

    [Space, Header("Particles"), SerializeField]
    private GameObject particlesPrefab;
    [SerializeField]
    private Vector3 particleOffset;
    [SerializeField]
    private Quaternion particleRotation;

    [Space, Header("Animation"), SerializeField]
    private AnimatorController shipAnimations;
    

    // Start is called before the first frame update
    void Start()
    {
        cannonList = new List<EnemyCanon>();
        LoadEnemiesBoats();
        LoadCanons();
        LoadColumnsWaterParticles();
    }

    private void LoadEnemiesBoats()
    {
        enemies = new List<Enemy>();


        for (int i = 0; i < preloadsList.Count; i++)
        {
            Enemy enemy = new Enemy();

            enemy.enemyAlive = true;
            enemy.totalModulesBroken = 0;

            enemy.shipParent = new GameObject("EnemyShip" + i);
            enemy.shipParent.transform.position = Vector3.zero;

            enemy.shipAnimator = enemy.shipParent.AddComponent<Animator>();
            enemy.shipAnimator.runtimeAnimatorController = shipAnimations;

            Vector3 direction = Vector3.zero;
            switch (preloadsList[i].side)
            {
                case Side.LEFT:
                    direction = Vector3.left;
                    break;
                case Side.RIGHT:
                    direction = Vector3.right;
                    break;
                default:
                    break;
            }


            enemy.ship = new List<List<EnemyModule>>();
            Vector3 originalPos = modulesManager.GetModulePositionAtSide(preloadsList[i].side) + direction * shipOffset;

            for (int j = 0; j < preloadsList[i].height; j++)
            {
                enemy.ship.Add(new List<EnemyModule>());

                for (int k = 0; k < preloadsList[i].width; k++)
                {
                    EnemyModule enemyModule = Instantiate(enemyModulePrefab, enemy.shipParent.transform).GetComponent<EnemyModule>();
                    enemyModule.SetManager(this);
                    enemyModule.SetShipId(i);
                    enemyModule.SetModuleId(j, k);
                    enemy.ship[j].Add(enemyModule);
                    enemyModule.transform.position = originalPos + (direction * moduleOffset * k) + (Vector3.forward * moduleOffset * j);
                }
            }

            enemies.Add(enemy);

        }

    }
    private void LoadCanons()
    {

        for (int i = 0; i < preloadsList.Count; i++) {

            for (int j = 1; j <= preloadsList[i].totalCanons; j++) 
            {
                Vector3 cannonPos = new Vector3(0, moduleHeight, 0);
                Vector3 lookDirection = Vector3.zero;
                switch (preloadsList[i].side)
                {
                    case Side.LEFT:
                        cannonPos.x = enemies[i].ship[0][0].transform.position.x;
                        lookDirection = Vector3.left;
                        break;
                    case Side.RIGHT:
                        cannonPos.x = enemies[i].ship[preloadsList[i].height - 1][0].transform.position.x;
                        lookDirection = Vector3.right;
                        break;
                    default:
                        break;
                }

                //Comprobar si el numero de cañones es mayor o igual al de las casillas de alto colocar un cañon en cada casilla y ya
                if (preloadsList[i].totalCanons >= preloadsList[i].height)
                {
                    if (enemies[i].ship.Count > j - 1)
                        cannonPos.z = CalculateCannosPosWithExactModules(i, j - 1);
                    else
                        continue;
                }
                else
                {
                    cannonPos.z = CalculateCannonsPosWithLessModules(i , j);
                }

                //Instanciar cañon
                GameObject currentCannon = Instantiate(cannonPrefab, enemies[i].ship[0][0].transform.parent);
                currentCannon.transform.position = cannonPos;
                currentCannon.transform.forward = lookDirection;
                cannonList.Add(currentCannon.GetComponent<EnemyCanon>());
            }
        }
    }
    private float CalculateCannosPosWithExactModules(int i, int j)
    {   
        return enemies[i].ship[j][0].transform.position.z;
    }
    private float CalculateCannonsPosWithLessModules(int i, int j)
    {
        //En caso de que haya menos cañones que casillas vamos a calcular donde ponerlos
        
        //Primero calculamos la posicion inicial en relacion a la primera casilla teniendo en cuenta que el pivote esta en el centro
        float starterZPos = enemies[i].ship[0][0].transform.position.z - moduleOffset / 2 + 1;

        //Calculamos la altura total en valores reales (si hay 5 modulos de altura y cada modulo mide 2 el resultado es 10)
        float totalHeightSclaed = preloadsList[i].height * moduleOffset;

        //La cantidad de cañones que vamos 
        int cannons = preloadsList[i].totalCanons + Mathf.Clamp(preloadsList[i].totalCanons, 0, 2);

        //Calculamos cual es el offset utilizando la escala total con los cañones que tenemos
        float offsetZ = (float)totalHeightSclaed / cannons;

        return starterZPos + offsetZ * j;
    }
    private void LoadColumnsWaterParticles()
    {
        foreach (Enemy item in enemies)
        {
            for (int i = 0; i < item.ship[0].Count; i++)
            {
                GameObject currentParticles = Instantiate(particlesPrefab, item.ship[0][0].transform.parent);
                Vector3 particlePosSpawn = item.ship[0][i].transform.position + particleOffset;

                currentParticles.transform.position = particlePosSpawn;
                currentParticles.transform.rotation = particleRotation;
            }
        }
    }


    public void ModuleHited(EnemyModule _module)
    {
        if (_module.IsModuleBroke())
        {
            //Encontrar otro modulo para romper
            CheckNextModulesToBroke(_module);
        }
        else
        {
            BreakModule(_module);
        }
    }

    private void CheckNextModulesToBroke(EnemyModule _module)
    {

        Vector2Int moduleId = _module.GetModuleId();
        int shipId = _module.GetShipId();
        // ESTO PARA ATACAR A LOS MODULOS DE ALREDEDOR
        ////Doble for
        //// 'i' sera para la coordenada 'y'
        //// 'j' sera para la coordenada 'x'
        for (int i = -1; i <= 1; i++)
        {
            //Comprobar si la columna existe, si no lo hace continuar la siguiente parte del bucle

            if (moduleId.y + i < 0 || moduleId.y + i >= enemies[shipId].ship.Count)
                continue;

            for (int j = -1; j <= 1; j++)
            {
                //Comprobar si la X esta dentro de la nave
                if (moduleId.x + j < 0 || moduleId.x + j >= enemies[shipId].ship[0].Count || enemies[shipId].ship[moduleId.y + i][moduleId.x + j].IsModuleBroke())
                    continue;

                BreakModule(enemies[shipId].ship[moduleId.y + i][moduleId.x + j]);
                return;
            }
        }


        //Romper modulo Random

        for (int i = 0; i <= enemies[shipId].ship.Count; i++)
        {
            for (int j = 0; j < enemies[shipId].ship[0].Count; j++)
            {
                if (!enemies[shipId].ship[i][j].IsModuleBroke())
                {
                    BreakModule(enemies[shipId].ship[i][j]);
                    return;
                }
            }
        }

    }

    private void BreakModule(EnemyModule _module)
    {
        _module.BreakModule();
        enemies[_module.GetShipId()].AddMolduleBroke();
        //Comprobar si se rompe el barco

        Debug.Log("SE ROMPE");
    }
}
