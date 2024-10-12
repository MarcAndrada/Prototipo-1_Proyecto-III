using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        public List<List<GameObject>> ship;
    }

    [SerializeField]
    private ModulesManager modulesManager;

    [Space,  SerializeField]
    private GameObject enemyModulePrefab;
    [SerializeField]
    private GameObject cannonPrefab;
    [SerializeField]
    private List<EnemyPreload> preloadsList;
    [SerializeField]
    private float shipOffset;
    [SerializeField]
    private float moduleOffset;
    private List<Enemy> enemies;
    private List<GameObject> cannonList;
    [SerializeField]
    private float moduleHeight;

    [Space, Header("Particles"), SerializeField]
    private GameObject particlesPrefab;
    [SerializeField]
    private Vector3 particleOffset;
    [SerializeField]
    private Quaternion particleRotation;

    

    // Start is called before the first frame update
    void Start()
    {
        cannonList = new List<GameObject>();
        LoadEnemiesBoats();
        LoadCanons();
        LoadColumnsWaterParticles();
    }

    private void LoadEnemiesBoats()
    {
        enemies = new List<Enemy>();


        for (int i = 0; i < preloadsList.Count; i++)
        {
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



            Enemy enemy = new Enemy();

            enemy.ship = new List<List<GameObject>>();
            Vector3 originalPos = modulesManager.GetModulePositionAtSide(preloadsList[i].side) + direction * shipOffset;

            for (int j = 0; j < preloadsList[i].height; j++)
            {
                enemy.ship.Add(new List<GameObject>());

                for (int k = 0; k < preloadsList[i].width; k++)
                {
                    GameObject newModule = Instantiate(enemyModulePrefab, Vector3.zero, Quaternion.identity);
                    enemy.ship[j].Add(newModule);
                    newModule.transform.position = originalPos + (direction * moduleOffset * k) + (Vector3.forward * moduleOffset * j);
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
                GameObject currentCannon = Instantiate(cannonPrefab, cannonPos, Quaternion.identity);
                currentCannon.transform.forward = lookDirection;
                cannonList.Add(currentCannon);
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
                GameObject currentParticles = Instantiate(particlesPrefab);
                Vector3 particlePosSpawn = item.ship[0][i].transform.position + particleOffset;

                currentParticles.transform.position = particlePosSpawn;
                currentParticles.transform.rotation = particleRotation;
            }
        }
    }

}
