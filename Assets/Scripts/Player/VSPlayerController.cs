using UnityEngine;
using UnityEngine.UI;

public class VSPlayerController : MonoBehaviour
{
    [Header("Script")]
    public VSPlayerInfo PlayerInfo;
    public VSPlayerControlWeapon PCW;
    public VSPlayerControlAnimator Anim;
    public VSPlayerHandleEquipment PlayerHandleEquipment;
    [Header("Handle Weapon")]
    public GameObject PrimaryWeapon;
    public GameObject SecondaryWeapon;
    public GameObject SupportWeapon;
    public GameObject Nade;
    private VSGun _primaryWeaponInfo;
    private VSGun _secondaryWeaponInfo;
    private VSSupportWeapon _supportWeaponInfo;
    private VSNade _nadeInfo;
    private GameObject _weaponUsing;
    [Header("Costume")]
    //public CostumeController costume;
    [Header("BackpackUI")]
    public GameObject PrimaryWeapon_UI;
    public GameObject SecondaryWeapon_UI;
    public GameObject SupportWeapon_UI;
    private GameObject _weaponUIPicking;
    [Header("UI")]
    public Image AttackButton_Img;
    public Sprite Shoot_Sprite;
    public Sprite Knife_Sprite;

    //Getter
    public VSNade NadeUsing { get => _nadeInfo; }
    
    public enum VSWeaponSlot
    {
        Primary,
        Secondary
    }
    private void Awake()
    {
        //costume.LoadCostumeSet(PlayerGlobalInfo.CostumeSetIds);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        MyInput();
#endif
    }

    public void SetUpWeapon(VSGun primaryWeapon, VSGun secondaryWeapon, VSSupportWeapon supportWeapon, VSNade nade)
    {
        _primaryWeaponInfo = primaryWeapon;
        _secondaryWeaponInfo = secondaryWeapon;
        _supportWeaponInfo = supportWeapon;
        _nadeInfo = nade;

        Instantiate(_primaryWeaponInfo.Model, PrimaryWeapon.transform);
        Instantiate(_secondaryWeaponInfo.Model, SecondaryWeapon.transform);
        Instantiate(_supportWeaponInfo.Model, SupportWeapon.transform);
        Instantiate(_nadeInfo.Model, Nade.transform);


        VSWeaponAmmoInfoIngame pWPAmmoIngame = PrimaryWeapon.GetComponent<VSWeaponAmmoInfoIngame>();
        pWPAmmoIngame.Magazine = _primaryWeaponInfo.Magazine;
        pWPAmmoIngame.CurrentMagazine = pWPAmmoIngame.Magazine;
        pWPAmmoIngame.CurrentAmmo = _primaryWeaponInfo.TotalAmmo - _primaryWeaponInfo.Magazine;

        VSWeaponAmmoInfoIngame sWPAmmoIngame = SecondaryWeapon.GetComponent<VSWeaponAmmoInfoIngame>();
        sWPAmmoIngame.Magazine = _secondaryWeaponInfo.Magazine;
        sWPAmmoIngame.CurrentMagazine = sWPAmmoIngame.Magazine;
        sWPAmmoIngame.CurrentAmmo = _secondaryWeaponInfo.TotalAmmo - _secondaryWeaponInfo.Magazine;

        //Gun Setup
        _weaponUsing = PrimaryWeapon;
        PCW.EquipGun(_primaryWeaponInfo, pWPAmmoIngame);
        PCW.FirePoint = _weaponUsing.transform.GetComponentInChildren<VSMuzzle>().transform;
        PCW.SetAttackState(VSAttackState.Shoot);
        Anim.WeaponAnimator = PrimaryWeapon.GetComponentInChildren<Animator>();
        //UI Setup
        PrimaryWeapon_UI.transform.Find("IconWP").GetComponent<Image>().sprite = _primaryWeaponInfo.Icon;
        PrimaryWeapon_UI.transform.Find("IconWP").GetComponent<Image>().SetNativeSize();
        SecondaryWeapon_UI.transform.Find("IconWP").GetComponent<Image>().sprite = _secondaryWeaponInfo.Icon;
        SecondaryWeapon_UI.transform.Find("IconWP").GetComponent<Image>().SetNativeSize();
        SupportWeapon_UI.transform.Find("IconWP").GetComponent<Image>().sprite = _supportWeaponInfo.Icon;
        SupportWeapon_UI.transform.Find("IconWP").GetComponent<Image>().SetNativeSize();

        _weaponUIPicking = PrimaryWeapon_UI;
        _weaponUIPicking.transform.Find("Stroke").gameObject.SetActive(true);
        VSInGameUIScript.instance.UpdateAmmoUI(PCW.CurrentMagazine, PCW.CurrentAmmo);

        //Nade Setup
        PCW.Nadeusing = _nadeInfo;
        PCW.NadeAmount = PCW.Nadeusing.Quantity;
        //Knife Setup
        PCW.KnifeUsing = _supportWeaponInfo;
    }

