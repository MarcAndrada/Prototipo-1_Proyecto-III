using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModulesGridUI : MonoBehaviour
{
    [SerializeField] private ModulesConfiguration config;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform gridParent;

    private void Start()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        DrawGrid();
    }

    private void DrawGrid()
    {
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
                    Image cellImage = cell.GetComponent<Image>();
                    cellImage.color = Color.red;

                    GameObject moduleObject = config.ModulesPositions[position];
                }
            }
        }
    }
}
