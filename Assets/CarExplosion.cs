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
    bool isExplosion = false;

    private GameObject _explostionVfx;
    public MeshRenderer meshRenderer;
    public List<Material> BurntBlackMat;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Explosion()
    {
        if (!isExplosion)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForce, transform.position - new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)), explosionRadius, upwardModifiers, ForceMode.Impulse);
            gameObject.layer = LayerMask.NameToLayer("IgnoreBulletForce");
            _explostionVfx = Instantiate(ExplosionVfx, transform);
            isExplosion = true;

            Commons.SetTimeout(this, 3f, () =>
            {
                gameObject.layer = LayerMask.NameToLayer("ObstacleLayer");
                rb.isKinematic = true;
            });

            Commons.SetTimeout(this, 8f, () =>
            {
                meshRenderer.SetMaterials(BurntBlackMat);
                Destroy(_explostionVfx);
            });
        }
    }
}
