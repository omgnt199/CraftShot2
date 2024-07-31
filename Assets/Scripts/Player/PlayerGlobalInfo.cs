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
            PlayerPrefs.SetInt("PlayerLevel", _Level);
            UpdatePlayerLevel?.Invoke(_Level);
        }
    }
    public static void Load()
    {
        CurrencyData.Load();
        Name = PlayerPrefs.GetString("PlayerName");
        Level = PlayerPrefs.GetInt("PlayerLevel");
    }
}
