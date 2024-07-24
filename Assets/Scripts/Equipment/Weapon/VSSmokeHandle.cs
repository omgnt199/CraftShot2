using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSSmokeHandle : VSNadeHandle
{
    private GameObject Vfx;
    public void OnEnable()
    {
        Invoke(nameof(Explosion), DelayTimeExplosion);
    }
    public override void Explosion()
    {
        gameObject.SetActive(false);
        Vfx = Instantiate(VfxPrefab, transform.position, Quaternion.identity);
        Invoke(nameof(DeactiveSmoke), NadeUsing.Duration);
    }
    void DeactiveSmoke()
    {
        Destroy(Vfx);
        Destroy(gameObject);
    }
}
