using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUIEffect : MonoBehaviour
{
    [SerializeField] private GameObject LightRayImg;
    [SerializeField] private float _rotateLoopTime;
    [SerializeField] private float _startScale;
    [SerializeField] private float _endScale;
    [SerializeField] private float _scaleLoopTime;
    private void OnEnable()
    {
        LightRayImg.transform.localScale = Vector3.one * _startScale;
        LightRayImg.transform.DORotate(new Vector3(0, 0, 360f), _rotateLoopTime, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        LightRayImg.transform.DOScale(_endScale, _scaleLoopTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void OnDisable()
    {

    }
}
