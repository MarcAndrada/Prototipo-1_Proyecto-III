using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleDropArea : MonoBehaviour, IDropHandler
{
    [SerializeField] private ModulesConfiguration config;
    private ModulesGridUI modulesGridUI;
    private bool avaliableModule = true;

    private void Awake()
    {
        modulesGridUI = GetComponentInParent<ModulesGridUI>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;

        if (droppedItem != null && droppedItem.GetComponent<StoreItem>() != null && avaliableModule)
        {
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

    public void SetAvaliableModule(bool _avaliableModule)
    {
        avaliableModule = _avaliableModule;
    }    
    public bool GetAvaliableModule()
    {
        return avaliableModule;
    }
}
