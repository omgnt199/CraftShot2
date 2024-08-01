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
        Vector2 rectSize = EquipmentContent.GetComponent<RectTransform>().rect.size;
        EquipmentContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2((rectSize.x - 50f - EquipmentContent.GetComponent<GridLayoutGroup>().spacing.x * 3) / 4f, EquipmentContent.GetComponent<GridLayoutGroup>().cellSize.y);
    }
    private void OnEnable()
    {
        LoadInventory(_equipmentType);
        Commons.WaitNextFrame(this, () => ShowEquipmentByIndex(0));
    }

    public void SetEquipementType(string equipementType) => _equipmentType = Commons.ToEnum<VSEquipmentType>(equipementType);

    public void ReloadWhenEquip() => LoadInventory(_equipmentType);
    private List<VSEquipment> GetListEquipmentByType(VSEquipmentType type)
    {
        return PlayerEquipmentInfo.GetListEquipmentByType(type);
    }

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
        rowAmount = Mathf.CeilToInt((float)listEquipment.Count / 3f);
        foreach (VSEquipment equipment in listEquipment)
        {
            GameObject equipmentCard = Instantiate(EquipmentCardPrefab, EquipmentContent.transform);
            equipmentCard.GetComponent<VSEquipmentCard>().Set(equipment);
        }
        //Resize EquipmentContent
        EquipmentContent.GetComponent<RectTransform>().sizeDelta = _equipmentContentSizeDefault;
        if (rowAmount > 2) EquipmentContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 400f * (rowAmount - 2));
    }

    public void ShowEquipmentByIndex(int index)
    {
        if (EquipmentContent.transform.childCount > 0)
            EquipmentContent.transform.GetChild(index).GetComponent<VSEquipmentCard>().MainButton.onClick?.Invoke();
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
        if (str2.Length > str1.Length) return false;
        for (int i = 0; i < str2.Length; i++)
        {
            if (char.ToLower(str2[i]) != char.ToLower(str1[i]) && char.ToUpper(str2[i]) != char.ToUpper(str1[i]))
                return false;
        }

        //foreach (var c in str2)
        //{
        //    if (!str1.Contains(char.ToLower(c)) && !str1.Contains(char.ToUpper(c))) return false;
        //}
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
