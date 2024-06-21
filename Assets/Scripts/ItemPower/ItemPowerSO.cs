using EditorAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemPowerType
{
    Immediate,
    Duration
}

public abstract class ItemPowerSO : ScriptableObject
{
    public string Name;
    public ItemPowerType PowerType;
    [ShowField(nameof(PowerType),ItemPowerType.Duration)]
    public float Duration;
    public GameObject Prefab;
    public abstract void Apply(GameObject Player);
    public abstract void Deactive();
}
