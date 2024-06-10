using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public GameObject WhoShoot;
    public Rigidbody Rb;
    public float lifeTime;
    public GameObject BulletDecal;
    public TrailRenderer BulletTrail;
    private GameObject _bulletParticle;
    private void Awake()
    {
    }
    public void Activate(GameObject whoShoot,VSGun gun,Vector3 muzzlePos, Vector3 velocity)
    {
        WhoShoot = whoShoot;
        //Set trail
        if (gun.Bullet.BulletTrail != null)
        {
            BulletTrail.time = gun.Bullet.BulletTrail.LifeTime;
            BulletTrail.minVertexDistance = gun.Bullet.BulletTrail.TrailMinVertextDistance;
            BulletTrail.colorGradient = gun.Bullet.BulletTrail.TrailGradientColor;
        }

        if (gun.Bullet.Particle != null)
        {
            _bulletParticle = Instantiate(gun.Bullet.Particle, transform);
            _bulletParticle.SetActive(true);
            BulletTrail.gameObject.SetActive(false);
        }
        else BulletTrail.gameObject.SetActive(true);

        if (gun.Bullet.BulletDecal != null) BulletDecal = gun.Bullet.BulletDecal;
        else BulletDecal = null;
        //
        GetComponent<CapsuleCollider>().radius = gun.Bullet.Radius;
        //
        transform.position = muzzlePos;
        transform.forward = velocity;
        Rb.velocity = velocity;
        gameObject.SetActive(true);
        StartCoroutine(Decay());
    }
    private IEnumerator Decay()
    {
        yield return new WaitForSeconds(lifeTime);
        Deactive();
    }
    public void Deactive()
    {
        BulletPool.instance.AddToPool(this);
        StopAllCoroutines();
        gameObject.SetActive(false);
        if (_bulletParticle != null) Destroy(_bulletParticle);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "BodyPart")
        {
            VSPlayerInfo victim = collision.gameObject.GetComponentInParent<VSPlayerInfo>();
            if (WhoShoot.GetComponent<VSPlayerInfo>().Team != victim.Team)
            {
                VSGun gunUsing = null;
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
                    if (WhoShoot.gameObject.CompareTag("Player")) Kill(killType);
                }
                //Show damage UI
                if (WhoShoot.CompareTag("Player")) VSInGameUIScript.instance.ShowDamgeScore(dam);
            }
        }
        else
        {
            if (BulletDecal != null)
                SpawnDecal(collision.contacts[0]);
        }

        Deactive();
    }

    void Kill(KillType type)
    {
        if (type != KillType.None) PlayerEventListener.InvokeSpecialKillEvent(type);
        PlayerEventListener.InvokeKillByEvent();
    }
    private void SpawnDecal(ContactPoint hit)
    {
        GameObject decal = Instantiate(BulletDecal, hit.point, Quaternion.LookRotation(hit.normal));
        decal.transform.position += 0.02f * hit.normal;
    }

}
