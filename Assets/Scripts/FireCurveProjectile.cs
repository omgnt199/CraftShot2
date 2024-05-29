using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCurveProjectile : MonoBehaviour
{
    public Transform Muzzle;
    public GameObject ProjectilePrefab;

    // Update is called once per frame
    void Update()
    {

    }
    public void Fire(Vector3 position)
    {
        GameObject projectile = Instantiate(ProjectilePrefab, Muzzle.transform.position, Quaternion.identity);
        projectile.GetComponent<CurveProjectile>().targetPosition = position;
        projectile.transform.forward = Muzzle.forward;
        projectile.GetComponent<Rigidbody>().velocity = Muzzle.forward * 20f;
    }
}
