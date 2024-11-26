using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyAndReputation : MonoBehaviour
{
    public static MoneyAndReputation Instance;
    public int money;
    public int reputation;

    // UI
    public TMP_Text money_text;
    public TMP_Text reputation_text;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        money = 2000;
        reputation = 500;
    }
    private void Update()
    {
        money_text.text = money.ToString();
        reputation_text.text = reputation.ToString();
    }
    public void AddMoney(int amount)
    {
        money += amount;
    }
    public void AddReputation(int amount)
    {
        reputation += amount;
    }
    public void MinusMoney(int amount)
    {
        money -= amount;
    }
    public void MinusReputation(int amount)
    {
        reputation -= amount;
    }
    public void ConvertReputationToMoney()
    {
        money += reputation * 10;
    }
    public void ResetMoneyAndReputation(int mon, int rep)
    {
        money = mon;
        reputation = rep;
    }
}
