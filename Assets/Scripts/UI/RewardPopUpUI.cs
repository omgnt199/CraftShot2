using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardPopUpUI : MonoBehaviour
{
    [SerializeField] private GameObject WinTitle;
    [SerializeField] private GameObject LoseTitle;
    [SerializeField] private TextMeshProUGUI CoinRewardText;
    [SerializeField] private TextMeshProUGUI ExpRewardText;
    [SerializeField] private TextMeshProUGUI CoinRewardAdsText;
    private int _coinReward = 0;
    private int _expReward = 0;
    private void OnEnable()
    {
        SetReward();
    }

    void SetReward()
    {
        if (GameManager.Instance.Mode == "Domination")
        {
            if (((DominationModeSO)GameManager.Instance.CurrentMode).TeamWin.TeamSide == VSTeamSide.TeamAlly)
            {
                WinTitle.SetActive(true);
                CoinRewardText.text = "+100";
                ExpRewardText.text = "+50";

                _coinReward = 100;
                _expReward = 50;
            }
            else if (((DominationModeSO)GameManager.Instance.CurrentMode).TeamWin.TeamSide == VSTeamSide.TeamEnemy)
            {
                LoseTitle.SetActive(true);
                CoinRewardText.text = "+30";
                ExpRewardText.text = "+15";
                _coinReward = 30;
                _expReward = 15;
            }
            CoinRewardAdsText.text = "+" + (_coinReward * 2).ToString();
        }
        else if (GameManager.Instance.Mode == "Deathmatch")
        {
            if (VSScoreBoardUI.Instance.PlayerRankInDeathmatchMode <= 3)
            {
                WinTitle.SetActive(true);
                CoinRewardText.text = "+100";
                ExpRewardText.text = "+50";
                _coinReward = 100;
                _expReward = 50;
            }
            else
            {
                LoseTitle.SetActive(true);
                CoinRewardText.text = "+30";
                ExpRewardText.text = "+15";
                _coinReward = 30;
                _expReward = 15;
            }
            CoinRewardAdsText.text = "+" + (_coinReward * 2).ToString();
        }
    }

    public void RewardNormal()
    {
        CurrencyData.UpdateCurrency(CurrencyType.Coin, _coinReward);
        CurrencyData.UpdateCurrency(CurrencyType.Exp, _expReward);
        SceneManager.LoadScene("VoxelStrikeHome");
    }

    public void RewardAds()
    {
        ServiceManager.ShowReward(x =>
        {
            _coinReward *= 2;
            CurrencyData.UpdateCurrency(CurrencyType.Coin, _coinReward);
            CurrencyData.UpdateCurrency(CurrencyType.Exp, _expReward);
            SceneManager.LoadScene("VoxelStrikeHome");
        });
    }
}
