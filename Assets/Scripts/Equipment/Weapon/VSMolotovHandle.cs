using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSMolotovHandle : VSNadeHandle
{
    private Vector3 _hitPoint;
    private GameObject Vfx;
    private bool _isFire = false;
    private float _burnDelayTime = 1f;
    private float _burnTimer = 0f;
    private void OnEnable()
    {
        Invoke(nameof(Explosion), DelayTimeExplosion);
    }
    private void Update()
    {
        if (_isFire)
        {
            if (_burnTimer < _burnDelayTime) _burnTimer += Time.deltaTime;
            else
            {
                _burnTimer = 0;
                Debug.Log("Burning");
                Collider[] hitcolliders;
                LayerMask MaskAttack = LayerMask.GetMask("Player", "Enemy");
                hitcolliders = Physics.OverlapSphere(transform.position, NadeUsing.ExplosionRadius, MaskAttack);
                foreach (var hit in hitcolliders)
                {
                    if (hit.gameObject.transform.position.y - transform.position.y <= 1.5f)
                    {
                        GameObject victim = hit.gameObject;
                        VSPlayerInfo victimInfo = victim.GetComponent<VSPlayerInfo>();
                        int damage = NadeUsing.DamageOverTime;
                        victimInfo.UpdateHP(-damage);
                        if (damage > 0 && WhoThrow.gameObject.CompareTag("Player")) VSInGameUIScript.instance.ShowDamgeScore(damage);
                        if (damage > 0 && victim.CompareTag("Player")) VSInGameUIScript.instance.ShowTakeDamagePopUp((WhoThrow.transform.position - victim.transform.position).normalized);
                        if (victimInfo.HP <= 0)
                        {
                            victimInfo.OnDeath();
                            VSInGameUIScript.instance.ShowKillReport(WhoThrow.GetComponent<VSPlayerInfo>().Name, WhoThrow.GetComponent<VSPlayerInfo>().Team.TeamSide, victim.GetComponent<VSPlayerInfo>().Name, victimInfo.Team.TeamSide, NadeUsing.NadeKillIcon, KillType.None);
                            if (WhoThrow.Team == victimInfo.Team) WhoThrow.Kills--;
                            else WhoThrow.Kills++;

                        }
                    }
                }
            }
        }
    }
    public override void Explosion()
    {
        Vfx = Instantiate(VfxPrefab, transform.position, Quaternion.LookRotation(Vector3.up));
        Invoke(nameof(DeactiveMolotov), NadeUsing.Duration);
        _isFire = true;
    }

    void DeactiveMolotov()
    {
        Destroy(Vfx);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CancelInvoke(nameof(Explosion));
            Explosion();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CancelInvoke(nameof(Explosion));
            Explosion();
            _hitPoint = collision.contacts[0].point;
            Destroy(gameObject.GetComponent<Rigidbody>());
            Destroy(gameObject.GetComponent<MeshRenderer>());
        }
    }
}
