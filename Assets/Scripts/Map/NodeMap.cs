using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NodeMap : MonoBehaviour
{
    [SerializeField] private RectTransform ship;
    [SerializeField] private Button[] mapButtons;
    [SerializeField] private string[] sceneNames;
    [SerializeField] private Sprite completedLevelSprite;
    [SerializeField] private float shipSpeed = 1000f;

    private int currentLevel;
    private RectTransform targetPoint;
    private bool isMoving = false;
    private void Start()
    {
        ship.position = mapButtons[currentLevel].GetComponent<RectTransform>().position;
        
        foreach (var button in mapButtons)
        {
            button.onClick.AddListener(() => OnMapButtonClicked(button));
        }
        
        MarkCompletedLevels();
        UpdateInteractableButtons();
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveShip();
        }
    }
    
    private void OnMapButtonClicked(Button clickedButton)
    {
        targetPoint = clickedButton.GetComponent<RectTransform>();
        isMoving = true;
    }
    
    private void MoveShip()
    {
        ship.position = Vector3.MoveTowards(ship.position, targetPoint.position, shipSpeed * Time.deltaTime);

        if (Vector3.Distance(ship.position, targetPoint.position) < 0.1f)
        {
            isMoving = false;
            currentLevel = Array.IndexOf(mapButtons, targetPoint.GetComponent<Button>());
            MarkCompletedLevel(currentLevel);

            LoadLevelScene(currentLevel);

            UpdateInteractableButtons();
        }
    }
    private void UpdateInteractableButtons()
    {
        for (int i = 0; i < mapButtons.Length; i++)
        {
            mapButtons[i].interactable = false;
        }

        if (currentLevel + 1 < mapButtons.Length)
        {
            mapButtons[currentLevel + 1].interactable = true;
        }
    }
    private void MarkCompletedLevels()
    {
        for (int i = 0; i < currentLevel; i++)
        {
            MarkCompletedLevel(i);
            mapButtons[i].interactable = false;
        }
    }
    private void MarkCompletedLevel(int level)
    {
        GameObject completedMarker = new GameObject("CompletedMarker");
        completedMarker.transform.SetParent(mapButtons[level].transform);

        Image markerImage = completedMarker.AddComponent<Image>();
        markerImage.sprite = completedLevelSprite;

        RectTransform markerRect = completedMarker.GetComponent<RectTransform>();
        markerRect.anchoredPosition = Vector2.zero;
        markerRect.sizeDelta = mapButtons[level].GetComponent<RectTransform>().sizeDelta;
    }

    private void LoadLevelScene(int level)
    {
        if (level < sceneNames.Length)
        {
            SceneManager.LoadScene(sceneNames[level]);
        }
    }
}
