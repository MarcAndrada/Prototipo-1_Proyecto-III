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
                cell.AddComponent<ModuleDropArea>();
                Vector2Int position = new Vector2Int(x, y);

                if (config.ModulesPositions.ContainsKey(position))
                {
                    Image cellImage = cell.GetComponent<Image>();
                    
                    // De momento marcar las casillas ocupadas en rojo
                    cellImage.color = Color.red;

                    GameObject moduleObject = config.ModulesPositions[position];
                }
            }
        }
    }
}
