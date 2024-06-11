using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AmmoItemPower",menuName = "ScriptableObject/ItemPower/AmmoPower")]
public class AmmoItemPowerSO : ItemPowerSO
{
    public override void Apply(GameObject Player)
    {
        Player.GetComponent<VSPlayerController>().ResetWeapon();
    }
}
