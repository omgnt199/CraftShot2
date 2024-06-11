using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDirectInteract : BulletInteract
{
    public override void Awake()
    {
        base.Awake();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "BodyPart")
        {
            VSPlayerInfo victim = collision.gameObject.GetComponentInParent<VSPlayerInfo>();
            VSGun gunUsing;
            if (WhoShoot.GetComponent<VSPlayerInfo>().Team != victim.Team)
            {
                if (WhoShoot.GetComponent<VSPlayerControlWeapon>() != null) gunUsing = WhoShoot.GetComponent<VSPlayerControlWeapon>().GunUsing;
                else gunUsing = WhoShoot.GetComponent<VSBotController>().GunUsing;
                VSPlayerInfo playerinfo = WhoShoot.GetComponent<VSPlayerInfo>();


                //Calculate damage
                int dam = 0;
                if (collision.gameObject.CompareTag(VSBodyPart.Body.ToString())) dam = gunUsing.DamageToBody;
                else if (collision.gameObject.CompareTag(VSBodyPart.Leg.ToString()) || collision.gameObject.CompareTag(VSBodyPart.Hand.ToString())) dam = gunUsing.DamageToHandLeg;
                else dam = gunUsing.DamageToHead;
                //victim.GetComponent<Damageable>().ReceiveDamage(dam);
                if (victim.gameObject.CompareTag("Player"))
                    VSInGameUIScript.instance.ShowTakeDamagePopUp((transform.position - victim.transform.position).normalized);
                victim.UpdateHP(-dam);
                if (victim.HP <= 0)
                {
                    //Update player's kills
                    playerinfo.Kills++;
                    //Show kill report
                    KillType killType = KillType.None;
                    if (collision.gameObject.CompareTag(VSBodyPart.Head.ToString())) killType = KillType.HeadShot;
                    VSInGameUIScript.instance.ShowKillReport(playerinfo.Name, playerinfo.Team.TeamSide, victim.Name, victim.Team.TeamSide, gunUsing.GunKillIcon, killType);
                    victim.OnDeath();
                    //'Kill' Event trigger
                    if (WhoShoot.gameObject.CompareTag("Player")) _Bullet.KillEvent(killType);
                }
                //Show damage UI
                if (WhoShoot.CompareTag("Player")) VSInGameUIScript.instance.ShowDamgeScore(dam);
            }
        }
        else
        {
            if (_Bullet.BulletDecal != null)
                _Bullet.SpawnDecal(collision.contacts[0]);
        }

        Destroy(GetComponent<BulletDirectInteract>());
        _Bullet.Deactive();
    }
}
