using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Damageable : MonoBehaviour
{
    [SerializeField] private HealthConfigSO _healthConfig;
    [SerializeField] private HealthSO _currentHealth;

    [SerializeField] private VoidEventChannelSO _updateHealthUI = default;
    [SerializeField] private VoidEventChannelSO _takeDamageUI = default;
    [SerializeField] private VoidEventChannelSO _deathEvent = default;

    [SerializeField] private GameObject _takeDamageVFX;
    [SerializeField] private Material _takeDamageMat;
    private List<List<Material>> _originMat = new List<List<Material>>();

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

    public void SpawnTakeDamageVFX(ContactPoint hitPoint)
    {
        Instantiate(_takeDamageVFX, hitPoint.point, Quaternion.LookRotation(hitPoint.normal));
        TurnOnTakeMat();
        if (IsInvoking("TurnOffTakeMat"))
        {
            CancelInvoke("TurnOffTakeMat");
            Invoke("TurnOffTakeMat", 0.2f);
        }
        else Invoke("TurnOffTakeMat", 0.2f);
    }
    void TurnOnTakeMat()
    {
        foreach (SkinnedMeshRenderer mesh in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            //mesh.shadowCastingMode = ShadowCastingMode.Off;
            List<Material> takeDamMats = new List<Material>();
            List<Material> tempMatList = new List<Material>();
            for (int i = 0; i < mesh.materials.Length; i++)
            {
                tempMatList.Add(mesh.materials[i]);
                takeDamMats.Add(_takeDamageMat);
            }
            _originMat.Add(tempMatList);
            mesh.SetMaterials(takeDamMats);
        }
    }

    void TurnOffTakeMat()
    {
        SkinnedMeshRenderer[] meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        int N = meshRenderers.Length;
        for (int i = 0; i < N; i++)
        {
            //meshRenderers[i].shadowCastingMode = ShadowCastingMode.On;
            List<Material> tempOriginMats = new List<Material>();
            for (int j = 0; j < meshRenderers[i].materials.Length; j++)
            {
                tempOriginMats.Add(_originMat[i][j]);
            }
            meshRenderers[i].SetMaterials(tempOriginMats);
        }
    }
}
