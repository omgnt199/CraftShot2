using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthSO _playerHealth;
    [SerializeField] private HealthConfigSO _playerHealthConfig;
    [SerializeField] private Image _heathBar;
    [SerializeField] private TextMeshProUGUI _heathBarText;

    [SerializeField] private VoidEventChannelSO _UIHeathUpdate;
    private void OnEnable()
    {
        _UIHeathUpdate.OnEventRaised += UpdateHeathBar;
        InitializeHeathBar();
    }
    private void OnDisable()
    {
        _UIHeathUpdate.OnEventRaised -= UpdateHeathBar;
    }
    void InitializeHeathBar()
    {
        _playerHealth.SetMaxHealth(_playerHealthConfig.InitialHealth);
        _playerHealth.SetCurrentHealth(_playerHealthConfig.InitialHealth);
        UpdateHeathBar();
    }
    void UpdateHeathBar()
    {
        _heathBarText.text = _playerHealth.CurrentHeath.ToString() + "/" + _playerHealth.MaxHeath.ToString();
        _heathBar.DOFillAmount((float)_playerHealth.CurrentHeath / (float)_playerHealth.MaxHeath, 0.4f);
    }
}
