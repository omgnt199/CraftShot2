using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ItemPower
{
    [CreateAssetMenu(fileName = "InvicibleItemPower", menuName = "ScriptableObject/ItemPower/InviciblePower")]
    public class InvicibleItemPowerSO : ItemPowerSO
    {
        [SerializeField] private GameObject InviciblePrefab;
        private GameObject _player;
        public override void Apply(GameObject Player)
        {
            _player = Player;
            Player.layer = LayerMask.NameToLayer("ObstacleLayer");
        }

        public override void Deactive()
        {
            _player.layer = LayerMask.NameToLayer("Player");
        }
    }
}
