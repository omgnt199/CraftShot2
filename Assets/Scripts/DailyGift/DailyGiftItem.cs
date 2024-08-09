using Assets.Scripts.Common;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class DailyGiftItem : MonoBehaviour
{
    [SerializeField] private int _dayClaim;
    [SerializeField] private List<ValueKeyPair<CurrencyType, int>> _currencyReward;
    [SerializeField] private VSEquipment _equipmentReward;
    [SerializeField] private GameObject _claimedOverlay;
    [SerializeField] private GameObject _sunshineEffect;

    [SerializeField] private TextMeshProUGUI _dayClaimText;

    [SerializeField] private GameObject _coinReward;
    [SerializeField] private TextMeshProUGUI _coinClaim;
    [SerializeField] private GameObject _diamondReward;
    [SerializeField] private TextMeshProUGUI _diamondClaim;
    [SerializeField] private Image _rewardIcon;


    public int DayClaim => _dayClaim;
    public bool IsClaimed;
    private void Awake()
    {
        //InitView();
    }
    private void Start()
    {
        InitView();
    }
    private void OnEnable()
    {
        if (DOTween.IsTweening(_sunshineEffect.transform)) _sunshineEffect.transform.DORestart();
    }
    private void OnDisable()
    {
        if (DOTween.IsTweening(_sunshineEffect.transform)) _sunshineEffect.transform.DOPause();
    }
    private void InitView()
    {
        this.WaitNextFrame(() =>
        {
            _dayClaimText.text = "Daily " + _dayClaim.ToString();
            if (_dayClaim < 7)
            {
                foreach (var reward in _currencyReward)
                {
                    if (reward.Key == CurrencyType.Coin)
                    {
                        _coinReward.SetActive(true);
                        _coinClaim.text = reward.Value.ToString();
                    }
                    else if (reward.Key == CurrencyType.Diamond)
                    {
                        _diamondReward.SetActive(true);
                        _diamondClaim.text = reward.Value.ToString();
                    }
                }
                if (_equipmentReward != null) _rewardIcon.sprite = _equipmentReward.Icon;
            }
            _claimedOverlay.SetActive(IsClaimed);
        });

    }

    public void ClaimNormal()
    {
        GlobalUI.Instance.ShowPopUp("ReceiveOverlay");
        List<ItemReceive> itemReceives = new List<ItemReceive>();
        foreach (var reward in _currencyReward)
        {
            CurrencyData.UpdateCurrency(reward.Key, reward.Value);
            ReceiveItemOverlay.Instance.AddCurrencyReceiveItem(reward.Key, reward.Value);
        }
        if (_equipmentReward != null)
        {
            PlayerEquipmentInfo.Add(_equipmentReward.Name);
            ReceiveItemOverlay.Instance.AddEquipmentReceiveItem(_equipmentReward, 1);
        }
        ReceiveItemOverlay.Instance.SetUI();

        PlayerPrefs.SetString("DailyGift" + _dayClaim + "Claimed", "true");
        _claimedOverlay.SetActive(true);
        TurnOffSunshine();
        DailyGiftController.instance.DeactiveClaimBtn();
        IsClaimed = true;
    }

    public void ClaimAds()
    {
        ServiceManager.ShowReward(x =>
        {
            GlobalUI.Instance.ShowPopUp("ReceiveOverlay");
            List<ItemReceive> itemReceives = new List<ItemReceive>();

            foreach (var reward in _currencyReward)
            {
                CurrencyData.UpdateCurrency(reward.Key, reward.Value * 2);
                ReceiveItemOverlay.Instance.AddCurrencyReceiveItem(reward.Key, reward.Value * 2);
            }
            if (_equipmentReward != null)
            {
                PlayerEquipmentInfo.Add(_equipmentReward.Name);
                ReceiveItemOverlay.Instance.AddEquipmentReceiveItem(_equipmentReward, 1);
            }

            PlayerPrefs.SetString("DailyGift" + _dayClaim + "Claimed", "true");
            _claimedOverlay.SetActive(true);
            IsClaimed = true;
            TurnOffSunshine();
            DailyGiftController.instance.DeactiveClaimBtn();
            ReceiveItemOverlay.Instance.SetUI();
        });

    }

    public void TurnOnSunshine()
    {
        _sunshineEffect.SetActive(true);
        _sunshineEffect.transform.DORotate(new Vector3(0, 0, 360f), 2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void TurnOffSunshine()
    {
        _sunshineEffect.SetActive(false);
        _sunshineEffect.transform.DOKill();
    }
}
