using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "VoxelStrikeEquipment/Nade")]
public class VSNade : VSEquipment
{
    public VSNadeType NadeType;
    public float ExplosionRadius;
    public float Duration;
    public int Maxdamage;
    public int Mindamage;
    public int DamageOverTime;
    public int Quantity;
    public Sprite NadeKillIcon;
}

public enum VSNadeType
{
    Grenade,
    Smoke,
    Molotov
}
