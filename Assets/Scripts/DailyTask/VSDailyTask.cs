
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CurrencyReward
{
    public CurrencyType CurrencyType;
    public int RewardValue;
}

[System.Serializable]
public class VSDailyTask : ScriptableObject
{
    public string TaskInfo;
    public List<CurrencyReward> CurrencyReward;
    public int GoalValue;
    public int AchivedValue;
    public bool IsCompleted = false;
    public bool IsClaimed = false;
    public string EventName;
    public virtual void StartObserverTask() 
    {

    }
    public virtual void UpdateProgress()
    {
    }
    public virtual void StopObserverTask() 
    {
    }
}
