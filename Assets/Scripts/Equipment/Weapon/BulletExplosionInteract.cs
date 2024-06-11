using System.Collections;
using System.Collections.Generic;
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
        Instantiate(gunUsing.Bullet.BulletExplosion, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        //Calculated Damage
        Collider[] hitcolliders;
        LayerMask MaskAttack = LayerMask.GetMask("Player", "Enemy");
        hitcolliders = Physics.OverlapSphere(transform.position, gunUsing.Bullet.ExplosionRadius, MaskAttack);

        foreach (var hit in hitcolliders)
        {
            GameObject victim = hit.gameObject;
            VSPlayerInfo victimInfo = victim.GetComponent<VSPlayerInfo>();
            int damage = 0;
            float distanceFromVictimToExplosion = Vector3.Distance(victim.transform.position, transform.position);
            if (distanceFromVictimToExplosion <= gunUsing.Bullet.ExplosionRadius && distanceFromVictimToExplosion > gunUsing.Bullet.ExplosionRadius / 2f) damage = 30;
            else if (distanceFromVictimToExplosion <= 2) damage = 50;
            victimInfo.UpdateHP(-damage);
            if (victimInfo.HP <= 0)
            {
                victimInfo.OnDeath();
                VSPlayerInfo whoShootInfo = WhoShoot.GetComponent<VSPlayerInfo>();
                VSInGameUIScript.instance.ShowKillReport(whoShootInfo.Name, whoShootInfo.Team.TeamSide, victimInfo.Name, victimInfo.Team.TeamSide, gunUsing.GunKillIcon, KillType.None);
                if (whoShootInfo.Team == victimInfo.Team) whoShootInfo.Kills--;
                else whoShootInfo.Kills++;
                //'Kill' Event trigger
                if (WhoShoot.gameObject.CompareTag("Player")) _Bullet.KillEvent(KillType.None);
            }
            if (damage > 0 && WhoShoot.gameObject.CompareTag("Player")) VSInGameUIScript.instance.ShowDamgeScore(damage);
            else if (damage > 0 && victim.CompareTag("Player")) VSInGameUIScript.instance.ShowTakeDamagePopUp((victim.transform.position - WhoShoot.transform.position).normalized);
        }

        Destroy(GetComponent<BulletExplosionInteract>());
        _Bullet.Deactive();
    }
}
