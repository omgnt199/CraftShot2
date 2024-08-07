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

    public Transform SkinLocate;
    public GameObject _gunHolder;
    [SerializeField] private EquipmentEventChanelSO _equipEquipmentLoadOut;


    private void OnEnable()
    {
        AttachEquipmentToSkin(GlobalData.Instance.EquipmentPool.GetEquipmentByName(PlayerPrefs.GetString("VSPrimaryWeaponUsing")));
        Load();
        _equipEquipmentLoadOut.OnEventRaised += AttachEquipmentToSkin;
    }
    void AttachEquipmentToSkin(VSEquipment equipment)
    {
        //Debug.Log(_gunHolder);
        if (_gunHolder != null)
        {

            if (_gunHolder.transform.childCount > 0)
            {
                foreach (Transform item in _gunHolder.transform) Destroy(item.gameObject);
            }
        }
        //SkinLocate.GetComponentInChildren<Animator>().runtimeAnimatorController = equipment.AnimatorControllerForBot;
        Instantiate(equipment.ModelForBot, _gunHolder.transform);
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
