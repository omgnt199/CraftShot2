using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadOutUI : MonoBehaviour
{
    [SerializeField] private Image PrimaryWeaponImg;
    [SerializeField] private Image SecondaryWeaponImg;
    [SerializeField] private Image CharacterSkinImg;
    [SerializeField] private Image SupportWeaponImg;
    [SerializeField] private Image NadeImg;

    private void OnEnable()
    {
        Load();
    }

    public void Load()
    {
        PrimaryWeaponImg.sprite = GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSPrimaryWeaponUsing")).Icon;
        SecondaryWeaponImg.sprite = GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSSecondaryWeaponUsing")).Icon;
        SupportWeaponImg.sprite = GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSSupportWeaponUsing")).Icon;
        NadeImg.sprite = GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSNadeUsing")).Icon;
    }
}
