using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedFurnitureVisual : MonoBehaviour
{
    //[SerializeField] private GameObject[] visualArray;
    [SerializeField] private Outline[] outlineArray;

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
        /*
        foreach (GameObject visual in visualArray)
        {
            visual.SetActive(true);
        }
        */
    }

    public void Hide()
    {
        foreach (Outline outline in outlineArray)
        {
            outline.enabled = false;
        }
        /*
        foreach (GameObject visual in visualArray)
        {
            visual.SetActive(false);
        }
        */
    }
}
