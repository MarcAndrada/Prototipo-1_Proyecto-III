using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleDropArea : MonoBehaviour
{
    [SerializeField] private ModulesConfiguration config;
    private Image moduleImage;

    private void Awake()
    {
        moduleImage = GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;

        if (droppedItem != null && droppedItem.GetComponent<StoreItem>() != null)
        {
            moduleImage.sprite = droppedItem.GetComponent<Image>().sprite;
            Debug.Log("Object Dropped");
            Vector2Int position = GetModulePosition();
            config.ModulesPositions[position] = droppedItem;
        }
    }

    public void SetConfig(ModulesConfiguration config)
    {
        this.config = config;
    }
    
    private Vector2Int GetModulePosition()
    {
        string[] splitName = gameObject.name.Replace("Cell (", "").Replace(")", "").Split(',');
        int x = int.Parse(splitName[0].Trim());
        int y = int.Parse(splitName[1].Trim());
        return new Vector2Int(x, y);
    }
}
