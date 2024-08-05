using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvicibilityBarUI : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private FloatEventChanelSO _applyInvicibilityForPlayer;
    private void OnEnable()
    {
        _applyInvicibilityForPlayer.OnEventRaised += StartProgressBar;
    }
    private void OnDisable()
    {
        _progressBar.fillAmount = 1f;
        _applyInvicibilityForPlayer.OnEventRaised -= StartProgressBar;
    }

    void StartProgressBar(float duration)
    {
        _progressBar.DOFillAmount(0, duration).OnComplete(() => gameObject.SetActive(false));
    }
}
