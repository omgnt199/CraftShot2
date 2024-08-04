using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Character
{
    public class BotDamageable : Damageable
    {
        private Rigidbody rb;
        private void Awake()
        {
            DeathEvent = ScriptableObject.CreateInstance<VoidEventChannelSO>();
            CurrentHealth = ScriptableObject.CreateInstance<HealthSO>();
            CurrentHealth.SetMaxHealth(HealthConfig.InitialHealth);
            CurrentHealth.SetCurrentHealth(HealthConfig.InitialHealth);

            _Info.HP = CurrentHealth;
        }
        public override void ReceiveDamage(int damage, GameObject byWho)
        {
            if (IsDead) return;
            GetHit = true;

            CurrentHealth.InflictDamage(damage);

            if (CurrentHealth.CurrentHeath <= 0)
            {
                GetComponent<VSBotController>().enabled = false;
                GetComponent<VSBotController>().agent.enabled = false;
                _Info.Deaths++;
                DeathEvent.RaiseEvent();
                //gameObject.SetActive(false);
                IsDead = true;
                gameObject.layer = LayerMask.NameToLayer("IgnoreBulletForce");
                rb = gameObject.AddComponent<Rigidbody>();
                //rb.velocity = transform.forward * 2f;
                GameManager.Instance.OnOnePlayerDead(gameObject);

            }
        }

        public override void Revive()
        {

            GetComponent<VSBotController>().enabled = true;
            GetComponent<VSBotController>().agent.enabled = true;
            CurrentHealth.SetCurrentHealth(HealthConfig.InitialHealth);
            GetComponent<VSBotController>().ControlAnimator.Idle();
            GetComponent<VSBotController>().SearchWalkPoint();

            gameObject.layer = LayerMask.NameToLayer("Enemy");
            Destroy(rb);
            IsDead = false;
            gameObject.SetActive(true);
        }
    }
}
