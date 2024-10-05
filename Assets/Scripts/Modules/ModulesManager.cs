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

    [Space, SerializeField, Range(0.0f, 1f)]
    private float secondaryModulesHitDamage;
    // Start is called before the first frame update
    void Start()
    {
        LoadModules();
        LoadModulesObjects();
        //LoadWalls();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ModuleAttacked(new Vector2Int(3, 10), 50f);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ModuleAttacked(new Vector2Int(1, 1), 50f);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ModuleAttacked(new Vector2Int(0, 0), 50f);
        }
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
            modules[modulePos.Key.y][modulePos.Key.x].starterObjectInModule = moduleObject;
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

    public void ModuleAttacked(Vector2Int _modulePos, float _damage)
    {
        if ((_modulePos.y < 0 || _modulePos.y >= modules.Count) || 
            (_modulePos.x < 0 || _modulePos.x >= modules[0].Count))
            return;

        //Recibir el daño maximo en la casilla golpeada
        modules[_modulePos.y][_modulePos.x].GetDamage(_damage);
        Debug.Log("Se hittea el del centro que es la posicion: " + _modulePos);

        //Doble for
        // 'i' sera para la coordenada 'y'
        // 'j' sera para la coordenada 'x'
        for (int i = -1; i <= 1 ; i++)
        {
            //Comprobar si la columna existe, si no lo hace continuar la siguiente parte del bucle

            if (_modulePos.y + i < 0 || _modulePos.y + i >= modules.Count)
                continue;

            for (int j = -1; j <= 1; j++)
            {
                //Comprobar si la X esta dentro de la nave
                if (_modulePos.x + j < 0 || _modulePos.x + j >= modules[0].Count)
                    continue;

                Debug.Log("Se hittea un lado con la posicion la posicion: " + new Vector2Int(_modulePos.x + j, _modulePos.y + i));
                modules[_modulePos.y + i][_modulePos.x + j].GetDamage(_damage * secondaryModulesHitDamage);

            }
        }
    }

}
