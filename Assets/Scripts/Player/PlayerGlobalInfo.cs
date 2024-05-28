using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PlayerGlobalInfo
{
    public static UnityEvent UpdatePlayerName = new UnityEvent();
    static string _playerName;
    public static string PlayerName {
        get => _playerName;
        set 
        {
            _playerName = value;
            UpdatePlayerName?.Invoke();
        }
    }

    public static void Load()
    {
        PlayerName = PlayerPrefs.GetString("PlayerName");
        CurrencyData.Load();
    }
    
}
