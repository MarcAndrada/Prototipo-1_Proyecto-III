using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedFurnitureVisual : MonoBehaviour
{
    [SerializeField] private BaseFurniture furniture;
    [SerializeField] private GameObject[] visualArray;

    private void Awake()
    {
        furniture = gameObject.GetComponent<BaseFurniture>();
    }

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += PlayerOnSelectedChestChanged;
    }
    private void PlayerOnSelectedChestChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedFurniture == furniture)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject visual in visualArray)
        {
            visual.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject visual in visualArray)
        {
            visual.SetActive(false);
        }    
    }
    
}
