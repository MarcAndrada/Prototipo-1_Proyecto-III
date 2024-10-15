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

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalParent = transform.parent;
        canvas = GetComponentInParent<Canvas>();
        image = GetComponent<RawImage>();
        moneyText.text = cost.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.position;
        transform.SetParent(canvas.transform);
        
        if (image != null)
        {
            image.raycastTarget = false; 
        }
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
                if (result.gameObject.GetComponent<ModuleDropArea>().GetAvaliableModule()) 
                {
                    transform.SetParent(result.gameObject.transform);
                    rectTransform.anchoredPosition = Vector3.zero;

                    moneyManager.SpendMoney(cost);
                    Destroy(originalParent.gameObject);
                    isOnModule = true;
                }
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
