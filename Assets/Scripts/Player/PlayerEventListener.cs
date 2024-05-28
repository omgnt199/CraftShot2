using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public static class PlayerEventListener 
{
    public static GameObject MainPlayer;
    public static Dictionary<string, UnityAction> DictEvent = new Dictionary<string, UnityAction>();
    public static UnityAction PlayerEvent;

    public static void RegisterEvent(string eventName, UnityAction action)
    {
        DictEvent[eventName] = action;
    }
    public static void RemoveEvent(string eventName) => DictEvent.Remove(eventName);
    public static void InvokeSpecialKillEvent(KillType type)
    {
        foreach (var it in DictEvent)
        {
            if (it.Key.Contains(type.ToString() + "SpecialKill"))
                it.Value?.Invoke();
        }
    }
    public static void InvokeKillByEvent()
    {
        foreach (var it in DictEvent)
        {
            if (it.Key.Contains("KillBy"))
                it.Value?.Invoke();
        }
    }
}

public enum PlayerEvent
{
    Kill,
    Death,
}
