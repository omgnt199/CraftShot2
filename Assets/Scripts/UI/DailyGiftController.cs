using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyGiftController : MonoBehaviour
{
    [SerializeField] private List<DailyGiftItem> _dailyGiftItemList;
    private DailyGiftItem _dailyGiftToday;
    private int _dailyClaimToday;
    private void Awake()
    {
        _dailyClaimToday = PlayerPrefs.GetInt("DailyGiftClaimDay");
        if (GlobalData.Instance.IsNewDay)
        {
            if(PlayerPrefs.GetString("DailyGift" + _dailyClaimToday + "Claimed") == "true") 
            {
                _dailyClaimToday++;
            }
        }

    }
    private void OnEnable()
    {
        
    }

}
