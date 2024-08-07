using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] _bodyParts;
    public void IgnoreBulletForce()
    {
        foreach (var part in _bodyParts) part.layer = LayerMask.NameToLayer("IgnoreBulletForce");
    }

    public void ApllyBulletForce()
    {
        foreach (var part in _bodyParts) part.layer = LayerMask.NameToLayer("BodyPart");
    }
}
