using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class VSTakeDamageUI : MonoBehaviour
{
    [SerializeField] private Image BloodImg;
    private void OnEnable()
    {
        TakeDamageEffect();
    }
    void TakeDamageEffect()
    {

        if (!DOTween.IsTweening(BloodImg))
        {
            gameObject.SetActive(true);
            BloodImg.DOColor(new Color32(255, 255, 255, 100), 0.5f).SetLoops(4, LoopType.Yoyo).OnComplete(() =>
            {
                int N = transform.childCount;
                for (int i = 0; i < N; i++) Destroy(transform.GetChild(i).gameObject);
                gameObject.SetActive(false);
            });
        }
    }
}
