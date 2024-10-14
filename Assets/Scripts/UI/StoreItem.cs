using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class StoreItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Transform originalParent; 
    private Canvas canvas; 

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalParent = transform.parent;
        canvas = GetComponentInParent<Canvas>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.position;
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
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
                transform.SetParent(result.gameObject.transform);
                rectTransform.anchoredPosition = Vector3.zero;

                Debug.Log("Dropped on Module");
                
                isOnModule = true;
                break;
            }
        }

        if (!isOnModule)
        {
            rectTransform.position = originalPosition;
            transform.SetParent(originalParent);
        }
    }
}
