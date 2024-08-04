using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
[CreateAssetMenu(fileName = "Heath", menuName = "ScriptableObject/HeathSO")]
public class HealthSO : ScriptableObject
{
    [SerializeField][ReadOnly] private int _maxHealth;
    [SerializeField][ReadOnly] private int _currentHealth;
    public int MaxHeath => _maxHealth;
    public int CurrentHeath => _currentHealth;
    public void SetMaxHealth(int newValue)
    {
        _maxHealth = newValue;
    }

    public void SetCurrentHealth(int newValue)
    {
        _currentHealth = newValue;
    }

    public void InflictDamage(int DamageValue)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - DamageValue);
    }

    public void RestoreHealth(int HealthValue)
    {
        _currentHealth += HealthValue;
        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;
    }
}
