using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VSEquipmentStatUI : MonoBehaviour
{

    public GameObject WeaponStat;
    public GameObject StatCardPrefab;
    public TextMeshProUGUI Name;
    public Button MainBtn;
    public TextMeshProUGUI MoreInfo;
    public Transform OnlySkinLocate;
    private VSEquipment _currentEquipent;
    private void OnEnable()
    {

    }
    public void Reset()
    {
        foreach (Transform item in WeaponStat.transform) Destroy(item.gameObject);
        Name.text = string.Empty;
        MainBtn.onClick.RemoveAllListeners();
        MoreInfo.text = string.Empty;
        _currentEquipent = null;
    }

    public void Show(VSEquipment equipment, List<VSStatics> statics)
    {
        Reset();
        WeaponStat.SetActive(true);
        MoreInfo.gameObject.SetActive(false);

        Name.text = equipment.Name;
        _currentEquipent = equipment;

        foreach (Transform stat in WeaponStat.transform) Destroy(stat.gameObject);

        foreach (var stat in statics)
        {
            GameObject card = Instantiate(StatCardPrefab, WeaponStat.transform);
            card.GetComponent<VSStaticCard>().Set(stat.Name, stat.Unit, stat.Rate);
        }

        if (OnlySkinLocate.transform.childCount > 0) foreach (Transform item in OnlySkinLocate) Destroy(item.gameObject);
        Instantiate(equipment.OnlyModel, OnlySkinLocate);
    }

    public void Show(VSEquipment equipment, string info)
    {
        Reset();
        WeaponStat.SetActive(false);
        MoreInfo.gameObject.SetActive(true);
        _currentEquipent = equipment;
        Name.text = equipment.Name;
        MoreInfo.text = info;

        if (OnlySkinLocate.transform.childCount > 0) foreach (Transform item in OnlySkinLocate) Destroy(item.gameObject);
        Instantiate(equipment.OnlyModel, OnlySkinLocate);
    }

    public void ShowCharacterSkin(VSEquipment equipment)
    {
        Reset();
        Name.text = equipment.Name;
        if (OnlySkinLocate.transform.childCount > 0) foreach (Transform item in OnlySkinLocate) Destroy(item.gameObject);
        Instantiate(equipment.OnlyModel, OnlySkinLocate);
    }
    public void Equip()
    {
        if (_currentEquipent == null) return;
        if (_currentEquipent.Type == VSEquipmentType.PrimaryWeapon)
            PlayerPrefs.SetString("VSPrimaryWeaponUsing", _currentEquipent.Name);
        else if (_currentEquipent.Type == VSEquipmentType.SecondaryWeapon)
            PlayerPrefs.SetString("VSSecondaryWeaponUsing", _currentEquipent.Name);
        else if (_currentEquipent.Type == VSEquipmentType.SupportWeapon)
            PlayerPrefs.SetString("VSSupportWeaponUsing", _currentEquipent.Name);
        else if (_currentEquipent.Type == VSEquipmentType.Nade)
            PlayerPrefs.SetString("VSNadeUsing", _currentEquipent.Name);
        else if (_currentEquipent.Type == VSEquipmentType.Character)
            PlayerPrefs.SetString("VSCharacterSkinUsing", _currentEquipent.Name);
        VSInventoryUIController.Instance.ReloadWhenEquip();
    }

    public void ToLoadOut()
    {
        HomeUI.Instance.ShowPopUp("ArmoryPopUp");
        ArmoryPopUpUI.Instance.ShowPopUp("LoadOut");
    }
}
