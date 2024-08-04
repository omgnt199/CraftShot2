using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>

[CreateAssetMenu(menuName = "Events/Void Event Channel")]
public class VoidEventChannelSO : SerializableScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {

        OnEventRaised?.Invoke();
    }
}