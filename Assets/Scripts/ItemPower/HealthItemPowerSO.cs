using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealthItemPower", menuName = "ScriptableObject/ItemPower/HealthPower")]
public class HealthItemPowerSO : ItemPowerSO
{
    public int RestoreAmount;
    public override void Apply(GameObject Player)
    {
        Player.GetComponent<VSPlayerInfo>().SetCurrentHP(RestoreAmount);
    }

    public override void Deactive()
    {

    }
}
