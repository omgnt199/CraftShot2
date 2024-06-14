using DG.Tweening;
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
        int coinValue = CurrencyData.GetCurrencyValue(CurrencyType.Coin);
        int diamondValue = CurrencyData.GetCurrencyValue(CurrencyType.Diamond);
        DOTween.To(() => int.Parse(Coin.text), coin => Coin.text = coin.ToString(), coinValue, 0.5f);
        DOTween.To(() => int.Parse(Diamond.text), diamond => Diamond.text = diamond.ToString(), diamondValue, 0.5f);
    }
}
