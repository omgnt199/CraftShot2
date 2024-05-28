using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "VoxelStrikeTask/WeaponKillTask")]
public class VSGunKillTask : VSDailyTask
{
    public VSGun Gun;
    public void OnKillByGun() 
    {
        if (PlayerEventListener.MainPlayer.GetComponent<VSPlayerControlWeapon>().GunUsing.Name == Gun.Name) UpdateProgress();
    }
    public override void StartObserverTask()
    {
        base.StartObserverTask();
        UnityAction action = null;
        action += OnKillByGun;
        string eventName = EventName + Gun.Name;
        PlayerEventListener.RegisterEvent(eventName, action);
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
        PlayerEventListener.RemoveEvent(EventName + Gun.Name);
    }

}
