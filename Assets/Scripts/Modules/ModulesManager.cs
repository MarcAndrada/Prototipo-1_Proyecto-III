using System.Collections.Generic;
using UnityEngine;

public class ModulesManager : MonoBehaviour
{

    [SerializeField]
    private ModulesConfiguration configuration;

    [SerializeField]
    private GameObject moduleBasePrefab;
    [SerializeField]
    private GameObject moduleWall;
    private List<List<Module>> modules;
    private List<List<GameObject>> modulesObjects;

    // Start is called before the first frame update
    void Start()
    {
        LoadModules();
        LoadModulesObjects();
        //LoadWalls();
    }

    private void LoadModules()
    {
        modules = new List<List<Module>>();
        modulesObjects = new List<List<GameObject>>();

        Vector3 modulePosition = Vector3.zero;
        for (int i = 0; i < configuration.Height; i++)
        {
            modules.Add(new List<Module>());
            modulesObjects.Add(new List<GameObject>());
            modulePosition.x = 0;
            for (int j = 0; j < configuration.Width; j++)
            {
                modules[i].Add(Instantiate(moduleBasePrefab, modulePosition, Quaternion.identity).GetComponent<Module>());
                modulesObjects[i].Add(null);

                modulePosition.x += configuration.ModuleOffset;
            }

            modulePosition.z += configuration.ModuleOffset;
        }
    }
    private void LoadModulesObjects()
    {
        foreach (KeyValuePair<Vector2Int, GameObject> modulePos in configuration.ModulesPositions) 
        {
            GameObject moduleObject = Instantiate(modulePos.Value, modules[modulePos.Key.y][modulePos.Key.x].transform.position, Quaternion.identity);  
            modulesObjects[modulePos.Key.y][modulePos.Key.x] = moduleObject;
        }
    }
    private void LoadWalls()
    {
        Vector3 moduleScale = modules[0][0].transform.localScale;
        // For para comprobar las paredes superiores y inferiores
        for (int i = 0; i < modulesObjects.Count; i++)
        {
            if (i == 0){
                //Estan en la parte de abajo
                for (int j = 0; j < modulesObjects[i].Count; j++)
                {
                    CreateWall(0, -1, moduleScale, modules[i][j].transform.position, Vector3.forward);
                }
            }
            else if (i >= modulesObjects.Count - 1)
            {
                //Estan en la parte de arriba
                for (int j = 0; j < modulesObjects[i].Count; j++)
                {
                    CreateWall(0, 1, moduleScale, modules[i][j].transform.position, -Vector3.forward);
                }

            }



            // For para comprobar las paredes laterales
            for (int j = 0; j < modulesObjects[i].Count; j++)
            {
                if (j == 0)
                {
                    //Esta a la izquierda
                    CreateWall(-1, 0, moduleScale, modules[i][j].transform.position, Vector3.right);
                }
                else if (j >= modulesObjects[i].Count - 1)
                {
                    //Esta a la derecha
                    CreateWall(1, 0, moduleScale, modules[i][j].transform.position, -Vector3.right);
                }
            }

        }

        
        
       

    }
    private void CreateWall(float _directionX, float _directionZ, Vector3 _moduleScale, Vector3 _basePosition, Vector3 _lookDirection)
    {
        GameObject wall = Instantiate(moduleWall, Vector3.zero, Quaternion.identity);

        float YOffset = (wall.transform.localScale.y / 2 + _moduleScale.y / 2);
        float XZOffset = configuration.ModuleOffset / 2;

        Vector3 endPosition = _basePosition + new Vector3(XZOffset * _directionX, YOffset, XZOffset * _directionZ);

        wall.transform.position = endPosition;
        wall.transform.forward = _lookDirection;
    }

}
