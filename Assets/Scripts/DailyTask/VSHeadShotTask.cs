using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "VoxelStrikeTask/HeadshotTask")]
public class VSHeadShotTask : VSDailyTask
{
    public void OnHeadShotKill()
    {
        UpdateProgress();
    }
    public override void StartObserverTask()
    {
        base.StartObserverTask();
        UnityAction action = null;
        action += OnHeadShotKill;
        PlayerEventListener.RegisterEvent(EventName, action);
    }
    public override void UpdateProgress()
    {
        base.UpdateProgress();
        Debug.Log(TaskInfo + "update");
        AchivedValue++;
        if (AchivedValue == GoalValue)
        {
            IsCompleted = true;
            StopObserverTask();
        }
    }
    public override void StopObserverTask()
    {
        base.StopObserverTask();
        PlayerEventListener.RemoveEvent(EventName);
    }
}
