using System;
using System.Collections.Generic;
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
    }
    struct Enemy
    {
        public List<List<GameObject>> ship;
    }

    [SerializeField]
    private ModulesManager modulesManager;

    [Space, SerializeField]
    private GameObject enemyModulePrefab;
    [SerializeField]
    private List<EnemyPreload> preloadsList;
    [SerializeField]
    private float shipOffset;
    [SerializeField]
    private float moduleOffset;
    List<Enemy> enemies;
    [Space, SerializeField]
    private GameObject particlesPrefab;
    [SerializeField]
    private Vector3 particleOffset;
    [SerializeField]
    private Quaternion particleRotation;

    // Start is called before the first frame update
    void Start()
    {
        LoadEnemiesBoats();
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
