using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemReceive
{
    public Sprite Icon;
    public int Amount;

    public ItemReceive(Sprite Icon, int Amount)
    {
        this.Icon = Icon;
        this.Amount = Amount;
    }
}
public class ReceiveItemOverlay : MonoBehaviour
{
    public static ReceiveItemOverlay Instance;
    [SerializeField] private RectTransform _Content;
    [SerializeField] private GameObject ReceiveItemPrefab;

    [SerializeField] private Sprite CoinReceiveItemIcon;
    [SerializeField] private Sprite DiamondReceiveItemIcon;
    [SerializeField] private Sprite ExpReceiveItemIcon;

    public List<ItemReceive> ReceiveItemList;
    private void Awake()
    {
        Instance = this;
    }

    public void OnEnable()
    {
    }
    private void OnDisable()
    {
        ReceiveItemList = new List<ItemReceive>();
    }
    public void SetUI()
    {
        foreach (var item in ReceiveItemList)
        {
            GameObject receiveItem = Instantiate(ReceiveItemPrefab, _Content.transform);
            receiveItem.GetComponent<ReceiveItem>().Set(item.Icon, item.Amount);
        }
    }

    public void AddCurrencyReceiveItem(CurrencyType type, int value)
    {
        if (type == CurrencyType.Coin) ReceiveItemList.Add(new ItemReceive(CoinReceiveItemIcon, value));
        else if (type == CurrencyType.Diamond) ReceiveItemList.Add(new ItemReceive(DiamondReceiveItemIcon, value));
    }

    public void AddEquipmentReceiveItem(VSEquipment equipment, int value)
    {
        ReceiveItemList.Add(new ItemReceive(equipment.Icon, value));
    }
}
