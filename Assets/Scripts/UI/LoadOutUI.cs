using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadOutUI : MonoBehaviour
{
    [SerializeField] private Image PrimaryWeaponImg;
    [SerializeField] private Image SecondaryWeaponImg;
    [SerializeField] private Image CharacterSkinImg;
    [SerializeField] private Image SupportWeaponImg;
    [SerializeField] private Image NadeImg;

    [SerializeField] private Transform SkinLocate;
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private EquipmentEventChanelSO _equipEquipmentLoadOut;
    private void Awake()
    {
        AttachEquipmentToSkin(GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSPrimaryWeaponUsing")));
        _equipEquipmentLoadOut.OnEventRaised += AttachEquipmentToSkin;
    }

    private void OnEnable()
    {
        Load();
    }

    void AttachEquipmentToSkin(VSEquipment equipment)
    {
        //Debug.Log(SkinLocate);
        if (_gunHolder.childCount == 1) Destroy(_gunHolder.GetChild(0).gameObject);
        SkinLocate.GetComponentInChildren<Animator>().runtimeAnimatorController = equipment.AnimatorControllerForBot;
        Instantiate(equipment.ModelForBot, _gunHolder);
    }
    public void Load()
    {
        PrimaryWeaponImg.sprite = GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSPrimaryWeaponUsing"))?.Icon;
        PrimaryWeaponImg.enabled = PrimaryWeaponImg.sprite != null;
        SecondaryWeaponImg.sprite = GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSSecondaryWeaponUsing"))?.Icon;
        SecondaryWeaponImg.enabled = SecondaryWeaponImg.sprite != null;
        SupportWeaponImg.sprite = GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSSupportWeaponUsing"))?.Icon;
        SupportWeaponImg.enabled = SupportWeaponImg.sprite != null;
        NadeImg.sprite = GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSNadeUsing"))?.Icon;
        NadeImg.enabled = NadeImg.sprite != null;
        CharacterSkinImg.sprite = GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSCharacterSkinUsing"))?.Icon;
        CharacterSkinImg.enabled = CharacterSkinImg.sprite != null;
    }
}
