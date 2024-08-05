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
        [SerializeField] private GameObject IgnoreBullet;
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
                gameObject.SetActive(false);
                GetComponent<VSBotController>().agent.enabled = false;
                GetComponent<VSBotController>().enabled = false;
                IsDead = true;

                //gameObject.layer = LayerMask.NameToLayer("IgnoreBulletForce");
                //rb = gameObject.AddComponent<Rigidbody>();
                //rb.interpolation = RigidbodyInterpolation.Interpolate;
                //rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                //rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);

                //transform.DOJump(transform.position + new Vector3(UnityEngine.Random.Range(-20f, 20f), transform.position.y, UnityEngine.Random.Range(-20f, 20f)), 3f, 1, 3f);
                //transform.DORotate(new Vector3(UnityEngine.Random.Range(0, 180f), UnityEngine.Random.Range(0f, 180f), UnityEngine.Random.Range(0, 180f)), 3f, RotateMode.FastBeyond360);
                IgnoreBullet.SetActive(true);

                int chanceToSpawnItemPower = UnityEngine.Random.Range(1, 101);
                if (chanceToSpawnItemPower <= 25)
                {
                    Instantiate(GameManager.Instance.PowerPool.GetRandomItemPower().Prefab, transform.position + new Vector3(0, 2f, 0), Quaternion.LookRotation(Vector3.up));
                }
                GameManager.Instance.OnOnePlayerDead(gameObject);



            }
        }

        public override void Revive()
        {
            IgnoreBullet.SetActive(false);
            gameObject.SetActive(true);

            GetComponent<VSBotController>().agent.enabled = true;
            GetComponent<VSBotController>().enabled = true;

            GetComponent<VSBotController>().agent.enabled = true;
            CurrentHealth.SetCurrentHealth(HealthConfig.InitialHealth);
            GetComponent<VSBotController>().ControlAnimator.Idle();
            GetComponent<VSBotController>().SearchWalkPoint();

            //gameObject.layer = LayerMask.NameToLayer("Enemy");
            Destroy(rb);
            IsDead = false;
        }
    }
}
