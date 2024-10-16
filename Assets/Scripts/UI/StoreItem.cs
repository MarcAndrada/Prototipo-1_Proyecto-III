using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class StoreItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private int cost;

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Transform originalParent; 
    private Canvas canvas; 
    private RawImage image;
    private ModuleDropArea currentModule;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalParent = transform.parent;
        canvas = GetComponentInParent<Canvas>();
        image = GetComponent<RawImage>();
        
        if (moneyText != null)
        {
            moneyText.text = cost.ToString();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentModule = GetComponentInParent<ModuleDropArea>();

        if (image != null)
        {
            image.raycastTarget = false; 
        }
        
        originalPosition = rectTransform.position;
        transform.SetParent(canvas.transform);

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        bool isOnModule = false;
        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.CompareTag("Module"))
            {
                ModuleDropArea newModule = result.gameObject.GetComponent<ModuleDropArea>();
                if (newModule.GetAvaliableModule())
                {
                    if (currentModule != null)
                    {
                        ModulesConfiguration config = currentModule.GetConfig();
                        
                        if (config.ModulesPositions.ContainsKey(currentModule.GetModulePosition()))
                        {
                            // Cambiamos el objeto del modulo de posicion
                            GameObject moduleData = config.ModulesPositions[currentModule.GetModulePosition()];
                            config.ModulesPositions.Remove(currentModule.GetModulePosition());
                            config.ModulesPositions[newModule.GetModulePosition()] = moduleData;
                        }
            
                        currentModule.SetAvaliableModule(true);
                    }
                    
                    transform.SetParent(result.gameObject.transform);
                    rectTransform.anchoredPosition = Vector3.zero;

                    if (moneyManager != null)
                    {
                        moneyManager.SpendMoney(cost);
                        Destroy(originalParent.gameObject);
                        moneyManager = null;
                        moneyText = null;
                    }

                    isOnModule = true;
                }
                originalParent = transform.parent;
                break;
            }
        }

        if (!isOnModule)
        {
            rectTransform.position = originalPosition;
            transform.SetParent(originalParent);
        }
        if (image != null)
        {
            image.raycastTarget = true;
        }
    }
}
