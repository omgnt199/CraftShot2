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
        LayerMask MaskAttack = LayerMask.GetMask("Player", "Enemy");
        hitcolliders = Physics.OverlapSphere(transform.position, NadeUsing.ExplosionRadius, MaskAttack);
        foreach (var hit in hitcolliders)
        {
            GameObject victim = hit.gameObject;
            VSPlayerInfo victimInfo = victim.GetComponent<VSPlayerInfo>();
            int damage = 0;
            if (Vector3.Distance(victim.transform.position, transform.position) <= NadeUsing.ExplosionRadius && Vector3.Distance(victim.transform.position, transform.position) > NadeUsing.ExplosionRadius / 2f) damage = 30;
            else if (Vector3.Distance(victim.transform.position, transform.position) <= 2) damage = 50;
            victimInfo.UpdateHP(-damage);
            if (victimInfo.HP <= 0)
            {
                victimInfo.OnDeath();
                VSInGameUIScript.instance.ShowKillReport(WhoThrow.GetComponent<VSPlayerInfo>().Name, WhoThrow.GetComponent<VSPlayerInfo>().Team.TeamSide, victim.GetComponent<VSPlayerInfo>().Name, victimInfo.Team.TeamSide, NadeUsing.NadeKillIcon, KillType.None);
                if (WhoThrow.Team == victimInfo.Team) WhoThrow.Kills--;
                else WhoThrow.Kills++;

            }
            if (damage > 0 && WhoThrow.gameObject.CompareTag("Player")) VSInGameUIScript.instance.ShowDamgeScore(damage);
            else if(damage >0 && victim.CompareTag("Player")) VSInGameUIScript.instance.ShowTakeDamagePopUp((victim.transform.position - WhoThrow.transform.position).normalized);
        }
        Destroy(gameObject);
    }
}
