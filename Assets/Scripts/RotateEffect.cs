using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    public Transform Target;
    private void Start()
    {
        Target.DORotate(new Vector3(0, 360f, 0), 60f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
    private void OnDestroy()
    {
        Target.DOKill();
    }
}
