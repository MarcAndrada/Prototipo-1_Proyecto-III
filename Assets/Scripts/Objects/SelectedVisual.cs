using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisual : MonoBehaviour
{
    [Header("For Objects With Outline")]
    [SerializeField] private Outline[] outlineArray;
    
    [Header("For Objects With Selected Visual")]
    [SerializeField] private GameObject[] visualArray;
    private void Awake()
    {
        Hide();
    }

    public void Show()
    {
        foreach (Outline outline in outlineArray)
        {
            outline.enabled = true;
        }
        foreach (GameObject visual in visualArray)
        {
            visual.SetActive(true);
        }
    }

    public void Hide()
    {
        foreach (Outline outline in outlineArray)
        {
            outline.enabled = false;
        }
        foreach (GameObject visual in visualArray)
        {
            visual.SetActive(false);
        }
    }
}
