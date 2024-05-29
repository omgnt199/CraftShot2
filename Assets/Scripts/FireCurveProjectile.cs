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
    }
}
