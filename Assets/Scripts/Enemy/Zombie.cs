using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private HealthConfigSO _zombieHealthConfig;
    [SerializeField] private HealthSO _currentHealth;
    private void Awake()
    {
        _currentHealth = ScriptableObject.CreateInstance<HealthSO>();
        _currentHealth.SetMaxHealth(_zombieHealthConfig.InitialHealth);
        _currentHealth.SetCurrentHealth(_zombieHealthConfig.InitialHealth);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
