using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemPowerSO : ScriptableObject
{
    public string Name;
    public float Duration;
    public GameObject Prefab;
    public abstract void Apply(GameObject Player);
    public abstract void Deactive();
}
