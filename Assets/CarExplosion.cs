using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarExplosion : MonoBehaviour
{
    public Rigidbody rb;
    public float explosionForce;
    public float explosionRadius;
    public float upwardModifiers;
    public GameObject ExplosionVfx;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Explosion()
    {
        rb.isKinematic = false;
        rb.AddExplosionForce(explosionForce, transform.position - new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)), explosionRadius, upwardModifiers, ForceMode.Impulse);
        gameObject.layer = LayerMask.NameToLayer("IgnoreBulletForce");
        Instantiate(ExplosionVfx, transform);
    }
}
