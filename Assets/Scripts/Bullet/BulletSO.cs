using EditorAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Bullet",menuName = "ScriptableObject/Bullet")]
public class BulletSO : ScriptableObject
{
    public BulletInteractType InteractType;
    public TrailSO BulletTrail;
    public float BulletRadius;
    public GameObject Particle;
    public GameObject BulletDecal;
    [ShowField(nameof(InteractType),BulletInteractType.Explosion)]
    public GameObject BulletExplosion;
    [ShowField(nameof(InteractType), BulletInteractType.Explosion)]
    public float ExplosionRadius;
    public AudioClip BulletSound;
}
public enum BulletInteractType
{
    Direct,
    Explosion
}