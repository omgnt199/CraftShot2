using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PlayerGlobalInfo
{
    public static UnityAction<string> UpdatePlayerName;
    public static UnityAction<int> UpdatePlayerLevel;
    static string _playerName;
    static int _Level;
    public static string Name
    {
        get => _playerName;
        set
        {
            _playerName = value;
            UpdatePlayerName?.Invoke(_playerName);
        }
    }

    public static int Level
    {
        get => _Level;
        set
        {
            _Level = value;
            UpdatePlayerLevel?.Invoke(_Level);
        }
    }
    public static void Load()
    {
        CurrencyData.Load();
        Name = PlayerPrefs.GetString("PlayerName");
        Level = PlayerPrefs.GetInt("PlayerLevel");
    }

    // Exp = 50/3 * ( x^3 - 6x^2 + 17x - 12 )  with x is Level
    public static int TotalExpToLevelUp(int level)
    {
        return Mathf.FloorToInt(50 * (Mathf.Pow(level, 3) - 6 * Mathf.Pow(level, 2) + 17 * level - 12) / 3);
    }
}
