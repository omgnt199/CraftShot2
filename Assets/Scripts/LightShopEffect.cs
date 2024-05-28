using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LightShopEffect : MonoBehaviour
{
    private float originScale;
    private void Awake()
    {
        originScale = transform.localScale.x;
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().DOFade(0.5f, 2f).SetLoops(-1, LoopType.Yoyo);
        transform.DOLocalRotate(new Vector3(0, 0, 360f), 8f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        transform.DOScale(originScale * 1.5f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnDestroy()
    {
        DOTween.Kill(gameObject);
    }
}
