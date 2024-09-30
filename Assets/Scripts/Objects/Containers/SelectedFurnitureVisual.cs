using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private Furniture furniture;
    [SerializeField] private GameObject visual;

    private void Awake()
    {
        furniture = gameObject.GetComponent<Furniture>();
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
        visual.SetActive(true);
    }

    private void Hide()
    {
        visual.SetActive(false);
    }
}
