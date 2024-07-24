using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Assets.Scripts.Common;

public class VSInventoryUIController : MonoBehaviour
{
    public static VSInventoryUIController Instance;
    public VSEquipmentStatUI EquipmentStatUI;
    public GameObject EquipmentContent;
    public GameObject EquipmentRowPrefab;
    public GameObject EquipmentCardPrefab;
    public VSEquipmentPool EquipmentPool;
    private List<VSEquipment> _listEquipment = new List<VSEquipment>();

    private VSEquipmentType _equipmentType;
    private Vector2 _equipmentContentSizeDefault;

    public int MaxCardPerRow;
    private void Awake()
    {
        Instance = this;
        _equipmentContentSizeDefault = EquipmentContent.GetComponent<RectTransform>().sizeDelta;
    }
    private void OnEnable()
    {
        LoadInventory(_equipmentType);
        ShowEquipmentByIndex(0);
    }

    public void SetEquipementType(string equipementType) => _equipmentType = Commons.ToEnum<VSEquipmentType>(equipementType);

    public void ReloadWhenEquip() => LoadInventory(_equipmentType);
    private List<VSEquipment> GetListEquipmentByType(VSEquipmentType type) => new List<VSEquipment>(EquipmentPool.GetEquipmentListByType(type));

    public void LoadInventory(VSEquipmentType type)
    {
        _equipmentType = type;
        _listEquipment = new List<VSEquipment>(GetListEquipmentByType(_equipmentType));
        LoadInventoryUI(_listEquipment);
    }

    private void LoadInventoryUI(List<VSEquipment> listEquipment)
    {
        foreach (Transform card in EquipmentContent.transform) Destroy(card.gameObject);
        int rowAmount;
        int cardAmount;
        rowAmount = listEquipment.Count / MaxCardPerRow + 1;
        cardAmount = listEquipment.Count;
        for (int i = 0; i < rowAmount; i++)
        {
            GameObject rowTemp = Instantiate(EquipmentRowPrefab, EquipmentContent.transform);
            //rowTemp.GetComponent<RectTransform>().sizeDelta = new Vector2(EquipmentContent.GetComponent<RectTransform>().sizeDelta.x, rowTemp.GetComponent<RectTransform>().sizeDelta.y);
            int itemNumber = 0;
            while (itemNumber < MaxCardPerRow && i * MaxCardPerRow + itemNumber < cardAmount)
            {
                GameObject equipmentCard = Instantiate(EquipmentCardPrefab, rowTemp.transform);
                equipmentCard.GetComponent<VSEquipmentCard>().Set(listEquipment[i * MaxCardPerRow + itemNumber]);
                itemNumber++;
            }
        }
        //Resize EquipmentContent
        EquipmentContent.GetComponent<RectTransform>().sizeDelta = _equipmentContentSizeDefault;
        if (rowAmount > 2) EquipmentContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 400f * (rowAmount - 2));
    }

    public void ShowEquipmentByIndex(int index)
    {
        int rowIndex = index / MaxCardPerRow;
        int cardIndex = index % MaxCardPerRow;
        EquipmentContent.transform.GetChild(rowIndex).GetChild(cardIndex).GetComponent<VSEquipmentCard>().MainButton.onClick?.Invoke();
    }

    public void Search(string name)
    {
        List<VSEquipment> listEquipment = new List<VSEquipment>();
        if (name == "" || name == null) LoadInventory(this._equipmentType);
        else
        {
            foreach (VSEquipmentCard equipmentCard in EquipmentContent.GetComponentsInChildren<VSEquipmentCard>())
            {
                if (IsContainString(equipmentCard.Equipment.Name, name)) listEquipment.Add(equipmentCard.Equipment);
            }
            LoadInventoryUI(listEquipment);
        }
    }
    bool IsContainString(string str1, string str2)
    {
        foreach (var c in str2)
        {
            if (!str1.Contains(char.ToLower(c)) && !str1.Contains(char.ToUpper(c))) return false;
        }
        return true;
    }
}



public enum VSEquipmentType
{
    PrimaryWeapon,
    SecondaryWeapon,
    SupportWeapon,
    Nade,
    Character
}
