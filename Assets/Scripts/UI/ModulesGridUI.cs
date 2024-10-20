using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

public class ModulesGridUI : MonoBehaviour
{
    [SerializeField] private ModulesConfiguration config;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform gridParent;

    [Header("Money")]
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private int expansionCost = 400;
    
    [Header("Buttons")]
    [SerializeField] private Button expandRightButton;
    [SerializeField] private Button expandLeftButton;
    
    [Space, SerializedDictionary("SceneObject", "Prefab")]
    public SerializedDictionary<GameObject, GameObject> objectsPrefabs;

    [Space, SerializedDictionary("SceneObject", "Prefab")]
    public SerializedDictionary<GameObject, Sprite> objectSprites;
    private void Start()
    {
        expandRightButton.onClick.AddListener(ExpandWidth);
        expandLeftButton.onClick.AddListener(ExpandHeight);
        
        DrawGrid();
    }
    private void ExpandWidth()
    {
        if (moneyManager.SpendMoney(expansionCost))
        {
            config.ExpandWidth();
            DrawGrid();
        }
    }

    private void ExpandHeight()
    {
        if (moneyManager.SpendMoney(expansionCost) && config.Height < 5)
        {
            config.ExpandHeight();
            DrawGrid();
        }
    }
    private void DrawGrid()
    {        
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        int width = config.Width;
        int height = config.Height;
        
        GridLayoutGroup gridLayout = gridParent.GetComponent<GridLayoutGroup>();
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = width;

        gridLayout.cellSize = new Vector2Int(100, 100);
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cell = Instantiate(cellPrefab, gridParent);
                cell.name = $"Cell ({x}, {y})";
                Vector2Int position = new Vector2Int(x, y);

                if (config.ModulesPositions.ContainsKey(position))
                {
                    ModuleDropArea module = cell.GetComponent<ModuleDropArea>();

                    GameObject moduleObject = config.ModulesPositions[position];
                    
                    // Crear una nueva imagen como hijo de la celda
                    GameObject spriteObject = new GameObject("ModuleSprite");
                    spriteObject.transform.SetParent(cell.transform);

                    // Asegurarse de que el objeto esté en el centro y escalado correctamente
                    RectTransform rectTransform = spriteObject.AddComponent<RectTransform>();
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.offsetMin = Vector2.zero;
                    rectTransform.offsetMax = Vector2.zero;
                    rectTransform.localScale = new Vector3(0.75f, 0.75f, 1);
                    
                    // Agregar el componente Image al nuevo objeto
                    Image imageComponent = spriteObject.AddComponent<Image>();

                    // Agregar el componente StoreItem al objeto
                    spriteObject.AddComponent<StoreItem>();
                    
                    if (objectSprites.ContainsKey(moduleObject))
                    {
                        imageComponent.sprite = objectSprites[moduleObject];
                    }
                    else
                    {
                        // Para los objetos que no tengan sprite
                        imageComponent.color = Color.red;
                    }

                    module.SetAvaliableModule(false);
                }
            }
        }
    }
}
