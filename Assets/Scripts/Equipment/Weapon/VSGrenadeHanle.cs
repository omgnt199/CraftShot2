using Assets.Scripts.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSGrenadeHanle : VSNadeHandle
{
    public void OnEnable()
    {
        Invoke(nameof(Explosion), DelayTimeExplosion);
    }
    public override void Explosion()
    {
        Instantiate(VfxPrefab, transform.position, Quaternion.identity);
        Collider[] hitcolliders;
        LayerMask MaskAttack = LayerMask.GetMask("Player", "Enemy", "ObstacleLayer");
        hitcolliders = Physics.OverlapSphere(transform.position, NadeUsing.ExplosionRadius, MaskAttack);
        foreach (var hit in hitcolliders)
        {

            GameObject victim = hit.gameObject;
            if (victim.GetComponent<CarExplosion>() != null)
            {
                victim.GetComponent<CarExplosion>().Explosion();
                continue;
            }
            if (victim.layer == LayerMask.NameToLayer("ObstacleLayer")) continue;
            VSPlayerInfo victimInfo = victim.GetComponent<VSPlayerInfo>();
            int damage = 0;
            if (Vector3.Distance(victim.transform.position, transform.position) <= NadeUsing.ExplosionRadius && Vector3.Distance(victim.transform.position, transform.position) > NadeUsing.ExplosionRadius / 2f) damage = 30;
            else if (Vector3.Distance(victim.transform.position, transform.position) <= 2) damage = 50;
            victim.GetComponent<Damageable>().ReceiveDamage(damage, WhoThrow.gameObject);
            if (victimInfo.HP.CurrentHeath <= 0)
            {
                VSInGameUIScript.instance.ShowKillReport(WhoThrow.GetComponent<VSPlayerInfo>().Name, WhoThrow.GetComponent<VSPlayerInfo>().Team.TeamSide, victim.GetComponent<VSPlayerInfo>().Name, victimInfo.Team.TeamSide, NadeUsing.NadeKillIcon, KillType.None);
                if (WhoThrow.Team == victimInfo.Team) WhoThrow.Kills--;
                else WhoThrow.Kills++;

            }
            if (damage > 0 && WhoThrow.gameObject.CompareTag("Player")) VSInGameUIScript.instance.ShowDamgeScore(damage);
        }
        Destroy(gameObject);
    }
}
