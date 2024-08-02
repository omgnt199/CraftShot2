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
    public Image BorderIcon;

    public GameObject EquippedBorder;

    private List<VSStatics> _statics = new List<VSStatics>();
    private string _Info;
    public Button MainButton;
    public List<VSStatics> Statics => _statics;
    public void OnSelected()
    {
        BorderIcon.gameObject.SetActive(true);
        foreach (Transform item in transform.parent)
        {
            if (!item.gameObject.Equals(gameObject))
                item.gameObject.GetComponent<VSEquipmentCard>().BorderIcon.gameObject.SetActive(false);
        }
        if(EquippedBorder.activeSelf) BorderIcon.gameObject.SetActive(false);
    }
    public void Set(VSEquipment equipment)
    {
        Equipment = equipment;
        Name.text = equipment.Name;
        Icon.sprite = equipment.Icon;
        //Icon.SetNativeSize();

        if (equipment.Name == PlayerPrefs.GetString("VSPrimaryWeaponUsing") || equipment.Name == PlayerPrefs.GetString("VSSecondaryWeaponUsing")
            || equipment.Name == PlayerPrefs.GetString("VSNadeUsing") || equipment.Name == PlayerPrefs.GetString("VSSupportWeaponUsing")
            || equipment.Name == PlayerPrefs.GetString("VSCharacterSkinUsing"))
            EquippedBorder.SetActive(true);
        else EquippedBorder.SetActive(false);

        if (equipment.Type == VSEquipmentType.PrimaryWeapon || equipment.Type == VSEquipmentType.SecondaryWeapon)
        {
            _statics.Add(new VSStatics("Damage", ((VSGun)equipment).DamageToHead.ToString(), ((VSGun)equipment).DamageToHead / 100f));
            _statics.Add(new VSStatics("Fire rate", ((VSGun)equipment).FireSpeed.ToString() + "/s", 1f / ((VSGun)equipment).FireSpeed / 10f));
            _statics.Add(new VSStatics("Reload time", ((VSGun)equipment).TimeReload.ToString() + "s", 1f / ((VSGun)equipment).TimeReload / 2f));
            MainButton.onClick.AddListener(() => VSInventoryUIController.Instance.EquipmentStatUI.ShowInInventory(Equipment, _statics));
        }
        else if (equipment.Type == VSEquipmentType.Nade)
        {
            _statics.Add(new VSStatics("Radius", ((VSNade)equipment).ExplosionRadius.ToString(), ((VSNade)equipment).ExplosionRadius / 5f));
            _statics.Add(new VSStatics("Damage", ((VSNade)equipment).Maxdamage.ToString(), ((VSNade)equipment).Maxdamage / 100f));

            MainButton.onClick.AddListener(() => VSInventoryUIController.Instance.EquipmentStatUI.ShowInInventory(Equipment, _statics));
        }
        else if (equipment.Type == VSEquipmentType.SupportWeapon)
        {
            _Info = ((VSSupportWeapon)equipment).Info;
            MainButton.onClick.AddListener(() => VSInventoryUIController.Instance.EquipmentStatUI.ShowInInventory(equipment, _Info));
        }
        else if (equipment.Type == VSEquipmentType.Character)
        {
            MainButton.onClick.AddListener(() => VSInventoryUIController.Instance.EquipmentStatUI.ShowCharacterSkinInInventory(equipment));
        }
    }

}
