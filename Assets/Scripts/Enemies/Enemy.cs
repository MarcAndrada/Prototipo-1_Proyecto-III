using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject normalModule;
    [SerializeField]
    private GameObject brokenModule;

    private void Start()
    {
        normalModule.SetActive(true);
        brokenModule.SetActive(false);
    }
    public void BreakModule()
    {
        normalModule.SetActive(false);
        brokenModule.SetActive(true);
    }
}
