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
        TaskRewardValueText.text = Task.RewardValue.ToString();
        TaskProgressText.text = Task.AchivedValue.ToString() + "/" + Task.GoalValue.ToString();
        TaskBarImg.fillAmount = (float)Task.AchivedValue / (float)Task.GoalValue;
        if (Task.RewardType == CurrencyType.Coin) TaskCurrencyRewardImg.sprite = CoinCurrencyIcon;
        else if (Task.RewardType == CurrencyType.Diamond) TaskCurrencyRewardImg.sprite = DiamondCurrencyIcon;
    }
    public void ClaimReward()
    {
        if (Task.IsCompleted)
        {
            CurrencyData.UpdateCurrency(Task.RewardType, Task.RewardValue);
            DailyTaskObserver.OnAnyDailyTaskClaimed(Task);
            VSDailyTaskPopUpUI.Instance.LoadTaskUI();
        }
    }
}
