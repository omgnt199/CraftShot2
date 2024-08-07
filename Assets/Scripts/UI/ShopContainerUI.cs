using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopContainerUI : MonoBehaviour
{
    public static ShopContainerUI Instance;
    public VSEquipmentStatUI EquipmentStatUI;
    public GameObject EquipmentContent;
    public GameObject EquipmentCardPrefab;
    public VSEquipmentPool EquipmentPool;
    private List<VSEquipment> _listEquipment = new List<VSEquipment>();

    public GameObject GunTab;
    public GameObject SupportWeaponTab;
    public GameObject NadeTab;
    public GameObject CharacterTab;
    private Dictionary<ShopEquipementType, GameObject> EquipmentTab;
    private ShopEquipementType _typeTabing;
    private Vector2 _equipmentContentSizeDefault;
    private void Awake()
    {
        Instance = this;
        EquipmentTab = new Dictionary<ShopEquipementType, GameObject>()
        {
            { ShopEquipementType.Gun,GunTab },
            { ShopEquipementType.Support,SupportWeaponTab },
            { ShopEquipementType.Nade,NadeTab },
            { ShopEquipementType.Character,CharacterTab }
        };
        _equipmentContentSizeDefault = EquipmentContent.GetComponent<RectTransform>().sizeDelta;
        GunTab.GetComponent<Button>().onClick.AddListener(delegate { Tab(ShopEquipementType.Gun); });
        SupportWeaponTab.GetComponent<Button>().onClick.AddListener(delegate { Tab(ShopEquipementType.Support); });
        NadeTab.GetComponent<Button>().onClick.AddListener(delegate { Tab(ShopEquipementType.Nade); });
        CharacterTab.GetComponent<Button>().onClick.AddListener(delegate { Tab(ShopEquipementType.Character); });
    }
    private void OnEnable()
    {
        GunTab.GetComponent<Button>().onClick?.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {

        //EquipmentContent.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<VSEquipment> GetListEquipmentByShopType(ShopEquipementType type)
    {
        List<VSEquipment> listEquipment = new List<VSEquipment>();
        List<VSEquipment> tempList = new List<VSEquipment>();

        switch (type)
        {
            case ShopEquipementType.Gun:
                tempList = EquipmentPool.GetEquipmentListByType(VSEquipmentType.PrimaryWeapon).Concat(EquipmentPool.GetEquipmentListByType(VSEquipmentType.SecondaryWeapon)).ToList();
                break;
            case ShopEquipementType.Support:
                tempList = EquipmentPool.GetEquipmentListByType(VSEquipmentType.SupportWeapon);
                break;
            case ShopEquipementType.Nade:
                tempList = EquipmentPool.GetEquipmentListByType(VSEquipmentType.Nade);
                break;
            case ShopEquipementType.Character:
                tempList = EquipmentPool.GetEquipmentListByType(VSEquipmentType.Character);
                break;
        }
        //foreach (var item in tempList)
        //{
        //    if (!PlayerEquipmentInfo.EquipmentSOList.Contains(item)) listEquipment.Add(item);
        //}

        return tempList;
    }

    public void UpdateTabWhenEquip() => Tab(_typeTabing);
    public void Tab(ShopEquipementType type)
    {
        _typeTabing = type;
        foreach (Transform card in EquipmentContent.transform) Destroy(card.gameObject);

        _listEquipment = GetListEquipmentByShopType(type);
        foreach (var equipment in _listEquipment)
        {
            if (equipment.Price != -1)
            {

                GameObject equipmentCard = Instantiate(EquipmentCardPrefab, EquipmentContent.transform);
                equipmentCard.GetComponent<ShopEquipmentCard>().Set(equipment);
            }
        }
        //Resize EquipmentContent
        EquipmentContent.GetComponent<RectTransform>().sizeDelta = _equipmentContentSizeDefault;
        int N = _listEquipment.Count;
        int row = Mathf.CeilToInt((float)N / 4f);
        if (row > 2) EquipmentContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 400f * (row - 2));
        //Tab on/off
        foreach (var tab in EquipmentTab)
            tab.Value.GetComponent<Image>().color = tab.Key == type ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0);

        Commons.WaitNextFrame(this, () =>
        {
            if (EquipmentContent.transform.childCount > 0)
                EquipmentContent.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
        });
    }
    public void Search(string name)
    {

        foreach (Transform equipment in EquipmentContent.transform)
            equipment.gameObject.SetActive(IsContainString(equipment.GetComponent<ShopEquipmentCard>().Equipment.Name, name));
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

public enum ShopEquipementType
{
    Gun,
    Support,
    Nade,
    Character
}
