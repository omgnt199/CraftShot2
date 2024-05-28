using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public int TimeSet;
    public TextMeshProUGUI CounterText;
    private float _timer = 0;
    private int _timeSet;
    private void OnEnable()
    {
        _timeSet = TimeSet;
        CounterText.text = _timeSet.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (_timeSet > 0)
        {
            _timer += Time.deltaTime;
            if (_timer >= 1f)
            {
                _timer = 0;
                _timeSet--;
                CounterText.text = _timeSet.ToString();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
