using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class CurrencyData
{
    public static Dictionary<CurrencyType, int> currencyData;
    public static UnityAction<CurrencyType, int> EconomyCurrencyChange;
    public static UnityAction<int> ExpCurrencyChange;

    public static void Load()
    {
        currencyData = new Dictionary<CurrencyType, int>();
        int N = System.Enum.GetValues(typeof(CurrencyType)).Length;
        for (int i = 0; i < N; i++)
        {
            int value = PlayerPrefs.GetInt("Player" + ((CurrencyType)i).ToString());
            currencyData.Add((CurrencyType)i, value);
        }
    }
    public static int GetCurrencyValue(CurrencyType type) => currencyData[type];

    public static void UpdateCurrency(CurrencyType type, int delta)
    {
        currencyData[type] += delta;
        PlayerPrefs.SetInt("Player" + type.ToString(), currencyData[type]);
        if (type != CurrencyType.Exp) EconomyCurrencyChange?.Invoke(type, currencyData[type]);
        else
        {
            while (ExperienceSystem.IsCanLevelUp()) ExperienceSystem.LevelUp();
            ExpCurrencyChange?.Invoke(currencyData[type]);
        }
    }
}
