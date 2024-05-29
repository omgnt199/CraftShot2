using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private HealthConfigSO _healthConfig;
    [SerializeField] private HealthSO _currentHealth;

    [SerializeField] private VoidEventChannelSO _updateHealthUI = default;
    [SerializeField] private VoidEventChannelSO _takeDamageUI = default;
    [SerializeField] private VoidEventChannelSO _deathEvent = default;

    public bool IsDead { get; set; }
    public bool GetHit {  get; set; }

    public UnityAction OnDie;
    private void Awake()
    {
        if(_currentHealth == null)
        {
            _currentHealth = ScriptableObject.CreateInstance<HealthSO>();
        }
        _currentHealth.SetMaxHealth(_healthConfig.InitialHealth);
        _currentHealth.SetCurrentHealth(_healthConfig.InitialHealth);
    }

    public void ReceiveDamage(int damage)
    {
        if (IsDead) return;
        GetHit = true;

        _currentHealth.InflictDamage(damage);
        if (_updateHealthUI != null) _updateHealthUI.RaiseEvent();
        if(_takeDamageUI != null) _takeDamageUI.RaiseEvent();

        if (_currentHealth.CurrentHeath <= 0)
        {
            IsDead = true;

            if (OnDie != null)
                OnDie.Invoke();

            if (_deathEvent != null)
                _deathEvent.RaiseEvent();

        }
    }
    void Revive()
    {
        if (_currentHealth != null)
        {
            _currentHealth.SetCurrentHealth(_healthConfig.InitialHealth);
        }
        if (_updateHealthUI != null) _updateHealthUI.RaiseEvent();
        IsDead = false;
    }
}
