using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterUI : MonoBehaviour
{
    [SerializeField] private CounterSO _counter;
    [SerializeField] private TextMeshProUGUI _counterText;
    [SerializeField] private VoidEventChannelSO _updateCounterUI;

    private int m, s;
    private string mT, sT;
    private void OnEnable()
    {
        _updateCounterUI.OnEventRaised += UpdateCounter;
    }
    void UpdateCounter()
    {
        m = _counter.CurrentTime / 60;
        s = _counter.CurrentTime % 60;
        mT = "00";
        sT = "00";
        if (m / 10 < 1) mT = "0" + m.ToString();
        else mT = m.ToString();
        if (s / 10 < 1) sT = "0" + s.ToString();
        else sT = s.ToString();
    }
}
