using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int startingMoney = 1000;
    [SerializeField] private TextMeshProUGUI moneyText;
    private int currentMoney;

    private void Start()
    {
        currentMoney = startingMoney;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough money!");
            return false;
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }
    private void UpdateMoneyUI()
    {
        moneyText.text = currentMoney.ToString();
    }
    
    public int GetCurrentMoney()
    {
        return currentMoney;
    }


}
