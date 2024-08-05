using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCoolDownUI : MonoBehaviour
{
    [SerializeField] private DashConfigSO _dashConfig;
    [SerializeField] private Image _dashCooldownImg;
    [SerializeField] private FloatEventChanelSO _updateDashCoolDownUI;
    private void OnEnable()
    {
        _updateDashCoolDownUI.OnEventRaised += UpdateUI;
    }
    private void OnDisable()
    {
        _updateDashCoolDownUI.OnEventRaised -= UpdateUI;
    }
    void UpdateUI(float currentCooldownTimer)
    {
        _dashCooldownImg.fillAmount = 1f - currentCooldownTimer / _dashConfig.DashStackCooldown;
    }
}
