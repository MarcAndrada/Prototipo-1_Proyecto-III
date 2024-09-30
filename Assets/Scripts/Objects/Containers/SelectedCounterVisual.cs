using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private Counter counter;
    [SerializeField] private GameObject visual;

    private void Awake()
    {
        counter = gameObject.GetComponent<Counter>();
    }

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += PlayerOnSelectedChestChanged;
    }
    private void PlayerOnSelectedChestChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == counter)
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
