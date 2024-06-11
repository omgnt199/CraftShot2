using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "ItemPowerPool", menuName = "ScriptableObject/ItemPower/ItemPool")]
public class ItemPowerPoolSO : ScriptableObject
{
    public List<ItemPowerSO> ItemPowerList;

    public ItemPowerSO GetRandomItemPower() => ItemPowerList[new System.Random().Next(ItemPowerList.Count)];
}
