using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VSTaskUI : MonoBehaviour
{
    public VSDailyTask Task;
    public TextMeshProUGUI TaskInfoText;
    public TextMeshProUGUI TaskRewardValueText;
    public TextMeshProUGUI TaskProgressText;
    public Image TaskBarImg;
    public Image TaskCurrencyRewardImg;
    public Sprite CoinCurrencyIcon;
    public Sprite DiamondCurrencyIcon;

    public void Set(VSDailyTask task)
    {
        Task = task;
        TaskInfoText.text = Task.TaskInfo;
        foreach (var reward in Task.CurrencyReward)
        {
            if (reward.CurrencyType != CurrencyType.Exp)
            {
                TaskRewardValueText.text = reward.RewardValue.ToString();
                if (reward.CurrencyType == CurrencyType.Coin) TaskCurrencyRewardImg.sprite = CoinCurrencyIcon;
                else if (reward.CurrencyType == CurrencyType.Diamond) TaskCurrencyRewardImg.sprite = DiamondCurrencyIcon;
                break;
            }
        }

        TaskProgressText.text = Task.AchivedValue.ToString() + "/" + Task.GoalValue.ToString();
        TaskBarImg.fillAmount = (float)Task.AchivedValue / (float)Task.GoalValue;
    }
    public void ClaimReward()
    {
        if (Task.IsCompleted)
        {
            foreach (var reward in Task.CurrencyReward) CurrencyData.UpdateCurrency(reward.CurrencyType, reward.RewardValue);
            DailyTaskObserver.OnAnyDailyTaskClaimed(Task);
            VSDailyTaskPopUpUI.Instance.LoadTaskUI();
            while (ExperienceSystem.IsCanLevelUp()) ExperienceSystem.LevelUp();
            PlayerProfileUI.instance.UpdateExpBarUI(CurrencyData.GetCurrencyValue(CurrencyType.Exp));
        }
    }
}
