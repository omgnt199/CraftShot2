using EditorAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public int Coin;
    public int Diamond;
    [Button("CheatCurrency")]
    public void CheatCurrency()
    {
        CurrencyData.UpdateCurrency(CurrencyType.Coin, Coin);
        CurrencyData.UpdateCurrency(CurrencyType.Diamond, Diamond);
    }
}
