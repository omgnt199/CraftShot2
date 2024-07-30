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

    private Dictionary<CurrencyType, TextMeshProUGUI> _currencyVisualizerDict = new Dictionary<CurrencyType, TextMeshProUGUI>();
    private void Awake()
    {
        _currencyVisualizerDict.Add(CurrencyType.Coin, Coin);
        _currencyVisualizerDict.Add(CurrencyType.Diamond, Diamond);
    }

    private void OnEnable()
    {
        Coin.text = PlayerPrefs.GetInt("Player" + CurrencyType.Coin).ToString();
        Diamond.text = PlayerPrefs.GetInt("Player" + CurrencyType.Diamond).ToString();
        CurrencyData.EconomyCurrencyChange += OnChangeCurrency;
    }
    private void OnDisable()
    {
        CurrencyData.EconomyCurrencyChange -= OnChangeCurrency;
    }
    public void OnChangeCurrency(CurrencyType type, int currencyValue)
    {
        DOTween.To(() => int.Parse(_currencyVisualizerDict[type].text), value => _currencyVisualizerDict[type].text = value.ToString(), currencyValue, 0.5f);
    }
}
