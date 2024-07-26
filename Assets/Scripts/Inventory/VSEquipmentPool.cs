using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "VoxelStrikeEquipment/EquipmentPool")]
public class VSEquipmentPool : ScriptableObject
{
    public List<VSEquipment> EquipmentList;

    public List<VSEquipment> GetEquipmentListByType(VSEquipmentType type)
    {
        List<VSEquipment> list = new List<VSEquipment>();
        foreach(var equipment in EquipmentList)
        {
            if (equipment.Type == type) list.Add(equipment);
        }
        return list;
    } 
    public VSEquipment GetEquipmentByName(string name)
    {
        foreach (var equipment in EquipmentList)
        {
            if (equipment.Name.Equals(name,StringComparison.Ordinal)) return equipment;
        }
        return null;
    }

    public VSEquipment GetRandomEquipmentByType(VSEquipmentType type)
    {
        List<VSEquipment> equipments = GetEquipmentListByType(type);
        return equipments[new System.Random().Next(equipments.Count)];
    }
}
