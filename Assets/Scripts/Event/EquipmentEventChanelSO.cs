using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Events/Equipment Event Channel")]
public class EquipmentEventChanelSO : ScriptableObject
{
    public UnityAction<VSEquipment> OnEventRaised;

    public void RaiseEvent(VSEquipment equipment)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(equipment);
    }
}
