using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopEquipmentCard : MonoBehaviour
{
    public VSEquipment Equipment;
    public Image BorderIcon;
    public Image Icon;
    public TextMeshProUGUI Price;
    public Image CurrencyIcon;
    public Sprite CurrencyCoinIcon;
    public Sprite CurrencyDiamondIcon;
    private List<VSStatics> _statics = new List<VSStatics>();
    private string _Info;
    public Button MainButton;
    public Button BuyButton;
    public List<VSStatics> Statics => _statics;

    [SerializeField] private EquipmentEventChanelSO _unlockEquipmentEvent;
    public void OnSelected()
    {
        BorderIcon.gameObject.SetActive(true);
        foreach (Transform item in transform.parent)
        {
            if (!item.gameObject.Equals(gameObject))
                item.gameObject.GetComponent<ShopEquipmentCard>().BorderIcon.gameObject.SetActive(false);
        }
    }
    public void Set(VSEquipment equipment)
    {

        Equipment = equipment;
        Icon.sprite = equipment.Icon;
        Price.text = Equipment.Price.ToString();
        CurrencyIcon.sprite = Equipment.Currency == CurrencyType.Coin ? CurrencyCoinIcon : CurrencyDiamondIcon;
        //Icon.SetNativeSize();

        if (equipment.Type == VSEquipmentType.PrimaryWeapon || equipment.Type == VSEquipmentType.SecondaryWeapon)
        {
            _statics.Add(new VSStatics("Damage", ((VSGun)equipment).DamageToHead.ToString(), ((VSGun)equipment).DamageToHead / 100f));
            _statics.Add(new VSStatics("Fire rate", ((VSGun)equipment).FireSpeed.ToString() + "/s", 1f / ((VSGun)equipment).FireSpeed / 10f));
            _statics.Add(new VSStatics("Reload time", ((VSGun)equipment).TimeReload.ToString() + "s", 1f / ((VSGun)equipment).TimeReload / 2f));
            MainButton.onClick.AddListener(() => ShopContainerUI.Instance.EquipmentStatUI.ShowInShop(Equipment, _statics));
        }
        else if (equipment.Type == VSEquipmentType.Nade)
        {
            _statics.Add(new VSStatics("Radius", ((VSNade)equipment).ExplosionRadius.ToString(), ((VSNade)equipment).ExplosionRadius / 5f));
            _statics.Add(new VSStatics("Damage", ((VSNade)equipment).Maxdamage.ToString(), ((VSNade)equipment).Maxdamage / 100f));

            MainButton.onClick.AddListener(() => ShopContainerUI.Instance.EquipmentStatUI.ShowInShop(Equipment, _statics));
        }
        else if (equipment.Type == VSEquipmentType.SupportWeapon)
        {
            _Info = ((VSSupportWeapon)equipment).Info;
            MainButton.onClick.AddListener(() => ShopContainerUI.Instance.EquipmentStatUI.ShowInShop(equipment, _Info));
        }
        else if (equipment.Type == VSEquipmentType.Character)
        {
            MainButton.onClick.AddListener(() => ShopContainerUI.Instance.EquipmentStatUI.ShowCharacterSkinInShop(equipment));
        }
    }

    public void Buy()
    {
        if (CurrencyData.GetCurrencyValue(Equipment.Currency) >= Equipment.Price)
        {
            CurrencyData.UpdateCurrency(Equipment.Currency, -Equipment.Price);
            PlayerEquipmentInfo.Add(Equipment.Name);
            PlayerEquipmentInfo.Save();
            BuyButton.gameObject.SetActive(false);

            ShopContainerUI.Instance.EquipmentStatUI.LoadMainButtonBehaviorInShop(Equipment);

            GlobalUI.Instance.ShowPopUp("BuyEquipment");
            _unlockEquipmentEvent.RaiseEvent(Equipment);
        }
        //else GlobalUI.Instance.ShowPopUp("ShopCurrency");
    }

}
