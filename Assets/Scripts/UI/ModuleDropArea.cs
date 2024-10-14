using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleDropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    [SerializeField] private ModulesConfiguration config;
    private ModulesGridUI modulesGridUI;
    private Image moduleImage;

    private void Awake()
    {
        modulesGridUI = GetComponentInParent<ModulesGridUI>();
        moduleImage = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered drop area: " + gameObject.name);
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped object was: " + eventData.pointerDrag);

        GameObject droppedItem = eventData.pointerDrag;

        Debug.Log(droppedItem);

        if (droppedItem != null && droppedItem.GetComponent<StoreItem>() != null)
        {
            moduleImage.sprite = droppedItem.GetComponent<Image>().sprite;

            Vector2Int position = GetModulePosition();
            config.ModulesPositions[position] = modulesGridUI.objectsPrefabs[droppedItem];
            Debug.Log(config.ModulesPositions[position]);
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
