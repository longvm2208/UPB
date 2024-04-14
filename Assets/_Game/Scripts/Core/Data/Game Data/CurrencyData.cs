using System;
using UnityEngine;

[Serializable]
public class CurrencyData
{
    public int CoinAmount = 0;

    public event Action CoinAmountChanged;

    public void ChangeCoinAmount(int amount, string place = null)
    {
        if (CoinAmount + amount < 0)
        {
            Debug.LogError("Coin amount need to be greater than or equal to 0");
            return;
        }

        CoinAmount += amount;
        CoinAmountChanged?.Invoke();
    }
}
