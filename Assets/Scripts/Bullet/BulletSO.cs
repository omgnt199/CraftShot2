using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Bullet",menuName = "ScriptableObject/Bullet")]
public class BulletSO : ScriptableObject
{
    public TrailSO BulletTrail;
    public float Radius;
    public GameObject Particle;
}
