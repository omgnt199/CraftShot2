using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class BulletSpawner : MonoBehaviour
{
    public GameObject WhoShoot;
    public Rigidbody Rb;
    public float lifeTime;
    public GameObject BulletDecal;
    public TrailRenderer BulletTrail;
    private GameObject _bulletParticle;
    private GameObject _bulletExplosionPrefab;
    private GameObject _bulletDecal;
    private VSGun _gunUsing;
    public void Activate(GameObject whoShoot, VSGun gun, Vector3 muzzlePos, Vector3 velocity)
    {
        WhoShoot = whoShoot;
        _gunUsing = gun;
        //Set trail
        if (gun.Bullet.BulletTrail != null)
        {
            BulletTrail.time = gun.Bullet.BulletTrail.LifeTime;
            BulletTrail.minVertexDistance = gun.Bullet.BulletTrail.TrailMinVertextDistance;
            BulletTrail.colorGradient = gun.Bullet.BulletTrail.TrailGradientColor;
            BulletTrail.widthCurve = gun.Bullet.BulletTrail.TrailAnimationCurve;
        }

        if (gun.Bullet.Particle != null)
        {
            _bulletParticle = Instantiate(gun.Bullet.Particle, transform);
            _bulletParticle.SetActive(true);
            BulletTrail.gameObject.SetActive(false);
        }
        else BulletTrail.gameObject.SetActive(true);

        BulletDecal = gun.Bullet.BulletDecal != null ? gun.Bullet.BulletDecal : null;
        _bulletExplosionPrefab = gun.Bullet.BulletExplosion != null ? gun.Bullet.BulletExplosion : null;
        //
        GetComponent<CapsuleCollider>().radius = gun.Bullet.BulletRadius;
        //
        if (gun.Bullet.InteractType == BulletInteractType.Direct) gameObject.AddComponent<BulletDirectInteract>();
        else if (gun.Bullet.InteractType == BulletInteractType.Explosion) gameObject.AddComponent<BulletExplosionInteract>();
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
        if (_bulletParticle != null) Destroy(_bulletParticle);
        if (_bulletDecal != null) Destroy(_bulletDecal);
        gameObject.SetActive(false);
    }
    public void SpawnDecal(ContactPoint hit)
    {
        _bulletDecal = Instantiate(BulletDecal, hit.point, Quaternion.LookRotation(hit.normal));
        _bulletDecal.transform.position += 0.02f * hit.normal;
    }

}
