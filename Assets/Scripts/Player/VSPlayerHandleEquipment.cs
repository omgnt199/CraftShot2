using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSPlayerHandleEquipment : MonoBehaviour
{
    [Header("Equipment")]
    public GameObject PrimaryWeapon;
    public GameObject SecondaryWeapon;
    public GameObject SupportWeapon;
    public GameObject Nade;
    private VSGun _primaryWeaponInfo;
    private VSGun _secondaryWeaponInfo;
    private VSSupportWeapon _supportWeaponInfo;
    private VSNade _nadeInfo;
    private GameObject _weaponUsing;

    public void SetUpEquipment(VSEquipment equipment, Transform equipmentLocate)
    {

    }
}