    public void ResetWeapon()
    {
        VSWeaponAmmoInfoIngame pWPAmmoIngame = PrimaryWeapon.GetComponent<VSWeaponAmmoInfoIngame>();
        pWPAmmoIngame.Magazine = _primaryWeaponInfo.Magazine;
        pWPAmmoIngame.CurrentMagazine = pWPAmmoIngame.Magazine;
        pWPAmmoIngame.CurrentAmmo = _primaryWeaponInfo.TotalAmmo - _primaryWeaponInfo.Magazine;

        VSWeaponAmmoInfoIngame sWPAmmoIngame = SecondaryWeapon.GetComponent<VSWeaponAmmoInfoIngame>();
        sWPAmmoIngame.Magazine = _secondaryWeaponInfo.Magazine;
        sWPAmmoIngame.CurrentMagazine = sWPAmmoIngame.Magazine;
        sWPAmmoIngame.CurrentAmmo = _secondaryWeaponInfo.TotalAmmo - _secondaryWeaponInfo.Magazine;
        PCW.NadeAmount = PCW.Nadeusing.Quantity;
        SelectPrimaryWeapon();
        Anim.WeaponAnimator = PrimaryWeapon.GetComponentInChildren<Animator>();
        PCW.OnSwitchWeapon();
        VSInGameUIScript.instance.UpdateAmmoUI(PCW.CurrentMagazine, PCW.CurrentAmmo);
    }

    public void SelectPrimaryWeapon()
    {
        _weaponUsing.SetActive(false);
        PrimaryWeapon.SetActive(true);
        _weaponUsing = PrimaryWeapon;

        PCW.EquipGun(_primaryWeaponInfo, PrimaryWeapon.GetComponent<VSWeaponAmmoInfoIngame>());
        Anim.WeaponAnimator = PrimaryWeapon.GetComponentInChildren<Animator>();
        PCW.FirePoint = _weaponUsing.transform.GetComponentInChildren<VSMuzzle>().transform;
        PCW.CancleReload();
        PCW.OnSwitchWeapon();
        PCW.SetAttackState(VSAttackState.Shoot);

        _weaponUIPicking.transform.Find("Stroke").gameObject.SetActive(false);
        _weaponUIPicking = PrimaryWeapon_UI;
        _weaponUIPicking.transform.Find("Stroke").gameObject.SetActive(true);
        AttackButton_Img.sprite = Shoot_Sprite;
        VSInGameUIScript.instance.UpdateAmmoUI(PCW.VSWpAmmoIngameInfo.CurrentMagazine, PCW.VSWpAmmoIngameInfo.CurrentAmmo);
    }
    public void SelectSecondaryWeapon()
    {
        _weaponUsing.SetActive(false);
        SecondaryWeapon.SetActive(true);
        _weaponUsing = SecondaryWeapon; ;

        PCW.EquipGun(_secondaryWeaponInfo, SecondaryWeapon.GetComponent<VSWeaponAmmoInfoIngame>());
        Anim.WeaponAnimator = SecondaryWeapon.GetComponentInChildren<Animator>();
        PCW.Crosshair.RifleNotAim();
        PCW.FirePoint = _weaponUsing.transform.GetComponentInChildren<VSMuzzle>().transform;
        PCW.CancleReload();
        PCW.OnSwitchWeapon();
        PCW.SetAttackState(VSAttackState.Shoot);

        _weaponUIPicking.transform.Find("Stroke").gameObject.SetActive(false);
        _weaponUIPicking = SecondaryWeapon_UI;
        _weaponUIPicking.transform.Find("Stroke").gameObject.SetActive(true);
        AttackButton_Img.sprite = Shoot_Sprite;
        VSInGameUIScript.instance.UpdateAmmoUI(PCW.VSWpAmmoIngameInfo.CurrentMagazine, PCW.VSWpAmmoIngameInfo.CurrentAmmo);
    }

    public void SelectSupportWeapon()
    {
        _weaponUsing.SetActive(false);
        SupportWeapon.SetActive(true);
        _weaponUsing = SupportWeapon;
        Anim.WeaponAnimator = SupportWeapon.GetComponentInChildren<Animator>();
        PCW.SetAttackState(VSAttackState.Knife);
        AttackButton_Img.sprite = Knife_Sprite;
        _weaponUIPicking.transform.Find("Stroke").gameObject.SetActive(false);
        _weaponUIPicking = SupportWeapon_UI;
        _weaponUIPicking.transform.Find("Stroke").gameObject.SetActive(true);
    }

    public void SelectNade()
    {
        _weaponUsing.SetActive(false);
        Nade.SetActive(true);
        _weaponUsing = Nade;
        Anim.WeaponAnimator = Nade.GetComponentInChildren<Animator>();
        PCW.SetAttackState(VSAttackState.ThrowNade);
    }

    public void OnTakeDamage() { }

    void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectPrimaryWeapon();
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSecondaryWeapon();
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSupportWeapon();
        else if (Input.GetKeyDown(KeyCode.Tab)) VSInGameUIScript.instance.ShowLeaderBoardPopUp();
        else if (Input.GetKeyDown(KeyCode.M)) VSInGameUIScript.instance.ShowHideLargeMap();
    }
}
