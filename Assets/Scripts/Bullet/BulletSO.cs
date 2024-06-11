using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Bullet",menuName = "ScriptableObject/Bullet")]
public class BulletSO : ScriptableObject
{
    public TrailSO BulletTrail;
    public float Radius;
    public GameObject Particle;
    public GameObject BulletDecal;
    public GameObject BulletExplosion;
    public AudioClip BulletSound;
    public BulletInteractType InteractType;
    //Only ExplosionInteract
    public float ExplosionRadius;
}
public enum BulletInteractType
{
    Direct,
    Explosion
}