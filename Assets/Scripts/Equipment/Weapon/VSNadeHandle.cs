using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VSNadeHandle : MonoBehaviour
{
    public VSNade NadeUsing;
    public VSPlayerInfo WhoThrow;
    public GameObject VfxPrefab;
    public float DelayTimeExplosion;

    public abstract void Explosion();
}
