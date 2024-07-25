using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Trail",menuName = "ScriptableObject/BulletTrail")]
public class TrailSO : ScriptableObject
{
    public float LifeTime;
    [Range(0, 1f)]
    public float TrailMinVertextDistance;
    public Gradient TrailGradientColor;
    public AnimationCurve TrailAnimationCurve;
}
