using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class StartBook : MonoBehaviour
{
    [Header("Book Animation Start")]
    [SerializeField] private Sprite[] bookSprites;
    [SerializeField] private float totalAnimationTime = 0.5f;
    [SerializeField] private float animationStartDelay = 1.2f; 

    private Image bookImage;
    
    private float frameTime;
    private int currentFrame = 0;
    private float timer = 0f;
    private bool hasAnimationStarted = false;
    
    [Header("Fade In")]
    [SerializeField] private Graphic[] uiElements;
    [SerializeField] private float contentAppearanceDelay = 1.2f;
    [SerializeField] private float fadeInDuration = 0.5f;

    private Graphic[] allGraphics;
    private bool isFadingIn = false;
    private float[] fadeInTimers;

    void Start()
    {
        bookImage = GetComponent<Image>();
        frameTime = totalAnimationTime / bookSprites.Length;
        
        InitializeGraphics();
        
        fadeInTimers = new float[allGraphics.Length];
        foreach (Graphic uiElement in allGraphics)
        {
            SetUIAlpha(uiElement, 0f);
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        
        if (!hasAnimationStarted && timer >= animationStartDelay)
        {
            hasAnimationStarted = true;
            timer = 0f;
        }

        if (hasAnimationStarted && timer >= frameTime && currentFrame < bookSprites.Length)
        {
            bookImage.sprite = bookSprites[currentFrame];
            currentFrame++;

            timer = 0f;
        }
        
        if (!isFadingIn && timer >= contentAppearanceDelay)
        {
            isFadingIn = true;
        }
        
        if (isFadingIn)
        {
            for (int i = 0; i < allGraphics.Length; i++)
            {
                fadeInTimers[i] += Time.deltaTime;

                float newAlpha = Mathf.Clamp01(fadeInTimers[i] / fadeInDuration);
                SetUIAlpha(allGraphics[i], newAlpha);
            }
        }
    }
    private void InitializeGraphics()
    {
        List<Graphic> graphicsList = new System.Collections.Generic.List<Graphic>();

        foreach (Graphic uiElement in uiElements)
        {
            Graphic[] graphicsInChildren = uiElement.GetComponentsInChildren<Graphic>();
            graphicsList.AddRange(graphicsInChildren);
        }

        allGraphics = graphicsList.ToArray();
    }

    private void SetUIAlpha(Graphic uiElement, float alpha)
    {
        Color currentColor = uiElement.color;
        currentColor.a = alpha;
        uiElement.color = currentColor;
    }
}
