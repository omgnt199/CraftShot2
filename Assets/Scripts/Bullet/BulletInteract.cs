using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInteract : MonoBehaviour
{
    protected BulletSpawner _Bullet;
    protected GameObject WhoShoot;
    public virtual void Awake()
    {
        _Bullet = GetComponent<BulletSpawner>();
        WhoShoot = _Bullet.WhoShoot;
    }
}
