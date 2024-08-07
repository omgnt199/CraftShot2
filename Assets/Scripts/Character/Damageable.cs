using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Character
{
    public abstract class Damageable : MonoBehaviour
    {
        public VSPlayerInfo _Info;

        public HealthConfigSO HealthConfig;
        public HealthSO CurrentHealth;

        public VoidEventChannelSO DeathEvent = default;
        public VoidEventChannelSO ReviveEvent = default;

        public GameObject TakeDamageVFX;
        public Material TakeDamageMat;

        public GameObject _skinLocate;
        private List<List<Material>> _originMat = new List<List<Material>>();

        public bool IsDead { get; set; }
        public bool GetHit { get; set; }

        private void OnDisable()
        {
            if (IsInvoking("TurnOffTakeMat"))
            {
                CancelInvoke("TurnOffTakeMat");
                TurnOffTakeMat();
            }
        }


        public abstract void ReceiveDamage(int damage, GameObject byWho);
        public abstract void Revive();

        public void SpawnTakeDamageVFX(ContactPoint hitPoint)
        {
            Instantiate(TakeDamageVFX, hitPoint.point, Quaternion.LookRotation(hitPoint.normal));
            if (!IsInvoking("TurnOffTakeMat"))
            {
                TurnOnTakeMat();
                Invoke("TurnOffTakeMat", 0.2f);
            }
        }
        void TurnOnTakeMat()
        {
            _originMat = new List<List<Material>>();
            foreach (SkinnedMeshRenderer mesh in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                //mesh.shadowCastingMode = ShadowCastingMode.Off;
                List<Material> takeDamMats = new List<Material>();
                List<Material> tempMatList = new List<Material>();
                for (int i = 0; i < mesh.materials.Length; i++)
                {
                    tempMatList.Add(mesh.materials[i]);
                    takeDamMats.Add(TakeDamageMat);
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
}
