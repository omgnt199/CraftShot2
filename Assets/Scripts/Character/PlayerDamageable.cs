using Assets.Scripts.Character;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlayerDamageable : Damageable
{
    [SerializeField] private VoidEventChannelSO _updateHealthUI = default;
    [SerializeField] private VoidEventChannelSO _takeDamageUI = default;
    private void Awake()
    {
        CurrentHealth.SetMaxHealth(HealthConfig.InitialHealth);
        CurrentHealth.SetCurrentHealth(HealthConfig.InitialHealth);
        _Info.HP = CurrentHealth;
    }
    public override void ReceiveDamage(int damage, GameObject byWho)
    {
        if (IsDead) return;
        GetHit = true;

        CurrentHealth.InflictDamage(damage);

        _updateHealthUI.RaiseEvent();
        _takeDamageUI.RaiseEvent();

        if (CurrentHealth.CurrentHeath <= 0)
        {
            IsDead = true;
            _Info.Deaths++;
            gameObject.SetActive(false);
            VSInGameUIScript.instance.LoadPLayerDeadUI();
            VSDeathCamera.Instance.SetTarget(transform.position, byWho.transform);
            GameManager.Instance.OnOnePlayerDead(gameObject);
            DeathEvent.RaiseEvent();
        }
    }
    public override void Revive()
    {
        CurrentHealth.SetCurrentHealth(100);
        GetComponent<VSPlayerMovement>().ResetCharacterHeight();
        GetComponent<VSPlayerController>().ResetWeapon();
        GetComponent<VSPlayerController>().Anim.Idle();
        VSInGameUIScript.instance.LoadPlayerAfterReviveUI();
        _updateHealthUI.RaiseEvent();
        IsDead = false;
        gameObject.SetActive(true);
    }

}
