using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AmmoItemPower", menuName = "ScriptableObject/ItemPower/AmmoPower")]
public class AmmoItemPowerSO : ItemPowerSO
{
    public GameObject PowerUpParticleUI;
    public override void Apply(GameObject Player)
    {
        Player.GetComponent<VSPlayerController>().ResetWeapon();
        GameObject mainCanvas = GameObject.Find("MainCanvas");
        Instantiate(PowerUpParticleUI);
    }

    public override void Deactive()
    {

    }
}
