using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealthItemPower", menuName = "ScriptableObject/ItemPower/HealthPower")]
public class HealthItemPowerSO : ItemPowerSO
{
    public int RestoreAmount;
    public GameObject PowerUpParticleUI;
    public override void Apply(GameObject Player)
    {
        Player.GetComponent<VSPlayerInfo>().SetCurrentHP(RestoreAmount);
        GameObject mainCanvas = GameObject.Find("MainCanvas");
        Instantiate(PowerUpParticleUI);
    }

    public override void Deactive()
    {

    }
}
