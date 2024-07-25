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
    public Image Icon;
    public TextMeshProUGUI MoreInfo;

    private VSEquipment _currentEquipent;

    public void Show(VSEquipment equipment, List<VSStatics> statics)
    {

        WeaponStat.SetActive(true);
        MoreInfo.gameObject.SetActive(false);

        Name.text = equipment.Name;
        _currentEquipent = equipment;
        Icon.sprite = _currentEquipent.Icon;

        foreach (Transform stat in WeaponStat.transform) Destroy(stat.gameObject);

        foreach (var stat in statics)
        {
            GameObject card = Instantiate(StatCardPrefab, WeaponStat.transform);
            card.GetComponent<VSStaticCard>().Set(stat.Name, stat.Unit, stat.Rate);
        }
    }

    public void Show(VSEquipment equipment, string info)
    {

        WeaponStat.SetActive(false);
        MoreInfo.gameObject.SetActive(true);

        _currentEquipent = equipment;
        Name.text = equipment.Name;
        Icon.sprite = _currentEquipent.Icon;
        MoreInfo.text = info;
    }

    public void Equip()
    {
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
