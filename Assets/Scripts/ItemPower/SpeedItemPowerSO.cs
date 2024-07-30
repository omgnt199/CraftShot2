using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpeedItemPower", menuName = "ScriptableObject/ItemPower/SpeedPower")]
public class SpeedItemPowerSO : ItemPowerSO
{
    public float SpeedMutiply;
    private VSPlayerMovement _playerMovement;
    public GameObject PowerUpParticleUI;
    public override void Apply(GameObject player)
    {
        _playerMovement = player.GetComponent<VSPlayerMovement>();
        _playerMovement.MoveSpeed = _playerMovement.WalkSpeed * SpeedMutiply;
        _playerMovement.LineSpeedVfx.Play();
        GameObject mainCanvas = GameObject.Find("MainCanvas");
        Instantiate(PowerUpParticleUI);
    }

    public override void Deactive()
    {
        _playerMovement.SpeedOnWalking();
        _playerMovement.LineSpeedVfx.Stop();
    }

}
