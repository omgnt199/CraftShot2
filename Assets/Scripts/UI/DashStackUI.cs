using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DashStackUI : MonoBehaviour
{
    [SerializeField] private DashConfigSO _dashConfigSO;
    [SerializeField] private TextMeshProUGUI _stackText;
    [SerializeField] private IntEventChanelSO _updateDashStackUI;

    private void OnEnable()
    {
        UpdateStackUI(_dashConfigSO.DashStack);
        _updateDashStackUI.OnEventRaised += UpdateStackUI;
    }
    private void OnDisable()
    {
        _updateDashStackUI.OnEventRaised -= UpdateStackUI;
    }
    void UpdateStackUI(int currentStack)
    {
        _stackText.text = currentStack.ToString() + "/" + _dashConfigSO.DashStack.ToString();
    }
}
