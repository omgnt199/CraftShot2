using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VSClockInGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clockText;
    public int GameTime = 180;
    private float timer = 0f;
    private int m, s;
    private string sT, mT;
    bool isTimeout = false;

    [SerializeField] private CounterSO _counterConfig;
    [SerializeField] private IntEventChanelSO _updateCounterUIEvent;
    [SerializeField] private VoidEventChannelSO _timeOverEvent;
    private void OnEnable()
    {
        UpdateUI(_counterConfig.InitialTime);
        _updateCounterUIEvent.OnEventRaised += UpdateUI;
        _timeOverEvent.OnEventRaised += TimeOverEffect;
    }

    private void OnDisable()
    {
        _updateCounterUIEvent.OnEventRaised -= UpdateUI;
        _timeOverEvent.OnEventRaised -= TimeOverEffect;
    }

    void UpdateUI(int currentTimeInt)
    {
        m = currentTimeInt / 60;
        s = currentTimeInt % 60;
        mT = "00";
        sT = "00";
        if (m / 10 < 1) mT = "0" + m.ToString();
        else mT = m.ToString();
        if (s / 10 < 1) sT = "0" + s.ToString();
        else sT = s.ToString();
        clockText.text = mT + ":" + sT;
        timer = 0;
    }
    void TimeOverEffect()
    {
        clockText.gameObject.transform.DOScale(1.3f, 0.5f).SetLoops(20, LoopType.Yoyo);
        clockText.DOColor(Color.red, 0.5f).SetLoops(20, LoopType.Yoyo); ;
    }
}
