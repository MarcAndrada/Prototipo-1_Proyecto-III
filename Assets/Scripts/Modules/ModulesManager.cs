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

        Vector3 modulePosition = Vector3.zero;
        for (int i = 0; i < configuration.Height; i++)
        {
            modules.Add(new List<Module>());
            modulePosition.x = 0;
            for (int j = 0; j < configuration.Width; j++)
            {
                modules[i].Add(Instantiate(moduleBasePrefab, modulePosition, Quaternion.identity).GetComponent<Module>());

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
        }
    }
    private void LoadWalls()
    {
        Vector3 moduleScale = modules[0][0].transform.localScale;
        // For para comprobar las paredes superiores y inferiores
        for (int i = 0; i < modules.Count; i++)
        {
            if (i == 0){
                //Estan en la parte de abajo
                for (int j = 0; j < modules[i].Count; j++)
                {
                    CreateWall(0, -1, moduleScale, modules[i][j].transform.position, Vector3.forward);
                }
            }
            else if (i >= modules.Count - 1)
            {
                //Estan en la parte de arriba
                for (int j = 0; j < modules[i].Count; j++)
                {
                    CreateWall(0, 1, moduleScale, modules[i][j].transform.position, -Vector3.forward);
                }

            }



            // For para comprobar las paredes laterales
            for (int j = 0; j < modules[i].Count; j++)
            {
                if (j == 0)
                {
                    //Esta a la izquierda
                    CreateWall(-1, 0, moduleScale, modules[i][j].transform.position, Vector3.right);
                }
                else if (j >= modules[i].Count - 1)
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
