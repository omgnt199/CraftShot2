using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum VSGunPriority
{
    Primary,
    Secondary
}
[CreateAssetMenu(menuName = "VoxelStrikeEquipment/Weapon")]
public class VSGun : VSEquipment
{
    public VSGunPriority Priority;
    public int Magazine;
    public int TotalAmmo;
    public float FirePower;
    public float FireSpeed;
    public float TimeReload;
    public float RecoilAmountX;
    public float RecoilAmountY;
    public VSCrossHair CrossHair;
    public int DamageToHead;
    public int DamageToBody;
    public int DamageToHandLeg;
    public Sprite GunKillIcon;
    public BulletSO Bullet;
}
