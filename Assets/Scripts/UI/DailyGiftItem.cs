using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public bool IsClaimed
    {
        get => IsClaimed;
        set
        {
            IsClaimed = value;
            _claimedOverlay.SetActive(value);
        }
    }
    private void Start()
    {
        InitView();
    }

    private void InitView()
    {
        _dayClaimText.text = "Daily " + _dayClaim.ToString();
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

    public void ClaimNormal()
    {
        foreach (var reward in _currencyReward) CurrencyData.UpdateCurrency(reward.Key, reward.Value);
        if (_equipmentReward != null) PlayerEquipmentInfo.Add(_equipmentReward.Name);
        PlayerPrefs.SetString("DailyGift" + _dayClaim + "Claimed", "true");
    }

    public void ClaimAds()
    {
        foreach (var reward in _currencyReward) CurrencyData.UpdateCurrency(reward.Key, reward.Value * 2);
        if (_equipmentReward != null) PlayerEquipmentInfo.Add(_equipmentReward.Name);
        PlayerPrefs.SetString("DailyGift" + _dayClaim + "Claimed", "true");
    }
}
