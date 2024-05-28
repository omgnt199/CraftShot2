using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VSInventoryUIController : MonoBehaviour
{
    public static VSInventoryUIController Instance;
    public GameObject EquipmentContent;
    public GameObject EquipmentCardPrefab;
    public VSEquipmentPool EquipmentPool;
    private List<VSEquipment> _listEquipment = new List<VSEquipment>();

    public GameObject WeaponTab;
    public GameObject SupportWeaponTab;
    public GameObject NadeTab;
    public GameObject CharacterTab;
    private Dictionary<VSEquipmentType, GameObject> EquipmentTab;
    private VSEquipmentType _typeTabing;
    private Vector2 _equipmentContentSizeDefault;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        EquipmentTab = new Dictionary<VSEquipmentType, GameObject>()
        {
            { VSEquipmentType.Weapon,WeaponTab },
            { VSEquipmentType.SupportWeapon,SupportWeaponTab },
            { VSEquipmentType.Nade,NadeTab },
            { VSEquipmentType.Character,CharacterTab }
        };
        _equipmentContentSizeDefault = EquipmentContent.GetComponent<RectTransform>().sizeDelta;
        WeaponTab.GetComponent<Button>().onClick.AddListener(delegate { Tab(VSEquipmentType.Weapon); });
        SupportWeaponTab.GetComponent<Button>().onClick.AddListener(delegate { Tab(VSEquipmentType.SupportWeapon); });
        NadeTab.GetComponent<Button>().onClick.AddListener(delegate { Tab(VSEquipmentType.Nade); });
        CharacterTab.GetComponent<Button>().onClick.AddListener(delegate { Tab(VSEquipmentType.Character); });

        WeaponTab.GetComponent<Button>().onClick.Invoke();
        EquipmentContent.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateTabWhenEquip() => Tab(_typeTabing);
    public void Tab(VSEquipmentType type)
    {
        _typeTabing = type;
        foreach (Transform card in EquipmentContent.transform) Destroy(card.gameObject);
        _listEquipment.Clear();
        _listEquipment = new List<VSEquipment>(EquipmentPool.GetEquipmentListByType(type));
        foreach(var equipment in _listEquipment)
        {
            GameObject equipmentCard = Instantiate(EquipmentCardPrefab, EquipmentContent.transform);
            equipmentCard.GetComponent<VSEquipmentCard>().Set(equipment);
        }
        //Resize EquipmentContent
        EquipmentContent.GetComponent<RectTransform>().sizeDelta = _equipmentContentSizeDefault;
        int N = _listEquipment.Count;
        int row = Mathf.CeilToInt((float)N / 3f);
        if (row > 2) EquipmentContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 345f * (row - 2));
        //Tab on/off
        foreach (var tab in EquipmentTab)
            tab.Value.GetComponent<Image>().color = tab.Key == type ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0);
    }
    public void Search(string name)
    {
        foreach (Transform equipment in EquipmentContent.transform) 
            equipment.gameObject.SetActive(IsContainString(equipment.GetComponent<VSEquipmentCard>().Equipment.Name, name));
    }
    bool IsContainString(string str1,string str2)
    {
        foreach(var c in str2)
        {
            if (!str1.Contains(char.ToLower(c)) && !str1.Contains(char.ToUpper(c))) return false;
        }
        return true;
    }
}



public enum VSEquipmentType
{
    Weapon,
    SupportWeapon,
    Nade,
    Character
}
