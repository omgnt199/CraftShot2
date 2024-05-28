using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SaveLoad
{
    public SaveLoad GameData;

}


[System.Serializable]
public class VoxelStrikeUserData
{
    public List<string> WeaponSkinOwn;
    public VoxelStrikeUserData()
    {
        this.WeaponSkinOwn = new List<string>() { "VSP-01", "VSP-02", "VSS-01" };
    }
}
