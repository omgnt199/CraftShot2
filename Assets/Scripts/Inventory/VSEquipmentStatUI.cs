using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VSEquipmentStatUI : MonoBehaviour
{
    public SkinSpinUI SkinSpin;
    public GameObject WeaponStat;
    public GameObject StatCardPrefab;
    public TextMeshProUGUI Name;
    public Button MainBtn;
    public TextMeshProUGUI MainBtnText;
    public TextMeshProUGUI MoreInfo;
    public Transform OnlySkinLocate;
    private VSEquipment _currentEquipent;
    [SerializeField] private EquipmentEventChanelSO _equipEquipmentLoadOut;
    [SerializeField] private EquipmentEventChanelSO _unlockEquipmentEvent;
    private void OnEnable()
    {

    }
    public void Reset()
    {
        SkinSpin.Reset();
        foreach (Transform item in WeaponStat.transform) Destroy(item.gameObject);
        Name.text = string.Empty;
        MainBtn.onClick.RemoveAllListeners();
        MoreInfo.text = string.Empty;
        _currentEquipent = null;
    }


    public void ShowInShop(VSEquipment equipment, List<VSStatics> statics)
    {
        Reset();

        LoadMainButtonBehaviorInShop(equipment);

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

    public void ShowInShop(VSEquipment equipment, string info)
    {
        Reset();

        LoadMainButtonBehaviorInShop(equipment);

        WeaponStat.SetActive(false);
        MoreInfo.gameObject.SetActive(true);
        _currentEquipent = equipment;
        Name.text = equipment.Name;
        MoreInfo.text = info;

        if (OnlySkinLocate.transform.childCount > 0) foreach (Transform item in OnlySkinLocate) Destroy(item.gameObject);
        Instantiate(equipment.OnlyModel, OnlySkinLocate);
    }

    public void LoadMainButtonBehaviorInShop(VSEquipment equipment)
    {
        MainBtn.onClick.RemoveListener(Buy);
        MainBtn.onClick.RemoveListener(ToLoadOut);
        if (!PlayerEquipmentInfo.EquipmentSOList.Contains(equipment))
        {
            MainBtn.onClick.AddListener(Buy);
            MainBtnText.text = "BUY";
        }
        else
        {
            MainBtn.onClick.AddListener(ToLoadOut);
            MainBtnText.text = "TO\nLOADOUTS";
        }
    }


    public void ShowInInventory(VSEquipment equipment, List<VSStatics> statics)
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

    public void ShowInInventory(VSEquipment equipment, string info)
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

    public void ShowCharacterSkinInShop(VSEquipment equipment)
    {
        Reset();

        LoadMainButtonBehaviorInShop(equipment);

        Name.text = equipment.Name;
        if (OnlySkinLocate.transform.childCount > 0) foreach (Transform item in OnlySkinLocate) Destroy(item.gameObject);
        Instantiate(equipment.OnlyModel, OnlySkinLocate);
    }

    public void ShowCharacterSkinInInventory(VSEquipment equipment)
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
        if (_currentEquipent.Type == VSEquipmentType.PrimaryWeapon || _currentEquipent.Type == VSEquipmentType.SecondaryWeapon)
            _equipEquipmentLoadOut.RaiseEvent(_currentEquipent);
    }

    public void ToLoadOut()
    {
        HomeUI.Instance.ShowPopUp("ArmoryPopUp");
        ArmoryPopUpUI.Instance.ShowPopUp("LoadOut");
    }

    public void Buy()
    {
        if (CurrencyData.GetCurrencyValue(_currentEquipent.Currency) >= _currentEquipent.Price)
        {
            CurrencyData.UpdateCurrency(_currentEquipent.Currency, -_currentEquipent.Price);
            PlayerEquipmentInfo.Add(_currentEquipent.Name);
            PlayerEquipmentInfo.Save();
            GlobalUI.Instance.ShowPopUp("BuyEquipment");

            foreach (Transform item in ShopContainerUI.Instance.EquipmentContent.transform)
            {
                if (item.GetComponent<ShopEquipmentCard>().Equipment == _currentEquipent)
                {
                    item.GetComponent<ShopEquipmentCard>().MainButton.gameObject.SetActive(false);
                    break;
                }
            }

            MainBtn.onClick.RemoveListener(Buy);
            MainBtn.onClick.AddListener(ToLoadOut);
            MainBtnText.text = "TO\nLOADOUTS";

            _unlockEquipmentEvent.RaiseEvent(_currentEquipent);
        }
        else GlobalUI.Instance.ShowPopUp("ShopCurrency");
    }
}
