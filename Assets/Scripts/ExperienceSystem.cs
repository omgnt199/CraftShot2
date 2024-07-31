using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public static class ExperienceSystem
{
    public static UnityAction LevelUpEvent;
    // Exp = 50/3 * ( x^3 - 6x^2 + 17x - 12 )  with x is Level
    public static int TotalExpToLevelUp(int level)
    {
        return Mathf.FloorToInt(50 * (Mathf.Pow(level, 3) - 6 * Mathf.Pow(level, 2) + 17 * level - 12) / 3);
    }

    public static bool IsCanLevelUp()
    {
        return CurrencyData.GetCurrencyValue(CurrencyType.Exp) >= TotalExpToLevelUp(PlayerGlobalInfo.Level + 1);
    }
    public static void LevelUp()
    {
        PlayerGlobalInfo.Level += 1;
    }
}