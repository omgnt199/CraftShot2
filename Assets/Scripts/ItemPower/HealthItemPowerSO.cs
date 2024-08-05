using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealthItemPower", menuName = "ScriptableObject/ItemPower/HealthPower")]
public class HealthItemPowerSO : ItemPowerSO
{
    public int RestoreAmount;
    public GameObject PowerUpParticleUI;
    public VoidEventChannelSO _updateHeathBar;
    public override void Apply(GameObject Player)
    {
        Player.GetComponent<VSPlayerInfo>().HP.RestoreHealth(RestoreAmount);
        _updateHeathBar.RaiseEvent();
        Instantiate(PowerUpParticleUI);
    }

    public override void Deactive()
    {

    }
}
