using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CurrencyVisualizer : MonoBehaviour
{
    public TextMeshProUGUI Coin;
    public TextMeshProUGUI Diamond;

    private void OnEnable()
    {
        Coin.text = PlayerPrefs.GetInt("Player" + CurrencyType.Coin).ToString();
        Diamond.text = PlayerPrefs.GetInt("Player" + CurrencyType.Diamond).ToString();
        CurrencyData.CurrencyChange += OnChangeCurrency;
    }
    private void OnDisable()
    {
        CurrencyData.CurrencyChange -= OnChangeCurrency;
    }
    public void OnChangeCurrency()
    {
        Coin.text = CurrencyData.GetCurrencyValue(CurrencyType.Coin).ToString();
        Diamond.text = CurrencyData.GetCurrencyValue(CurrencyType.Diamond).ToString();
    }
}
