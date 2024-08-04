using Assets.Scripts.Character;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BulletExplosionInteract : BulletInteract
{
    public override void Awake()
    {
        base.Awake();
    }
    private void OnCollisionEnter(Collision collision)
    {
        VSGun gunUsing;
        if (WhoShoot.GetComponent<VSPlayerControlWeapon>() != null) gunUsing = WhoShoot.GetComponent<VSPlayerControlWeapon>().GunUsing;
        else gunUsing = WhoShoot.GetComponent<VSBotController>().GunUsing;
        //Explosion VFX
        GameObject explostionVfx = Instantiate(gunUsing.Bullet.BulletExplosion, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        explostionVfx.transform.position += 0.02f * collision.contacts[0].normal;
        //Calculated Damage
        Collider[] hitcolliders;
        LayerMask MaskAttack = LayerMask.GetMask("Player", "Enemy", "ObstacleLayer");
        hitcolliders = Physics.OverlapSphere(transform.position, gunUsing.Bullet.ExplosionRadius, MaskAttack);
        List<GameObject> victimCheckedList = new List<GameObject>();
        foreach (var hit in hitcolliders)
        {
            if(hit.gameObject.GetComponent<CarExplosion>()!= null)
            {
                hit.gameObject.GetComponent<CarExplosion>().Explosion();
                continue;
            }

            if (hit.gameObject.layer == LayerMask.NameToLayer("ObstacleLayer")) continue;

            //if (WhoShoot.Equals(hit.gameObject)) continue;
            VSPlayerInfo whoShootInfo = WhoShoot.GetComponent<VSPlayerInfo>();
            GameObject victim = hit.gameObject;
            if (victimCheckedList.Contains(victim)) continue;
            else victimCheckedList.Add(victim);
            VSPlayerInfo victimInfo = victim.GetComponent<VSPlayerInfo>();
            if (victimInfo.Team != whoShootInfo.Team)
            {
                int damage = 0;
                float distanceFromVictimToExplosion = Vector3.Distance(victim.transform.position, transform.position);
                if (distanceFromVictimToExplosion <= gunUsing.Bullet.ExplosionRadius && distanceFromVictimToExplosion > gunUsing.Bullet.ExplosionRadius / 2f) damage = 30;
                else if (distanceFromVictimToExplosion <= 2) damage = 50;
                //victimInfo.UpdateHP(-damage);

                victim.GetComponent<Damageable>().ReceiveDamage(damage,WhoShoot);
                victim.GetComponent<Damageable>().SpawnTakeDamageVFX(collision.contacts[0]);

                if (victimInfo.HP.CurrentHeath <= 0)
                {
                    VSInGameUIScript.instance.ShowKillReport(whoShootInfo.Name, whoShootInfo.Team.TeamSide, victimInfo.Name, victimInfo.Team.TeamSide, gunUsing.GunKillIcon, KillType.None);
                    if (whoShootInfo.Team == victimInfo.Team) whoShootInfo.Kills--;
                    else whoShootInfo.Kills++;
                    //'Kill' Event trigger
                    if (WhoShoot.gameObject.CompareTag("Player")) PlayerEventListener.RaiseKillByEvent();
                }
                if (damage > 0 && WhoShoot.gameObject.CompareTag("Player")) VSInGameUIScript.instance.ShowDamgeScore(damage);
            }
        }

        Destroy(GetComponent<BulletExplosionInteract>());
        _Bullet.Rb.velocity = Vector3.zero;
        _Bullet.Deactive();
    }
}
