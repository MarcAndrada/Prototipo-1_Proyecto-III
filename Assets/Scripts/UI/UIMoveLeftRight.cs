using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveLeftRight : MonoBehaviour
{
    [SerializeField] private RectTransform uiElement;
    [SerializeField] private float startX = -500f;
    [SerializeField] private float endX = 500f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private AnimationCurve movementCurve;

    private float elapsedTime = 0f;
    private bool movingRight = true;

    void Start()
    {
        if (movementCurve == null)
        {
            movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }

        uiElement.anchoredPosition = new Vector2(startX, uiElement.anchoredPosition.y);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        float t = elapsedTime / duration;
        float curveValue = movementCurve.Evaluate(t);
        float newX = Mathf.Lerp(startX, endX, curveValue);

        uiElement.anchoredPosition = new Vector2(newX, uiElement.anchoredPosition.y);

        if (t >= 1f)
        {
            elapsedTime = 0f;
            movingRight = !movingRight;

            float temp = startX;
            startX = endX;
            endX = temp;
        }
    } 
}
