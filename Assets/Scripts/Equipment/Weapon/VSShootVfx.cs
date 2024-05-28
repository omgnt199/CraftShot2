using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSShootVfx : MonoBehaviour
{
    public GameObject Particle;
    private GameObject _currentParticle;
    public void Spawn()
    {
        _currentParticle = Instantiate(Particle, transform);
    }
    private void OnDisable()
    {
        if (_currentParticle != null) Destroy(_currentParticle);
    }
}
