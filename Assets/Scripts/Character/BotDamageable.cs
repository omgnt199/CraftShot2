using Assets.Scripts.Common;
using DG.Tweening;
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
                _Info.Deaths++;
                DeathEvent.RaiseEvent();
                //gameObject.SetActive(false);

                GetComponentInChildren<BodyPartBehaviour>().IgnoreBulletForce();
                gameObject.layer = LayerMask.NameToLayer("IgnoreBulletForce");

                GetComponent<VSBotController>().agent.enabled = false;
                GetComponent<VSBotController>().enabled = false;
                GetComponent<VSBotController>().ControlAnimator.Controller.enabled = false;
                IsDead = true;

                rb = gameObject.AddComponent<Rigidbody>();
                rb.mass = 5f;
                //rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                //rb.AddForceAtPosition((transform.position - (byWho.transform.position + new Vector3(0, -4f, 0))).normalized * 20f, transform.position, ForceMode.Impulse);
                rb.AddForceAtPosition((transform.position - byWho.transform.position).normalized * 25f, transform.position + new Vector3(0, 2f, 0), ForceMode.Impulse);
                rb.AddForceAtPosition((transform.position - byWho.transform.position).normalized * 25f, transform.position + new Vector3(0, 1.3f, 0), ForceMode.Impulse);
                rb.AddForceAtPosition(transform.up * 30f, transform.position, ForceMode.Impulse);

                //rb.AddExplosionForce(10f, transform.position, 1f, 5f);
                int chanceToSpawnItemPower = UnityEngine.Random.Range(1, 101);
                if (chanceToSpawnItemPower <= 35)
                {
                    Instantiate(GameManager.Instance.PowerPool.GetRandomItemPower().Prefab, transform.position + new Vector3(0, 2f, 0), Quaternion.LookRotation(Vector3.up));
                }
                GameManager.Instance.OnOnePlayerDead(gameObject);



            }
        }

        public override void Revive()
        {

            gameObject.SetActive(true);
            GetComponentInChildren<BodyPartBehaviour>().ApllyBulletForce();
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            Destroy(rb);

            GetComponent<VSBotController>().ControlAnimator.Controller.enabled = true;
            GetComponent<VSBotController>().agent.enabled = true;
            GetComponent<VSBotController>().enabled = true;

            GetComponent<VSBotController>().agent.enabled = true;
            CurrentHealth.SetCurrentHealth(HealthConfig.InitialHealth);
            GetComponent<VSBotController>().ControlAnimator.Idle();
            GetComponent<VSBotController>().SearchWalkPoint();

            Destroy(rb);
            IsDead = false;
        }
    }
}
