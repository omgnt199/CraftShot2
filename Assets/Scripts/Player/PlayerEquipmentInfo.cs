using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class PlayerEquipmentInfo
{
    public static List<string> EquipmentList = new List<string>();
    public static List<VSEquipment> EquipmentSOList
    {
        get
        {
            List<VSEquipment> list = new List<VSEquipment>();
            foreach (var item in EquipmentList)
            {
                if (GlobalData.Instance.EquipmentPool.GetEquipmentByName(item) != null)
                    list.Add(GlobalData.Instance.EquipmentPool.GetEquipmentByName(item));
            }
            return list;
        }
    }

    const string filePath = "playerEquipmentData.dat";

    public static void Add(string name)
    {
        if (!EquipmentList.Contains(name)) EquipmentList.Add(name);
    }

    public static void Load()
    {
        EquipmentList = new List<string>(DataManager.Load<List<string>>(filePath));
    }

    public static void Save()
    {
        DataManager.Save(filePath, EquipmentList);
    }

    public static List<VSEquipment> GetListEquipmentByType(VSEquipmentType type)
    {
        List<VSEquipment> equipments = new List<VSEquipment>();
        foreach (var item in EquipmentSOList)
        {
            if (item.Type == type) equipments.Add(item);
        }
        return equipments;
    }
    public static VSEquipment GetEquipmentByName(string name)
    {
        foreach (var item in EquipmentSOList)
            if (item.Name == name) return item;
        return null;
    }
}
