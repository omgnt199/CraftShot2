using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInteract : MonoBehaviour
{
    protected Bullet _Bullet;
    protected GameObject WhoShoot;
    public virtual void Awake()
    {
        _Bullet = GetComponent<Bullet>();
        WhoShoot = _Bullet.WhoShoot;
    }
}
