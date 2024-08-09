using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftController : MonoBehaviour
{
    public static DailyGiftController instance;
    [SerializeField] private List<DailyGiftItem> _dailyGiftItemList;
    private DailyGiftItem _dailyGiftToday;
    private int _dailyClaimToday;

    [SerializeField] private GameObject _Container;

    [SerializeField] private Button GetBtn;
    [SerializeField] private GameObject GetBtnOverlay;
    [SerializeField] private Button GetX2Btn;
    [SerializeField] private GameObject GetX2BtnOverlay;

    public int DailyClaimToday => _dailyClaimToday;
    private void Awake()
    {
        instance = this;

        _dailyClaimToday = PlayerPrefs.GetInt("DailyGiftClaimDay") != 0 ? PlayerPrefs.GetInt("DailyGiftClaimDay") : 1;
        if (GlobalData.Instance.IsNewDay)
        {
            if (PlayerPrefs.GetString("DailyGift" + _dailyClaimToday + "Claimed") == "true")
            {
                _dailyClaimToday = Mathf.Min(_dailyGiftItemList.Count, _dailyClaimToday + 1);
                PlayerPrefs.SetInt("DailyGiftClaimDay", _dailyClaimToday);
            }
        }
        foreach (var item in _dailyGiftItemList)
        {
            if (item.DayClaim < _dailyClaimToday)
            {
                item.IsClaimed = true;
            }
            else if (item.DayClaim == _dailyClaimToday)
            {
                if (PlayerPrefs.GetString("DailyGift" + _dailyClaimToday + "Claimed") == "true")
                {
                    item.IsClaimed = true;
                    DeactiveClaimBtn();
                }
                else
                {
                    item.IsClaimed = false;
                    item.TurnOnSunshine();
                    GetBtn.onClick.AddListener(item.ClaimNormal);
                    GetBtn.onClick.AddListener(DeactiveClaimBtn);
                    GetX2Btn.onClick.AddListener(item.ClaimAds);
                    GetX2Btn.onClick.AddListener(DeactiveClaimBtn);
                }
            }
            else item.IsClaimed = false;
        }
    }
    private void OnEnable()
    {
        //_Container.SetActive(true);
        _Container.transform.localScale = Vector3.zero;
        _Container.transform.DOScale(1f, 0.5f);
    }

    public void DeactiveClaimBtn()
    {
        GetBtnOverlay.SetActive(true);
        GetX2BtnOverlay.SetActive(true);
        GetBtn.onClick.RemoveAllListeners();
        GetX2Btn.onClick.RemoveAllListeners();
    }

}
