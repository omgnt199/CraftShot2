using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockEquipmentUI : MonoBehaviour
{
    [SerializeField] private GameObject Equipment;
    [SerializeField] private GameObject Name;
    [SerializeField] private GameObject Tap;

    public TextMeshProUGUI NameTxt;
    public Image Icon;

    private Sequence seq;

    [SerializeField] private EquipmentEventChanelSO _unlockEquipmentEvent;
    private void OnEnable()
    {
        _unlockEquipmentEvent.OnEventRaised += SetUI;
        seq = DOTween.Sequence();

        Equipment.transform.localScale = Vector3.zero;
        Name.transform.localScale = Vector3.zero;
        Tap.transform.localScale = Vector3.zero;

        seq.Insert(0, Equipment.transform.DOScale(1f, 1f));
        seq.Insert(0, Name.transform.DOScale(1f, 1f));
        seq.Insert(0, Tap.transform.DOScale(1f, 1f));
    }
    private void OnDisable()
    {
        _unlockEquipmentEvent.OnEventRaised -= SetUI;
        seq.Kill();
    }

    void SetUI(VSEquipment e)
    {
        NameTxt.text = e.Name;
        Icon.sprite = e.Icon;
    }
}
