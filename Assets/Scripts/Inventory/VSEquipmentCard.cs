using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VSEquipmentCard : MonoBehaviour
{
    public VSEquipment Equipment;

    public TextMeshProUGUI Name;
    public Image Icon;

    public GameObject Equipped;

    private List<VSStatics> _statics = new List<VSStatics>();
    private string _Info;

    public void Set(VSEquipment equipment)
    {
        Equipment = equipment;
        Name.text = Equipment.Name;
        Icon.sprite = Equipment.Icon;
        Icon.SetNativeSize();

        if (equipment.Type == VSEquipmentType.Weapon)
        {
            _statics.Add(new VSStatics("Damage", ((VSGun)equipment).DamageToHead.ToString(), ((VSGun)equipment).DamageToHead / 100f));
            _statics.Add(new VSStatics("Fire rate", ((VSGun)equipment).FireSpeed.ToString() + "/s", 1f / ((VSGun)equipment).FireSpeed / 10f));
            _statics.Add(new VSStatics("Reload time", ((VSGun)equipment).TimeReload.ToString() + "s", 1f / ((VSGun)equipment).TimeReload / 2f));
            if (equipment.Name == PlayerPrefs.GetString("VSPrimaryWeaponUsing")) Equipped.SetActive(true);
            else if (equipment.Name == PlayerPrefs.GetString("VSSecondaryWeaponUsing")) Equipped.SetActive(true);
            GetComponent<Button>().onClick.AddListener(() => VSEquipmentStatUI.Instance.Show(Equipment, _statics));
        }
        else if(equipment.Type == VSEquipmentType.Nade)
        {
            _statics.Add(new VSStatics("Radius", ((VSNade)equipment).ExplosionRadius.ToString(), ((VSNade)equipment).ExplosionRadius / 5f));
            _statics.Add(new VSStatics("Damage", ((VSNade)equipment).Maxdamage.ToString(), ((VSNade)equipment).Maxdamage / 100f));

            if (equipment.Name == PlayerPrefs.GetString("VSNadeUsing")) Equipped.SetActive(true);
            GetComponent<Button>().onClick.AddListener(() => VSEquipmentStatUI.Instance.Show(Equipment, _statics));
        }
        else if(equipment.Type == VSEquipmentType.SupportWeapon)
        {
            _Info = ((VSSupportWeapon)equipment).Info;

            if (equipment.Name == PlayerPrefs.GetString("VSSupportWeaponUsing")) Equipped.SetActive(true);
            GetComponent<Button>().onClick.AddListener(() => VSEquipmentStatUI.Instance.Show(equipment,_Info));
        }
    }

}
