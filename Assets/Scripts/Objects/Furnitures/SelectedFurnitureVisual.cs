using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedFurnitureVisual : MonoBehaviour
{
    [SerializeField] private GameObject[] visualArray;

    public void Show()
    {
        foreach (GameObject visual in visualArray)
        {
            visual.SetActive(true);
        }
    }

    public void Hide()
    {
        foreach (GameObject visual in visualArray)
        {
            visual.SetActive(false);
        }    
    }
    
}
