using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpeedItemPower", menuName = "ScriptableObject/ItemPower/SpeedPower")]
public class SpeedItemPowerSO : ItemPowerSO
{
    public float SpeedMutiply;
    public float Duration;
    public override void Apply(GameObject Player)
    {
        Player.GetComponent<VSPlayerMovement>().MoveSpeed *= SpeedMutiply;
        Player.GetComponent<VSPlayerMovement>().LineSpeedVfx.Play();
    }

    IEnumerator WaitForPowerFinish(float duration, GameObject player)
    {
        yield return new WaitForSeconds(duration);
        player.GetComponent<VSPlayerMovement>().MoveSpeed /= SpeedMutiply;
        player.GetComponent<VSPlayerMovement>().LineSpeedVfx.Stop();
    }
}
