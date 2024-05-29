using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class VSTakeDamageUI : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _takeDamageUI;
    [SerializeField] private Image BloodImg;
    private void OnEnable()
    {
        TakeDamageEffect();
    }
    private void OnDisable()
    {
        //_takeDamageUI.OnEventRaised -= TakeDamageEffect;
    }
    void TakeDamageEffect()
    {

        if (!DOTween.IsTweening(BloodImg))
        {
            gameObject.SetActive(true);
            BloodImg.DOColor(new Color32(255, 255, 255, 100), 0.5f).SetLoops(4, LoopType.Yoyo).OnComplete(() =>
            {
                gameObject.SetActive(false);
                int N = transform.childCount;
                for (int i = 0; i < N; i++) Destroy(transform.GetChild(i).gameObject);
            });
        }
    }
}
