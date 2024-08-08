using Assets.Scripts.Character;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletDirectInteract : BulletInteract
{
    public override void Awake()
    {
        base.Awake();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "BodyPart" || LayerMask.LayerToName(collision.gameObject.layer) == "MainPlayerBodyPart")
        {
            VSPlayerInfo victim = collision.gameObject.GetComponentInParent<VSPlayerInfo>();
            VSPlayerInfo whoShootInfo = WhoShoot.GetComponent<VSPlayerInfo>();
            VSGun gunUsing;
            if (whoShootInfo.Team != victim.Team && victim.gameObject != whoShootInfo.gameObject)
            {
                if (WhoShoot.GetComponent<VSPlayerControlWeapon>() != null) gunUsing = WhoShoot.GetComponent<VSPlayerControlWeapon>().GunUsing;
                else gunUsing = WhoShoot.GetComponent<VSBotController>().GunUsing;

                //Calculate damage
                int dam = 0;
                if (collision.gameObject.CompareTag(VSBodyPart.Body.ToString())) dam = gunUsing.DamageToBody;
                else if (collision.gameObject.CompareTag(VSBodyPart.Leg.ToString()) || collision.gameObject.CompareTag(VSBodyPart.Hand.ToString())) dam = gunUsing.DamageToHandLeg;
                else dam = gunUsing.DamageToHead;
                victim.GetComponent<Damageable>().ReceiveDamage(dam, WhoShoot);
                victim.GetComponent<Damageable>().SpawnTakeDamageVFX(collision.contacts[0]);

                if (victim.HP.CurrentHeath <= 0)
                {
                    //Update player's kills
                    whoShootInfo.Kills++;
                    //Show kill report
                    KillType killType = KillType.None;
                    if (collision.gameObject.CompareTag(VSBodyPart.Head.ToString())) killType = KillType.HeadShot;
                    VSInGameUIScript.instance.ShowKillReport(whoShootInfo.Name, whoShootInfo.Team.TeamSide, victim.Name, victim.Team.TeamSide, gunUsing.GunKillIcon, killType);
                    //'Kill' Event trigger
                    if (WhoShoot.gameObject.CompareTag("Player"))
                    {
                        if (killType != KillType.None) PlayerEventListener.RaiseSpecialKillEvent(killType);
                        PlayerEventListener.RaiseKillByEvent();
                    }
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
        _Bullet.Rb.velocity = Vector3.zero;
        Commons.SetTimeout(_Bullet, 0.2f, () =>
        {
            _Bullet.Deactive();
        });
    }
}
