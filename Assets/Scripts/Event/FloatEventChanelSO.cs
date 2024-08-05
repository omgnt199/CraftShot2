﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Events/Float Event Channel")]
public class FloatEventChanelSO : SerializableScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void RaiseEvent(int value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
